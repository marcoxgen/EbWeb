using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

public interface IExcelExportService
{
    byte[] GenerateAbilitazioniExcel(IEnumerable<AbilitazioneMifidDetailViewModel> viewModel);
}