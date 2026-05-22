using EbWeb.Models.AlimentazioneBudget.InputModels;

namespace EbWeb.Models.AlimentazioneBudget.ViewModels;

public class PubblicazioneBudgetListViewModel
{
    public ListViewModel<PubblicazioneBudgetViewModel> PubblicazioniBudget { get; set; } = new();
    public PubblicazioneBudgetListInputModel Input { get; set; } = default!;
}