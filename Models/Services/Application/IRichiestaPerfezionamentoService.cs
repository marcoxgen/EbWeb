using EbWeb.Models.InputModels;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

public interface IRichiestaPerfezionamentoService
{
    Task<ListViewModel<RichiestaPerfezionamentoViewModel>> GetRichiestePerfezionamentoAsync(RichiestaPerfezionamentoListInputModel model);
    Task<string> AssegnaRichiestaAsync(int idRichiesta, int assegnatarioIndex);
    Task RevocaRichiestaAsync(int idRichiesta, int assegnatarioIndex);
}