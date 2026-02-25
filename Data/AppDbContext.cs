using ActividadApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActividadApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Users { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Accion> Acciones { get; set; }
    public DbSet<Solucion> Soluciones { get; set; }
    public DbSet<Actividad> Actividades { get; set; }
    public DbSet<ConfirmacionPlanAccion> ConfirmacionesPlanAccion { get; set; }
    public DbSet<Eficacia> Eficacias { get; set; }
    public DbSet<Estado> Estados { get; set; }

    public DbSet<Maestro> Maestros { get; set; }
    public DbSet<Proceso> Procesos { get; set; }
    public DbSet<ResponsableProceso> ResponsablesProceso { get; set; }
    public DbSet<SeguimientoSgi> SeguimientosSgi { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Rol (1) -> (*) Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Organizacion)
                .WithMany()
                .HasForeignKey(u => u.OrganizacionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Cargo)
                .WithMany()
                .HasForeignKey(u => u.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Agencia)
                .WithMany()
                .HasForeignKey(u => u.AgenciaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Accion -> Usuario (UsuarioId), Usuario (ResponsableId), Maestro FKs
        modelBuilder.Entity<Accion>(entity =>
        {
            entity.HasIndex(e => e.Id);

            entity.HasOne(a => a.Usuario)
                .WithMany(u => u.Acciones)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(a => a.ResponsableId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Maestro>()
                .WithMany()
                .HasForeignKey(a => a.OrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Maestro>()
                .WithMany()
                .HasForeignKey(a => a.EntidadId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Maestro>()
                .WithMany()
                .HasForeignKey(a => a.SistemaGestionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Maestro>()
                .WithMany()
                .HasForeignKey(a => a.OrganizacionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Maestro>()
                .WithMany()
                .HasForeignKey(a => a.SitioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Proceso)
                .WithMany()
                .HasForeignKey(a => a.ProcesoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Estado)
                .WithMany()
                .HasForeignKey(a => a.EstadoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Accion (1) -> (1) Solucion
        modelBuilder.Entity<Solucion>(entity =>
        {
            entity.HasOne<Accion>()
                .WithOne(a => a.Solucion)
                .HasForeignKey<Solucion>(s => s.AccionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.UsuarioInvestiga)
                .WithMany()
                .HasForeignKey(s => s.UsuarioInvestigaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.UsuarioCoordina)
                .WithMany()
                .HasForeignKey(s => s.UsuarioCoordinaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Solucion (1) -> (*) Actividad
        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.HasOne<Solucion>()
                .WithMany(s => s.Actividades)
                .HasForeignKey(a => a.SolucionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.Responsable)
                .WithMany()
                .HasForeignKey(a => a.ResponsableId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Accion (1) -> (1) ConfirmacionPlanAccion
        modelBuilder.Entity<ConfirmacionPlanAccion>(entity =>
        {
            entity.HasOne<Accion>()
                .WithOne(a => a.ConfirmacionPlanAccion)
                .HasForeignKey<ConfirmacionPlanAccion>(c => c.AccionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Responsable)
                .WithMany()
                .HasForeignKey(c => c.ResponsableId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Accion (1) -> (1) Eficacia
        modelBuilder.Entity<Eficacia>(entity =>
        {
            entity.HasOne<Accion>()
                .WithOne(a => a.Eficacia)
                .HasForeignKey<Eficacia>(e => e.AccionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Accion (1) -> (*) SeguimientoSgi
        modelBuilder.Entity<SeguimientoSgi>(entity =>
        {
            entity.HasOne<Accion>()
                .WithMany(a => a.SeguimientosSgi)
                .HasForeignKey(s => s.AccionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Usuario)
                .WithMany()
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Proceso (1) ? (*) Accion
        modelBuilder.Entity<Proceso>(entity =>
        {
            entity.HasOne(p => p.Responsable)
                .WithMany()
                .HasForeignKey(p => p.ResponsableId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Maestro: enum -> string conversion
        modelBuilder.Entity<Maestro>(entity =>
        {
            entity.Property(m => m.Tipo).HasConversion<string>();
        });
    }
}
