using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels;

public class SchedaBudgetListViewModel
{
    public ListViewModel<SchedaBudgetViewModel> SchedeBudget { get; set; }
    public SchedaBudgetListInputModel Input { get; set; }
    public List<string> EtichetteData { get; set; } = new List<string>();
}