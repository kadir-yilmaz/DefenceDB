using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.ViewComponents;

public class MascotAssistantViewComponent : ViewComponent
{
    private readonly IMascotSettingService _mascotService;

    public MascotAssistantViewComponent(IMascotSettingService mascotService)
    {
        _mascotService = mascotService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var allSettings = await _mascotService.GetAllAsync();
        var activeSettings = allSettings.Where(s => s.IsActive).ToList();

        if (!activeSettings.Any())
            return Content(string.Empty);

        var request = HttpContext.Request;
        var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        var pathAndQuery = $"{request.Path}{request.QueryString}";
        var pathOnly = request.Path.Value ?? "";

        // Helper to check matches in split target paths
        MascotSetting FindMatch(Func<string, bool> predicate)
        {
            return activeSettings.FirstOrDefault(setting => 
            {
                var targets = setting.TargetPath
                    .Split(new[] { ',', '\n', '\r', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim());
                return targets.Any(predicate);
            });
        }

        // Öncelik sırasına göre eşleşme ara: Tam URL -> Path+Query -> Sadece Path -> Genel (*)
        var setting = FindMatch(t => string.Equals(t, fullUrl, StringComparison.OrdinalIgnoreCase))
                   ?? FindMatch(t => string.Equals(t, pathAndQuery, StringComparison.OrdinalIgnoreCase))
                   ?? FindMatch(t => string.Equals(t, pathOnly, StringComparison.OrdinalIgnoreCase))
                   ?? FindMatch(t => t == "*");

        if (setting == null)
        {
            return Content(string.Empty);
        }

        return View(setting);
    }
}
