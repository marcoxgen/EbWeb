using EbWeb.Models.Exceptions;
using EbWeb.Models.Exceptions.Application;
using EbWeb.Models.AbilitazioniMifid.InputModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;
//using System.Buffers;
using EbWeb.Models.AbilitazioniMifid.Entities;
using EbWeb.Models.AbilitazioniMifid.Services.Infrastructure;
using EbWeb.Models.AbilitazioniMifid.ViewModels;
using EbWeb.Models.Common.Services.Application;

namespace EbWeb.Models.AbilitazioniMifid.Services.Application;

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
        IQueryable<AnagAbilitatoMifid> baseQuery = _dbContext.AnagAbilitatiMifid;

        baseQuery = baseQuery.Where(a => a.Escluso == false);

        if (model.Matricola != null)
        {
            baseQuery = baseQuery.Where(a => a.Matricola == model.Matricola);
        }

        if (!string.IsNullOrWhiteSpace(model.Intestazione))
        {
            baseQuery = baseQuery.Where(a => a.Intestazione.Contains(model.Intestazione));
        }

        if (!string.IsNullOrWhiteSpace(model.DescrUO))
        {
            baseQuery = baseQuery.Where(a => a.Descr_UO.Contains(model.DescrUO));
        }

        if (!string.IsNullOrWhiteSpace(model.Ruolo))
        {
            baseQuery = baseQuery.Where(a => a.Ruolo.Contains(model.Ruolo));
        }

        if (model.FlagAbilitatoMifid != null)
        {
            baseQuery = baseQuery.Where(a => a.Flag_Abilitato_Mifid == model.FlagAbilitatoMifid);
        }
        
        if (model.AbilitatoFinanceWMP != null)
        {
            baseQuery = baseQuery.Where(a => a.Abilitato_Finance_WMP == model.AbilitatoFinanceWMP);
        }

        switch(model.OrderBy)
        {
            case "Matricola":
                baseQuery = model.Ascending ? baseQuery.OrderBy(a => a.Matricola) : baseQuery.OrderByDescending(a => a.Matricola);
                break;
            case "Intestazione":
                baseQuery = model.Ascending ? baseQuery.OrderBy(a => a.Intestazione) : baseQuery.OrderByDescending(a => a.Intestazione);
                break;
            case "DescrUO":
                baseQuery = model.Ascending ? baseQuery.OrderBy(a => a.Descr_UO) : baseQuery.OrderByDescending(a => a.Descr_UO);
                break;
            case "Ruolo":
                baseQuery = model.Ascending ? baseQuery.OrderBy(a => a.Ruolo) : baseQuery.OrderByDescending(a => a.Ruolo);
                break;
            case "DataSospensione":
                baseQuery = model.Ascending ? baseQuery.OrderBy(a => a.Data_sospensione) : baseQuery.OrderByDescending(a => a.Data_sospensione);
                break;
            case "FineSupervisione":
                baseQuery = model.Ascending ? baseQuery.OrderBy(a => a.Data_fine_supervisione) : baseQuery.OrderByDescending(a => a.Data_fine_supervisione);
                break;
        }

        IQueryable<AbilitazioneMifidViewModel> queryLinq = baseQuery
            .AsNoTracking()
            .Select(abilitazione => AbilitazioneMifidViewModel.FromEntity(abilitazione));

        List<AbilitazioneMifidViewModel> abilitazioni = await queryLinq                
            .Skip(model.Offset)
            .Take(model.Limit)
            .ToListAsync();

        int totalCount = await queryLinq.CountAsync();

        return new ListViewModel<AbilitazioneMifidViewModel>
        {
            Results = abilitazioni,
            TotalCount = totalCount
        };
    }

    public async Task<AbilitazioneMifidDetailViewModel> GetAbilitazioneMifidAsync(int matricola)
    {
        IQueryable<AbilitazioneMifidDetailViewModel> queryLinq = _dbContext.AnagAbilitatiMifid
            .AsNoTracking()
            .Where(abilitato => abilitato.Matricola == matricola)
            .Select(abilitato => AbilitazioneMifidDetailViewModel.FromEntity(abilitato));

        AbilitazioneMifidDetailViewModel viewModel = await queryLinq.SingleAsync();

        return viewModel;
    }

    public async Task<AbilitazioneMifidDetailViewModel> CreateAbilitazioneMifidAsync(AbilitazioneMifidCreateInputModel inputModel)
    {
        int matricola = inputModel.Matricola;

        var abilitato = new BaseAbilitatoMifid(matricola);
        abilitato.ChangeDataUltimoAggiornamento();

        _dbContext.Add(abilitato);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exc) when (exc.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new AbilitazioneMifidMatricolaUnavailableException(matricola, exc);
        }

        return await _dbContext.AnagAbilitatiMifid
            .AsNoTracking()
            .Where(x => x.Matricola == abilitato.Matricola)
            .Select(x => AbilitazioneMifidDetailViewModel.FromEntity(x))
            .SingleAsync();
    }

    public async Task<AbilitazioneMifidDetailViewModel> EditAbilitazioneMifidAsync(AbilitazioneMifidEditInputModel inputModel)
    {
        BaseAbilitatoMifid abilitato = await _dbContext.BaseAbilitatiMifid.FindAsync(inputModel.Matricola);
        
        if (abilitato == null)
        {
            throw new AbilitazioneMifidNotFoundException(inputModel.Matricola);
        }

        abilitato.ChangeTitoloDiStudio(inputModel.TitoloStudio);
        abilitato.ChangeTitoloDiStudioMifidCod(inputModel.TitoloStudioMifidCod);
        abilitato.ChangeDataConseguimentoTitoloStudio(inputModel.DataConseguimentoTitoloStudio);
        abilitato.ChangeDataAbilitazioneMifid(inputModel.DataAbilitazioneMifid);
        abilitato.ChangeDataSospensione(inputModel.DataSospensione);
        abilitato.ChangeDataTermineSospensione(inputModel.DataTermineSospensione);
        abilitato.ChangeNecessarioAssessment(inputModel.NecessarioAssessment);
        abilitato.ChangeDataSuperamentoAssessment(inputModel.DataSuperamentoAssessment);
        abilitato.ChangeDataAbilitazioneTitoli(inputModel.DataAbilitazioneTitoli);
        abilitato.ChangeDataInizioSupervisione(inputModel.DataInizioSupervisione);
        abilitato.ChangeDataFineSupervisione(inputModel.DataFineSupervisione);
        abilitato.ChangeMatricolaSupervisore(inputModel.MatricolaSupervisore);
        abilitato.ChangeMatricolaSostitutoSupervisore(inputModel.MatricolaSostitutoSupervisore);
        abilitato.ChangeFormazione2024(inputModel.Formazione2024);
        abilitato.ChangeFormazione2025(inputModel.Formazione2025);
        abilitato.ChangeNote(inputModel.Note);
        abilitato.ChangeDataUltimoAggiornamento();
        abilitato.ChangeDataFineAbilitazioneMifid(inputModel.DataFineAbilitazioneMifid);
        abilitato.ChangeDataFineAbilitazioneTitoli(inputModel.DataFineAbilitazioneTitoli);
        
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exc) when (exc.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new AbilitazioneMifidMatricolaUnavailableException(inputModel.Matricola, exc);
        }

        return await _dbContext.AnagAbilitatiMifid
            .AsNoTracking()
            .Where(x => x.Matricola == inputModel.Matricola)
            .Select(x => AbilitazioneMifidDetailViewModel.FromEntity(x))
        .SingleAsync();
    }
    
    public async Task<AbilitazioneMifidEditInputModel> GetAbilitazioneMifidForEditingAsync(int matricola)
    {
        var abilitato = await _dbContext.AnagAbilitatiMifid
            .AsNoTracking()
            .Where(x => x.Matricola == matricola)
            .FirstOrDefaultAsync() ?? throw new AbilitazioneMifidNotFoundException(matricola);

        if (abilitato == null)
        {
            throw new AbilitazioneMifidNotFoundException(matricola);
        }

        return AbilitazioneMifidEditInputModel.FromEntity(abilitato);
    }
    
    public async Task DeleteAbilitazioneMifidAsync(int matricola)
    {
        BaseAbilitatoMifid abilitato = await _dbContext.BaseAbilitatiMifid.FindAsync(matricola);
        
        if (abilitato == null)
        {
            throw new AbilitazioneMifidNotFoundException(matricola);
        }

        abilitato.ChangeDataUltimoAggiornamento();
        abilitato.ChangeEscluso(true);

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<AnagDipendentiLookupViewModel>> GetAnagDipendentiLookupAsync()
    {
        return await _dbContext.AnagDipendenti
            .AsNoTracking()
            .OrderBy(d => d.Cognome)
            .ThenBy(d => d.Nome)
            .Select(d => new AnagDipendentiLookupViewModel
            {
                Value = d.Matricola_Int,
                Text = d.Matricola_Int + " - " + d.Cognome + " " + d.Nome 
            })
            .ToListAsync();
    }

    public async Task<List<SupervisoriLookupViewModel>> GetSupervisoriLookupAsync()
    {
        return await _dbContext.Supervisori
            .AsNoTracking()
            .OrderBy(s => s.Intestazione)
            .Select(s => new SupervisoriLookupViewModel
            {
                Value = s.Matricola,
                Text = s.Intestazione
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<AbilitazioneMifidDetailViewModel>> GetAllAbilitazioniMifidAsync(DateTime? dataRiferimento)
    {
        dataRiferimento ??= DateTime.Now;
        DateTime dataRifStorica = dataRiferimento.Value.Date.AddDays(1).AddTicks(-1);

        using var connection = new SqlConnection(_dbContext.Database.GetConnectionString());
        var sql = "SELECT * FROM [Anag].[Storico_Abilitati_Mifid](@dataRif)";
        var entities = await connection.QueryAsync<AnagAbilitatoMifid>(sql, new { dataRif = dataRifStorica });

        return entities.Select(e => AbilitazioneMifidDetailViewModel.FromEntity(e))
            .OrderBy(x => x.Intestazione);
    }
    
    public async Task<IEnumerable<SelectOptionsViewModel>> GetTitoliStudioMifidLookupAsync()
    {
        return await _dbContext.TitoliSudioMifid
            .AsNoTracking()
            .OrderBy(t => t.Titolo_di_Studio_Mifid_Des)
            .Select(t => new SelectOptionsViewModel
            {
                Value = t.Titolo_di_Studio_Mifid_Cod.ToString(),
                Text = t.Titolo_di_Studio_Mifid_Des
            })
            .ToListAsync();
    }
}