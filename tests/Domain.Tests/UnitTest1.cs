using System;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Tests;

public class UnitTest1
{
    [Fact]
    public void CrearUsuario_ConDatosValidos_CreaUsuario()
    {
        var nombreUsuario = "Juan Perez";
        var emailUsuario = "juan@example.com";

        var usuario = new Usuario(nombreUsuario, emailUsuario);

        Assert.Equal(nombreUsuario, usuario.Nombre);
        Assert.Equal(emailUsuario, usuario.Email);
        Assert.False(usuario.EsAdmin);
        Assert.Equal(0, usuario.Id);
    }

    [Fact]
    public void CrearUsuario_NombreVacio_LanzaArgumentException()
    {
        var emailUsuario = "juan@example.com";

        Assert.Throws<ArgumentException>(() => new Usuario(string.Empty, emailUsuario));
    }

    [Fact]
    public void CrearUsuario_EmailVacio_LanzaArgumentException()
    {
        var nombreUsuario = "Juan Perez";

        Assert.Throws<ArgumentException>(() => new Usuario(nombreUsuario, string.Empty));
    }

    [Fact]
    public void CrearTareaSimple_ConDatosValidos_TieneEstadoPendienteYDiasRestantes()
    {
        var titulo = "Comprar leche";
        var fechaLimite = DateTime.Today.AddDays(5);
        var prioridad = PrioridadTarea.Media;
        var usuarioId = 1;

        var tarea = new TareaSimple(titulo, fechaLimite, prioridad, usuarioId, "Ir al supermercado");

        Assert.Equal(titulo, tarea.Titulo);
        Assert.Equal(prioridad, tarea.Prioridad);
        Assert.Equal(EstadoTarea.Pendiente, tarea.Estado);
        Assert.True(tarea.ObtenerDiasRestantes >= 5 - 1); // puede depender de hora, aceptamos >=4
        Assert.False(tarea.EstaVencida());
    }

    [Fact]
    public void TareaPrioritaria_NivelUrgenciaFueraDeRango_LanzaArgumentException()
    {
        var titulo = "Tarea urgente";
        var fechaLimite = DateTime.Today.AddDays(2);
        var prioridad = PrioridadTarea.Alta;
        var usuarioId = 2;

        Assert.Throws<ArgumentException>(() => new TareaPrioritaria(titulo, fechaLimite, prioridad, 0, usuarioId));
        Assert.Throws<ArgumentException>(() => new TareaPrioritaria(titulo, fechaLimite, prioridad, 11, usuarioId));
    }

    [Fact]
    public void TareaRecurrente_IntervaloInvalido_LanzaArgumentException()
    {
        var titulo = "Riego plantas";
        var fechaLimite = DateTime.Today.AddDays(3);
        var prioridad = PrioridadTarea.Baja;
        var usuarioId = 3;

        Assert.Throws<ArgumentException>(() => new TareaRecurrente(titulo, fechaLimite, prioridad, usuarioId, 0));
    }

    [Fact]
    public void GenerarSiguiente_SoloSiCompletada_GeneraNuevaOcurrencia()
    {
        var titulo = "Backup";
        var fechaLimite = DateTime.Today.AddDays(1);
        var prioridad = PrioridadTarea.Media;
        var usuarioId = 4;
        var intervaloDias = 7;

        var tareaRecurrente = new TareaRecurrente(titulo, fechaLimite, prioridad, usuarioId, intervaloDias);

        // Intentar generar sin completar debe lanzar
        Assert.Throws<InvalidOperationException>(() => tareaRecurrente.GenerarSiguiente());

        // Completar y generar siguiente
        var completado = tareaRecurrente.Completar();
        Assert.True(completado);

        var siguiente = tareaRecurrente.GenerarSiguiente();
        Assert.NotNull(siguiente);
        Assert.Equal(tareaRecurrente.ProximaOcurrencia, siguiente.FechaLimite);
    }

    [Fact]
    public void Completar_CambiaEstadoACompletadaYNoPermiteRepetir()
    {
        var titulo = "Leer libro";
        var fechaLimite = DateTime.Today.AddDays(4);
        var prioridad = PrioridadTarea.Baja;
        var usuarioId = 5;

        var tarea = new TareaSimple(titulo, fechaLimite, prioridad, usuarioId);

        var primera = tarea.Completar();
        Assert.True(primera);
        Assert.Equal(EstadoTarea.Completada, tarea.Estado);

        var segunda = tarea.Completar();
        Assert.False(segunda);
    }

    [Fact]
    public void Cancelar_PoneMotivoYEstadoYNoPermiteCancelarDosVeces()
    {
        var titulo = "Cancelar ejemplo";
        var fechaLimite = DateTime.Today.AddDays(2);
        var prioridad = PrioridadTarea.Media;
        var usuarioId = 6;

        var tarea = new TareaSimple(titulo, fechaLimite, prioridad, usuarioId);

        var cambiado = tarea.Cancelar("No es necesario");
        Assert.True(cambiado);
        Assert.Equal(EstadoTarea.Cancelada, tarea.Estado);
        Assert.Equal("No es necesario", tarea.MotivoCancelacion);

        var segundo = tarea.Cancelar("otro motivo");
        Assert.False(segundo);
    }

    [Fact]
    public void ActualizarFechaLimite_SiEsAnterior_LanzaArgumentException()
    {
        var titulo = "Actualizar fecha";
        var fechaLimite = DateTime.Today.AddDays(3);
        var prioridad = PrioridadTarea.Media;
        var usuarioId = 7;

        var tarea = new TareaSimple(titulo, fechaLimite, prioridad, usuarioId);

        Assert.Throws<ArgumentException>(() => tarea.ActualizarFechaLimite(DateTime.Today.AddDays(-1)));
    }
}
