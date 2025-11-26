using EbWeb.Models.InputModels;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

public interface IAgendaStipulaService
{
    Task<ListViewModel<AgendaStipulaViewModel>> GetAgendaStipuleAsync(AgendaStipulaListInputModel model);
    Task<string> AssegnaStipulaAsync(int idRichiesta);
    Task RevocaStipulaAsync(int idRichiesta);
}