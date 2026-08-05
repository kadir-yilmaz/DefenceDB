using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using DefenceDB.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class MascotManagementController : Controller
{
    private readonly IMascotSettingService _mascotService;
    private readonly INotificationService _notificationService;

    public MascotManagementController(IMascotSettingService mascotService, INotificationService notificationService)
    {
        _mascotService = mascotService;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        var settings = await _mascotService.GetAllAsync();
        return View(settings);
    }

    public IActionResult Create()
    {
        return View(new MascotSetting());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MascotSetting model)
    {
        ModelState.Remove("Title");
        model.Title = "";
        
        if (ModelState.IsValid)
        {
            model.TargetPath = model.TargetPath.ToLower().Trim();
            await _mascotService.AddAsync(model);
            _notificationService.Success("Maskot ayarı başarıyla eklendi.");
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var setting = await _mascotService.GetByIdAsync(id);
        if (setting == null) return NotFound();
        return View(setting);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MascotSetting model)
    {
        ModelState.Remove("Title");
        model.Title = "";
        
        if (ModelState.IsValid)
        {
            var setting = await _mascotService.GetByIdAsync(model.Id);
            if (setting == null) return NotFound();

            setting.TargetPath = model.TargetPath.ToLower().Trim();
            setting.Title = model.Title;
            setting.Message = model.Message;
            setting.LinksJson = model.LinksJson;
            setting.IsActive = model.IsActive;

            await _mascotService.UpdateAsync(setting);
            _notificationService.Success("Maskot ayarı başarıyla güncellendi.");
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _mascotService.DeleteAsync(id);
        _notificationService.Success("Maskot ayarı silindi.");
        return RedirectToAction(nameof(Index));
    }
}
