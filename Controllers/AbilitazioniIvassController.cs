using EbWeb.Models.AbilitazioniIvass.InputModels;
using EbWeb.Models.AbilitazioniIvass.Services.Application;
using EbWeb.Models.AbilitazioniIvass.ViewModels;
using EbWeb.Models.AlimentazioneBudget.InputModels;
using EbWeb.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Controllers;

[Authorize(Policy = "IvassAccess")]
public class AbilitazioniIvassController : Controller
{
    private readonly IAbilitazioneIvassService _abilitazioneIvassService;
    private readonly IExportAbilitazioneIvassService _excelExportService;
    public AbilitazioniIvassController(IAbilitazioneIvassService abilitazioneIvassService, IExportAbilitazioneIvassService excelExportService)
    {
        _abilitazioneIvassService = abilitazioneIvassService;
        _excelExportService = excelExportService;
    }
        
    public async Task<IActionResult> Index(AbilitazioneIvassListInputModel input)
    {
        ViewData["Title"] = "Abilitazioni IVASS";
        ListViewModel<AbilitazioneIvassViewModel> abilitazioniIvass = await _abilitazioneIvassService.GetAbilitazioniIvassAsync(input);
        AbilitazioneIvassListViewModel viewModel = new()
        {
            AbilitazioniIvass = abilitazioniIvass,
            Input = input
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Detail(int id)
    {
        AbilitazioneIvassDetailViewModel viewModel = await _abilitazioneIvassService.GetAbilitazioneIvassAsync(id);
        ViewData["Title"] = "Dettaglio abilitazione IVASS";
        return View(viewModel);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Nuova abilitazione IVASS";
        var inputModel = new AbilitazioneIvassCreateInputModel();
        return View(inputModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AbilitazioneIvassCreateInputModel inputModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                AbilitazioneIvassDetailViewModel abilitazioni = await _abilitazioneIvassService.CreateAbilitazioneIvassAsync(inputModel);
                return RedirectToAction(
                    nameof(Edit),
                    new { id = abilitazioni.Id }
                );
            }
            catch (AbilitazioneIvassMatricolaUnavailableException)
            {
                ModelState.AddModelError(nameof(AbilitazioneIvassDetailViewModel.Matricola), "Questa matricola è già presente");
            }
        }

        ViewData["Title"] = "Nuova abilitazione IVASS";
        return View(inputModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Modifica abilitazione IVASS";
        AbilitazioneIvassEditInputModel inputModel = await _abilitazioneIvassService.GetAbilitazioneIvassForEditingAsync(id);
        return View(inputModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AbilitazioneIvassEditInputModel inputModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                AbilitazioneIvassDetailViewModel _abilitazioneIvass = await _abilitazioneIvassService.EditAbilitazioneIvassAsync(inputModel);
                TempData["ConfirmationMessage"] = "I dati sono stati salvati con successo";
                return RedirectToAction(nameof(Detail), new { id = inputModel.Id });
            }
            catch (AbilitazioneIvassMatricolaUnavailableException)
            {
                ModelState.AddModelError(nameof(AbilitazioneIvassDetailViewModel.Id), "Questo id è già presente");
            }
        }

        ViewData["Title"] = "Modifica abilitazione IVASS";
        return View(inputModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _abilitazioneIvassService.DeleteAbilitazioneIvassAsync(id);
        TempData["ConfirmationMessage"] = "L'abilitazione è stata eliminata";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ExportToExcel(DateTime dataRiferimento)
    {
        DateTime dataRifStorica = dataRiferimento.Date.AddDays(1).AddTicks(-1);

        IEnumerable<AbilitazioneIvassDetailViewModel> dati = await _abilitazioneIvassService.GetAllAbilitazioniIvassAsync(dataRifStorica);
        
        byte[] fileBytes = _excelExportService.GenerateAbilitazioniExcel(dati);
        
        string nomeFile = $"Export_Abilitazioni_{DateTime.Now:yyyyMMdd}.xlsx";
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nomeFile);
    }
    
    [HttpGet]
    public async Task<JsonResult> GetAnagDipendenti()
    {
        var anagDipendenti = await _abilitazioneIvassService.GetAnagDipendentiLookupAsync();

        return Json(anagDipendenti);
    }
}