using Microsoft.AspNetCore.Mvc;

using EbWeb.Models.AlimentazioneBudget.Services.Application;
using EbWeb.Models.AlimentazioneBudget.ViewModels;
using EbWeb.Models.AlimentazioneBudget.InputModels;

namespace EbWeb.Controllers;

public class AlimentazioneBudgetController : Controller
{
    private readonly IAlimentazioneBudgetService alimentazioneBudgetService;
    private readonly IEsecutoreComandiService esecutoreComandiService;
    public AlimentazioneBudgetController(IAlimentazioneBudgetService alimentazioneBudgetService, IEsecutoreComandiService esecutoreComandiService)
    {
        this.alimentazioneBudgetService = alimentazioneBudgetService;
        this.esecutoreComandiService = esecutoreComandiService;
    }

    public async Task<IActionResult> Index(PubblicazioneBudgetListInputModel input)
    {
        ViewData["Title"] = "Pubblicazioni Budget";

        var pubblicazioniBudget = await alimentazioneBudgetService.GetPubblicazioniBudgetAsync(input);
        
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
            bool creataNuova = await alimentazioneBudgetService.ElaboraNuovaPubblicazioneAsync(model.TipoCod, model.DataRiferimento);

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
        ViewData["Title"] = "Azioni Pubblicazione Budget";

        var azioniPubblicazione = await alimentazioneBudgetService.GetAzioniPubblicazioneIdAsync(id);
        
        if (azioniPubblicazione == null) return NotFound();

        return View(azioniPubblicazione);
    }

    [HttpGet]
    public async Task<IActionResult> GetDettaglio(int id)
    {
        // Recupera l'azione dal service
        var azione = await alimentazioneBudgetService.GetAzionePubblicazioneAsync(id);

        if (azione == null) return NotFound();

        // =========================================================================
        // RIPRISTINO DATI PER LA TABELLA (Se ci sono risultati salvati in formato JSON)
        // =========================================================================
        if (!string.IsNullOrWhiteSpace(azione.Risultati))
        {
            try
            {
                // Tentiamo di deserializzare la stringa JSON in una lista di Dizionari
                var righeSalvate = System.Text.Json.JsonSerializer
                    .Deserialize<List<Dictionary<string, object>>>(azione.Risultati);

                if (righeSalvate != null && righeSalvate.Any())
                {
                    azione.TabellaRisultati = new JsonTabellaResult
                    {
                        // Estraiamo i nomi delle colonne prendendo le chiavi del primo record
                        Colonne = righeSalvate.First().Keys.ToList(),
                        Righe = righeSalvate
                    };
                }
            }
            catch (System.Text.Json.JsonException)
            {
                // Se la stringa dentro 'Risultati' non è un JSON valido (magari è un errore di testo),
                // non facciamo nulla. La View andrà in fallback mostrando il testo grezzo senza rompersi.
            }
        }

        // Restituisce la Partial View passandogli il modello finalmente completo
        return PartialView("_DettaglioAzione", azione);
    }

    [HttpPost]
    public async Task<IActionResult> EseguiAzione(string dbName, string sqlComando)
    {
        // Usiamo il ViewModel definitivo che hai condiviso tu
        var viewModel = new AzionePubblicazioneViewModel
        {
            NomeDatabase = dbName,
            Comando = sqlComando,
            DataEsecuzione = DateTime.Now
        };

        try
        {
            // 1. Il servizio esegue la SELECT normale e restituisce le DataTable
            var sqlResult = await esecutoreComandiService.EseguiComandoDinamicoAsync(dbName, sqlComando);

            viewModel.Esito = true;

            // 2. Gestione dei messaggi di testo (PRINT)
            if (sqlResult.HasMessages)
            {
                viewModel.Messaggio = string.Join(Environment.NewLine, sqlResult.Messages);
            }

            // 3. Gestione dei dati della query
            if (sqlResult.HasResults)
            {
                var dataTable = sqlResult.ResultSets.First();

                // Trasformiamo la DataTable in una lista di Dizionari in memoria
                var listaRighe = new List<Dictionary<string, object>>();
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    var riga = new Dictionary<string, object>();
                    foreach (System.Data.DataColumn col in dataTable.Columns)
                    {
                        riga[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                    }
                    listaRighe.Add(riga);
                }

                // A) Generiamo il JSON dai dati reali. 
                // Questa stringa va inserita nella proprietà 'Risultati' (che poi salverai in SQL)
                viewModel.Risultati = System.Text.Json.JsonSerializer.Serialize(listaRighe);

                // B) Popoliamo direttamente l'oggetto TabellaRisultati per il rendering immediato della View
                viewModel.TabellaRisultati = new JsonTabellaResult
                {
                    Colonne = dataTable.Columns.Cast<System.Data.DataColumn>().Select(c => c.ColumnName).ToList(),
                    Righe = listaRighe
                };
            }
        }
        catch (Exception ex)
        {
            viewModel.Esito = false;
            viewModel.Messaggio = ex.Message;
        }

        return PartialView("_PannelloRisultati", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SaveActionState(ActionSaveInputModel input)
    {
        // Validazione base dell'input (competenza del Controller)
        if (input == null || input.IdAzione <= 0)
        {
            return Json(new { success = false, message = "Dati della richiesta non validi." });
        }

        try
        {
            // Deleghiamo tutta la logica al servizio dedicato
            await alimentazioneBudgetService.SalvaStatoAzioneAsync(input);

            // Istanziamo al volo il ViewModel impostando DateTime.Now (l'istante del salvataggio)
            // per fargli sputare la stringa formattata (es. "0 min") tramite la tua proprietà get {}
            var vm = new AzionePubblicazioneViewModel
            {
                DataEsecuzione = DateTime.Now
            };

            // Restituiamo il successo insieme alla stringa calcolata in C#
            return Json(new { success = true, dataFriendly = vm.DataEsecuzioneFriendly });
        }
        catch (KeyNotFoundException ex)
        {
            // Gestione specifica se l'azione non esiste nel DB
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            // Logga l'eccezione a sistema qui (es. _logger.LogError...)
            return Json(new { success = false, message = $"Errore interno durante il salvataggio: {ex.Message}" });
        }
    }

    [HttpGet]
    public async Task<JsonResult> GetTipiPubblicazione()
    {
        var tipiPubblicazione = await alimentazioneBudgetService.GetTipiPubblicazioneLookupAsync();
        
        return Json(tipiPubblicazione);
    }

    [HttpGet]
    public async Task<JsonResult> GetCalendariDinamici(string tipoCod)
    {
        var calendariDinamici = await alimentazioneBudgetService.GetCalendariDinamiciLookupAsync(tipoCod);
        
        return Json(calendariDinamici);
    }
}