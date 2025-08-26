using EbWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Infrastructure;

public partial class IstruttoriaDbContext : DbContext
{
    public IstruttoriaDbContext()
    {
    }

    public IstruttoriaDbContext(DbContextOptions<IstruttoriaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Istruttoria> Istruttorie { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Istruttoria>(entity =>
        {
            entity.ToTable("Cluster_Istruttoria", schema: "IST"); //Superfluo se la tabella si chiama come la proprietà che espone il DbSet
            entity.HasNoKey();
            
            #region Mapping generato automaticamente dal tool di reverse engineering
            /*
            entity
                .HasNoKey()
                .ToTable("Cluster_Istruttoria", "IST");

            entity.HasIndex(e => e.NumeroPratica, "INDEX_Cluster").IsClustered();

            entity.Property(e => e.ClusterPratica)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Cluster_Pratica");
            entity.Property(e => e.DataInserimentoPratica).HasColumnName("Data_Inserimento_Pratica");
            entity.Property(e => e.DataUltimoGoBack).HasColumnName("Data_Ultimo_GoBack");
            entity.Property(e => e.DataUltimoTrasferimento).HasColumnName("Data_Ultimo_Trasferimento");
            entity.Property(e => e.EliminaCode).HasColumnName("Elimina_Code");
            entity.Property(e => e.Gestore)
                .HasMaxLength(99)
                .IsUnicode(false);
            entity.Property(e => e.GradoRischioCod)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Grado_Rischio_Cod");
            entity.Property(e => e.Intestazione)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Istruttore)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Note)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.NoteEscalationIndicatoriBilancio)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("Note_escalation_indicatori_bilancio");
            entity.Property(e => e.NoteNuovoQuestionarioEsg)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("Note_nuovo_questionario_ESG");
            entity.Property(e => e.NumeroPratica).HasColumnName("Numero_Pratica");
            entity.Property(e => e.OrganoDeliberanteCod)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Organo_Deliberante_Cod");
            entity.Property(e => e.ProfiloAntiriciclaggio)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Profilo_Antiriciclaggio");
            entity.Property(e => e.StatoPraticaCod)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Stato_Pratica_Cod");
            entity.Property(e => e.TipoControparteCod)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Tipo_Controparte_Cod");
            entity.Property(e => e.TipoIstruttoriaCod)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Tipo_Istruttoria_Cod");
            entity.Property(e => e.UltimoCodGoBack)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Ultimo_Cod_GoBack");
                */
            #endregion
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
