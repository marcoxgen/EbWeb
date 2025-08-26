using EbWeb.Models.InputModels;

namespace EbWeb.Models.ViewModels
{
    public class IstruttoriaListViewModel
    {
        public ListViewModel<IstruttoriaViewModel> Istruttorie { get; set; }
        public IstruttoriaListInputModel Input { get; set; }
    }
}