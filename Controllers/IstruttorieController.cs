using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Application;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Controllers;

public class IstruttorieController : Controller
{
    private readonly IIstruttoriaService istruttoriaService;
    public IstruttorieController(IIstruttoriaService istruttoriaService)
    {
        this.istruttoriaService = istruttoriaService;
    }

    public async Task<IActionResult> Index(IstruttoriaListInputModel input)
    {
        ViewData["Title"] = "Cruscotto Istruttoria";
        ListViewModel<IstruttoriaViewModel> istruttorie = await istruttoriaService.GetIstruttorieAsync(input);
        IstruttoriaListViewModel viewModel = new IstruttoriaListViewModel
        {
            Istruttorie = istruttorie,
            Input = input
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign([FromForm] long numeroPratica)
    {
        string nomeIstruttore = await istruttoriaService.AssegnaPraticaAsync(numeroPratica);

        return new JsonResult(new { success = true, istruttore = nomeIstruttore });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Revoke([FromForm] long numeroPratica)
    {
        await istruttoriaService.RevocaPraticaAsync(numeroPratica);
        return new JsonResult(new { success = true, istruttore = "" });
    }
}