namespace EbWeb.Models.ControlliUatu.Entities;

public class ColonnaEccezione
{
    public int IdColonnaEccezione { get; set; }
    public int IdSource { get; set; }
    public int Posizione { get; set; }
    public string NomeColonna { get; set; }
    public string TipoDato { get; set; }
    public string? ColonnaOrigine { get; set; }
}