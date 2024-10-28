using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestCultureProvider _requestCultureProvider;
    private readonly ILogger<LocalizationMiddleware> _logger;

    public LocalizationMiddleware(
        RequestDelegate next,
        RequestCultureProvider requestCultureProvider,
        ILogger<LocalizationMiddleware> logger
        )
    {
        _next = next;
        _requestCultureProvider = requestCultureProvider;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Определяем culture пользователя с помощью RequestCultureProvider
        var culture = _requestCultureProvider.DetermineRequestCulture(context);
        context.Items["RequestCulture"] = culture;

        _logger.LogInformation("Культура запроса установлена: {Culture}", culture.Name);

        // Передача управления следующему middleware
        await _next(context);
    }
}
