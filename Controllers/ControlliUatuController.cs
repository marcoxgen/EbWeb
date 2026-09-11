using EbWeb.Models.ControlliUatu.InputModels;
using EbWeb.Models.ControlliUatu.ViewModels;
using EbWeb.Models.ControlliUatu.Services.Application;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Controllers;

public class ControlliUatuController : Controller
{
    private readonly IControlloUatuService _controlloUatuService;
    public ControlliUatuController(IControlloUatuService controlloUatuService)
    {
        _controlloUatuService = controlloUatuService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "UATU";

        var viewModel = await _controlloUatuService.GetControlliUatuAsync();

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetLastRefresh()
    {
        var lastRefresh = await _controlloUatuService.GetLastRefreshAsync();
        return Json(lastRefresh);
    }

    public async Task<IActionResult> Detail(AnomaliaListInputModel input)
    {
        AnomaliaListViewModel viewModel = await _controlloUatuService.GetAnomalieAsync(input);

        if (viewModel == null) return NotFound();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SaveActionState(AggiornaAzioneInputModel azione)
    {
        try
        {
            // 1. Il servizio salva/aggiorna il DB e restituisce l'oggetto con l'ID e la data aggiornata
            var risultato = await _controlloUatuService.AggiornaAzioneAsync(azione);

            return Json(new
            {
                success = true,
                idAzione = risultato.IdAzione,
                dataModifica = risultato.DataModificaFormatted
            });
        }
        catch (Exception ex)
        {
            // 2. In caso di errore, si restituisce success = false senza riferimenti a proprieta inesistenti
            return Json(new
            {
                success = false,
                message = "Errore durante il salvataggio: " + ex.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> StoricoAzioni(int idSource, string chiaveHash)
    {
        var model = await _controlloUatuService
            .GetStoricoAzioniAsync(idSource, chiaveHash);

        return PartialView("_StoricoAzioni", model);
    }

    [HttpGet]
    public async Task<IActionResult> Eccezione(int idSource, string chiaveHash)
    {
        var viewModel = await _controlloUatuService.GetEccezioneAsync(idSource, chiaveHash);

        if (viewModel == null)
            return NotFound();

        return PartialView("_EccezioneModal", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SalvaEccezione(IFormCollection form)
    {
        try
        {
            await _controlloUatuService.SalvaEccezioneAsync(form);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }
}