using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GestorTareasDbContext : DbContext
    {
        public GestorTareasDbContext(DbContextOptions<GestorTareasDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Tarea> Tareas => Set<Tarea>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(usuario => usuario.Id);

                entity.HasIndex(usuario => usuario.Email).IsUnique();

                entity.Property(usuario => usuario.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(usuario => usuario.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(usuario => usuario.EsAdmin)
                    .IsRequired();
            });

            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(tarea => tarea.Id);

                entity.Property(tarea => tarea.Titulo)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(tarea => tarea.FechaLimite)
                    .IsRequired();

                entity.Property(tarea => tarea.FechaCreacion)
                    .IsRequired();

                entity.HasOne(tarea => tarea.Usuario)
                    .WithMany(usuario => usuario.Tareas)
                    .HasForeignKey(tarea => tarea.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasDiscriminator<string>("TipoTarea")
                    .HasValue<TareaSimple>("Simple")
                    .HasValue<TareaPrioritaria>("Prioritaria")
                    .HasValue<TareaRecurrente>("Recurrente");
            });

            modelBuilder.Entity<TareaPrioritaria>(entity =>
            {
                entity.Property(tarea => tarea.Prioridad)
                    .IsRequired();
            });

            modelBuilder.Entity<TareaRecurrente>(entity =>
            {
                entity.Property(tarea => tarea.IntervaloDias)
                    .IsRequired();

                entity.Property(tarea => tarea.ProximaOcurrencia)
                    .IsRequired();
            });
        }
    }
}