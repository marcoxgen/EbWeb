using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EbWeb.Models.Exceptions;
using EbWeb.Models.Exceptions.Application;

namespace MyCourse.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            switch(feature.Error)
            {
                case AnomaliaNagDuplicateException exc:
                    ViewData["Title"] = "Nag duplicato";
                    return View("AnomaliaDuplicate");

                default:
                    ViewData["Title"] = "Errore";
                    return View();
            }
        }
    }
}