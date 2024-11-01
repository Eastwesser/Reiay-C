using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Relay.Middlewares
{
    public class RequestCultureProvider
    {
        private readonly ILogger<RequestCultureProvider> _logger;

        public RequestCultureProvider(ILogger<RequestCultureProvider> logger)
        {
            _logger = logger;
        }

        public CultureInfo DetermineRequestCulture(HttpContext context)
        {
            var langQuery = context.Request.Query["lang"].ToString();
            if (!string.IsNullOrEmpty(langQuery))
            {
                // Проверка на допустимость культуры
                if (IsCultureValid(langQuery))
                {
                    return new CultureInfo(langQuery);
                }
                else
                {
                    // Логгирование недопустимого языка
                    _logger.LogWarning($"Запрашиваемая культура недействительна: {langQuery}. Устанавливается культура по умолчанию 'ru'.");
                }
            }

            var acceptLanguage = context.Request.Headers["Accept-Language"].FirstOrDefault();
            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                var cultures = acceptLanguage.Split(',');
                if (cultures.Length > 0)
                {
                    // Проверка на допустимость культуры
                    if (IsCultureValid(cultures[0]))
                    {
                        return new CultureInfo(cultures[0]);
                    }
                }
            }

            // По умолчанию возвращаем русский
            return new CultureInfo("ru");
        }

        private bool IsCultureValid(string cultureName)
        {
            try
            {
                CultureInfo.GetCultureInfo(cultureName);
                return true;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }
    }
}
