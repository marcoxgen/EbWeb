using EbWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Infrastructure;

public class MifidDbContext : DbContext
{
    public MifidDbContext(DbContextOptions<MifidDbContext> options) : base(options)
    {
    }

    public virtual DbSet<BaseAbilitatoMifid> BaseAbilitatiMifid { get; set; }
    public virtual DbSet<AnagAbilitatoMifid> AnagAbilitatiMifid { get; set; }
    public virtual DbSet<AnagDipendenti> AnagDipendenti { get; set; }
    public virtual DbSet<Supervisori> Supervisori { get; set; }
    public virtual DbSet<TitoloStudioMifid> TitoliSudioMifid  { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseAbilitatoMifid>(entity =>
        {
            entity.ToTable("Abilitati_Mifid", schema: "Base", t => t.HasTrigger("trg_Audit_AbilitatiMifid"));
            entity.HasKey(e => e.Matricola);
        });

        modelBuilder.Entity<AnagAbilitatoMifid>(entity =>
        {
            entity.ToTable("Abilitati_Mifid", schema: "Anag");
            entity.HasNoKey();
        });

        modelBuilder.Entity<AnagDipendenti>(entity =>
        {
            entity.ToTable("AnagDipendenti", schema: "Anag");
            entity.HasNoKey();
        });

        modelBuilder.Entity<Supervisori>(entity =>
        {
            entity.ToTable("Supervisori", schema: "Anag");
            entity.HasNoKey();
        });

        modelBuilder.Entity<TitoloStudioMifid>(entity =>
        {
            entity.ToTable("Titolo_di_studio_Mifid", schema: "Deco");
            entity.HasNoKey();
        });
    }
}