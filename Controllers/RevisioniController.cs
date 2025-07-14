using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Application;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace EbWeb.Controllers
{
    public class RevisioniController : Controller
    {
        private readonly IRevisioneService revisioneService;

        public RevisioniController(IRevisioneService revisioneService)
        {
            this.revisioneService = revisioneService;
        }

        public async Task<IActionResult> Index(RevisioneListInputModel input)
        {
            ViewData["Title"] = "Elenco Revisioni";
            ListViewModel<RevisioneViewModel> revisioni = await revisioneService.GetRevisioniAsync(input);
            RevisioneListViewModel viewModel = new RevisioneListViewModel
            {
                Revisioni = revisioni,
                Input = input
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Detail(int nag)
        {
            ViewData["Title"] = "Dettaglio Revisione";
            List<RevisioneViewModel> revisione = await revisioneService.GetRevisioneAsync(nag);
            return View(revisione);
        }

        public async Task<IActionResult> Edit(int nag, string nomeColonna)
        {
            ViewData["Title"] = "Modifica Note Istruttore";
            var revisione = (await revisioneService.GetRevisioneAsync(nag)).FirstOrDefault();
            if (revisione == null)
                return NotFound();
            return View(revisione);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        public async Task<IActionResult> CreatePdf(int nag)
        {
            List<RevisioneViewModel> revisione = await revisioneService.GetRevisioneAsync(nag);

            var pdfLegenda = PdfUtils.CreaLegendaPdf(revisione);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "Legenda report.pdf");
            var pdfEsistente = System.IO.File.ReadAllBytes(path);

            var pdfUnito = PdfUtils.UnisciPdf(pdfLegenda, pdfEsistente);

            return File(pdfUnito, "application/pdf", $"dettaglio_indicatori_{nag}.pdf");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNote(int id, string noteIstruttore)
        {
            int updated = await revisioneService.EditNoteAsync(id, noteIstruttore ?? "");

            if (updated > 0)
                return Ok(new { message = "Nota aggiornata correttamente" });
            else
                return NotFound("Revisione non trovata");
        }
    }
}