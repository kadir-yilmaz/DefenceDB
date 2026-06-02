using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL.Seed;

/// <summary>
/// Runtime seed data initialization for AppUser and AppRole.
/// Note: Categories and Products are seeded via EF Core HasData in Config files.
/// </summary>
public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // Create roles
        string[] roles = { "Admin", "Editor", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole
                {
                    Name = role,
                    Description = role switch
                    {
                        "Admin" => "Tam yetkili yönetici",
                        "Editor" => "İçerik düzenleme yetkisi",
                        "User" => "Standart kullanıcı",
                        _ => role
                    }
                });
            }
        }

        // Create default admin user from appsettings
        var adminEmail = configuration["AdminSettings:Email"];
        var adminPassword = configuration["AdminSettings:Password"];
        var adminFirstName = configuration["AdminSettings:FirstName"];
        var adminLastName = configuration["AdminSettings:LastName"];

        if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
        {
            throw new InvalidOperationException("AdminSettings:Email and AdminSettings:Password must be configured in appsettings.json");
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = adminFirstName ?? "Admin",
                LastName = adminLastName ?? "",
                EmailConfirmed = true,
                ProfileImageUrl = null,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create admin user: {errors}");
            }
        }
        else
        {
            // Ensure existing admin has the Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        
        // Note: Categories and Products are now seeded via Config files using HasData
    }
}
