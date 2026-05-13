using Application.DTOs;

namespace Application.Interfaces;

public interface IUsuarioService
{
    UsuarioResponseDto CrearUsuario(CrearUsuarioDto dto);

    List<UsuarioResponseDto> ObtenerTodos();

    UsuarioResponseDto? ObtenerPorId(int id);

    bool Eliminar(int id);
}