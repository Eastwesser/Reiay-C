using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Relay.Data;
using Relay.Services;
using System.Text;
using Relay.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Настройка логирования
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Настройка подключения к базе данных PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка аутентификации с использованием JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Извлекаем ключ JWT из конфигурации
        var key = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("JWT Key отсутствует в конфигурации.");
        }
        else
        {
            Console.WriteLine($"JWT Key: {key}"); // Логируем значение ключа для отладки
        }

        // Настройка параметров проверки токенов
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Проверка издателя
            ValidateAudience = true, // Проверка аудитории
            ValidateLifetime = true, // Проверка срока действия токена
            ValidateIssuerSigningKey = true, // Проверка ключа подписи токена
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Указание допустимого издателя
            ValidAudience = builder.Configuration["Jwt:Audience"], // Указание допустимой аудитории
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // Ключ подписи
        };
    });

// Регистрация сервисов приложения
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<RequestCultureProvider>();
// (Добавьте другие сервисы здесь)

// Добавление контроллеров и Swagger для документации API
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Инициализация базы данных при запуске приложения
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await DbInitializer.InitializeAsync(dbContext, logger);
}

// Настройка промежуточного ПО (middleware) и маршрутизации
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Подключение Swagger в режиме разработки
    app.UseSwaggerUI();
}

app.UseMiddleware<LocalizationMiddleware>(); // Промежуточное ПО для локализации
app.UseMiddleware<CultureMiddleware>(); // ПО для локализации
app.UseHttpsRedirection(); // Перенаправление HTTP на HTTPS
app.UseRouting(); // Настройка маршрутизации запросов
app.UseAuthentication(); // Подключение аутентификации
app.UseAuthorization(); // Подключение авторизации
app.MapControllers(); // Маршрутизация к контроллерам

app.Run(); // Запуск приложения
