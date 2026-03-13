using EbWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Infrastructure;

public class BudgetDbContext : DbContext
{
    public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SchedaBudget> SchedeBudget { get; set; }
    public virtual DbSet<Abilitazione> Abilitazioni { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SchedaBudget>(entity =>
        {
            entity.ToTable("Elenco_Schede_Budget", schema: "PDF");
            entity.HasNoKey();
        });

        modelBuilder.Entity<Abilitazione>(entity =>
        {
            entity.ToTable("Abilitazioni", schema: "PDF");
            entity.HasNoKey();
        });
    }
}