using Microsoft.EntityFrameworkCore;
using EbWeb.Models.ControlliThanos2.Entities;

namespace EbWeb.Models.ControlliThanos2.Services.Infrastructure;

public class ControlliDWHDbContext : DbContext
{
    public ControlliDWHDbContext(DbContextOptions<ControlliDWHDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Source> Sources { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Source>(entity =>
        {
            entity.ToTable("Source", schema: "Thanos");
            entity.HasKey(e => e.IdSource);
        });
    }
}
