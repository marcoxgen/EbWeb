using EbWeb.Models.Common.ViewModels;
using EbWeb.Models.AlimentazioneBudget.ViewModels;

namespace EbWeb.Models.AlimentazioneBudget.ViewModels;

public class AzionePubblicazioneListViewModel
{
    public int IdPubblicazione { get; set; }
    public ListViewModel<AzionePubblicazioneViewModel> Azioni { get; set; } = new();
}