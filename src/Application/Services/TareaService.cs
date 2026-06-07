using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class TareaService : ITareaService
    {
        private readonly ITareaRepositorio _repositorio;

        public TareaService(ITareaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<TareaResponseDto>> ObtenerTodasAsync()
        {
            var tareas = await _repositorio.GetAllAsync();
            return tareas.Select(MapearADto).ToList();
        }

        public async Task<IEnumerable<TareaResponseDto>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            var tareas = await _repositorio.GetByUsuarioAsync(usuarioId);
            return tareas.Select(MapearADto).ToList();
        }

        public async Task<TareaResponseDto?> ObtenerPorIdAsync(int id)
        {
            var tarea = await _repositorio.GetByIdAsync(id);
            return tarea is null ? null : MapearADto(tarea);
        }

        public async Task<PaginadoResponseDto<TareaResponseDto>> ObtenerPaginadasAsync(int numeroPagina, int tamanoPagina)
        {
            var (items, total) = await _repositorio.GetPagedAsync(numeroPagina, tamanoPagina);
            var dtos = items.Select(MapearADto).ToList();

            return new PaginadoResponseDto<TareaResponseDto>
            {
                Elementos = dtos,
                TotalElementos = total,
                NumeroPagina = numeroPagina,
                TamanoPagina = tamanoPagina
            };
        }

        public async Task<PaginadoResponseDto<TareaResponseDto>> ObtenerPaginadoPorUsuarioAsync(int usuarioId, int numeroPagina, int tamanoPagina)
        {
            var (items, total) = await _repositorio.GetPagedByUsuarioAsync(usuarioId, numeroPagina, tamanoPagina);
            var dtos = items.Select(MapearADto).ToList();

            return new PaginadoResponseDto<TareaResponseDto>
            {
                Elementos = dtos,
                TotalElementos = total,
                NumeroPagina = numeroPagina,
                TamanoPagina = tamanoPagina
            };
        }

        public async Task<TareaResponseDto> CrearSimple(CrearTareaSimpleDto dto)
        {
            var prioridadEnum = dto.Prioridad;
            var tarea = new TareaSimple(dto.Titulo, dto.FechaLimite, prioridadEnum, dto.UsuarioId, dto.Descripcion);
            await _repositorio.AddAsync(tarea);
            return MapearADto(tarea);
        }

        public async Task<TareaResponseDto> CrearPrioritaria(CrearTareaPrioritariaDto dto)
        {
            var prioridadEnum = dto.Prioridad;
            var tarea = new TareaPrioritaria(dto.Titulo, dto.FechaLimite, prioridadEnum, dto.NivelUrgencia, dto.UsuarioId, dto.Descripcion);
            await _repositorio.AddAsync(tarea);
            return MapearADto(tarea);
        }

        public async Task<TareaResponseDto> CrearRecurrente(CrearTareaRecurrenteDto dto)
        {
            var prioridadEnum = dto.Prioridad;
            var tarea = new TareaRecurrente(dto.Titulo, dto.FechaLimite, prioridadEnum, dto.UsuarioId, dto.IntervaloDias, dto.Descripcion);
            await _repositorio.AddAsync(tarea);
            return MapearADto(tarea);
        }

        public async Task<TareaResponseDto?> ActualizarAsync(int id, CrearTareaSimpleDto dto)
        {
            var tarea = await _repositorio.GetByIdAsync(id);
            if (tarea is null) return null;

            tarea.ActualizarDescripcion(dto.Descripcion);
            tarea.ActualizarFechaLimite(dto.FechaLimite);

            await _repositorio.UpdateAsync(tarea);

            return MapearADto(tarea);
        }

        public async Task<TareaResponseDto?> ActualizarSimple(int id, CrearTareaSimpleDto dto)
        {
            return await ActualizarAsync(id, dto);
        }

        public async Task<TareaResponseDto?> ActualizarPrioritaria(int id, CrearTareaPrioritariaDto dto)
        {
            var tarea = await _repositorio.GetByIdAsync(id);
            if (tarea is null) return null;

            if (tarea is not TareaPrioritaria tareaPrioritaria)
            {
                throw new ArgumentException("La tarea no es de tipo prioritaria", nameof(id));
            }

            tareaPrioritaria.ActualizarDescripcion(dto.Descripcion);
            tareaPrioritaria.ActualizarFechaLimite(dto.FechaLimite);
            tareaPrioritaria.ActualizarNivelUrgencia(dto.NivelUrgencia);

            await _repositorio.UpdateAsync(tareaPrioritaria);

            return MapearADto(tareaPrioritaria);
        }

        public async Task<TareaResponseDto?> ActualizarRecurrente(int id, CrearTareaRecurrenteDto dto)
        {
            var tarea = await _repositorio.GetByIdAsync(id);
            if (tarea is null) return null;

            if (tarea is not TareaRecurrente tareaRecurrente)
            {
                throw new ArgumentException("La tarea no es de tipo recurrente", nameof(id));
            }

            tareaRecurrente.ActualizarDescripcion(dto.Descripcion);
            tareaRecurrente.ActualizarFechaLimite(dto.FechaLimite);
            tareaRecurrente.ActualizarIntervaloDias(dto.IntervaloDias);

            await _repositorio.UpdateAsync(tareaRecurrente);

            return MapearADto(tareaRecurrente);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _repositorio.DeleteAsync(id);
        }

        public async Task<bool> MarcarComoCompletada(int id)
        {
            var tarea = await _repositorio.GetByIdAsync(id);
            if (tarea is null) return false;

            var cambiado = tarea.Completar();
            if (!cambiado) return false;

            await _repositorio.UpdateAsync(tarea);
            return true;
        }

        public async Task<bool> CancelarAsync(int id, string motivo)
        {
            var tarea = await _repositorio.GetByIdAsync(id);
            if (tarea is null) return false;

            var cambiado = tarea.Cancelar(motivo);
            if (!cambiado) return false;

            await _repositorio.UpdateAsync(tarea);
            return true;
        }

        private static TareaResponseDto MapearADto(Tarea tarea)
        {
            var dto = new TareaResponseDto
            {
                Id = tarea.Id,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaCreacion = tarea.FechaCreacion,
                FechaLimite = tarea.FechaLimite,
                Estado = tarea.Estado.ToString(),
                Prioridad = tarea.Prioridad.ToString(),
                EstaVencida = tarea.EstaVencida(),
                UsuarioId = tarea.UsuarioId,
                NombreUsuario = tarea.Usuario?.Nombre ?? string.Empty,
                TipoTarea = tarea switch
                {
                    TareaPrioritaria => "Prioritaria",
                    TareaRecurrente => "Recurrente",
                    _ => "Simple"
                }
            };

            if (tarea is TareaPrioritaria tareaPrioritaria)
            {
                dto.NivelUrgencia = tareaPrioritaria.NivelUrgencia;
            }

            if (tarea is TareaRecurrente tareaRecurrente)
            {
                dto.IntervaloDias = tareaRecurrente.IntervaloDias;
                dto.ProximaOcurrencia = tareaRecurrente.ProximaOcurrencia;
            }

            return dto;
        }
    }
}