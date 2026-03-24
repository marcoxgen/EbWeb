using EbWeb.Models.Entities;
using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Infrastructure;
using EbWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Application;

public class EFCoreRichiestaPerfezionamentoService : IRichiestaPerfezionamentoService
{
    private readonly ThinsoftDbContext _dbContext;
    private readonly IUserService _userService;

    public EFCoreRichiestaPerfezionamentoService(ThinsoftDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<ListViewModel<RichiestaPerfezionamentoViewModel>> GetRichiestePerfezionamentoAsync(RichiestaPerfezionamentoListInputModel model)
    {
        var baseQuery =
            from r in _dbContext.RichiestaPerfezionamento
            join a in _dbContext.AssegnaPerfezionamento
                on r.Id_Richiesta equals a.Id_Richiesta into gj
            from a in gj.DefaultIfEmpty()
            select new
            {
                Richiesta = r,
                AssegnatarioEffettivo = a != null
                    ? (string.IsNullOrEmpty(a.Assegnatario1) ? "" : a.Assegnatario1)
                    : ""
            };

        if (model.Id_Richiesta.HasValue)
            baseQuery = baseQuery.Where(x => x.Richiesta.Id_Richiesta == model.Id_Richiesta.Value);

        int totalCount = await baseQuery.CountAsync();

        var results = await baseQuery
            .AsNoTracking()
            .Skip(model.Offset)
            .Take(model.Limit)
            .Select(x => new RichiestaPerfezionamentoViewModel
            {
                IdRichiesta = x.Richiesta.Id_Richiesta,
                DataRichiesta = x.Richiesta.Data_Richiesta,
                Filiale = x.Richiesta.Filiale,
                Nag = x.Richiesta.Nag,
                Intestazione = x.Richiesta.Intestazione,
                StatoRichiesta = x.Richiesta.Stato_Richiesta,
	            TipoRichiesta1 = x.Richiesta.TipoRichiesta1,
	            TipoRichiesta2 = x.Richiesta.TipoRichiesta2,
	            TipoRichiesta3 = x.Richiesta.TipoRichiesta3,
                Assegnatario1 = x.AssegnatarioEffettivo
            })
            .ToListAsync();

        return new ListViewModel<RichiestaPerfezionamentoViewModel>
        {
            Results = results,
            TotalCount = totalCount
        };
    }

    public async Task<string> AssegnaRichiestaAsync(int idRichiesta, int assegnatarioIndex)
    {
        var nomeAssegnatario = _userService.GetDisplayName().ToUpperInvariant();
        var assegnazione = await _dbContext.AssegnaPerfezionamento
            .FirstOrDefaultAsync(x => x.Id_Richiesta == idRichiesta);
        if (assegnazione == null)
        {
            assegnazione = new AssegnaPerfezionamento
            {
                Id_Richiesta = idRichiesta,
                Assegnatario1 = null,
                Assegnatario2 = null,
                Assegnatario3 = null,
            };
            _dbContext.AssegnaPerfezionamento.Add(assegnazione);
        }
        if (assegnatarioIndex == 1)
        {
            assegnazione.Assegnatario1 = nomeAssegnatario;
        }
        else if (assegnatarioIndex == 2)
        {
            assegnazione.Assegnatario2 = nomeAssegnatario;
        }
        else if (assegnatarioIndex == 3)
        {
            assegnazione.Assegnatario3 = nomeAssegnatario;
        }
        assegnazione.Data = DateTime.Now;

        await _dbContext.SaveChangesAsync();
        return nomeAssegnatario;
    }
    
    public async Task RevocaRichiestaAsync(int idRichiesta, int assegnatarioIndex)
    {
        var assegnazione = await _dbContext.AssegnaPerfezionamento
            .FirstOrDefaultAsync(x => x.Id_Richiesta == idRichiesta);

        if (assegnazione != null)
        {
            string propertyName = $"Assegnatario{assegnatarioIndex}";
            var propertyInfo = assegnazione.GetType().GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(assegnazione, null);
                assegnazione.Data = DateTime.Now;

                _dbContext.AssegnaPerfezionamento.Update(assegnazione);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}