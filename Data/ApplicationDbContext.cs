using Microsoft.EntityFrameworkCore;
using Relay.Models;

/// <summary>
/// Контекст базы данных, определяющий доступ к таблицам пользователей, ролей, сообщений и других сущностей.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApplicationDbContext"/> с заданными параметрами.
    /// </summary>
    /// <param name="options">Параметры конфигурации контекста базы данных.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    /// <summary>
    /// Таблица пользователей.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Таблица ролей пользователей.
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Таблица сообщений.
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Таблица клиентов.
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    /// Таблица хостеров.
    /// </summary>
    public DbSet<Hoster> Hosters { get; set; }

    /// <summary>
    /// Таблица приватных серверов.
    /// </summary>
    public DbSet<PrivateServer> PrivateServers { get; set; }
}
