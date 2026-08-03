using System.Collections.Generic;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.Services;

public class MigrationLogItem
{
    public string RelativePath { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class MigrationResult
{
    public int TotalFiles { get; set; }
    public int SuccessCount { get; set; }
    public int FailedCount { get; set; }
    public long TotalBytesUploaded { get; set; }
    public List<MigrationLogItem> Logs { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;
}

public interface IBackblazeMigrationService
{
    List<string> GetLocalMediaFiles();
    Task<MigrationLogItem> UploadSingleFileAsync(string relativePath, string keyId, string applicationKey, string bucketName, string serviceUrl);
    Task<MigrationResult> MigrateLocalMediaAsync(string keyId, string applicationKey, string bucketName, string serviceUrl);
}
