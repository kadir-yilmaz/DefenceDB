using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.WebUI.Models;
using System.Diagnostics;

namespace DefenceDB.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public HomeController(
        ILogger<HomeController> logger,
        ICategoryService categoryService,
        IProductService productService)
    {
        _logger = logger;
        _categoryService = categoryService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Categories = await _categoryService.GetRootCategoriesAsync();
        ViewBag.RecentProducts = await _productService.GetRecentProductsAsync(5);
        ViewBag.ShowcaseProducts = await _productService.GetShowcaseProductsAsync();
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
