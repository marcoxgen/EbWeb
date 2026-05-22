using EbWeb.Models.AbilitazioniMifid.ViewModels;

namespace EbWeb.Models.AbilitazioniMifid.Services.Application;

public interface IExcelExportService
{
    byte[] GenerateAbilitazioniExcel(IEnumerable<AbilitazioneMifidDetailViewModel> viewModel);
}