using System;

namespace Arzand.Modules.Identity.DTOs;

public class AuthResponse
{
    public string Token { get; set; }
    public string Email { get; set; }
}