using System.ComponentModel.DataAnnotations; 

namespace Functions.DTOs;

public class ContactRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    [MaxLength(254)]
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MinLength(1)]
    [MaxLength(5000)]
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}