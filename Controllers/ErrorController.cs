using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EbWeb.Models.Exceptions.Application;

namespace EbWeb.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        
        if (feature?.Error is AnomaliaNagDuplicateException)
        {
            ViewData["Title"] = "Nag duplicato";
            return View("AnomaliaDuplicate");
        }

        ViewData["Title"] = "Errore";
        return View();
    }
}