using System;
using System.Collections.Generic;

namespace EbWeb.Models.Entities;

public partial class AnagAnagrafeGenerale
{
    public int Nag { get; set; }

    public string? TipoControparteCod { get; set; }

    public string? Intestazione { get; set; }

    public short? SettoreEconomicoCod { get; set; }

    public short? RamoEconomicoCod { get; set; }

    public short? ProfessioneCod { get; set; }

    public int? FilialeCod { get; set; }

    public string? PiazzaCod { get; set; }

    public byte? StatoRecordCod { get; set; }

    public DateOnly? DataCensimento { get; set; }

    public DateOnly? DataEstizione { get; set; }

    public string? ProfiloGianosCod { get; set; }
}
