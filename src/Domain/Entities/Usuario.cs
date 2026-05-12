using System;
namespace Domain.Entities;

public class Usuario
{
    protected Usuario() { }

    public Usuario(string nombre, string email, bool esAdmin = false)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("El email es obligatorio.", nameof(email));
        }

        Id = 0;
        Nombre = nombre.Trim();
        Email = email.Trim();
        EsAdmin = esAdmin;

    }
    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool EsAdmin { get; private set; }

    //Relacion entre Tarea y Usuario
    public List<Tarea> Tareas { get; private set; } = new();
}
