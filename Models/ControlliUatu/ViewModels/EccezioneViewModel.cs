namespace EbWeb.Models.ControlliUatu.ViewModels;

public class EccezioneViewModel
{
    public int IdSource { get; set; }
    public string Eccezioni { get; set; } = string.Empty;    
    public List<ColonnaEccezioneViewModel> Colonne { get; set; } = [];
}