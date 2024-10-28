using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Relay.Services;
using Relay.Data;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Конфигурация служб приложения, включая JWT-аутентификацию, базы данных и зависимости.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        // Настройка JWT-аутентификации
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = Configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(key))
                {
                    throw new InvalidOperationException("JWT Key отсутствует в конфигурации.");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        // Настройка подключения к базе данных PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        // Регистрация сервисов приложения
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IRoleService, RoleService>();

        // Добавление контроллеров и Swagger
        services.AddControllers();
        services.AddSwaggerGen();
    }

    /// <summary>
    /// Конфигурация пайплайна обработки запросов, включая маршрутизацию, авторизацию и локализацию.
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // Включение Swagger для документирования API в режиме разработки
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Включение Middleware для локализации
        app.UseMiddleware<LocalizationMiddleware>();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication(); // Включение аутентификации
        app.UseAuthorization();  // Включение авторизации

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Маршрутизация для контроллеров
        });
    }
}
