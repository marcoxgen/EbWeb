using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels;

public class AgendaStipulaListViewModel
{
    public ListViewModel<AgendaStipulaViewModel> AgendaStipule { get; set; }
    public AgendaStipulaListInputModel Input { get; set; }
}