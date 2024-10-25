using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Relay.Data;
using Relay.Services;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // Метод настройки сервисов
    public void ConfigureServices(IServiceCollection services)
    {
        // Настройка подключения к базе данных
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        // Добавление зависимостей сервисов
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IRoleService, RoleService>();

        // Добавление контроллеров
        services.AddControllers();

        // Настройка Swagger/OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Relay API", Version = "v1" });
        });
    }

    // Метод настройки конвейера обработки запросов
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Если приложение в режиме разработки, подключаем Swagger UI
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Relay API v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        // Подключение конечных точек
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
