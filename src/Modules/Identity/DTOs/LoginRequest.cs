using System;
using System.ComponentModel.DataAnnotations;

namespace Arzand.Modules.Identity.DTOs;

public class LoginRequest
{
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
