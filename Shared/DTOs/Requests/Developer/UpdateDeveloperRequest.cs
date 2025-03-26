using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Requests.Developer;

public class UpdateDeveloperRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string GreetingTitle { get; set; } = string.Empty;

    [Required]
    [StringLength(350, MinimumLength = 2)]
    public string GreetingMessage { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string MissionTitle { get; set; } = string.Empty;

    [Required]
    [StringLength(350, MinimumLength = 2)]
    public string MissionDescription { get; set; } = string.Empty;

    [Required] public DateTime ItExperienceStartDate { get; set; }

    [Required] public DateTime WorkExperienceStartDate { get; set; }
}