using EbWeb.Models.Entities;
using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels;

public class RichiestaPerfezionamentoListViewModel
{
    public ListViewModel<RichiestaPerfezionamentoViewModel> RichiestePerfezionamento { get; set; }
    public RichiestaPerfezionamentoListInputModel Input { get; set; }
}