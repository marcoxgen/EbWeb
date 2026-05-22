using DocumentFormat.OpenXml.InkML;
using EbWeb.Models.AlimentazioneBudget.Entities;
using EbWeb.Models.AlimentazioneBudget.InputModels;
using EbWeb.Models.AlimentazioneBudget.Services.Infrastructure;
using EbWeb.Models.AlimentazioneBudget.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.AlimentazioneBudget.Services.Application;

public class EFCoreAlimentazioneBudgetService : IAlimentazioneBudgetService
{
    private readonly AlimentazioneBudgetDbContext _dbContext;

    public EFCoreAlimentazioneBudgetService(AlimentazioneBudgetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ListViewModel<PubblicazioneBudgetViewModel>> GetPubblicazioniBudgetAsync(PubblicazioneBudgetListInputModel model)
    {
        var results = await (from p in _dbContext.Pubblicazioni
                             join t in _dbContext.TipiPubblicazione
                                 on p.Tipo_Pubblicazione_Cod equals t.Tipo_Pubblicazione_Cod
                             select new PubblicazioneBudgetViewModel
                             {
                                 IdPubblicazione = p.Id_Pubblicazione,
                                 TipoPubblicazioneCod = p.Tipo_Pubblicazione_Cod,
                                 TipoPubblicazioneDes = t.Tipo_Pubblicazione_Des,
                                 DataRiferimento = p.Data_Riferimento,
                                 FlagStato = p.Flag_Stato,
                                 DataUltimaLavorazione = p.Data_Ultima_Lavorazione
                             })
            .ToListAsync();

        return new ListViewModel<PubblicazioneBudgetViewModel>
        {
            Results = results,
            TotalCount = results.Count
        };
    }

    public async Task<AzionePubblicazioneListViewModel> GetAzioniPubblicazioneIdAsync(int id)
    {
        var entities = await _dbContext.AzioniPubblicazione
            .Where(a => a.Id_Pubblicazione == id)
            .OrderBy(a => a.Ordine)
            .ToListAsync();

        return new AzionePubblicazioneListViewModel
        {
            IdPubblicazione = id,
            Azioni = new ListViewModel<AzionePubblicazioneViewModel>
            {
                TotalCount = entities.Count,

                Results = entities.Select(e => AzionePubblicazioneViewModel.FromEntity(e)).ToList()
            }
        };
    }

    public async Task<List<TipoPubblicazioneLookupViewModel>> GetTipiPubblicazioneLookupAsync()
    {
        return await _dbContext.TipiPubblicazione
            .AsNoTracking()
            .OrderBy(t => t.Tipo_Pubblicazione_Des)
            .Select(t => new TipoPubblicazioneLookupViewModel
            {
                Value = t.Tipo_Pubblicazione_Cod,
                Text = t.Tipo_Pubblicazione_Des
            })
            .ToListAsync();
    }

    public async Task<List<CalendarioDinamicoLookupViewModel>> GetCalendariDinamiciLookupAsync(string tipoCod)
    {
        var query = _dbContext.CalendariDinamici.AsNoTracking();

        // Filtra solo se tipoCod è valorizzato
        if (!string.IsNullOrEmpty(tipoCod))
        {
            query = query.Where(c => c.Tipo_Pubblicazione_Cod == tipoCod);
        }

        return await query
            .OrderByDescending(c => c.Data_Riferimento_Dati)
            .Select(c => new CalendarioDinamicoLookupViewModel
            {
                Value = c.Data_Riferimento_Dati.ToString("yyyy-MM-dd"),
                Text = c.Data_Riferimento_Dati.ToString("dd/MM/yyyy"),
                TipoCod = c.Tipo_Pubblicazione_Cod
            })
            .ToListAsync();
    }

    public async Task<AzionePubblicazioneViewModel> GetAzionePubblicazioneAsync(int id)
    {
        var entity = await _dbContext.AzioniPubblicazione
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id_Azione == id);

        if (entity == null)
            return null;

        return AzionePubblicazioneViewModel.FromEntity(entity);
    }

    public async Task SalvaStatoAzioneAsync(ActionSaveInputModel input)
    {
        var azione = await _dbContext.AzioniPubblicazione
            .FirstOrDefaultAsync(a => a.Id_Azione == input.IdAzione);

        if (azione == null)
        {
            throw new KeyNotFoundException($"Azione con ID {input.IdAzione} non trovata.");
        }

        azione.Note = input.Note;
        azione.Esito = input.Esito;
        azione.Messaggio = input.Messaggio;

        if (!string.IsNullOrWhiteSpace(input.Risultati))
        {
            azione.Risultati = input.Risultati;
        }

        if (input.Esito && azione.Data_Esecuzione == null)
        {
            azione.Data_Esecuzione = DateTime.Now;
        }
        else if (!input.Esito)
        {
            azione.Data_Esecuzione = DateTime.Now;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ElaboraNuovaPubblicazioneAsync(string tipoCod, DateOnly dataRiferimento)
    {
        if (await _dbContext.Pubblicazioni
            .AnyAsync(p => p.Tipo_Pubblicazione_Cod == tipoCod && p.Data_Riferimento == dataRiferimento))
        {
            return false;
        }

        var pubblicazione = new Pubblicazione
        {
            Tipo_Pubblicazione_Cod = tipoCod,
            Data_Riferimento = dataRiferimento,
            Flag_Stato = false,
            Data_Ultima_Lavorazione = DateTime.Now,
        };

        _dbContext.Pubblicazioni.Add(pubblicazione);

        await _dbContext.SaveChangesAsync();

        var tasksPubblicazione = await _dbContext.TasksPubblicazione
            .Where(tp => tp.Tipo_Pubblicazione_Cod == tipoCod)
            .Join(_dbContext.TemplateTasks,
                  tp => tp.Id_Task,
                  t => t.Id_Task,
                  (tp, t) => new { tp, t })
            .OrderBy(joined => joined.t.Ordine)
            .Select(joined => new Azione
            {
                Id_Pubblicazione = pubblicazione.Id_Pubblicazione,
                Id_Task = joined.tp.Id_Task
            })
            .ToListAsync();

        foreach (var task in tasksPubblicazione)
        {
            var azione = new Azione
            {
                Id_Pubblicazione = pubblicazione.Id_Pubblicazione,
                Id_Task = task.Id_Task
            };

            _dbContext.Azioni.Add(azione);
        }

        await _dbContext.SaveChangesAsync();

        return true;
    }
}
