using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.WebUI.Models;
using System.Diagnostics;

namespace DefenceDB.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryService _categoryService;
    private readonly IProductQueryService _productQueryService;

    public HomeController(
        ILogger<HomeController> logger,
        ICategoryService categoryService,
        IProductQueryService productQueryService)
    {
        _logger = logger;
        _categoryService = categoryService;
        _productQueryService = productQueryService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Categories = await _categoryService.GetRootCategoriesAsync();
        ViewBag.RecentProducts = await _productQueryService.GetRecentProductsAsync(5);
        ViewBag.ShowcaseProducts = await _productQueryService.GetShowcaseProductsAsync();
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
