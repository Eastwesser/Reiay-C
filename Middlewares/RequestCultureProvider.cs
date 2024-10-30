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
            // Извлекаем первую культуру из заголовка или устанавливаем 'ru' по умолчанию
            var cultureName = cultureHeader?.Split(',')[0].Trim() ?? "ru";

            // Проверяем, является ли полученная культура допустимой
            if (IsValidCulture(cultureName))
            {
                _logger.LogInformation("Язык, указанный в запросе: {Culture}", cultureName); // Логируем информацию о выбранной культуре
                return new CultureInfo(cultureName); // Возвращаем объект CultureInfo для указанной культуры
            }

            _logger.LogWarning("Неверный код языка в запросе: {Culture}. Установлен 'ru'", cultureName); // Логируем предупреждение о неверной культуре
            return new CultureInfo("ru"); // Возвращаем объект CultureInfo для культуры 'ru' по умолчанию
        }

        // Метод для проверки, является ли культура допустимой
        private bool IsValidCulture(string cultureName)
        {
            try
            {
                _ = new CultureInfo(cultureName); // Пытаемся создать объект CultureInfo для указанной культуры
                return true; // Если успешно, возвращаем true
            }
            catch (CultureNotFoundException)
            {
                return false; // Если возникло исключение, возвращаем false
            }
        }
    }
}
