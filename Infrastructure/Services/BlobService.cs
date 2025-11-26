using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.Exceptions;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <summary>
///     Provides services for interacting with Azure Blob Storage, including operations like moving, retrieving, and
///     deleting blobs.
/// </summary>
/// <remarks>
///     This service is responsible for managing blob operations within a specific Azure Blob Storage container.
///     It supports operations such as moving temporary files, retrieving blob URLs, and cleaning up directories.
/// </remarks>
public class BlobService(BlobServiceClient blobServiceClient, ILogger<BlobService> logger) : IBlobService
{
    /// <inheritdoc />
    public async Task<string?> MoveTemporaryProjectImageAsync(string containerName, string tempDirectoryPath,
        string destinationDirectoryPath)
    {
        // Get the container client for the specified container
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Retrieve files from the temporary directory
        var tempFiles = await GetBlobNamesAsync(containerName, tempDirectoryPath);
        if (!tempFiles.Any())
        {
            // Log and return null if no files are found
            logger.LogInformation("No temporary files found in directory: {TempDirectoryPath}", tempDirectoryPath);
            return null;
        }

        // Select the first file from the temporary directory
        var sourceFileName = tempFiles.First();
        var destinationFileName = $"{destinationDirectoryPath}/project-image";

        // Move the blob to the destination directory
        await MoveBlobAsync(containerName, sourceFileName, destinationFileName);

        // Get the URL of the moved file
        var destinationBlob = containerClient.GetBlobClient(destinationFileName);
        var fileUrl = destinationBlob.Uri.ToString();

        // Clean up the temporary directory
        await DeleteDirectoryAsync(containerName, tempDirectoryPath);

        // Log the successful file move
        logger.LogInformation("Successfully moved project image from {Source} to {Destination}", sourceFileName,
            destinationFileName);
        return fileUrl;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<string>> GetBlobUrlsAsync(string containerName, string directoryPath)
    {
        // Get the container client for the specified container
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var urls = new List<string>();

        // Retrieve blob URLs within the specified directory
        await foreach (var blobItem in containerClient.GetBlobsByHierarchyAsync(prefix: directoryPath, delimiter: "/"))
            if (blobItem.Blob != null)
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Blob.Name);
                urls.Add(blobClient.Uri.ToString());
            }

        return urls.AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<string>> GetBlobNamesAsync(string containerName, string directoryPath)
    {
        // Get the container client for the specified container
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var names = new List<string>();

        // Retrieve blob names within the specified directory
        await foreach (var blobItem in containerClient.GetBlobsByHierarchyAsync(prefix: directoryPath, delimiter: "/"))
            if (blobItem.Blob != null)
                names.Add(blobItem.Blob.Name);

        return names.AsReadOnly();
    }

    /// <inheritdoc />
    public async Task DeleteDirectoryAsync(string containerName, string directoryPath, bool recursive = false)
    {
        // Get the container client for the specified container
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Delete blobs in the specified directory
        // If recursive is false, only delete blobs in the immediate directory
        // If recursive is true, delete blobs in all subdirectories
        await foreach (var blobItem in containerClient.GetBlobsByHierarchyAsync(
                           prefix: directoryPath,
                           delimiter: recursive ? null : "/"))
            if (blobItem.Blob != null)
                await containerClient.DeleteBlobAsync(blobItem.Blob.Name);
    }

    /// <inheritdoc />
    public async Task DeleteBlobAsync(string containerName, string blobName)
    {
        // Get the container client for the specified container
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Delete the specified blob
        await containerClient.DeleteBlobAsync(blobName);
    }

    /// <inheritdoc />
    public async Task MoveBlobAsync(string containerName, string sourceBlobName, string destinationBlobName)
    {
        // Get the container client for the specified container
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var sourceBlob = containerClient.GetBlobClient(sourceBlobName);

        // Verify that the source blob exists
        if (!await sourceBlob.ExistsAsync())
            throw new BlobNotFoundException($"Could not find blob: {sourceBlobName}");

        // Create destination blob and initiate copy operation
        var destinationBlob = containerClient.GetBlobClient(destinationBlobName);
        await destinationBlob.StartCopyFromUriAsync(sourceBlob.Uri);

        // Wait for copy to complete and then delete source blob
        await WaitForBlobCopyAsync(destinationBlob);
        await sourceBlob.DeleteAsync();
    }

    /// <summary>
    ///     Waits for a blob copy operation to complete.
    /// </summary>
    /// <param name="blobClient">The destination blob client for the copy operation.</param>
    /// <exception cref="BlobOperationException">Thrown if the blob copy operation fails.</exception>
    /// <remarks>
    ///     This method polls the blob's copy status and waits until the operation is complete.
    ///     It throws an exception if the copy operation fails.
    /// </remarks>
    private static async Task WaitForBlobCopyAsync(BlobClient blobClient)
    {
        // Retrieve initial properties of the destination blob
        var properties = await blobClient.GetPropertiesAsync();

        // Poll until copy is no longer pending
        while (properties.Value.CopyStatus == CopyStatus.Pending)
        {
            // Wait briefly between checks to avoid excessive API calls
            await Task.Delay(200);
            properties = await blobClient.GetPropertiesAsync();
        }

        // Throw exception if copy was not successful
        if (properties.Value.CopyStatus != CopyStatus.Success)
            throw new BlobOperationException($"Blob copy operation failed with status: {properties.Value.CopyStatus}");
    }
}