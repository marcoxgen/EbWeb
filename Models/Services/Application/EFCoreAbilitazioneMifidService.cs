using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Infrastructure;
using EbWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Application;

public class EFCoreAbilitazioneMifidService : IAbilitazioneMifidService
{
    private readonly MifidDbContext _dbContext;
    private readonly IUserService _userService;

    public EFCoreAbilitazioneMifidService(MifidDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<ListViewModel<AbilitazioneMifidViewModel>> GetAbilitazioniMifidAsync(AbilitazioneMifidListInputModel model)
    {
        //IQueryable<AbilitatoMifid> query = _dbContext.AbilitatiMifid;
        var query = _dbContext.AbilitatiMifid.AsQueryable();

        if (model.Id_Matricola.HasValue)
            query = query.Where(x => x.Matricola == model.Id_Matricola.Value);

        int totalCount = await query.CountAsync();

        var results = await query
            .AsNoTracking()
            .Skip(model.Offset)
            .Take(model.Limit)
            .Select(x => new AbilitazioneMifidViewModel
            {
                Matricola = x.Matricola,
                Intestazione = x.Intestazione,
                DescrUO = x.Descr_UO,
                CodiceFiscale = x.Codice_Fiscale,
                Ruolo = x.Ruolo,
                TitoloStudio = x.Titolo_di_studio,
                TitoloStudioMifid = x.Titolo_di_studio_Mifid,
                MesiPeriodoSupervisione = x.Mesi_periodo_di_supervisione,
                DataConseguimentoTitoloStudio = x.Data_conseguimento_titolo_di_studio,
                DataAbilitazioneMifid = x.Data_abilitazione_Mifid,
                DataSospensione = x.Data_sospensione,
                DataTermineDospensione = x.Data_termine_sospensione,
                NecessarioAssessment = x.Necessario_assessment,
                DataSuperamentoAssessment = x.Data_superamento_assessment,
                DataAbilitazioneTitoli = x.Data_abilitazione_titoli,
                AnniEsperienzadeguata = x.Esperienza_adeguata_in_anni,
                DataInizioSupervisione = x.Data_inizio_supervisione,
                DataFineSupervisione = x.Data_fine_supervisione,
                MatricolaSupervisore = x.Matricola_supervisore,
                IntestazioneSupervisore = x.Intestazione_supervisore,
                MatricolaSostitutoSupervisore = x.Matricola_sostituto_supervisore,
                IntestazioneSostitutoSupervisore = x.Intestazione_sostituto_supervisore,
                Formazione2024 = x.Formazione_2024,
                Formazione2025 = x.Formazione_2025,
                Note = x.Note,
                DataUltimoAggiornamento = x.Data_Ultimo_Aggiornamento,
                GeneraLetteraX = x.Genera_Lettera_X,
                GeneraLetteraY = x.Genera_Lettera_Y,
                GeneraLetteraZ = x.Genera_Lettera_Z 
            })
            .ToListAsync();

        return new ListViewModel<AbilitazioneMifidViewModel>
        {
            Results = results,
            TotalCount = totalCount
        };
    }

    /*
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
    */
}