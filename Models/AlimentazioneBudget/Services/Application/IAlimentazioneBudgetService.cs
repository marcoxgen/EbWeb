using EbWeb.Models.AlimentazioneBudget.InputModels;
using EbWeb.Models.AlimentazioneBudget.ViewModels;
using Microsoft.Data.SqlClient;
using static EbWeb.Models.AlimentazioneBudget.Services.Application.AdoNetEsecutoreComandiService;

namespace EbWeb.Models.AlimentazioneBudget.Services.Application;

public interface IAlimentazioneBudgetService
{
    Task<ListViewModel<PubblicazioneBudgetViewModel>> GetPubblicazioniBudgetAsync(PubblicazioneBudgetListInputModel model);
    Task<AzionePubblicazioneListViewModel> GetAzioniPubblicazioneIdAsync(int id);
    Task<List<TipoPubblicazioneLookupViewModel>> GetTipiPubblicazioneLookupAsync();
    Task<List<CalendarioDinamicoLookupViewModel>> GetCalendariDinamiciLookupAsync(string TtipoCod);
    Task<AzionePubblicazioneViewModel> GetAzionePubblicazioneAsync(int id);
    Task SalvaStatoAzioneAsync(ActionSaveInputModel input);
    Task<bool> ElaboraNuovaPubblicazioneAsync(string tipoCod, DateOnly dataRiferimento);
}