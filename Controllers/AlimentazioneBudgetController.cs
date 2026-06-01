using Microsoft.AspNetCore.Mvc;

using EbWeb.Models.AlimentazioneBudget.Services.Application;
using EbWeb.Models.AlimentazioneBudget.ViewModels;
using EbWeb.Models.AlimentazioneBudget.InputModels;

namespace EbWeb.Controllers;

public class AlimentazioneBudgetController : Controller
{
    private readonly IAlimentazioneBudgetService _alimentazioneBudgetService;
    public AlimentazioneBudgetController(IAlimentazioneBudgetService alimentazioneBudgetService)
    {
        _alimentazioneBudgetService = alimentazioneBudgetService;
    }

    public async Task<IActionResult> Index(PubblicazioneBudgetListInputModel input)
    {
        ViewData["Title"] = "Pubblicazioni Budget";

        var pubblicazioniBudget = await _alimentazioneBudgetService.GetPubblicazioniBudgetAsync(input);
        
        var viewModel = new PubblicazioneBudgetListViewModel {
            PubblicazioniBudget = pubblicazioniBudget,
            Input = input
        };
        return View(viewModel);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Nuova pubblicazione";
        var inputModel = new PubblicazioneBudgetCreateInputModel(); 
        return View(inputModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PubblicazioneBudgetCreateInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            bool creataNuova = await _alimentazioneBudgetService.CreatePubblicazioneAsync(model.TipoCod, model.DataRiferimento);

            if (creataNuova)
            {
                TempData["SuccessMessage"] = "Flusso budget generato con successo insieme a tutte le azioni.";
            }
            else
            {
                TempData["WarningMessage"] = "Attenzione: La pubblicazione per questo tipo e data esiste già a sistema.";
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Errore durante l'elaborazione: " + ex.Message);
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Azioni(int id)
    {
        ViewData["Title"] = "Azioni Pubblicazione";

        var azioniPubblicazione = await _alimentazioneBudgetService.GetAzioniPubblicazioneIdAsync(id);
        
        if (azioniPubblicazione == null) return NotFound();

        return View(azioniPubblicazione);
    }

    [HttpGet]
    public async Task<IActionResult> GetDettaglio(int id)
    {
        // Recupera l'azione dal service
        var azione = await _alimentazioneBudgetService.GetAzionePubblicazioneAsync(id);

        if (azione == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(azione.Risultati))
        {
            try
            {
                // Deserializza la stringa JSON in una lista di Dizionari
                var righeSalvate = System.Text.Json.JsonSerializer
                    .Deserialize<List<Dictionary<string, object>>>(azione.Risultati);

                if (righeSalvate != null && righeSalvate.Any())
                {
                    azione.TabellaRisultati = new JsonTabellaResult
                    {
                        // Memorizza i nomi delle colonne prendendo le chiavi del primo record
                        Colonne = righeSalvate.First().Keys.ToList(),
                        Righe = righeSalvate
                    };
                }
            }
            catch (System.Text.Json.JsonException)
            {
            }
        }

        // Restituisce la PartialView passandogli il modello completo
        return PartialView("_DettaglioAzione", azione);
    }

    [HttpPost]
    public async Task<IActionResult> EseguiAzione(int idAzione)
    {
        var viewModel = new AzionePubblicazioneViewModel
        {
            IdAzione = idAzione,
            DataEsecuzione = DateTime.Now
        };

        try
        {
            var risultato = await _alimentazioneBudgetService.EseguiAzioneAsync(idAzione);

            viewModel.Esito = risultato.Esito;
            viewModel.Messaggio = risultato.Messaggio;

            if (risultato.Righe != null && risultato.Righe.Any())
            {
                viewModel.TabellaRisultati = new JsonTabellaResult
                {
                    Colonne = risultato.Colonne,
                    Righe = risultato.Righe
                };

                // Genera la stringa JSON per il campo hidden tecnico richiesto dallo script
                viewModel.Risultati = System.Text.Json.JsonSerializer.Serialize(risultato.Righe);
            }
        }
        catch (Exception ex)
        {
            viewModel.Esito = false;
            viewModel.Messaggio = ex.Message;
        }

        // Restituisce la PartialView
        return PartialView("_PannelloRisultati", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SaveActionState(ActionSaveInputModel input)
    {
        if (input == null || input.IdAzione <= 0)
        {
            return Json(new { success = false, message = "Dati della richiesta non validi." });
        }

        try
        {
            // Delega al servizio che salva Note ed Esito
            await _alimentazioneBudgetService.SalvaStatoAzioneAsync(input);

            // Restituisce solo il successo
            return Json(new { success = true });
        }
        catch (KeyNotFoundException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Errore interno durante il salvataggio: {ex.Message}" });
        }
    }

    [HttpGet]
    public async Task<JsonResult> GetTipiPubblicazione()
    {
        var tipiPubblicazione = await _alimentazioneBudgetService.GetTipiPubblicazioneLookupAsync();
        
        return Json(tipiPubblicazione);
    }

    [HttpGet]
    public async Task<JsonResult> GetCalendariDinamici(char tipoCod)
    {
        var calendariDinamici = await _alimentazioneBudgetService.GetCalendariDinamiciLookupAsync(tipoCod);
        
        return Json(calendariDinamici);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePubblicazione(int idPubblicazione)
    {
        // Recupera il risultato del metodo
        bool esito = await _alimentazioneBudgetService.DeletePubblicazioneAsync(idPubblicazione);

        if (!esito)
        {
            TempData["MessaggioErrore"] = "La pubblicazione selezionata è inesistente o è già stata eliminata.";
        }
        else
        {
            TempData["MessaggioSuccesso"] = "Pubblicazione ed elenco azioni eliminate correttamente.";
        }

        return RedirectToAction(nameof(Index));
    }
}