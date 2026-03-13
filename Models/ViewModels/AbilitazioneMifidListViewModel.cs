using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels;

public class AbilitazioneMifidListViewModel
{
    public ListViewModel<AbilitazioneMifidViewModel> AbilitazioniMifid { get; set; }
    public AbilitazioneMifidListInputModel Input { get; set; }
}