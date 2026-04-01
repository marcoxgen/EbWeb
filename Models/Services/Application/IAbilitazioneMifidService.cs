using EbWeb.Models.InputModels;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

public interface IAbilitazioneMifidService
{
    Task<ListViewModel<AbilitazioneMifidViewModel>> GetAbilitazioniMifidAsync(AbilitazioneMifidListInputModel model);
    Task<AbilitazioneMifidDetailViewModel> GetAbilitazioneMifidAsync(int matricola);
    Task<AbilitazioneMifidDetailViewModel> CreateAbilitazioneMifidAsync(AbilitazioneMifidCreateInputModel inputModel);
    Task<AbilitazioneMifidDetailViewModel> EditAbilitazioneMifidAsync(AbilitazioneMifidEditInputModel inputModel);
    Task<AbilitazioneMifidEditInputModel> GetAbilitazioneMifidForEditingAsync(int matricola);
    Task DeleteAbilitazioneMifidAsync(AbilitazioneMifidDeleteInputModel inputModel);
    Task<List<AnagDipendentiLookupViewModel>> GetAnagDipendentiLookupAsync();
    Task<IEnumerable<AbilitazioneMifidDetailViewModel>> GetAllAbilitazioniMifidAsync();
    Task<List<SupervisoriLookupViewModel>> GetSupervisoriLookupAsync();
    Task<IEnumerable<SelectOptionsViewModel>> GetTitoliStudioMifidLookupAsync();
}