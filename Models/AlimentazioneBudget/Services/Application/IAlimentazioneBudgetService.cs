using EbWeb.Models.AlimentazioneBudget.InputModels;
using EbWeb.Models.AlimentazioneBudget.Services.Infrastructure;
using EbWeb.Models.AlimentazioneBudget.ViewModels;

namespace EbWeb.Models.AlimentazioneBudget.Services.Application;

public interface IAlimentazioneBudgetService
{
    Task<ListViewModel<PubblicazioneBudgetViewModel>> GetPubblicazioniBudgetAsync(PubblicazioneBudgetListInputModel model);
    Task<AzionePubblicazioneListViewModel> GetAzioniPubblicazioneIdAsync(int idPubblicazione);
    Task<AzionePubblicazioneViewModel> GetAzionePubblicazioneAsync(int idAzione);
    Task<bool> CreatePubblicazioneAsync(char tipoCod, DateOnly dataRiferimento);
    Task<bool> DeletePubblicazioneAsync(int idPubblicazione);
    Task SalvaStatoAzioneAsync(ActionSaveInputModel input);
    Task<SqlExecutionResult> EseguiAzioneAsync(int idAzione);
    Task<List<TipoPubblicazioneLookupViewModel>> GetTipiPubblicazioneLookupAsync();
    Task<List<CalendarioDinamicoLookupViewModel>> GetCalendariDinamiciLookupAsync(char tipoCod);
}