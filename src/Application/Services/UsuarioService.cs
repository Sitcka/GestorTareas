using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepositorio _repositorio;

        public UsuarioService(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
        {
            var usuarios = await _repositorio.GetAllAsync();

            return usuarios
                .Select(usuario => new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    EsAdmin = usuario.EsAdmin
                })
                .ToList();
        }

        public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
        {
            var usuario = await _repositorio.GetByIdAsync(id);
            if (usuario is null) return null;

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                EsAdmin = usuario.EsAdmin
            };
        }

        public async Task<UsuarioResponseDto> CreateAsync(CrearUsuarioDto dto)
        {
            // Validación de negocio extra: email único
            var emailEnUso = await ExistsEmailAsync(dto.Email);
            if (emailEnUso)
            {
                // Esta excepción la convertirá el middleware en un 400
                throw new ArgumentException("Ya existe un usuario con ese email.", nameof(dto.Email));
            }

            var usuario = new Usuario(
                nombre: dto.Nombre,
                email: dto.Email,
                esAdmin: dto.EsAdmin
            );

            await _repositorio.AddAsync(usuario);

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                EsAdmin = usuario.EsAdmin
            };
        }

        public async Task<UsuarioResponseDto?> UpdateAsync(int id, CrearUsuarioDto dto)
        {
            var usuario = await _repositorio.GetByIdAsync(id);
            if (usuario is null) return null;

            // Aquí podrías decidir si permites cambiar el email y,
            // en ese caso, volver a comprobar que no está duplicado.

            var usuarioActualizado = new Usuario(
                nombre: dto.Nombre,
                email: dto.Email,
                esAdmin: dto.EsAdmin
            );

            // Truco simple: obligamos a mantener el mismo Id
            typeof(Usuario)
                .GetProperty(nameof(Usuario.Id))!
                .SetValue(usuarioActualizado, id);

            await _repositorio.UpdateAsync(usuarioActualizado);

            return new UsuarioResponseDto
            {
                Id = usuarioActualizado.Id,
                Nombre = usuarioActualizado.Nombre,
                Email = usuarioActualizado.Email,
                EsAdmin = usuarioActualizado.EsAdmin
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repositorio.DeleteAsync(id);
        }

        public async Task<bool> ExistsEmailAsync(string email)
        {
            return await _repositorio.ExistsEmailAsync(email);
        }
    }
}