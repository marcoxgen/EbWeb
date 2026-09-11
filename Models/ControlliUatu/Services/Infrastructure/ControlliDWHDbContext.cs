using Microsoft.EntityFrameworkCore;
using EbWeb.Models.ControlliUatu.Entities;

namespace EbWeb.Models.ControlliUatu.Services.Infrastructure;

public class ControlliDWHDbContext : DbContext
{
    public ControlliDWHDbContext(DbContextOptions<ControlliDWHDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Source> Sources { get; set; }
    public virtual DbSet<Azione> Azioni { get; set; }
    public virtual DbSet<ColonnaEccezione> ColonneEccezioni  { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Source>(entity =>
        {
            entity.ToTable("Source", schema: "Thanos");
            entity.HasKey(e => e.IdSource);
        });

        modelBuilder.Entity<Azione>(entity =>
        {
            entity.ToTable("Azioni", schema: "Thanos");
            entity.HasKey(e => e.IdAzione);
            entity.Property(e => e.IdAzione).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ColonnaEccezione>(entity =>
        {
            entity.ToTable("ColonneEccezioni", schema: "Thanos");
            entity.HasNoKey();
        });
    }
}