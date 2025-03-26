using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Requests.Experience;

public class CreateExperienceRequest
{
    [Required] public string ExperienceTypeName { get; set; } = string.Empty;

    [Required] public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LocationName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;
}