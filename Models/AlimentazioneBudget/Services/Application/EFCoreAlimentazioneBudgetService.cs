using System.Data;
using Microsoft.EntityFrameworkCore;
using EbWeb.Models.AlimentazioneBudget.Entities;
using EbWeb.Models.AlimentazioneBudget.InputModels;
using EbWeb.Models.AlimentazioneBudget.Services.Infrastructure;
using EbWeb.Models.AlimentazioneBudget.ViewModels;

namespace EbWeb.Models.AlimentazioneBudget.Services.Application;

public class EFCoreAlimentazioneBudgetService : IAlimentazioneBudgetService
{
    private readonly AlimentazioneBudgetDbContext _dbContext;
    private readonly IEsecutoreComandiService _esecutoreComandiService;
    public EFCoreAlimentazioneBudgetService(AlimentazioneBudgetDbContext dbContext, IEsecutoreComandiService esecutoreComandiService)
    {
        _dbContext = dbContext;
        _esecutoreComandiService = esecutoreComandiService;
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
                DataUltimaLavorazione = p.Data_Ultima_Lavorazione,
                Note = p.Note
            })
            .ToListAsync();

        return new ListViewModel<PubblicazioneBudgetViewModel>
        {
            Results = results,
            TotalCount = results.Count
        };
    }

    public async Task<AzionePubblicazioneListViewModel> GetAzioniPubblicazioneIdAsync(int idPubblicazione)
    {
        var pubblicazione = await (from p in _dbContext.Pubblicazioni
            join tp in _dbContext.TipiPubblicazione
            on p.Tipo_Pubblicazione_Cod equals tp.Tipo_Pubblicazione_Cod
            where p.Id_Pubblicazione == idPubblicazione
            select new { p, tp })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        var azioni = await _dbContext.AzioniPubblicazione
            .Where(a => a.Id_Pubblicazione == idPubblicazione)
            .OrderBy(a => a.Ordine)
            .ToListAsync();

        return new AzionePubblicazioneListViewModel
        {
            IdPubblicazione = idPubblicazione,
            TipoPubblicazioneCod = pubblicazione.p.Tipo_Pubblicazione_Cod,
            TipoPubblicazioneDes = pubblicazione.tp.Tipo_Pubblicazione_Des,
            DataRiferimento = pubblicazione.p.Data_Riferimento,
            Azioni = new ListViewModel<AzionePubblicazioneViewModel>
            {
                TotalCount = azioni.Count,
                Results = azioni.Select(e => AzionePubblicazioneViewModel.FromEntity(e)).ToList()
            }
        };
    }

    public async Task<AzionePubblicazioneViewModel> GetAzionePubblicazioneAsync(int idAzione)
    {
        var entity = await _dbContext.AzioniPubblicazione
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id_Azione == idAzione);

        if (entity == null)
            return null;

        return AzionePubblicazioneViewModel.FromEntity(entity);
    }

    public async Task<bool> CreatePubblicazioneAsync(char tipoCod, DateOnly dataRiferimento)
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
            .GroupJoin(
                _dbContext.DipendenzeTasks,
                tp => tp.Id_Task,
                dt => dt.Id_Task_Successore,
                (tp, dtGroup) => new
                {
                    Id_Task = tp.Id_Task,
                    Flag_Esecuzione = !dtGroup.Any()
                }
            )
            .ToListAsync();

        foreach (var task in tasksPubblicazione)
        {
            var azione = new Azione
            {
                Id_Pubblicazione = pubblicazione.Id_Pubblicazione,
                Id_Task = task.Id_Task,
                Flag_Esecuzione = task.Flag_Esecuzione
            };

            _dbContext.Azioni.Add(azione);
        }

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletePubblicazioneAsync(int idPubblicazione)
    {
        try
        {
            int righeCoinvolte = await _dbContext.Pubblicazioni
                .Where(p => p.Id_Pubblicazione == idPubblicazione)
                .ExecuteDeleteAsync();

            return righeCoinvolte > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Errore durante la cancellazione della pubblicazione ID {idPubblicazione}.", ex);
        }
    }

    public async Task<SqlExecutionResult> EseguiAzioneAsync(int idAzione)
    {
        var risultato = new SqlExecutionResult();

        var datiVista = await _dbContext.AzioniPubblicazione
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id_Azione == idAzione);

        if (datiVista == null)
        {
            throw new KeyNotFoundException($"Dati dell'azione con ID {idAzione} non trovata nella vista.");
        }

        var azioneTabella = await _dbContext.Azioni
            .FirstOrDefaultAsync(a => a.Id_Azione == idAzione);

        if (azioneTabella == null)
        {
            throw new KeyNotFoundException($"Impossibile aggiornare lo stato: record non trovato nella tabella Azioni.");
        }

        try
        {
            string comandoFormattato = datiVista.Comando;
            if (!string.IsNullOrWhiteSpace(comandoFormattato))
            {
                string dataRifFormattata = datiVista.DataRif.ToString("yyyy-MM-dd") ?? "NULL";
                string tipoPubFormattata = datiVista.TipoPub.ToString() ?? "";

                comandoFormattato = comandoFormattato
                    .Replace("@DataRif", $"'{dataRifFormattata}'")
                    .Replace("@TipoPub", $"'{tipoPubFormattata}'");
            }

            var sqlResult = await _esecutoreComandiService.EseguiComandoDinamicoAsync(datiVista.Nome_Database, comandoFormattato);

            azioneTabella.Messaggio = sqlResult.HasMessages ? sqlResult.MessagesText : null;
            azioneTabella.Data_Esecuzione = DateTime.Now;

            risultato.Messaggio = sqlResult.MessagesText;

            if (sqlResult.HasResults)
            {
                var primaTabella = sqlResult.ResultSets[0];

                var righeConvertite = ConvertiDataTableInLista(primaTabella);

                azioneTabella.Risultati = System.Text.Json.JsonSerializer.Serialize(righeConvertite);

                risultato.Colonne = primaTabella.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
                risultato.Righe = righeConvertite;
            }
            else
            {
                azioneTabella.Risultati = null;

                risultato.Colonne = new List<string>();
                risultato.Righe = new List<Dictionary<string, object>>();
            }

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            azioneTabella.Esito = false;
            azioneTabella.Messaggio = $"[ERRORE SERVIZIO] {ex.Message}";
            azioneTabella.Data_Esecuzione = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            risultato.Esito = false;
            risultato.Messaggio = $"[ERRORE SERVIZIO] {ex.Message}";
            risultato.Colonne = new List<string>();
            risultato.Righe = new List<Dictionary<string, object>>();
        }

        return risultato;
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


    private List<Dictionary<string, object>> ConvertiDataTableInLista(DataTable dt)
    {
        var list = new List<Dictionary<string, object>>();
        foreach (DataRow row in dt.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
            }
            list.Add(dict);
        }
        return list;
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

    public async Task<List<CalendarioDinamicoLookupViewModel>> GetCalendariDinamiciLookupAsync(char tipoCod)
    {
        var query = _dbContext.CalendariDinamici.AsNoTracking();

        if (tipoCod != default)
        {
            query = query.Where(c => c.Tipo_Pubblicazione_Cod == tipoCod);
        }

        query = query.Where(c => !_dbContext.Pubblicazioni
            .Any(p => p.Data_Riferimento == c.Data_Riferimento_Dati &&
                      p.Tipo_Pubblicazione_Cod == c.Tipo_Pubblicazione_Cod));

        var dati = await query
            .OrderBy(c => c.Data_Riferimento_Dati)
            .ToListAsync();

        return dati.Select(c => new CalendarioDinamicoLookupViewModel
        {
            Value = c.Data_Riferimento_Dati.ToString("yyyy-MM-dd"),
            Text = c.Data_Riferimento_Dati.ToString("dd/MM/yyyy"),
            TipoCod = c.Tipo_Pubblicazione_Cod
        })
        .ToList();
    }
}