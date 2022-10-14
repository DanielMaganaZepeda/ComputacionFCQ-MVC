using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ComputacionFCQ_MVC.Models
{
    public partial class ComputacionFCQContext : DbContext
    {
        public ComputacionFCQContext()
        {
        }

        public ComputacionFCQContext(DbContextOptions<ComputacionFCQContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrador> Administradors { get; set; } = null!;
        public virtual DbSet<Carrera> Carreras { get; set; } = null!;
        public virtual DbSet<Computadora> Computadoras { get; set; } = null!;
        public virtual DbSet<Fecha> Fechas { get; set; } = null!;
        public virtual DbSet<Frecuencium> Frecuencia { get; set; } = null!;
        public virtual DbSet<Programa> Programas { get; set; } = null!;
        public virtual DbSet<Reporte> Reportes { get; set; } = null!;
        public virtual DbSet<Reservacion> Reservacions { get; set; } = null!;
        public virtual DbSet<Sala> Salas { get; set; } = null!;
        public virtual DbSet<Sesion> Sesions { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<VistaReservacione> VistaReservaciones { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-F7IH21V\\SQLEXPRESS ; Database=ComputacionFCQ ; Trusted_Connection=True;");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.ToTable("Administrador");

                entity.HasIndex(e => e.Id, "UQ__Administ__3213E83E9E982EC9")
                    .IsUnique();

                entity.HasIndex(e => e.Usuario, "UQ__Administ__9AFF8FC686693A37")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Contrasena)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("contrasena");

                entity.Property(e => e.CreadoPor).HasColumnName("creado_por");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creacion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usuario");

                entity.HasOne(d => d.CreadoPorNavigation)
                    .WithMany(p => p.InverseCreadoPorNavigation)
                    .HasForeignKey(d => d.CreadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Administr__cread__398D8EEE");
            });

            modelBuilder.Entity<Carrera>(entity =>
            {
                entity.ToTable("Carrera");

                entity.HasIndex(e => e.Id, "UQ__Carrera__3213E83ECEC216F5")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Computadora>(entity =>
            {
                entity.ToTable("Computadora");

                entity.HasIndex(e => e.Id, "UQ__Computad__3213E83ED08C5AD8")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Funcional)
                    .IsRequired()
                    .HasColumnName("funcional")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Numero).HasColumnName("numero");

                entity.Property(e => e.SalaId).HasColumnName("sala_id");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.Computadoras)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Computado__sala___49C3F6B7");
            });

            modelBuilder.Entity<Fecha>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Fecha");

                entity.Property(e => e.Fecha1)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Frecuencium>(entity =>
            {
                entity.HasIndex(e => e.Id, "UQ__Frecuenc__3213E83EBA231FE5")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Curso)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("curso");

                entity.Property(e => e.PeriodoFin)
                    .HasColumnType("datetime")
                    .HasColumnName("periodo_fin");

                entity.Property(e => e.PeriodoInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("periodo_inicio");
            });

            modelBuilder.Entity<Programa>(entity =>
            {
                entity.ToTable("Programa");

                entity.HasIndex(e => e.Id, "UQ__Programa__3213E83E1F592109")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Reporte>(entity =>
            {
                entity.ToTable("Reporte");

                entity.HasIndex(e => e.Id, "UQ__Reporte__3213E83E2308C587")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComputadoraId).HasColumnName("computadora_id");

                entity.Property(e => e.Detalle)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("detalle");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creacion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechaSolucion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_solucion");

                entity.HasOne(d => d.Computadora)
                    .WithMany(p => p.Reportes)
                    .HasForeignKey(d => d.ComputadoraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reporte_Computadora");
            });

            modelBuilder.Entity<Reservacion>(entity =>
            {
                entity.ToTable("Reservacion");

                entity.HasIndex(e => e.Id, "UQ__Reservac__3213E83E7DF78A09")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activa)
                    .HasColumnName("activa")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CantidadAlumnos).HasColumnName("cantidad_alumnos");

                entity.Property(e => e.Curso)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("curso");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_fin");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_inicio");

                entity.Property(e => e.FrecuenciaId).HasColumnName("frecuencia_id");

                entity.Property(e => e.ProgramaId).HasColumnName("programa_id");

                entity.Property(e => e.SalaId).HasColumnName("sala_id");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.HasOne(d => d.Frecuencia)
                    .WithMany(p => p.Reservacions)
                    .HasForeignKey(d => d.FrecuenciaId)
                    .HasConstraintName("FK_Reservacion_Frecuencia");

                entity.HasOne(d => d.Programa)
                    .WithMany(p => p.Reservacions)
                    .HasForeignKey(d => d.ProgramaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservaci__progr__59FA5E80");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.Reservacions)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservaci__sala___59063A47");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Reservacions)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservaci__usuar__5812160E");
            });

            modelBuilder.Entity<Sala>(entity =>
            {
                entity.ToTable("Sala");

                entity.HasIndex(e => e.Id, "UQ__Sala__3213E83EAC92874A")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.HasMany(d => d.Programas)
                    .WithMany(p => p.Salas)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProgramaSala",
                        l => l.HasOne<Programa>().WithMany().HasForeignKey("ProgramaId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__ProgramaS__progr__5DCAEF64"),
                        r => r.HasOne<Sala>().WithMany().HasForeignKey("SalaId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__ProgramaS__sala___5CD6CB2B"),
                        j =>
                        {
                            j.HasKey("SalaId", "ProgramaId").HasName("PK__Programa__0ECDDE323445B406");

                            j.ToTable("ProgramaSala");

                            j.IndexerProperty<int>("SalaId").HasColumnName("sala_id");

                            j.IndexerProperty<int>("ProgramaId").HasColumnName("programa_id");
                        });
            });

            modelBuilder.Entity<Sesion>(entity =>
            {
                entity.ToTable("Sesion");

                entity.HasIndex(e => e.Id, "UQ__Sesion__3213E83E888A90DB")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComputadoraId).HasColumnName("computadora_id");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_fin");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_inicio")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProgramaId).HasColumnName("programa_id");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.HasOne(d => d.Computadora)
                    .WithMany(p => p.Sesions)
                    .HasForeignKey(d => d.ComputadoraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sesion__computad__52593CB8");

                entity.HasOne(d => d.Programa)
                    .WithMany(p => p.Sesions)
                    .HasForeignKey(d => d.ProgramaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sesion__programa__534D60F1");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Sesions)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sesion__usuario___5165187F");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.HasIndex(e => e.Matricula, "UQ__Usuario__30962D159249D4CC")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "UQ__Usuario__3213E83E92AA433B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellidos");

                entity.Property(e => e.CarreraId).HasColumnName("carrera_id");

                entity.Property(e => e.Correo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.EsAlumno)
                    .HasColumnName("es_alumno")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Matricula)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("matricula");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.Carrera)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.CarreraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Usuario__carrera__412EB0B6");
            });

            modelBuilder.Entity<VistaReservacione>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VistaReservaciones");

                entity.Property(e => e.CantidadAlumnos).HasColumnName("cantidad_alumnos");

                entity.Property(e => e.Curso)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("curso");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_fin");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_inicio");

                entity.Property(e => e.FrecuenciaId).HasColumnName("frecuencia_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ProgramaId).HasColumnName("programa_id");

                entity.Property(e => e.SalaId).HasColumnName("sala_id");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
