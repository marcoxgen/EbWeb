using EbWeb.Models.AbilitazioniMifid.InputModels;

namespace EbWeb.Models.AbilitazioniMifid.ViewModels;

public class AbilitazioneMifidListViewModel
{
    public ListViewModel<AbilitazioneMifidViewModel> AbilitazioniMifid { get; set; } = new();
    public AbilitazioneMifidListInputModel Input { get; set; } = default!;
}
