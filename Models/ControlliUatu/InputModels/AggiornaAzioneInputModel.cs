namespace EbWeb.Models.ControlliUatu.InputModels;

public class AggiornaAzioneInputModel
{
    public long? IdAzione { get; set; }
    public int IdSource { get; set; }
    public string Descrizione { get; set; } = string.Empty;
    public string ChiaveHash { get; set; } = string.Empty;
    public string? Nota { get; set; }
    public bool? Risolto { get; set; }
}