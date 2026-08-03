using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DefenceDB.EL.Models;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Editor")]
public class DashboardController : Controller
{
    private readonly IProductQueryService _productQueryService;
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public DashboardController(
        IProductQueryService productQueryService,
        ICategoryQueryService categoryQueryService,
        UserManager<AppUser> userManager,
        AppDbContext context)
    {
        _productQueryService = productQueryService;
        _categoryQueryService = categoryQueryService;
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Hafif COUNT sorguları — tüm ürünleri belleğe çekmeden
        var totalProducts = await _context.DefenseProducts.CountAsync();
        var activeProducts = await _context.DefenseProducts.CountAsync(p => p.Status == "Active");

        var categories = await _categoryQueryService.GetAllCategoriesAsync();
        var users = _userManager.Users.ToList();

        ViewBag.TotalProducts = totalProducts;
        ViewBag.TotalCategories = categories.Count;
        ViewBag.TotalUsers = users.Count;
        ViewBag.ActiveProducts = activeProducts;

        // Sadece son 5 ürün (Include ile)
        ViewBag.RecentProducts = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).Take(1))
            .OrderByDescending(p => p.Id)
            .Take(5)
            .ToListAsync();

        // Sadece vitrin ürünleri
        ViewBag.ShowcaseProducts = await _productQueryService.GetShowcaseProductsAsync();

        var currentUser = await _userManager.GetUserAsync(User);
        ViewBag.CurrentUser = currentUser;

        ViewData["Title"] = "Dashboard";
        return View();
    }
}
