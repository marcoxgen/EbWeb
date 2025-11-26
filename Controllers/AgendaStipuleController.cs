using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Application;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Controllers;

public class AgendaStipuleController : Controller
{
    private readonly IAgendaStipulaService agendaStipulaService;
    public AgendaStipuleController(IAgendaStipulaService agendaStipulaService)
    {
        this.agendaStipulaService = agendaStipulaService;
    }

    public async Task<IActionResult> Index(AgendaStipulaListInputModel input)
    {
        ViewData["Title"] = "Richieste Agenda Stipule";
        ListViewModel<AgendaStipulaViewModel> agendaStipule = await agendaStipulaService.GetAgendaStipuleAsync(input);
        AgendaStipulaListViewModel viewModel = new AgendaStipulaListViewModel
        {
            AgendaStipule = agendaStipule,
            Input = input
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign([FromForm] int idRichiesta)
    {
        string nomeAssegnatorio = await agendaStipulaService.AssegnaStipulaAsync(idRichiesta);

        return new JsonResult(new { success = true, assegnatario = nomeAssegnatorio });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Revoke([FromForm] int idRichiesta)
    {
        await agendaStipulaService.RevocaStipulaAsync(idRichiesta);
        return new JsonResult(new { success = true, assegnatario = "" });
    }
}
 