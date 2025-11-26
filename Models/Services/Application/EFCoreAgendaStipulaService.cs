using EbWeb.Models.Entities;
using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Infrastructure;
using EbWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Application;

public class EFCoreAgendaStipulaService : IAgendaStipulaService
{
    private readonly ThinsoftDbContext _dbContext;
    private readonly IUserService _userService;

    public EFCoreAgendaStipulaService(ThinsoftDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<ListViewModel<AgendaStipulaViewModel>> GetAgendaStipuleAsync(AgendaStipulaListInputModel model)
    {
        var baseQuery =
            from s in _dbContext.AgendaStipule
            join a in _dbContext.AssegnaAgendaStipule
                on s.Id_Richiesta equals a.Id_Richiesta into gj
            from a in gj.DefaultIfEmpty()
            select new
            {
                Stipula = s,
                AssegnatarioEffettivo = a != null
                    ? (string.IsNullOrEmpty(a.Assegnatario) ? "" : a.Assegnatario)
                    : ""
            };

        if (model.Id_Richiesta.HasValue)
            baseQuery = baseQuery.Where(x => x.Stipula.Id_Richiesta == model.Id_Richiesta.Value);

        int totalCount = await baseQuery.CountAsync();

        var results = await baseQuery
            .AsNoTracking()
            .Skip(model.Offset)
            .Take(model.Limit)
            .Select(x => new AgendaStipulaViewModel
            {
                IdRichiesta = x.Stipula.Id_Richiesta,
                DataRichiesta = x.Stipula.Data_Richiesta,
                Filiale = x.Stipula.Filiale,
                Nag = x.Stipula.Nag,
                Intestazione = x.Stipula.Intestazione,
                DescrizioneProdotto = x.Stipula.Descrizione_Prodotto!,
                TipoAtto = x.Stipula.Tipo_Atto,
                ImportoDeliberato = x.Stipula.Importo_Deliberato,
                MutuoSAL = x.Stipula.Mutuo_SAL,
                NumeroMutuo = x.Stipula.Numero_Mutuo,
                StatoFondo = x.Stipula.Stato_Fondo,
                StatoRichiesta = x.Stipula.Stato_Richiesta,
                StatoStipula = x.Stipula.Stato_Stipula,
                Assegnatario = x.AssegnatarioEffettivo
            })
            .ToListAsync();

        return new ListViewModel<AgendaStipulaViewModel>
        {
            Results = results,
            TotalCount = totalCount
        };
    }

    public async Task<string> AssegnaStipulaAsync(int idRichiesta)
    {
        var nomeAssegnatario = _userService.GetDisplayName().ToUpperInvariant();
        var record = await _dbContext.AssegnaAgendaStipule
            .FirstOrDefaultAsync(x => x.Id_Richiesta == idRichiesta);

        if (record != null)
        {
            // Se esiste già, aggiorna l'assegnatario (anche se è già valorizzato)
            record.Assegnatario = nomeAssegnatario;
            _dbContext.AssegnaAgendaStipule.Update(record);
        }
        else
        {
            // Se non esiste, crea un nuovo record
            var nuovaAssegnazione = new AssegnaAgendaStipula
            {
                Id_Richiesta = idRichiesta,
                Assegnatario = nomeAssegnatario,
                Data = DateTime.Now
            };
            _dbContext.AssegnaAgendaStipule.Add(nuovaAssegnazione);
        }

        await _dbContext.SaveChangesAsync();
        return nomeAssegnatario;
    }
    
    public async Task RevocaStipulaAsync(int idRichiesta)
    {
        var record = await _dbContext.AssegnaAgendaStipule
            .FirstOrDefaultAsync(x => x.Id_Richiesta == idRichiesta);

        if (record != null)
        {
            // Se esiste: aggiorna mettendo la stringa vuota
            record.Assegnatario = null;
            _dbContext.AssegnaAgendaStipule.Update(record);
        }
        else
        {
            // Se non esiste: crea un nuovo record
            var nuovoRecord = new AssegnaAgendaStipula
            {
                Id_Richiesta = idRichiesta,
                Assegnatario = null,
                Data = DateTime.Now
            };
            _dbContext.AssegnaAgendaStipule.Add(nuovoRecord);
        }

        await _dbContext.SaveChangesAsync();
    }
}