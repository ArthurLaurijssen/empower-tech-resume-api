using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Shared.DTOs.Requests.SocialMediaLink;

public class CreateSocialMediaLinkRequest
{
    [Required]
    public string SocialMediaUrl { get; set; } = string.Empty;

    [Required]
    public string SocialMediaNetworkName { get; set; } = string.Empty;
}