using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Application;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Controllers;

public class SchedeBudgetController : Controller
{
    private readonly ISchedaBudgetService schedaBudgetService;
    public SchedeBudgetController(ISchedaBudgetService schedaBudgetService)
    {
        this.schedaBudgetService = schedaBudgetService;
    }

    [HttpGet("api/pdf/generate")]
    public async Task<IActionResult> GeneratePdfFromPowerBi([FromQuery] SchedaBudgetListInputModel input)
    {
        var schedeBudget = await schedaBudgetService.GetSchedeBudgetAsync(input);
        byte[] pdfBytes = PdfUtils.GeneraTabellaBudgetPdf(schedeBudget.Results);

        string nomeFile = $"Budget_{input.EtichettaSelezionata?.Replace("/", "-") ?? "Generico"}.pdf";
        return File(pdfBytes, "application/pdf", nomeFile);
    }

    public async Task<IActionResult> Index(SchedaBudgetListInputModel input)
    {
        ViewData["Title"] = "Schede Budget";

        var etichetteData = await PrepareInputModel(input);
        var schedeBudget = await schedaBudgetService.GetSchedeBudgetAsync(input);
        
        var viewModel = new SchedaBudgetListViewModel {
            SchedeBudget = schedeBudget,
            Input = input,
            EtichetteData = etichetteData
        };
        return View(viewModel);
    }

    public async Task<IActionResult> CreatePdf(SchedaBudgetListInputModel input)
    {
        await PrepareInputModel(input);

        var schedeBudget = await schedaBudgetService.GetSchedeBudgetAsync(input);

        byte[] pdfBytes = PdfUtils.GeneraTabellaBudgetPdf(schedeBudget.Results);

        string nomeFile = $"Budget_{input.EtichettaSelezionata?.Replace("/", "-")}.pdf";
        return File(pdfBytes, "application/pdf", nomeFile);
    }

    private async Task<List<string>> PrepareInputModel(SchedaBudgetListInputModel input)
    {
        List<string> etichetteData = await schedaBudgetService.GetEtichetteDataAsync();
        
        if (string.IsNullOrEmpty(input.EtichettaSelezionata) && etichetteData.Any())
        {
            input.EtichettaSelezionata = etichetteData.Last().Trim();
        }
        else if (!string.IsNullOrEmpty(input.EtichettaSelezionata))
        {
            input.EtichettaSelezionata = input.EtichettaSelezionata.Trim();
        }

        return etichetteData;
    }
}