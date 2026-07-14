using EbWeb.Models.AbilitazioniIvass.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.AbilitazioniIvass.Services.Infrastructure;

public class IvassDbContext : DbContext
{
    public IvassDbContext(DbContextOptions<IvassDbContext> options) : base(options)
    {
    }

    public virtual DbSet<AbilitatoIvass> AbilitatiIvass { get; set; }
    public virtual DbSet<ElencoAbilitatoIvass> ElencoAbilitatiIvass { get; set; }
    public virtual DbSet<AnagDipendenti> AnagDipendenti { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbilitatoIvass>(entity =>
        {
            entity.ToTable("Abilitati_IVASS", schema: "Base", t => t.HasTrigger("trg_Audit_AbilitatiIvass"));
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ElencoAbilitatoIvass>(entity =>
        {
            entity.ToTable("Elenco_Abilitati_IVASS", schema: "Anag");
            entity.HasNoKey();
        });

        modelBuilder.Entity<AnagDipendenti>(entity =>
        {
            entity.ToTable("AnagDipendenti", schema: "Anag");
            entity.HasNoKey();
        });
    }
}