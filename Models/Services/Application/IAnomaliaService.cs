using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application
{
    public interface IAnomaliaService
    {
        Task<List<AnomaliaRegistrazioneViewModel>> GetAnomalieRegistrazioniAsync();
        Task ForzaAnomaliaAsync(int id);
    }
}