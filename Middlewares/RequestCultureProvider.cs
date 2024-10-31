using System.Globalization; // Подключаем пространство имен для работы с культурами
using Microsoft.AspNetCore.Http; // Подключаем пространство имен для работы с HTTP-контекстом
using Microsoft.Extensions.Logging; // Подключаем пространство имен для логирования

namespace Relay.Middlewares
{
    // Middleware для определения культуры запроса на основе заголовка Accept-Language
    public class RequestCultureProvider
    {
        private readonly ILogger<RequestCultureProvider> _logger; // Логгер для записи информации и предупреждений

        // Конструктор, принимающий логгер в качестве зависимости
        public RequestCultureProvider(ILogger<RequestCultureProvider> logger)
        {
            _logger = logger; // Инициализируем логгер
        }

        // Метод для определения культуры запроса на основе заголовка Accept-Language
        public CultureInfo DetermineRequestCulture(HttpContext context)
        {
            // Получаем значение заголовка Accept-Language из запроса
            var cultureHeader = context.Request.Headers["Accept-Language"].ToString();

            // Если заголовок не указан, возвращаем культуру по умолчанию ("ru")
            if (string.IsNullOrEmpty(cultureHeader))
            {
                _logger.LogInformation("Заголовок 'Accept-Language' не найден. Установлена культура по умолчанию: 'ru'.");
                return new CultureInfo("ru");
            }

            // Разделяем заголовок по запятым и удаляем лишние пробелы
            var cultures = cultureHeader.Split(',')
                                        .Select(c => c.Trim())
                                        .ToList();

            // Если "ru" присутствует в предпочтениях, выбираем её
            if (cultures.Contains("ru-RU") || cultures.Contains("ru"))
            {
                _logger.LogInformation("Приоритетная культура выбрана: 'ru'.");
                return new CultureInfo("ru");
            }

            // Берём первую указанную культуру, если она не null и допустима
            var primaryCulture = cultures.FirstOrDefault();
            if (primaryCulture != null && IsValidCulture(primaryCulture))
            {
                _logger.LogInformation("Язык, указанный в запросе: {Culture}", primaryCulture);
                return new CultureInfo(primaryCulture);
            }

            // Если первая культура недопустима, возвращаем культуру по умолчанию "ru"
            _logger.LogWarning("Неверный код языка в запросе: {Culture}. Установлена 'ru'", primaryCulture);
            return new CultureInfo("ru");
        }

        // Метод для проверки, является ли культура допустимой
        private bool IsValidCulture(string cultureName)
        {
            try
            {
                // Пытаемся создать объект CultureInfo для указанной культуры
                _ = new CultureInfo(cultureName);
                return true; // Если успешно, возвращаем true
            }
            catch (CultureNotFoundException)
            {
                // Если возникло исключение, возвращаем false
                return false;
            }
        }
    }
}
