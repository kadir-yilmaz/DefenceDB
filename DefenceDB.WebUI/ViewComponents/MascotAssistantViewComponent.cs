using DefenceDB.BLL.Abstract;
using Microsoft.AspNetCore.Mvc;
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
        var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}".ToLower();
        var pathAndQuery = $"{request.Path}{request.QueryString}".ToLower();
        var pathOnly = request.Path.Value?.ToLower() ?? "";

        // Öncelik sırasına göre eşleşme ara: Tam URL -> Path+Query -> Sadece Path -> Genel (*)
        var setting = activeSettings.FirstOrDefault(s => s.TargetPath.ToLower().Trim() == fullUrl)
                   ?? activeSettings.FirstOrDefault(s => s.TargetPath.ToLower().Trim() == pathAndQuery)
                   ?? activeSettings.FirstOrDefault(s => s.TargetPath.ToLower().Trim() == pathOnly)
                   ?? activeSettings.FirstOrDefault(s => s.TargetPath.Trim() == "*");

        if (setting == null || !setting.IsActive)
        {
            // Do not render anything if no active setting for this page
            return Content(string.Empty);
        }

        return View(setting);
    }
}
