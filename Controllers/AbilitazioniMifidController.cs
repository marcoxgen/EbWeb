using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Application;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Exceptions;

namespace EbWeb.Controllers;

public class AbilitazioniMifidController : Controller
{
    private readonly IAbilitazioneMifidService abilitazioneMifidService;
    private readonly IExcelExportService excelExportService;
    public AbilitazioniMifidController(IAbilitazioneMifidService abilitazioneMifidService, IExcelExportService excelExportService)
    {
        this.abilitazioneMifidService = abilitazioneMifidService;
        this.excelExportService = excelExportService;
    }

    public async Task<IActionResult> Index(AbilitazioneMifidListInputModel input)
    {
        ViewData["Title"] = "Abilitazioni MiFID";
        ListViewModel<AbilitazioneMifidViewModel> abilitazioniMifid = await abilitazioneMifidService.GetAbilitazioniMifidAsync(input);
        AbilitazioneMifidListViewModel viewModel = new AbilitazioneMifidListViewModel
        {
            AbilitazioniMifid = abilitazioniMifid,
            Input = input
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Detail(int matricola)
    {
        AbilitazioneMifidDetailViewModel viewModel = await abilitazioneMifidService.GetAbilitazioneMifidAsync(matricola);
        ViewData["Title"] = "Dettaglio Abilitazione MiFID";
        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        List<AnagDipendentiLookupViewModel> anagDipendenti = await abilitazioneMifidService.GetAnagDipendentiLookupAsync();

        ViewData["Title"] = "Nuova abilitazione MiFID";
        var inputModel = new AbilitazioneMifidCreateInputModel();
        inputModel.AnagDipendentiLookup = anagDipendenti;
        return View(inputModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AbilitazioneMifidCreateInputModel inputModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                AbilitazioneMifidDetailViewModel abilitazioni = await abilitazioneMifidService.CreateAbilitazioneMifidAsync(inputModel);
                return RedirectToAction(nameof(Edit), new { matricola = abilitazioni.Matricola });
            }
            catch (AbilitazioneMifidMatricolaUnavailableException)
            {
                ModelState.AddModelError(nameof(AbilitazioneMifidDetailViewModel.Matricola), "Questa matricola è già presente");
            }
        }

        ViewData["Title"] = "Nuova abilitazione MiFID";
        return View(inputModel);
    }

    public async Task<IActionResult> Edit(int matricola)
    {
        ViewData["Title"] = "Modifica abilitazione MiFID";
        AbilitazioneMifidEditInputModel inputModel = await abilitazioneMifidService.GetAbilitazioneMifidForEditingAsync(matricola);
        return View(inputModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AbilitazioneMifidEditInputModel inputModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                AbilitazioneMifidDetailViewModel abilitazioneMifid = await abilitazioneMifidService.EditAbilitazioneMifidAsync(inputModel);
                TempData["ConfirmationMessage"] = "I dati sono stati salvati con successo";
                return RedirectToAction(nameof(Detail), new { matricola = inputModel.Matricola });
            }
            catch (AbilitazioneMifidMatricolaUnavailableException)
            {
                ModelState.AddModelError(nameof(AbilitazioneMifidDetailViewModel.Matricola), "Questa matricola è già presente");
            }
        }

        ViewData["Title"] = "Modifica abilitazione MiFID";
        return View(inputModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(AbilitazioneMifidDeleteInputModel inputModel)
    {
        await abilitazioneMifidService.DeleteAbilitazioneMifidAsync(inputModel);
        TempData["ConfirmationMessage"] = "L'abilitazione è stata eliminata";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ExportToExcel()
    {
        IEnumerable<AbilitazioneMifidDetailViewModel> dati = await abilitazioneMifidService.GetAllAbilitazioniMifidAsync();
        
        byte[] fileBytes = excelExportService.GenerateAbilitazioniExcel(dati);
        
        string nomeFile = $"Export_Abilitazioni_{DateTime.Now:yyyyMMdd}.xlsx";
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nomeFile);
    }

    [HttpGet]
    public async Task<JsonResult> GetAnagDipendenti()
    {
        var anagDipendenti = await abilitazioneMifidService.GetAnagDipendentiLookupAsync();

        return Json(anagDipendenti);
    }

    [HttpGet]
    public async Task<JsonResult> GetTitoliStudioMifid()
    {
        var titoliStudioMifid = await abilitazioneMifidService.GetTitoliStudioMifidLookupAsync();
        
        return Json(titoliStudioMifid);
    }
}