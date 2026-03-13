using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Infrastructure;
using EbWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Application;

public class EFCoreSchedaBudgetService : ISchedaBudgetService
{
    private readonly BudgetDbContext _dbContext;
    private readonly IUserService _userService;

    public EFCoreSchedaBudgetService(BudgetDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }
    
    public async Task<List<string>> GetEtichetteDataAsync()
    {
        return await _dbContext.SchedeBudget
            .Where(x => x.Etichetta_Data != null)
            .Select(x => x.Etichetta_Data!)
            .Distinct()
            .OrderByDescending(x => x)
            .ToListAsync();
    }

    public async Task<ListViewModel<SchedaBudgetViewModel>> GetSchedeBudgetAsync(SchedaBudgetListInputModel model)
    {
        var utenzaTarget = _userService.GetUserName();
        //var utenzaTarget = "DX00172";
        var etichettaTarget = model.EtichettaSelezionata ?? string.Empty;

        var basequery = _dbContext.SchedeBudget
            .GroupJoin(
                _dbContext.Abilitazioni,
                sb => sb.ID_UO,
                a => a.ID_UO_Abilitata,
                (sb, joinedAbilitazioni) => new { sb, joinedAbilitazioni }
            )
            .SelectMany(
                x => x.joinedAbilitazioni.DefaultIfEmpty(),
                (x, a) => new { x.sb, a }
            )
            .Where(x => x.a!.Utenza_Dominio_BccSi == utenzaTarget 
                    && x.sb.Etichetta_Data == etichettaTarget)
            .OrderBy(x => x.sb.Ordinamento)
            .Select(x => new 
            {
                SchedaBudget = x.sb 
            });

        int totalCount = await basequery.CountAsync();

        var results = await basequery
            .AsNoTracking()
            .Select(x => new SchedaBudgetViewModel
            {
                ID_UO = x.SchedaBudget.ID_UO,
                EtichettaData = x.SchedaBudget.Etichetta_Data,
                EtichettaUO = x.SchedaBudget.Etichetta_UO,
                TipoUtenteDes = x.SchedaBudget.Tipo_Utente_Des,
                Ordinamento = x.SchedaBudget.Ordinamento,
                MetricaDes = x.SchedaBudget.Metrica_Des,
                TipologiaDes = x.SchedaBudget.Tipologia_Des,
                DataRif = x.SchedaBudget.Data_Rif,
                ValoreFineAnnoPrecedente = x.SchedaBudget.Valore_Fine_Anno_Precedente,
                Rettifiche = x.SchedaBudget.Rettifiche,
                ValoreAnnoPrecedenteRettificato = x.SchedaBudget.Valore_Anno_Precedente_Rettificato,
                ValoreStessoPeriodoAnnoPrecedenteRettificato = x.SchedaBudget.Valore_Stesso_Periodo_Anno_Precedente_Rettificato,
                ValoreAllaDataRif = x.SchedaBudget.Valore_Alla_Data_Rif,
                FlussoAllaData = x.SchedaBudget.Flusso_Alla_Data,
                ObiettivoAnnuale = x.SchedaBudget.Obiettivo_Annuale,
                ValoreDaRaggiungereAnnuale = x.SchedaBudget.Valore_Da_Raggiungere_Annuale,
                PercentualeRaggiungimentoObiettivoAnnuale = x.SchedaBudget.Percentuale_Raggiungimento_Obiettivo_Annuale,
                PercentualeRaggiungimentoObiettivoAnnualeIstogramma = x.SchedaBudget.Percentuale_Raggiungimento_Obiettivo_Annuale_Istogramma,
                ObiettivoMensile = x.SchedaBudget.Obiettivo_Mensile,
                ValoreDaRaggiungereMensile = x.SchedaBudget.Valore_Da_Raggiungere_Mensile,
                PercentualeRaggiungimentoObiettivoMensile = x.SchedaBudget.Percentuale_Raggiungimento_Obiettivo_Mensile,
                Punteggio = x.SchedaBudget.Punteggio,
                AdObiettivo = x.SchedaBudget.Ad_Obiettivo
            })
            .ToListAsync();

        return new ListViewModel<SchedaBudgetViewModel>
        {
            Results = results,
            TotalCount = totalCount
        };
    }
}