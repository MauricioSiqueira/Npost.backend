using Microsoft.EntityFrameworkCore;
using npost.Core.Auth.Model;
using npost.Models;

namespace npost.Data;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }
    
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Notation> Notations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("PostgreSQL:GeneratedOption", "C.utf8");
        modelBuilder.HasPostgresExtension("unaccent");
        
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName(EnumConstraints.pkUsuario.ToString());
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Notation>(entity =>
        {
            entity.HasKey(e => e.NotationId).HasName(EnumConstraints.pkNotation.ToString());
            
            entity.HasOne(d => d.User)
                .WithMany(p => p.Notations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName(EnumConstraints.fkNotationUsuario.ToString());
        });

        // Forçar todas as colunas String para não-unicode ou seja Varchar e não Nvarchar
        // desde que nenhum tipo de coluna seja definido explicitamente com HasColumnType.
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(
                         p => p.ClrType == typeof(string)    // Tipo String
                              && p.GetColumnType() == null           // Não definido HasColumnType
                     ))
        {
            property.SetIsUnicode(false);
        }

        OnModelCreatingPartial(modelBuilder);
    }
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
