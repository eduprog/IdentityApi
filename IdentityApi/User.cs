using Microsoft.AspNetCore.Identity;

namespace IdentityApi;

public class User : IdentityUser
{
    public string? Initials { get; set; }
}