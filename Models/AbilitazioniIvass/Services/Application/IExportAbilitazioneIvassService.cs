using EbWeb.Models.AbilitazioniIvass.ViewModels;

namespace EbWeb.Models.AbilitazioniIvass.Services.Application;

public interface IExportAbilitazioneIvassService
{
    byte[] GenerateAbilitazioniExcel(IEnumerable<AbilitazioneIvassDetailViewModel> viewModel);
}