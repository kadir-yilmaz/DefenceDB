using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Editor")]
public class CategoryManagementController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryManagementController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        // Get all categories, we can filter to root categories if we want
        // But for showcase, maybe we just list root categories?
        var rootCategories = (await _categoryService.GetCategoriesWithChildrenAsync()).ToList();
        return View(rootCategories);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleShowcase(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        category.IsShowcase = !category.IsShowcase;
        await _categoryService.UpdateCategoryAsync(category);

        TempData["Success"] = $"'{category.Name}' kategorisi vitrin durumu güncellendi.";
        return RedirectToAction(nameof(Index));
    }
}
