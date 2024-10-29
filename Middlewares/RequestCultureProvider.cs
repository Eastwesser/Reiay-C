using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class RequestCultureProvider
{
    private readonly ILogger<RequestCultureProvider> _logger;

    public RequestCultureProvider(ILogger<RequestCultureProvider> logger)
    {
        _logger = logger;
    }

    public CultureInfo DetermineRequestCulture(HttpContext context)
    {
        var cultureHeader = context.Request.Headers["Accept-Language"].ToString();
        var cultureName = cultureHeader?.Split(',')[0].Trim() ?? "ru"; // Основной язык в случае отсутствия заголовка

        // Проверка корректности culture
        if (IsValidCulture(cultureName))
        {
            _logger.LogInformation("Язык, указанный в запросе: {Culture}", cultureName);
            return new CultureInfo(cultureName);
        }

        _logger.LogWarning("Неверный код языка в запросе: {Culture}. Установлен 'ru'", cultureName);
        return new CultureInfo("ru");
    }


    private bool IsValidCulture(string cultureName)
    {
        try
        {
            _ = new CultureInfo(cultureName);
            return true;
        }
        catch (CultureNotFoundException)
        {
            return false;
        }
    }
}