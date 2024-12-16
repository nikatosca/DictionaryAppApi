using Microsoft.EntityFrameworkCore;
using dictionary_app.Interfaces;
using dictionary_app.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// 1. Настройка контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=words.db"));

// 2. Регистрация Database как сервиса с интерфейсом IDatabase
builder.Services.AddScoped<IDatabase, Database>();

// 3. Добавление поддержки контроллеров (API)
builder.Services.AddControllers();

// 4. Подключение Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Создание приложения
var app = builder.Build();




// Настройка Swagger UI
if (app.Environment.IsDevelopment()) // в разработке
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dictionary API v1");
        c.RoutePrefix = string.Empty; // Можно задать Swagger UI на корневом пути
    });
}

app.UseAuthorization();

// Маршруты контроллеров
app.MapControllers();

app.Run();
