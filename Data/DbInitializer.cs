using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Relay.Models;

namespace Relay.Data
{
    /// <summary>
    /// Класс для инициализации базы данных при первом запуске приложения.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Инициализирует базу данных, применяет миграции и добавляет начальные данные.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="logger">Логгер для вывода информации о процессе инициализации.</param>
        public static async Task InitializeAsync(ApplicationDbContext context, ILogger logger)
        {
            logger.LogInformation("Начало миграции базы данных...");

            // Применение всех миграций, если они еще не применены
            await context.Database.MigrateAsync();

            // Проверка, существует ли хотя бы один пользователь в базе данных
            if (!await context.Users.AnyAsync())
            {
                logger.LogInformation("Создание начального пользователя администратора...");

                // Добавление пользователя администратора по умолчанию
                await context.Users.AddAsync(new User { Username = "admin", Password = "password" });
                await context.SaveChangesAsync();

                logger.LogInformation("Пользователь администратор успешно создан.");
            }
            else
            {
                logger.LogInformation("База данных уже инициализирована, администратор не требуется.");
            }

            logger.LogInformation("Инициализация базы данных завершена.");
        }
    }
}
