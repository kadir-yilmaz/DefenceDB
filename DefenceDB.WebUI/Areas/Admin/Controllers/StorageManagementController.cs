using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DefenceDB.WebUI.Services;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

public class SingleFileUploadRequest
{
    public string RelativePath { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
    public string ApplicationKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
}

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class StorageManagementController : Controller
{
    private readonly IBackblazeMigrationService _migrationService;
    private readonly IConfiguration _configuration;

    public StorageManagementController(IBackblazeMigrationService migrationService, IConfiguration configuration)
    {
        _migrationService = migrationService;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.ServiceUrl = _configuration["Backblaze:ServiceUrl"] ?? "https://s3.us-west-004.backblazeb2.com";
        ViewBag.BucketName = _configuration["Backblaze:BucketName"] ?? "defencedb";
        ViewBag.KeyId = _configuration["Backblaze:KeyId"] ?? "";
        ViewBag.ApplicationKey = _configuration["Backblaze:ApplicationKey"] ?? "";

        return View();
    }

    [HttpPost]
    public IActionResult GetFileList()
    {
        var files = _migrationService.GetLocalMediaFiles();
        return Json(new { success = true, totalFiles = files.Count, files });
    }

    [HttpPost]
    public async Task<IActionResult> UploadSingleFile([FromBody] SingleFileUploadRequest req)
    {
        if (req == null || string.IsNullOrWhiteSpace(req.RelativePath))
        {
            return Json(new { isSuccess = false, message = "Geçersiz istek parametresi!" });
        }

        var result = await _migrationService.UploadSingleFileAsync(
            req.RelativePath, 
            req.KeyId, 
            req.ApplicationKey, 
            req.BucketName, 
            req.ServiceUrl
        );

        return Json(result);
    }
}
