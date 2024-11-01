using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Relay.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestCultureProvider _cultureProvider;

        public CultureMiddleware(RequestDelegate next, RequestCultureProvider cultureProvider)
        {
            _next = next;
            _cultureProvider = cultureProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var culture = _cultureProvider.DetermineRequestCulture(context);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _next(context);
        }
    }

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

            if (string.IsNullOrEmpty(cultureHeader))
            {
                _logger.LogInformation("Заголовок 'Accept-Language' не найден. Установлена культура по умолчанию: 'ru'.");
                return new CultureInfo("ru");
            }

            var cultures = cultureHeader.Split(',')
                                        .Select(c => c.Trim().ToLowerInvariant())
                                        .Distinct()
                                        .ToList();

            if (cultures.Any(c => c.StartsWith("ru")))
            {
                _logger.LogInformation("Приоритетная культура выбрана: 'ru'.");
                return new CultureInfo("ru");
            }

            if (cultures.Any(c => c.StartsWith("en")))
            {
                _logger.LogInformation("Выбрана английская культура.");
                return new CultureInfo("en");
            }

            _logger.LogWarning("Неподдерживаемый язык в запросе. Установлена культура по умолчанию: 'ru'.");
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
}