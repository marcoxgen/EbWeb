using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels;

public class RichiestaPerfezionamentoListViewModel
{
    public ListViewModel<RichiestaPerfezionamentoViewModel> RichiestePerfezionamento { get; set; } = new();
    public RichiestaPerfezionamentoListInputModel Input { get; set; } = default!;
}