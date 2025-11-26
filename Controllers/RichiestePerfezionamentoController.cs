using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Application;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Controllers;

public class RichiestePerfezionamentoController : Controller
{
    private readonly IRichiestaPerfezionamentoService richiestaPerfezionamentoService;
    public RichiestePerfezionamentoController(IRichiestaPerfezionamentoService richiestaPerfezionamentoService)
    {
        this.richiestaPerfezionamentoService = richiestaPerfezionamentoService;
    }

    public async Task<IActionResult> Index(RichiestaPerfezionamentoListInputModel input)
    {
        ViewData["Title"] = "Richieste Perfezionamento";
        ListViewModel<RichiestaPerfezionamentoViewModel> richiestaPerfezionamento = await richiestaPerfezionamentoService.GetRichiestePerfezionamentoAsync(input);
        RichiestaPerfezionamentoListViewModel viewModel = new RichiestaPerfezionamentoListViewModel
        {
            RichiestePerfezionamento = richiestaPerfezionamento,
            Input = input
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign([FromForm] int idRichiesta, [FromForm] int assegnatarioIndex)
    {
        string nomeAssegnatorio = await richiestaPerfezionamentoService.AssegnaRichiestaAsync(idRichiesta, assegnatarioIndex);
        string propertyName = $"assegnatario{assegnatarioIndex}";
    
        dynamic result = new System.Dynamic.ExpandoObject();
        result.success = true;
        ((IDictionary<string, object>)result)[propertyName] = nomeAssegnatorio;

        return new JsonResult(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Revoke([FromForm] int idRichiesta, [FromForm] int assegnatarioIndex)
    {
        await richiestaPerfezionamentoService.RevocaRichiestaAsync(idRichiesta, assegnatarioIndex);
        string propertyName = $"assegnatario{assegnatarioIndex}";

        var responseObject = new Dictionary<string, object>
        {
            { "success", true },
            { propertyName, "" } 
        };
    
        return new JsonResult(responseObject);
    }
}