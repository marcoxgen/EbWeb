using Microsoft.EntityFrameworkCore;
using EbWeb.Models.AlimentazioneBudget.Entities;

namespace EbWeb.Models.AlimentazioneBudget.Services.Infrastructure;

public class AlimentazioneBudgetDbContext : DbContext
{
    public AlimentazioneBudgetDbContext(DbContextOptions<AlimentazioneBudgetDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pubblicazione> Pubblicazioni { get; set; }
    public virtual DbSet<TipoPubblicazione> TipiPubblicazione { get; set; }
    public virtual DbSet<CalendarioDinamico> CalendariDinamici { get; set; }
    public virtual DbSet<Azione> Azioni { get; set; }
    public virtual DbSet<AzionePubblicazione> AzioniPubblicazione { get; set; }
    public virtual DbSet<TaskPubblicazione> TasksPubblicazione { get; set; }
    public virtual DbSet<TemplateTask> TemplateTasks { get; set; }
    public virtual DbSet<DipendenzeTask> DipendenzeTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pubblicazione>(entity =>
        {
            entity.ToTable("Pubblicazioni", schema: "dbo");
            entity.HasKey(e => e.Id_Pubblicazione);
        });

        modelBuilder.Entity<TipoPubblicazione>(entity =>
        {
            entity.ToTable("Tipo_Pubblicazione", schema: "Anag");
            entity.HasNoKey();
        });

        modelBuilder.Entity<CalendarioDinamico>(entity =>
        {
            entity.ToTable("Calendario_Dinamico", schema: "Anag");
            entity.HasNoKey();
        });

        modelBuilder.Entity<AzionePubblicazione>(entity =>
        {
            entity.ToTable("Azioni_Pubblicazioni", schema: "dbo");
            entity.HasKey(a => a.Id_Azione);
        });

        modelBuilder.Entity<Azione>(entity =>
        {
            entity.ToTable("Azioni", schema: "dbo");
            entity.HasKey(e => e.Id_Azione);
        });

        modelBuilder.Entity<TaskPubblicazione>(entity =>
        {
            entity.ToTable("Task_Pubblicazione", schema: "dbo");
            entity.HasNoKey();
        });

        modelBuilder.Entity<TemplateTask>(entity =>
        {
            entity.ToTable("Task", schema: "dbo");
            entity.HasNoKey();
        });

        modelBuilder.Entity<DipendenzeTask>(entity =>
        {
            entity.ToTable("Dipendenze_Task", schema: "dbo");
            entity.HasNoKey();
        });
    }
}
