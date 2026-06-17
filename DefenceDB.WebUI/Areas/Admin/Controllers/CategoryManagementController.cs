using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Editor")]
public class CategoryManagementController : Controller
{
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly ICategoryCommandService _categoryCommandService;

    public CategoryManagementController(
        ICategoryQueryService categoryQueryService,
        ICategoryCommandService categoryCommandService)
    {
        _categoryQueryService = categoryQueryService;
        _categoryCommandService = categoryCommandService;
    }

    public async Task<IActionResult> Index()
    {
        // Get all categories, we can filter to root categories if we want
        // But for showcase, maybe we just list root categories?
        var rootCategories = (await _categoryQueryService.GetCategoriesWithChildrenAsync()).ToList();
        return View(rootCategories);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleShowcase(int id)
    {
        var category = await _categoryQueryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        category.IsShowcase = !category.IsShowcase;
        await _categoryCommandService.UpdateCategoryAsync(category);

        TempData["Success"] = $"'{category.Name}' kategorisi vitrin durumu güncellendi.";
        return RedirectToAction(nameof(Index));
    }
}
