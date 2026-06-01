using NToastNotify;

namespace DefenceDB.WebUI.Services;

public class ToastNotificationService : INotificationService
{
    private readonly IToastNotification _toastNotification;

    public ToastNotificationService(IToastNotification toastNotification)
    {
        _toastNotification = toastNotification;
    }

    public void Success(string message, string? title = null)
    {
        _toastNotification.AddSuccessToastMessage(message, new ToastrOptions
        {
            Title = title ?? "Başarılı"
        });
    }

    public void Error(string message, string? title = null)
    {
        _toastNotification.AddErrorToastMessage(message, new ToastrOptions
        {
            Title = title ?? "Hata"
        });
    }

    public void Warning(string message, string? title = null)
    {
        _toastNotification.AddWarningToastMessage(message, new ToastrOptions
        {
            Title = title ?? "Uyarı"
        });
    }

    public void Info(string message, string? title = null)
    {
        _toastNotification.AddInfoToastMessage(message, new ToastrOptions
        {
            Title = title ?? "Bilgi"
        });
    }
}
