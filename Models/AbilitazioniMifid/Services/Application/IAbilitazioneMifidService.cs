using EbWeb.Models.AbilitazioniMifid.InputModels;
using EbWeb.Models.AbilitazioniMifid.ViewModels;

namespace EbWeb.Models.AbilitazioniMifid.Services.Application;

public interface IAbilitazioneMifidService
{
    Task<ListViewModel<AbilitazioneMifidViewModel>> GetAbilitazioniMifidAsync(AbilitazioneMifidListInputModel model);
    Task<AbilitazioneMifidDetailViewModel> GetAbilitazioneMifidAsync(int matricola);
    Task<AbilitazioneMifidDetailViewModel> CreateAbilitazioneMifidAsync(AbilitazioneMifidCreateInputModel inputModel);
    Task<AbilitazioneMifidDetailViewModel> EditAbilitazioneMifidAsync(AbilitazioneMifidEditInputModel inputModel);
    Task<AbilitazioneMifidEditInputModel> GetAbilitazioneMifidForEditingAsync(int matricola);
    Task DeleteAbilitazioneMifidAsync(int matricola);
    Task<List<AnagDipendentiLookupViewModel>> GetAnagDipendentiLookupAsync();
    Task<IEnumerable<AbilitazioneMifidDetailViewModel>> GetAllAbilitazioniMifidAsync(DateTime? dataRiferimento);
    Task<List<SupervisoriLookupViewModel>> GetSupervisoriLookupAsync();
    Task<IEnumerable<SelectOptionsViewModel>> GetTitoliStudioMifidLookupAsync();
}