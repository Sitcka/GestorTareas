using API.Middleware;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Incluir comentarios XML para Swagger
var xmlFile = System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".xml");
builder.Services.AddSwaggerGen(c =>
{
    if (System.IO.File.Exists(xmlFile))
    {
        c.IncludeXmlComments(xmlFile);
    }
});

// DbContext
builder.Services.AddDbContext<GestorTareasDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("GestorTareas")
    )
);

// Repositorios
builder.Services.AddScoped<ITareaRepositorio, TareaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// Servicios
builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


var corsPolicyName = "FrontendDev";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.WithOrigins("http://localhost:4200")   
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS ANTES de auth/autorización
app.UseCors(corsPolicyName);

// Middleware de excepciones antes del resto
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();