using EbWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Infrastructure;

public class ThinsoftDbContext : DbContext
{
    public ThinsoftDbContext(DbContextOptions<ThinsoftDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgendaStipula> AgendaStipule { get; set; }
    public virtual DbSet<AssegnaAgendaStipula> AssegnaAgendaStipule { get; set; }
    public virtual DbSet<RichiestaPerfezionamento> RichiestaPerfezionamento { get; set; }
    public virtual DbSet<AssegnaPerfezionamento> AssegnaPerfezionamento { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgendaStipula>(entity =>
        {
            entity.ToTable("WF_619_Elenco_Richieste", schema: "WF");
            entity.HasNoKey();
        });

        modelBuilder.Entity<AssegnaAgendaStipula>(entity =>
        {
            entity.ToTable("Assegna_Agenda_Stipula", schema: "INPUT");
            entity.HasKey(e => e.Id_Richiesta);
        });

        modelBuilder.Entity<RichiestaPerfezionamento>(entity =>
        {
            entity.ToTable("WF_609_Elenco_Richieste", schema: "WF");
            entity.HasNoKey();
        });

        modelBuilder.Entity<AssegnaPerfezionamento>(entity =>
        {
            entity.ToTable("Assegna_Perfezionamento", schema: "INPUT");
            entity.HasKey(e => e.Id_Richiesta);
        });
    }
}