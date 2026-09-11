using EbWeb.Models.ControlliUatu.InputModels;
using EbWeb.Models.ControlliUatu.ViewModels;

namespace EbWeb.Models.ControlliUatu.Services.Application;

public interface IControlloUatuService
{
    Task<List<ControlloUatuViewModel>> GetControlliUatuAsync();
    Task RefreshView();
    Task<DateTime?> GetLastRefreshAsync();
    Task<AnomaliaListViewModel> GetAnomalieAsync(AnomaliaListInputModel model);
    Task<EsitoAzioneViewModel> AggiornaAzioneAsync(AggiornaAzioneInputModel azione);
    Task<List<AnomaliaViewModel>> GetStoricoAzioniAsync(int idSource, string chiaveHash);
    Task<EccezioneViewModel> GetEccezioneAsync(int idSource, string chiaveHash);
    Task SalvaEccezioneAsync(IFormCollection form);
}