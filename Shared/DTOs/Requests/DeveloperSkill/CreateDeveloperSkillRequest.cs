using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Requests.DeveloperSkill;

public class CreateDeveloperSkillRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required] [Range(-1, 100)] public int ProficiencyLevel { get; set; }
}