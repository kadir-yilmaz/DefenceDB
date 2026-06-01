using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DefenceDB.EL.Models;
using DefenceDB.WebUI.Services;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UserManagementController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly INotificationService _notificationService;

    public UserManagementController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, INotificationService notificationService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var userRoles = new Dictionary<string, IList<string>>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRoles[user.Id] = roles;
        }

        ViewBag.UserRoles = userRoles;
        ViewBag.AllRoles = _roleManager.Roles.ToList();
        ViewData["Title"] = "Kullanıcı Yönetimi";
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _notificationService.Error("Kullanıcı bulunamadı.", "Hata");
            return RedirectToAction(nameof(Index));
        }

        // Remove all current roles
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        // Add new role
        if (!string.IsNullOrEmpty(newRole))
        {
            await _userManager.AddToRoleAsync(user, newRole);
        }

        _notificationService.Success($"{user.Email} kullanıcısının rolü '{newRole}' olarak güncellendi.", "Başarılı");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _notificationService.Error("Kullanıcı bulunamadı.", "Hata");
            return RedirectToAction(nameof(Index));
        }

        // Prevent deleting yourself
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser?.Id == user.Id)
        {
            _notificationService.Error("Kendi hesabınızı silemezsiniz.", "Hata");
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            _notificationService.Success($"{user.Email} kullanıcısı başarıyla silindi.", "Başarılı");
        }
        else
        {
            _notificationService.Error("Kullanıcı silinirken bir hata oluştu.", "Hata");
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult CreateUser()
    {
        ViewBag.AllRoles = _roleManager.Roles.ToList();
        ViewData["Title"] = "Yeni Kullanıcı Ekle";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(string email, string password, string firstName, string lastName, string role)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            _notificationService.Error("E-posta ve şifre alanları zorunludur.", "Hata");
            ViewBag.AllRoles = _roleManager.Roles.ToList();
            return View();
        }

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            _notificationService.Success($"{email} kullanıcısı başarıyla oluşturuldu.", "Başarılı");
            return RedirectToAction(nameof(Index));
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        _notificationService.Error($"Kullanıcı oluşturulamadı: {errors}", "Hata");
        ViewBag.AllRoles = _roleManager.Roles.ToList();
        return View();
    }
}
