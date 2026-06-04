using Domain.Entities;
using Domain.Enums;
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
        public DbSet<TareaSimple> TareasSimples => Set<TareaSimple>();
        public DbSet<TareaPrioritaria> TareasPrioritarias => Set<TareaPrioritaria>();
        public DbSet<TareaRecurrente> TareasRecurrentes => Set<TareaRecurrente>();

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

                //Relacion 1 : N usuario-tarea
                entity.HasMany(usuario => usuario.Tareas)
                .WithOne(tarea => tarea.Usuario)
                .HasForeignKey(tarea => tarea.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(tarea => tarea.Id);

                entity.Property(tarea => tarea.Titulo)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(tarea => tarea.Descripcion)
                    .HasMaxLength(300);

                entity.Property(tarea => tarea.FechaCreacion)
                    .IsRequired();

                entity.Property(tarea => tarea.FechaLimite)
                    .IsRequired();

                entity.Property(tarea => tarea.Prioridad)
                    .IsRequired();

                entity.Property(tarea => tarea.UsuarioId)
                    .IsRequired();

                entity.Property<EstadoTarea>("_estado")
                    .HasColumnName("Estado")
                    .IsRequired();

                entity.Property<string?>("_motivoCancelacion")
                    .HasColumnName("MotivoCancelacion")
                    .HasMaxLength(300);

                entity.HasDiscriminator<string>("TipoTarea")
                    .HasValue<TareaSimple>("Simple")
                    .HasValue<TareaPrioritaria>("Prioritaria")
                    .HasValue<TareaRecurrente>("Recurrente");
            });

            //Backing fields
            modelBuilder.Entity<TareaPrioritaria>(entity =>
            {
                entity.Property(tarea => tarea.NivelUrgencia)
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