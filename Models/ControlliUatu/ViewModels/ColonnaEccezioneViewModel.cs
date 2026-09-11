public class ColonnaEccezioneViewModel
{
    public int Posizione { get; set; }
    public string NomeColonna { get; set; } = string.Empty;
    public string TipoDato { get; set; } = string.Empty;
    public string? ColonnaOrigine { get; set; }
    public object? Valore { get; set; }
    public bool ReadOnly => !string.IsNullOrEmpty(ColonnaOrigine);
}