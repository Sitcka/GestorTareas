using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Servicios de la API
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<GestorTareasDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Repositorios
builder.Services.AddScoped<Infrastructure.Repositories.ITareaRepositorio, Infrastructure.Repositories.TareaRepositorio>();
builder.Services.AddScoped<Infrastructure.Repositories.IUsuarioRepositorio, Infrastructure.Repositories.UsuarioRepositorio>();
// Servicios de aplicación
builder.Services.AddScoped<Application.Interfaces.ITareaService, Application.Services.TareaService>();
builder.Services.AddScoped<Application.Interfaces.IUsuarioService, Application.Services.UsuarioService>();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();