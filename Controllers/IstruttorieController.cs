using EbWeb.Models.Entities;
using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Application;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace EbWeb.Controllers
{
    public class IstruttorieController : Controller
    {
        private readonly IIstruttoriaService istruttoriaService;

        public IstruttorieController(IIstruttoriaService istruttoriaService)
        {
            this.istruttoriaService = istruttoriaService;
        }

        public async Task<IActionResult> Index(IstruttoriaListInputModel input)
        {
            ViewData["Title"] = "Istruttorie";
            ListViewModel<IstruttoriaViewModel> istruttorie = await istruttoriaService.GetIstruttorieAsync(input);
            IstruttoriaListViewModel viewModel = new IstruttoriaListViewModel
            {
                Istruttorie = istruttorie,
                Input = input
            };
            return View(viewModel);
        }

    }
}