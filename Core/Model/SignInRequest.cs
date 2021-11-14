using System.ComponentModel.DataAnnotations;

namespace Hera.Core.Model;

public record SignInRequest {
    [Required]
    public String Username { get; set; }

    [Required]
    public String Password { get; set; }
}