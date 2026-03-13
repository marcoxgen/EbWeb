using EbWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Infrastructure;

public class MifidDbContext : DbContext
{
    public MifidDbContext(DbContextOptions<IstruttoriaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AbilitatoMifid> AbilitatiMifid { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbilitatoMifid>(entity =>
        {
            entity.ToTable("Abilitati_Mifid", schema: "Anag");
            entity.HasNoKey();
        });
    }
}