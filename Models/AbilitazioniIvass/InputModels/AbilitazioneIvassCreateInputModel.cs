using EbWeb.Models.AbilitazioniIvass.ViewModels;

namespace EbWeb.Models.AbilitazioniIvass.InputModels;

public class AbilitazioneIvassCreateInputModel
{
    public int Matricola { get; set; }
    public List<AnagDipendentiLookupViewModel> AnagDipendentiLookup { get; set; } = new();
}   