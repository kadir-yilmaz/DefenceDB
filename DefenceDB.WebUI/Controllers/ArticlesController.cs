using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.WebUI.Controllers;

public class ArticlesController : Controller
{
    private readonly AppDbContext _context;

    public ArticlesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("Makaleler")]
    public async Task<IActionResult> Index(string? categorySlug, int page = 1)
    {
        const int pageSize = 12;
        page = Math.Max(1, page);

        var categories = await _context.ArticleCategories
            .AsNoTracking()
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        var query = _context.Articles
            .AsNoTracking()
            .Include(a => a.ArticleCategory)
            .Where(a => a.IsPublished);

        ArticleCategory? currentCategory = null;
        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            currentCategory = categories.FirstOrDefault(c => c.Slug == categorySlug);
            if (currentCategory == null)
                return NotFound();

            query = query.Where(a => a.ArticleCategoryId == currentCategory.Id);
        }

        var totalItems = await query.CountAsync();
        var articles = await query
            .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.CurrentCategory = currentCategory;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return View(articles);
    }

    [HttpGet("Makaleler/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return NotFound();

        var article = await _context.Articles
            .AsNoTracking()
            .Include(a => a.ArticleCategory)
            .FirstOrDefaultAsync(a => a.Slug == slug && a.IsPublished);

        if (article == null)
            return NotFound();

        return View(article);
    }
}
