using JHF.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext -> đọc chuỗi từ appsettings.Development.json
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS cho Next.js (dev)
builder.Services.AddCors(o =>
{
    o.AddPolicy("web", p => p
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();     // bật OpenAPI JSON
    app.UseSwaggerUI();   // UI tại /swagger
}

app.UseCors("web");

// Nếu bạn muốn ép HTTPS, giữ dòng dưới; còn không có thể bỏ.
// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
