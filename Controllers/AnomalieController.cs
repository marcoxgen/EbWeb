using EbWeb.Models.Services.Application;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Controllers;


public class AnomalieController : Controller
{
    private readonly IAnomaliaService anomaliaService;
    public AnomalieController(IAnomaliaService anomaliaService)
    {
        this.anomaliaService = anomaliaService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Anomalie Registrazioni";
        List<AnomaliaRegistrazioneViewModel> anomaliaRegistrazione = await anomaliaService.GetAnomalieRegistrazioniAsync();
        return View(anomaliaRegistrazione);
    }

    [HttpPost]
    public IActionResult Forza(AnomaliaRegistrazioneViewModel viewModel)
    {
        TempData["Nag"] = viewModel.Nag;
        TempData["CodiceFiscale"] = viewModel.CodiceFiscale;
        TempData["Intestazione"] = viewModel.Intestazione;

        return RedirectToAction("ConfermaForza");
    }

    public IActionResult ConfermaForza()
    {
        ViewData["Title"] = "Anomalia";
        var viewModel = new AnomaliaRegistrazioneViewModel
        {
            Nag = Convert.ToInt32(TempData["Nag"] ?? 0),
            CodiceFiscale = TempData["CodiceFiscale"]?.ToString(),
            Intestazione = TempData["Intestazione"]?.ToString(),
        };
        
        return View(viewModel);
    }
    
    public async Task<IActionResult> EseguiForzatura(int id)
    {
        await anomaliaService.ForzaAnomaliaAsync(id);
        return RedirectToAction("Index");
    }

    public IActionResult AnnullaForzatura()
    {
        return RedirectToAction("Index");
    }
}