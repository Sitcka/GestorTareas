using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;

namespace Application.Services;

public class TareaService : ITareaService
{
    private readonly ITareaRepositorio _repo;

    public TareaService(ITareaRepositorio repo)
    {
        _repo = repo;
    }

    public TareaSimpleResponseDto CrearTareaSimple(CrearTareaSimpleDto dto)
    {
        var entidad = new TareaSimple(dto.Titulo, dto.FechaLimite, dto.Prioridad, dto.UsuarioId, dto.Descripcion);
        _repo.AddAsync(entidad).GetAwaiter().GetResult();

        return new TareaSimpleResponseDto
        {
            Id = entidad.Id,
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            FechaCreacion = entidad.FechaCreacion,
            FechaLimite = entidad.FechaLimite,
            Prioridad = entidad.Prioridad,
            Estado = entidad.Estado,
            UsuarioId = entidad.UsuarioId
        };
    }

    public TareaPrioritariaResponseDto CrearTareaPrioritaria(CrearTareaPrioritariaDto dto)
    {
        var entidad = new TareaPrioritaria(dto.Titulo, dto.FechaLimite, dto.Prioridad, dto.NivelUrgencia, dto.UsuarioId, dto.Descripcion);
        _repo.AddAsync(entidad).GetAwaiter().GetResult();

        return new TareaPrioritariaResponseDto
        {
            Id = entidad.Id,
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            FechaCreacion = entidad.FechaCreacion,
            FechaLimite = entidad.FechaLimite,
            Prioridad = entidad.Prioridad,
            NivelUrgencia = entidad is TareaPrioritaria tp ? tp.NivelUrgencia : 0,
            Estado = entidad.Estado,
            UsuarioId = entidad.UsuarioId
        };
    }

    public TareaRecurrenteResponseDto CrearTareaRecurrente(CrearTareaRecurrenteDto dto)
    {
        var entidad = new TareaRecurrente(dto.Titulo, dto.FechaLimite, dto.Prioridad, dto.IntervaloDias, dto.UsuarioId, dto.Descripcion);
        _repo.AddAsync(entidad).GetAwaiter().GetResult();

        return new TareaRecurrenteResponseDto
        {
            Id = entidad.Id,
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            FechaCreacion = entidad.FechaCreacion,
            FechaLimite = entidad.FechaLimite,
            Prioridad = entidad.Prioridad,
            IntervaloDias = entidad is TareaRecurrente tr ? tr.IntervaloDias : 0,
            Estado = entidad.Estado,
            UsuarioId = entidad.UsuarioId
        };
    }

    public List<TareaResponseDto> ObtenerTodas()
    {
        var items = _repo.GetAllAsync().GetAwaiter().GetResult();
        return items.Select(t => new TareaResponseDto
        {
            Id = t.Id,
            Titulo = t.Titulo,
            FechaLimite = t.FechaLimite,
            EstaCompletada = t.Estado == EstadoTarea.Completada,
            TipoTarea = t.GetType().Name,
            NombreUsuario = t.Usuario?.Nombre ?? string.Empty
        }).ToList();
    }

    public TareaResponseDto? ObtenerPorId(int id)
    {
        var t = _repo.GetByIdAsync(id).GetAwaiter().GetResult();
        if (t is null) return null;
        return new TareaResponseDto
        {
            Id = t.Id,
            Titulo = t.Titulo,
            FechaLimite = t.FechaLimite,
            EstaCompletada = t.Estado == EstadoTarea.Completada,
            TipoTarea = t.GetType().Name,
            NombreUsuario = t.Usuario?.Nombre ?? string.Empty
        };
    }

    public List<TareaResponseDto> ObtenerPendientes()
    {
        var items = _repo.GetAllAsync().GetAwaiter().GetResult()
            .Where(t => t.Estado != EstadoTarea.Completada && t.Estado != EstadoTarea.Cancelada);

        return items.Select(t => new TareaResponseDto
        {
            Id = t.Id,
            Titulo = t.Titulo,
            FechaLimite = t.FechaLimite,
            EstaCompletada = false,
            TipoTarea = t.GetType().Name,
            NombreUsuario = t.Usuario?.Nombre ?? string.Empty
        }).ToList();
    }

    public List<TareaResponseDto> ObtenerPorUsuarioId(int usuarioId)
    {
        var items = _repo.GetByUsuarioAsync(usuarioId).GetAwaiter().GetResult();
        return items.Select(t => new TareaResponseDto
        {
            Id = t.Id,
            Titulo = t.Titulo,
            FechaLimite = t.FechaLimite,
            EstaCompletada = t.Estado == EstadoTarea.Completada,
            TipoTarea = t.GetType().Name,
            NombreUsuario = t.Usuario?.Nombre ?? string.Empty
        }).ToList();
    }

    public bool MarcarComoCompletada(int id)
    {
        var t = _repo.GetByIdAsync(id).GetAwaiter().GetResult();
        if (t is null) return false;
        var result = t.Completar();
        if (!result) return false;
        _repo.UpdateAsync(t).GetAwaiter().GetResult();
        return true;
    }

    public bool Eliminar(int id)
    {
        return _repo.DeleteAsync(id).GetAwaiter().GetResult();
    }
}
