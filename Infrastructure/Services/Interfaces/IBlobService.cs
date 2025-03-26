namespace Infrastructure.Services.Interfaces;

/// <summary>
///     Defines the contract for blob storage operations.
///     Provides methods for managing files in cloud blob storage,
///     including moving, listing, and deleting blobs and directories.
/// </summary>
public interface IBlobService
{
    /// <summary>
    ///     Moves a project image from a temporary location to its permanent destination in blob storage.
    /// </summary>
    /// <param name="containerName">The name of the blob container</param>
    /// <param name="tempDirectoryPath">The path to the temporary directory containing the image</param>
    /// <param name="destinationDirectoryPath">The path where the image should be permanently stored</param>
    /// <returns>The URL of the moved image if successful; otherwise, null</returns>
    /// <example>
    ///     Move a temporary project image:
    ///     <code>
    /// var imageUrl = await MoveTemporaryProjectImageAsync("projects", "temp/uploads", "projects/images");
    /// </code>
    /// </example>
    Task<string?> MoveTemporaryProjectImageAsync(string containerName, string tempDirectoryPath,
        string destinationDirectoryPath);

    /// <summary>
    ///     Retrieves the URLs for all blobs in a specified directory.
    /// </summary>
    /// <param name="containerName">The name of the blob container</param>
    /// <param name="directoryPath">The path to the directory within the container</param>
    /// <returns>A read-only collection of URLs for all blobs in the specified directory</returns>
    /// <example>
    ///     Get all image URLs in a project directory:
    ///     <code>
    /// var urls = await GetBlobUrlsAsync("projects", "projects/images");
    /// </code>
    /// </example>
    Task<IReadOnlyCollection<string>> GetBlobUrlsAsync(string containerName, string directoryPath);

    /// <summary>
    ///     Retrieves the names of all blobs in a specified directory.
    /// </summary>
    /// <param name="containerName">The name of the blob container</param>
    /// <param name="directoryPath">The path to the directory within the container</param>
    /// <returns>A read-only collection of blob names in the specified directory</returns>
    /// <example>
    ///     Get all image names in a project directory:
    ///     <code>
    /// var names = await GetBlobNamesAsync("projects", "projects/images");
    /// </code>
    /// </example>
    Task<IReadOnlyCollection<string>> GetBlobNamesAsync(string containerName, string directoryPath);

    /// <summary>
    ///     Deletes a directory and optionally its contents from blob storage.
    /// </summary>
    /// <param name="containerName">The name of the blob container</param>
    /// <param name="directoryPath">The path to the directory to delete</param>
    /// <param name="recursive">If true, deletes all contents of the directory; if false, fails if directory is not empty</param>
    /// <returns>A task representing the asynchronous delete operation</returns>
    /// <example>
    ///     Delete a project's image directory and all its contents:
    ///     <code>
    /// await DeleteDirectoryAsync("projects", "projects/images", recursive: true);
    /// </code>
    /// </example>
    Task DeleteDirectoryAsync(string containerName, string directoryPath, bool recursive = false);

    /// <summary>
    ///     Deletes a specific blob from storage.
    /// </summary>
    /// <param name="containerName">The name of the blob container</param>
    /// <param name="blobName">The full name/path of the blob to delete</param>
    /// <returns>A task representing the asynchronous delete operation</returns>
    /// <example>
    ///     Delete a specific project image:
    ///     <code>
    /// await DeleteBlobAsync("projects", "projects/images/project1.jpg");
    /// </code>
    /// </example>
    Task DeleteBlobAsync(string containerName, string blobName);

    /// <summary>
    ///     Moves a blob from one location to another within the same container.
    /// </summary>
    /// <param name="containerName">The name of the blob container</param>
    /// <param name="sourceBlobName">The current name/path of the blob</param>
    /// <param name="destinationBlobName">The new name/path for the blob</param>
    /// <returns>A task representing the asynchronous move operation</returns>
    /// <example>
    ///     Move a project image to a new location:
    ///     <code>
    /// await MoveBlobAsync("projects", "temp/image1.jpg", "projects/images/image1.jpg");
    /// </code>
    /// </example>
    Task MoveBlobAsync(string containerName, string sourceBlobName, string destinationBlobName);
}