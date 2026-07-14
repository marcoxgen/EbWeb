using EbWeb.Models.AbilitazioniIvass.InputModels;

namespace EbWeb.Models.AbilitazioniIvass.ViewModels;

public class AbilitazioneIvassListViewModel
{
    public ListViewModel<AbilitazioneIvassViewModel> AbilitazioniIvass { get; set; } = new();
    public AbilitazioneIvassListInputModel Input { get; set; } = default!;
}