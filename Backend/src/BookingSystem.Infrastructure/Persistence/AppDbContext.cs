using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Rol> Roles { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<UsuarioRol> UsuariosRoles { get; set; } = null!;
    public DbSet<Prestador> Prestadores { get; set; } = null!;
    public DbSet<Solicitante> Solicitantes { get; set; } = null!;
    public DbSet<SolicitantePrestador> SolicitantesPrestadores { get; set; } = null!;
    public DbSet<Cita> Citas { get; set; } = null!;
    public DbSet<Cupo> Cupos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Rol
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.Cod);
            entity.Property(e => e.Cod).HasColumnName("cod");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(50).IsRequired();
        });

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");
            entity.HasKey(e => e.Cod);
            entity.Property(e => e.Cod).HasColumnName("cod");
            entity.Property(e => e.Username).HasColumnName("usuario").HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Clave).HasColumnName("clave").HasMaxLength(255).IsRequired();
            entity.Property(e => e.RazonSocial).HasColumnName("razon_social").HasMaxLength(200).IsRequired();
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
        });

        // UsuarioRol
        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.ToTable("usuarios_roles");
            entity.HasKey(e => new { e.CodUsuario, e.CodRol });
            entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");
            entity.Property(e => e.CodRol).HasColumnName("cod_rol");
            entity.Property(e => e.FechaAsignacion).HasColumnName("fecha_asignacion").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.UsuariosRoles)
                  .HasForeignKey(e => e.CodUsuario);

            entity.HasOne(e => e.Rol)
                  .WithMany(r => r.UsuariosRoles)
                  .HasForeignKey(e => e.CodRol);
        });

        // Prestador
        modelBuilder.Entity<Prestador>(entity =>
        {
            entity.ToTable("prestadores");
            entity.HasKey(e => e.CodUsuario);
            entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");
            entity.Property(e => e.Especialidad).HasColumnName("especialidad").HasMaxLength(150);

            entity.HasOne(e => e.Usuario)
                  .WithOne(u => u.Prestador)
                  .HasForeignKey<Prestador>(e => e.CodUsuario);
        });

        // Solicitante
        modelBuilder.Entity<Solicitante>(entity =>
        {
            entity.ToTable("solicitantes");
            entity.HasKey(e => e.CodUsuario);
            entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");

            entity.HasOne(e => e.Usuario)
                  .WithOne(u => u.Solicitante)
                  .HasForeignKey<Solicitante>(e => e.CodUsuario);
        });

        // SolicitantePrestador
        modelBuilder.Entity<SolicitantePrestador>(entity =>
        {
            entity.ToTable("solicitantes_prestadores");
            entity.HasKey(e => new { e.CodUsuarioSolicitante, e.CodUsuarioPrestador });
            entity.Property(e => e.CodUsuarioSolicitante).HasColumnName("cod_usuario_solicitante");
            entity.Property(e => e.CodUsuarioPrestador).HasColumnName("cod_usuario_prestador");
            entity.Property(e => e.FechaSuscripcion).HasColumnName("fecha_suscripcion").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Solicitante)
                  .WithMany(s => s.Suscripciones)
                  .HasForeignKey(e => e.CodUsuarioSolicitante);

            entity.HasOne(e => e.Prestador)
                  .WithMany(p => p.Suscriptores)
                  .HasForeignKey(e => e.CodUsuarioPrestador);
        });

        // Cita
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.ToTable("citas");
            entity.HasKey(e => e.Cod);
            entity.Property(e => e.Cod).HasColumnName("cod");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Fecha).HasColumnName("fecha").IsRequired();
            entity.Property(e => e.CuposTotales).HasColumnName("cupos_totales").IsRequired();
            entity.Property(e => e.CuposDisponibles).HasColumnName("cupos_disponibles").IsRequired();
            entity.Property(e => e.CodUsuarioPrestador).HasColumnName("cod_usuario_prestador");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Activa).HasColumnName("activa").HasDefaultValue(true);

            entity.HasOne(e => e.Prestador)
                  .WithMany(p => p.Citas)
                  .HasForeignKey(e => e.CodUsuarioPrestador);
        });

        // Cupo
        modelBuilder.Entity<Cupo>(entity =>
        {
            entity.ToTable("cupos");
            entity.HasKey(e => new { e.CodCita, e.CodUsuarioSolicitante });
            entity.Property(e => e.CodCita).HasColumnName("cod_cita");
            entity.Property(e => e.CodUsuarioSolicitante).HasColumnName("cod_usuario_solicitante");
            entity.Property(e => e.FechaReserva).HasColumnName("fecha_reserva").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Cita)
                  .WithMany(c => c.Cupos)
                  .HasForeignKey(e => e.CodCita);

            entity.HasOne(e => e.Solicitante)
                  .WithMany(s => s.Cupos)
                  .HasForeignKey(e => e.CodUsuarioSolicitante);
        });
    }
}
