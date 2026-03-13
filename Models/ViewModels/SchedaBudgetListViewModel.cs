using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels;

public class SchedaBudgetListViewModel
{
    public ListViewModel<SchedaBudgetViewModel> SchedeBudget { get; set; } = default!;
    public SchedaBudgetListInputModel Input { get; set; } = default!;
    public List<string> EtichetteData { get; set; } = new List<string>();
}