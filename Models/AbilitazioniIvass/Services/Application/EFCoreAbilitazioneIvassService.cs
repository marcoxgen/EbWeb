using EbWeb.Models.Exceptions;
using EbWeb.Models.Exceptions.Application;
using EbWeb.Models.AbilitazioniIvass.InputModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;
using EbWeb.Models.AbilitazioniIvass.Entities;
using EbWeb.Models.AbilitazioniIvass.Services.Infrastructure;
using EbWeb.Models.AbilitazioniIvass.ViewModels;
using EbWeb.Models.Common.Services.Application;

namespace EbWeb.Models.AbilitazioniIvass.Services.Application;

public class EFCoreAbilitazioneIvassService : IAbilitazioneIvassService
{
    private readonly IvassDbContext _dbContext;
    private readonly IUserService _userService;

    public EFCoreAbilitazioneIvassService(IvassDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<ListViewModel<AbilitazioneIvassViewModel>> GetAbilitazioniIvassAsync(AbilitazioneIvassListInputModel model)
    {
        IQueryable<ElencoAbilitatoIvass> baseQuery = _dbContext.ElencoAbilitatiIvass;

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
        
        if (model.FlagFormatoMifid != null)
        {
            baseQuery = baseQuery.Where(a => a.Flag_Formato_Mifid == model.FlagFormatoMifid);
        }
        
        if (model.FlagAbilitatoFinanza != null)
        {
            baseQuery = baseQuery.Where(a => a.Flag_Abilitato_Finanza == model.FlagAbilitatoFinanza);
        }

        if (model.AbilitatoOperativitaIvass != null)
        {
            baseQuery = baseQuery.Where(a => a.Abilitato_Operativita_Ivass == model.AbilitatoOperativitaIvass);
        }

        switch (model.OrderBy)
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
                baseQuery = model.Ascending ? baseQuery.OrderBy(a => a.Data_fine_abilitazione_IVASS) : baseQuery.OrderByDescending(a => a.Data_fine_abilitazione_IVASS);
                break;
        }

        IQueryable<AbilitazioneIvassViewModel> queryLinq = baseQuery
            .AsNoTracking()
            .Select(abilitazione => AbilitazioneIvassViewModel.FromEntity(abilitazione));

        List<AbilitazioneIvassViewModel> abilitazioni = await queryLinq                
            .Skip(model.Offset)
            .Take(model.Limit)
            .ToListAsync();

        int totalCount = await queryLinq.CountAsync();

        return new ListViewModel<AbilitazioneIvassViewModel>
        {
            Results = abilitazioni,
            TotalCount = totalCount
        };
    }

    public async Task<AbilitazioneIvassDetailViewModel> GetAbilitazioneIvassAsync(int id)
    {
        IQueryable<AbilitazioneIvassDetailViewModel> queryLinq = _dbContext.ElencoAbilitatiIvass
            .AsNoTracking()
            .Where(abilitato => abilitato.Id == id)
            .Select(abilitato => AbilitazioneIvassDetailViewModel.FromEntity(abilitato));

        AbilitazioneIvassDetailViewModel viewModel = await queryLinq.SingleAsync();

        return viewModel;
    }

    public async Task<AbilitazioneIvassDetailViewModel> CreateAbilitazioneIvassAsync(AbilitazioneIvassCreateInputModel inputModel)
    {
        int matricola = inputModel.Matricola;

        var abilitato = new AbilitatoIvass(matricola);
        abilitato.ChangeDataUltimoAggiornamento();

        _dbContext.Add(abilitato);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exc) when (exc.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new AbilitazioneIvassMatricolaUnavailableException(matricola, exc);
        }

        return await _dbContext.ElencoAbilitatiIvass
            .AsNoTracking()
            .Where(x => x.Matricola == matricola)
            .Select(x => AbilitazioneIvassDetailViewModel.FromEntity(x))
            .SingleAsync();
    }

    public async Task<AbilitazioneIvassDetailViewModel> EditAbilitazioneIvassAsync(AbilitazioneIvassEditInputModel inputModel)
    {
            AbilitatoIvass abilitato = await _dbContext.AbilitatiIvass.FindAsync(inputModel.Id);

        if (abilitato == null)
        {
            throw new AbilitazioneIvassNotFoundException(inputModel.Id);
        }

        abilitato.ChangeDataEsame(inputModel.DataEsame);
        abilitato.ChangeDataAbilitazioneOperativa(inputModel.DataAbilitazioneOperativa);
        abilitato.ChangeDataFineAbilitazioneOperativa(inputModel.DataFineAbilitazioneOperativa);
        abilitato.ChangeDataAbilitazioneIvass(inputModel.DataAbilitazioneIvass);
        abilitato.ChangeDataFineAbilitazioneIvass(inputModel.DataFineAbilitazioneIvass);
        abilitato.ChangeDataSospensione(inputModel.DataSospensione);
        abilitato.ChangeDataTermineSospensione(inputModel.DataTermineSospensione);
        abilitato.ChangeNote(inputModel.Note);
        abilitato.ChangeFormazione2021(inputModel.Formazione2021);
        abilitato.ChangeFormazione2022(inputModel.Formazione2022);
        abilitato.ChangeFormazione2023(inputModel.Formazione2023);
        abilitato.ChangeFormazione2024(inputModel.Formazione2024);
        abilitato.ChangeFormazione2025(inputModel.Formazione2025);
        abilitato.ChangeFormazione2026(inputModel.Formazione2026);
        abilitato.ChangeDataUltimoAggiornamento();
        
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exc) when (exc.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new AbilitazioneIvassMatricolaUnavailableException(inputModel.Id, exc);
        }

        return await _dbContext.ElencoAbilitatiIvass
            .AsNoTracking()
            .Where(x => x.Id == inputModel.Id)
            .Select(x => AbilitazioneIvassDetailViewModel.FromEntity(x))
        .SingleAsync();
    }
    
    public async Task<AbilitazioneIvassEditInputModel> GetAbilitazioneIvassForEditingAsync(int id)
    {
        var abilitato = await _dbContext.ElencoAbilitatiIvass
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync() ?? throw new AbilitazioneIvassNotFoundException(id);

        if (abilitato == null)
        {
            throw new AbilitazioneIvassNotFoundException(id);
        }

        return AbilitazioneIvassEditInputModel.FromEntity(abilitato);
    }
    
    public async Task DeleteAbilitazioneIvassAsync(int id)
    {
        AbilitatoIvass abilitato = await _dbContext.AbilitatiIvass.FindAsync(id);
        
        if (abilitato == null)
        {
            throw new AbilitazioneIvassNotFoundException(id);
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

    public async Task<IEnumerable<AbilitazioneIvassDetailViewModel>> GetAllAbilitazioniIvassAsync(DateTime? dataRiferimento)
    {
        dataRiferimento ??= DateTime.Now;
        DateTime dataRifStorica = dataRiferimento.Value.Date.AddDays(1).AddTicks(-1);

        using var connection = new SqlConnection(_dbContext.Database.GetConnectionString());
        var sql = "SELECT * FROM [Anag].[Storico_Abilitati_IVASS](@dataRif)";
        var entities = await connection.QueryAsync<ElencoAbilitatoIvass>(sql, new { dataRif = dataRifStorica });

        return entities.Select(e => AbilitazioneIvassDetailViewModel.FromEntity(e))
            .OrderBy(x => x.Intestazione);
    }
}