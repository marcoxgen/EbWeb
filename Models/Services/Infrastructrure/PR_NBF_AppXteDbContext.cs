using EbWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class PR_NBF_AppXteDbContext : DbContext
{
    public PR_NBF_AppXteDbContext(DbContextOptions<PR_NBF_AppXteDbContext> options) : base(options) { }

    public DbSet<AnomaliaRegistrazioneForzatura> AnomalieRegistrazioneForzature { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AnomaliaRegistrazioneForzatura>(entity =>
        {
            entity.HasNoKey();
            entity.ToTable("Anomalie_Registrazione_Forzature", schema: "Attr");

            // Colonne
            entity.Property(e => e.NAG).HasColumnName("NAG");
            entity.Property(e => e.Utente).HasColumnName("Utente").HasMaxLength(20);
            entity.Property(e => e.DataForzatura).HasColumnName("Data_Forzatura");
        });
    }
}
