using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Tests;

public class TareaRepositoryTests
{
    private GestorTareasDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<GestorTareasDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new GestorTareasDbContext(options);
    }

    [Fact]
    public async Task Paginacion_Devolver_Elementos_Y_Total()
    {
        using var context = CreateInMemoryContext("paginacion_test");
        var repo = new TareaRepositorio(context);

        // Seed usuarios y tareas
        var usuario = new Usuario("Alice", "alice@example.com");
        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();

        for (int i = 1; i <= 25; i++)
        {
            var tarea = new TareaSimple($"Tarea {i}", DateTime.Today.AddDays(i), PrioridadTarea.Media, usuario.Id);
            await context.Tareas.AddAsync(tarea);
        }
        await context.SaveChangesAsync();

        var (items, total) = await repo.GetPagedAsync(2, 10);

        Assert.Equal(25, total);
        Assert.Equal(10, items.Count());
        Assert.Equal("Tarea 11", items.First().Titulo);
    }

    [Fact]
    public async Task GetPagedByUsuario_Incluye_Usuario()
    {
        using var context = CreateInMemoryContext("include_test");
        var repo = new TareaRepositorio(context);

        var usuario = new Usuario("Bob", "bob@example.com");
        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();

        var tarea = new TareaSimple("Tarea X", DateTime.Today.AddDays(5), PrioridadTarea.Alta, usuario.Id);
        await context.Tareas.AddAsync(tarea);
        await context.SaveChangesAsync();

        var (items, total) = await repo.GetPagedByUsuarioAsync(usuario.Id, 1, 10);

        Assert.Equal(1, total);
        var first = items.First();
        Assert.NotNull(first.Usuario);
        Assert.Equal("Bob", first.Usuario.Nombre);
    }
}
