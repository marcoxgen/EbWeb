namespace EbWeb.Models.ControlliUatu.ViewModels;

public class AnomaliaViewModel
{
    public long? IdAzione { get; set; }
    public int IdSource { get; set; }
    public string? Descrizione { get; set; }
    public string? ChiaveHash { get; set; }
    public string? Nota { get; set; }
    public bool? Risolta { get; set; }
    public bool? Attiva { get; set; }
    public DateTime? UltimaModifica { get; set; }
    public DateTime? DataDisattivazione { get; set; }
    public string? Utente { get; set; }
}