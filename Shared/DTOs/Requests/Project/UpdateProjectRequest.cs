using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Requests.Project;

public class UpdateProjectRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 2)]
    public string Description { get; set; } = string.Empty;
}