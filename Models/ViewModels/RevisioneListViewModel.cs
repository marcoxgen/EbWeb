using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels;

public class RevisioneListViewModel
{
    public ListViewModel<RevisioneViewModel> Revisioni { get; set; } = new();
    public RevisioneListInputModel Input { get; set; } = default!;
}