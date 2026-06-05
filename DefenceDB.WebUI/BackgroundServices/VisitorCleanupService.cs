using DefenceDB.BLL.Abstract;

namespace DefenceDB.WebUI.BackgroundServices;

/// <summary>
/// Her gün otomatik olarak eski ziyaretçi kayıtlarını temizleyen background service.
/// KVKK uyumu için 30 günden eski kayıtlar silinir.
/// </summary>
public class VisitorCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<VisitorCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(1); // Her gün çalış

    public VisitorCleanupService(
        IServiceProvider serviceProvider,
        ILogger<VisitorCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Visitor Cleanup Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(_cleanupInterval, stoppingToken);

                _logger.LogInformation("Running visitor cleanup task");

                using var scope = _serviceProvider.CreateScope();
                var visitorService = scope.ServiceProvider.GetRequiredService<IVisitorService>();
                
                await visitorService.CleanupOldVisitorsAsync(daysToKeep: 30);

                _logger.LogInformation("Visitor cleanup task completed successfully");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Visitor Cleanup Service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during visitor cleanup");
                // Hata durumunda servis devam etsin
            }
        }
    }
}
