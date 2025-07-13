using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application
{
    public interface IAnomaliaService
    {
        Task<List<AnomaliaRegistrazioneViewModel>> GetAnomalieRegistrazioniAsync();
        Task<AnagraficaViewModel> GetAnagraficaAsync(int id);
        Task ForzaAnomaliaAsync(int id);
    }
}