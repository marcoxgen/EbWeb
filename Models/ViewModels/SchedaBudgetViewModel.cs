namespace EbWeb.Models.ViewModels;

public class SchedaBudgetViewModel
{
    public string? EtichettaData { get; set; }
    public int ID_UO { get; set; }
    public string? EtichettaUO { get; set; }
    public string? TipoUtenteDes { get; set; }
    public string? Ordinamento { get; set; }
    public string? MetricaDes { get; set; }
    public string? TipologiaDes { get; set; }
    public DateOnly? DataRif { get; set; }
    public double? ValoreFineAnnoPrecedente { get; set; }
    public double? Rettifiche { get; set; }
    public double? ValoreAnnoPrecedenteRettificato { get; set; }
    public double? ValoreStessoPeriodoAnnoPrecedenteRettificato { get; set; }
    public double? ValoreAllaDataRif { get; set; }
    public double? FlussoAllaData { get; set; }
    public double? ObiettivoAnnuale { get; set; }
    public double? ValoreDaRaggiungereAnnuale { get; set; }
    public string? PercentualeRaggiungimentoObiettivoAnnuale { get; set; }
    public double? PercentualeRaggiungimentoObiettivoAnnualeIstogramma { get; set; }
    public double? ObiettivoMensile { get; set; }
    public double? ValoreDaRaggiungereMensile { get; set; }
    public string? PercentualeRaggiungimentoObiettivoMensile { get; set; }
    public short? Punteggio { get; set; }
    public byte? AdObiettivo { get; set; }
 }