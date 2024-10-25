var builder = WebApplication.CreateBuilder(args);

// Настройка базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление сервисов
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Настройки Swagger для документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Создание и настройка приложения
var app = builder.Build();

// Применение миграций при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.InitializeAsync(dbContext);
}

// Настройки для режима разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Relay API v1");
        c.RoutePrefix = string.Empty; // Позволяет открыть Swagger по корневому URL
    });
}

// Применение Middleware
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Маршрутизация контроллеров
app.MapControllers();

// Запуск приложения
app.Run();
