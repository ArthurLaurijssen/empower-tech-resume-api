namespace Domain.Enums;

/// <summary>
///     Represents the different types of experience entries that can be associated with a developer's profile.
///     Each value indicates a distinct category of professional development or achievement.
/// </summary>
public enum ExperienceType
{
    /// <summary>
    ///     Represents professional work experience, such as employment history or professional roles.
    /// </summary>
    Work = 1,

    /// <summary>
    ///     Represents formal education experience, such as university degrees or academic programs.
    /// </summary>
    Education = 2,

    /// <summary>
    ///     Represents professional certifications or specialized training achievements.
    /// </summary>
    Certification = 3,

    /// <summary>
    ///     Represents an internships, such as at the end of your education
    /// </summary>
    Internship = 4
}