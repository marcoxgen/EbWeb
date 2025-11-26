using EbWeb.Models.Entities;
using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Infrastructure;
using EbWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Application;

public class EFCoreIstruttoriaService : IIstruttoriaService
{
    private readonly IstruttoriaDbContext _dbContext;
    private readonly IUserService _userService;

    public EFCoreIstruttoriaService(IstruttoriaDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<ListViewModel<IstruttoriaViewModel>> GetIstruttorieAsync(IstruttoriaListInputModel model)
    {
        var baseQuery =
            from i in _dbContext.Istruttorie
            join a in _dbContext.Assegna_Pratiche
                on i.Numero_Pratica equals a.Numero_Pratica into gj
            from a in gj.DefaultIfEmpty()
            select new
            {
                Istruttoria = i,
                IstruttoreEffettivo = a != null
                    ? (string.IsNullOrEmpty(a.Istruttore) ? "" : a.Istruttore)
                    : i.Istruttore
            };

        if (model.Nag.HasValue)
            baseQuery = baseQuery.Where(x => x.Istruttoria.Nag == model.Nag.Value);

        int totalCount = await baseQuery.CountAsync();

        var results = await baseQuery
            .AsNoTracking()
            .Skip(model.Offset)
            .Take(model.Limit)
            .Select(x => new IstruttoriaViewModel
            {
                Nag = x.Istruttoria.Nag,
                Intestazione = x.Istruttoria.Intestazione,
                NumeroPratica = x.Istruttoria.Numero_Pratica,
                ClusterPratica = x.Istruttoria.Cluster_Pratica,
                EliminaCode = x.Istruttoria.Elimina_Code,
                Gestore = x.Istruttoria.Gestore,
                Rating = x.Istruttoria.Rating,
                Istruttore = x.IstruttoreEffettivo,
                Note = x.Istruttoria.Note,
                NoteEscalationIndicatoriBilancio = x.Istruttoria.Note_escalation_indicatori_bilancio
            })
            .ToListAsync();

        return new ListViewModel<IstruttoriaViewModel>
        {
            Results = results,
            TotalCount = totalCount
        };
    }

    public async Task<string> AssegnaPraticaAsync(long numeroPratica)
    {
        var nomeIstruttore = _userService.GetDisplayName().ToUpperInvariant();
        var record = await _dbContext.Assegna_Pratiche
            .FirstOrDefaultAsync(x => x.Numero_Pratica == numeroPratica);

        if (record != null)
        {
            // Se esiste già, aggiorna l'istruttore (anche se è già valorizzato)
            record.Istruttore = nomeIstruttore;
            _dbContext.Assegna_Pratiche.Update(record);
        }
        else
        {
            // Se non esiste, crea un nuovo record
            var nuovaAssegnazione = new Assegna_Pratica
            {
                Numero_Pratica = numeroPratica,
                Istruttore = nomeIstruttore,
                Data = DateTime.Now
            };
            _dbContext.Assegna_Pratiche.Add(nuovaAssegnazione);
        }

        await _dbContext.SaveChangesAsync();
        return nomeIstruttore;
    }
    
    public async Task RevocaPraticaAsync(long numeroPratica)
    {
        var record = await _dbContext.Assegna_Pratiche
            .FirstOrDefaultAsync(x => x.Numero_Pratica == numeroPratica);

        if (record != null)
        {
            // Se esiste: aggiorna mettendo la stringa vuota
            record.Istruttore = null;
            _dbContext.Assegna_Pratiche.Update(record);
        }
        else
        {
            // Se non esiste: crea un nuovo record
            var nuovoRecord = new Assegna_Pratica
            {
                Numero_Pratica = numeroPratica,
                Istruttore = null,
                Data = DateTime.Now
            };
            _dbContext.Assegna_Pratiche.Add(nuovoRecord);
        }

        await _dbContext.SaveChangesAsync();
    }
}