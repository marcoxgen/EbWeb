using Microsoft.AspNetCore.Mvc;
using EbWeb.Models.ControlliThanos2.Services.Application;

namespace EbWeb.Controllers;

public class ControlliThanos2Controller : Controller
{
    private readonly IControlloThanos2Service _controlloThanos2Service;
    public ControlliThanos2Controller(IControlloThanos2Service controlloThanos2Service)
    {
        _controlloThanos2Service = controlloThanos2Service;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Controlli Thanos²";

        var viewModel = await _controlloThanos2Service.GetControlliThanos2Async();

        return View(viewModel);
    }
}