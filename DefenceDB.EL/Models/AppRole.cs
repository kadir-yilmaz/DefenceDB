using Microsoft.AspNetCore.Identity;

namespace DefenceDB.EL.Models;

public class AppRole : IdentityRole
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
