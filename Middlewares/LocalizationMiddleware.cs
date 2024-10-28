using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LocalizationMiddleware> _logger;

    public LocalizationMiddleware(RequestDelegate next, ILogger<LocalizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Установка языка по запросу (по умолчанию "ru").
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var cultureQuery = context.Request.Query["lang"].ToString();
        var culture = !string.IsNullOrEmpty(cultureQuery) && IsValidCulture(cultureQuery)
            ? cultureQuery
            : "ru";

        CultureInfo.CurrentCulture = new CultureInfo(culture);
        CultureInfo.CurrentUICulture = new CultureInfo(culture);

        _logger.LogInformation("Установлен язык: {Culture}", culture);

        await _next(context);
    }

    /// <summary>
    /// Проверка доступности языка.
    /// </summary>
    private bool IsValidCulture(string culture)
    {
        try
        {
            _ = new CultureInfo(culture);
            return true;
        }
        catch (CultureNotFoundException)
        {
            return false;
        }
    }
}
