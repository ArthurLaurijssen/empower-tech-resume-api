using Domain.Common;

namespace Domain.Entities;

/// <summary>
///     Represents the association between DeveloperSkills and Project.
/// </summary>
public class DeveloperSkillsUsedInProject : BaseEntity
{
    public Guid DeveloperSkillsId { get; set; }
    public DeveloperSkill DeveloperSkill { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}