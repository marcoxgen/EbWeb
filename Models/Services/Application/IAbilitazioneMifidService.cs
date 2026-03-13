using EbWeb.Models.InputModels;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

public interface IAbilitazioneMifidService
{
    Task<ListViewModel<AbilitazioneMifidViewModel>> GetAbilitazioniMifidAsync(AbilitazioneMifidListInputModel model);
    //Task<string> AssegnaRichiestaAsync(int idRichiesta, int assegnatarioIndex);
    //Task RevocaRichiestaAsync(int idRichiesta, int assegnatarioIndex);
}