using EbWeb.Models.InputModels;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application
{
    public interface IIstruttoriaService
    {
        Task<ListViewModel<IstruttoriaViewModel>> GetIstruttorieAsync(IstruttoriaListInputModel model);
    }
}