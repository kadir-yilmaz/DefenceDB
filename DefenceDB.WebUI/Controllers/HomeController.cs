using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using Microsoft.EntityFrameworkCore;
using DefenceDB.WebUI.Models;
using System.Diagnostics;

namespace DefenceDB.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly IProductQueryService _productQueryService;
    private readonly AppDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        ICategoryQueryService categoryQueryService,
        IProductQueryService productQueryService,
        AppDbContext context)
    {
        _logger = logger;
        _categoryQueryService = categoryQueryService;
        _productQueryService = productQueryService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Categories = await _categoryQueryService.GetRootCategoriesAsync();
        ViewBag.RecentProducts = await _productQueryService.GetRecentProductsAsync(5);
        ViewBag.ShowcaseProducts = await _productQueryService.GetShowcaseProductsAsync();
        ViewBag.ShowcaseArticles = await _context.Articles
            .AsNoTracking()
            .Include(a => a.ArticleCategory)
            .Where(a => a.IsPublished && a.IsShowcase)
            .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt)
            .Take(5)
            .ToListAsync();
        ViewData["Title"] = "Ana Sayfa - Savunma Wiki";

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
