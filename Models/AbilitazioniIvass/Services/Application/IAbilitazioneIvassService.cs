using EbWeb.Models.AbilitazioniIvass.InputModels;
using EbWeb.Models.AbilitazioniIvass.ViewModels;

namespace EbWeb.Models.AbilitazioniIvass.Services.Application;

public interface IAbilitazioneIvassService
{
    Task<ListViewModel<AbilitazioneIvassViewModel>> GetAbilitazioniIvassAsync(AbilitazioneIvassListInputModel model);
    Task<AbilitazioneIvassDetailViewModel> GetAbilitazioneIvassAsync(int id);
    Task<AbilitazioneIvassDetailViewModel> CreateAbilitazioneIvassAsync(AbilitazioneIvassCreateInputModel inputModel);
    Task<AbilitazioneIvassDetailViewModel> EditAbilitazioneIvassAsync(AbilitazioneIvassEditInputModel inputModel);
    Task<AbilitazioneIvassEditInputModel> GetAbilitazioneIvassForEditingAsync(int id);
    Task DeleteAbilitazioneIvassAsync(int id);
    Task<List<AnagDipendentiLookupViewModel>> GetAnagDipendentiLookupAsync();
    Task<IEnumerable<AbilitazioneIvassDetailViewModel>> GetAllAbilitazioniIvassAsync(DateTime? dataRiferimento);
}