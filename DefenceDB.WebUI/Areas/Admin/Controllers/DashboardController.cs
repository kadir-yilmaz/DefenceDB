using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DefenceDB.EL.Models;
using DefenceDB.BLL.Abstract;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Editor")]
public class DashboardController : Controller
{
    private readonly IProductQueryService _productQueryService;
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly UserManager<AppUser> _userManager;

    public DashboardController(
        IProductQueryService productQueryService,
        ICategoryQueryService categoryQueryService,
        UserManager<AppUser> userManager)
    {
        _productQueryService = productQueryService;
        _categoryQueryService = categoryQueryService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productQueryService.GetAllProductsAsync();
        var categories = await _categoryQueryService.GetAllCategoriesAsync();
        var users = _userManager.Users.ToList();

        ViewBag.TotalProducts = products.Count;
        ViewBag.TotalCategories = categories.Count;
        ViewBag.TotalUsers = users.Count;
        ViewBag.ActiveProducts = products.Count(p => p.Status == "Active");
        ViewBag.RecentProducts = products.OrderByDescending(p => p.Id).Take(5).ToList();
        ViewBag.ShowcaseProducts = products.Where(p => p.IsShowcase).OrderByDescending(p => p.Id).ToList();

        var currentUser = await _userManager.GetUserAsync(User);
        ViewBag.CurrentUser = currentUser;

        ViewData["Title"] = "Dashboard";
        return View();
    }
}
