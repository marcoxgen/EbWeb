using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels
{
    public class IstruttoriaListViewModel
    {
        public ListViewModel<IstruttoriaViewModel> Istruttorie { get; set; } = new();
        public IstruttoriaListInputModel Input { get; set; } = default!;
    }
}