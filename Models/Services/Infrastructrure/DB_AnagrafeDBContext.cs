using System;
using System.Collections.Generic;
using EbWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Infrastructure;

public partial class DB_AnagrafeDbContext : DbContext
{
    public DB_AnagrafeDbContext(DbContextOptions<DB_AnagrafeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnagAnagrafeGenerale> AnagAnagrafeGenerales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnagAnagrafeGenerale>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Anag_Anagrafe_Generale");

            entity.HasIndex(e => e.Nag, "INDEX_Anag_Anagrafe_Generale").IsClustered();

            entity.Property(e => e.DataCensimento).HasColumnName("Data_Censimento");
            entity.Property(e => e.DataEstizione).HasColumnName("Data_Estizione");
            entity.Property(e => e.FilialeCod).HasColumnName("Filiale_Cod");
            entity.Property(e => e.Intestazione)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Nag).HasColumnName("NAG");
            entity.Property(e => e.PiazzaCod)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Piazza_Cod");
            entity.Property(e => e.ProfessioneCod).HasColumnName("Professione_Cod");
            entity.Property(e => e.ProfiloGianosCod)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Profilo_Gianos_Cod");
            entity.Property(e => e.RamoEconomicoCod).HasColumnName("Ramo_Economico_Cod");
            entity.Property(e => e.SettoreEconomicoCod).HasColumnName("Settore_Economico_Cod");
            entity.Property(e => e.StatoRecordCod).HasColumnName("Stato_Record_Cod");
            entity.Property(e => e.TipoControparteCod)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Tipo_Controparte_Cod");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
