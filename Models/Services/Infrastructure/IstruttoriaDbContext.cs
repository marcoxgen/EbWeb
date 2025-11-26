using EbWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Infrastructure;

public class IstruttoriaDbContext : DbContext
{
    public IstruttoriaDbContext(DbContextOptions<IstruttoriaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Istruttoria> Istruttorie { get; set; }
    public virtual DbSet<Assegna_Pratica> Assegna_Pratiche { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Istruttoria>(entity =>
        {
            entity.ToTable("Cluster_Istruttoria", schema: "IST");
            entity.HasKey(e => e.Numero_Pratica);
        });

        modelBuilder.Entity<Assegna_Pratica>(entity =>
        {
            entity.ToTable("Assegna_Pratica", schema: "Input");
            entity.HasKey(e => e.Numero_Pratica);
        });
    }
}