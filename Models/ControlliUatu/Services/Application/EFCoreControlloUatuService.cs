using Dapper;
using Hangfire;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using EbWeb.Models.Common.Services.Application;
using EbWeb.Models.ControlliUatu.Entities;
using EbWeb.Models.ControlliUatu.InputModels;
using EbWeb.Models.ControlliUatu.Services.Infrastructure;
using EbWeb.Models.ControlliUatu.ViewModels;

namespace EbWeb.Models.ControlliUatu.Services.Application;

public class EFCoreControlloUatuService : IControlloUatuService
{
    private readonly ControlliDWHDbContext _dbContext;
    private readonly IUserService _userService;

    public EFCoreControlloUatuService(ControlliDWHDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<List<ControlloUatuViewModel>> GetControlliUatuAsync()
    {
        return await _dbContext.Sources
            .AsNoTracking()
            .Where(x => x.Abilitato && x.NumeroRecord != 0)
            .OrderBy(x => x.Priorita)
            .ThenBy(x => x.Anomalia)
            .Select(x => new ControlloUatuViewModel
            {
                IdSource = x.IdSource,
                Anomalia = x.Anomalia,
                Priorita = x.Priorita,
                NumeroRecord = x.NumeroRecord,
                UltimoAggiornamento = x.UltimoAggiornamento
            })
            .ToListAsync();
    }

    [DisableConcurrentExecution(3600)]
    public async Task RefreshView()
    {
        var sources = await _dbContext.Sources
            .Where(x => x.Abilitato)
            .ToListAsync();

            var connection = _dbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            foreach (var source in sources)
            {
                var sql = $"SELECT COUNT(DISTINCT {source.Descrizione}) " +
                    $"FROM [{source.NomeSchema}].[{source.NomeOggetto}]";

                if (!string.IsNullOrWhiteSpace(source.Filtro))
                {
                    sql += $" WHERE {source.Filtro}";
                }

                long count;

                try
                {
                    count = await connection.ExecuteScalarAsync<long>(
                        new CommandDefinition(sql, commandTimeout: 60));

                    source.NumeroRecord = count;
                    source.UltimoAggiornamento = DateTime.Now;
                }
                catch (SqlException ex) when (ex.Number == -2)
                {
                    source.NumeroRecord = 0;
                    source.UltimoAggiornamento = DateTime.Now;
                }
            }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<DateTime?> GetLastRefreshAsync()
    {
        return await _dbContext.Sources
            .Where(x => x.Abilitato)
            .MaxAsync(x => x.UltimoAggiornamento);
    }

    public async Task<AnomaliaListViewModel> GetAnomalieAsync(AnomaliaListInputModel model)
    {
        int? idSource = model.IdSource;

        var source = await _dbContext.Sources
            .SingleOrDefaultAsync(s => s.IdSource == idSource);

        if (source == null || !idSource.HasValue)
        {
            return new AnomaliaListViewModel();
        }

        var queryChiaviSorgente = $@"
        SELECT DISTINCT CONVERT(varchar(64), HASHBYTES('SHA2_256', {source.Descrizione}), 2) AS Value
        FROM [{source.NomeSchema}].[{source.NomeOggetto}]
        WHERE Attiva = 1";

        if (!string.IsNullOrWhiteSpace(source.Filtro))
        {
            queryChiaviSorgente += $" WHERE {source.Filtro}";
        }

        var chiaviValide = _dbContext.Database.SqlQueryRaw<string>(queryChiaviSorgente);

        await _dbContext.Azioni
            .Where(a => a.IdSource == idSource.Value
                     && a.Attiva
                     && !chiaviValide.Contains(a.ChiaveHash))
            .ExecuteUpdateAsync(s => s
                .SetProperty(a => a.Attiva, false)
                .SetProperty(a => a.DataDisattivazione, DateTime.Now));

        var countSql = $"SELECT COUNT(DISTINCT {source.Descrizione}) AS Value FROM [{source.NomeSchema}].[{source.NomeOggetto}]";
        if (!string.IsNullOrWhiteSpace(source.Filtro))
        {
            countSql += $" WHERE {source.Filtro}";
        }

        int totalCount = await _dbContext.Database
            .SqlQueryRaw<int>(countSql)
            .SingleOrDefaultAsync();

        var sql = $@"
        WITH SourceRows AS
        (
            SELECT DISTINCT
                {source.Descrizione} AS Descrizione,
                CONVERT(varchar(64), HASHBYTES('SHA2_256', {source.Descrizione}), 2) AS ChiaveHash
            FROM [{source.NomeSchema}].[{source.NomeOggetto}]";

            if (!string.IsNullOrWhiteSpace(source.Filtro))
            {
                sql += $" WHERE {source.Filtro}";
            }

            sql += $@"
        )
        SELECT
            s.ChiaveHash,
            {{0}} AS IdSource,
            s.Descrizione,
            n.IdAzione,
            n.Nota,
            n.UltimaModifica,
            n.Risolta,
            ISNULL(n.Attiva, 1) AS Attiva,
            n.DataDisattivazione,
            n.Utente
        FROM SourceRows s
        LEFT JOIN Thanos.Azioni n
            ON n.ChiaveHash = s.ChiaveHash
            AND n.IdSource = {{0}}
        WHERE (n.Attiva IS NULL OR n.Attiva <> 0)
        ORDER BY s.Descrizione 
        OFFSET {{1}} ROWS 
        FETCH NEXT {{2}} ROWS ONLY";

        var anomalie = await _dbContext.Database
            .SqlQueryRaw<AnomaliaViewModel>(sql, idSource.Value, model.Offset, model.Limit)
            .ToListAsync();

        return new AnomaliaListViewModel
        {
            Anomalia = source.Anomalia,
            FlagEccezioni = !string.IsNullOrWhiteSpace(source.Eccezioni),
            Input = model,
            Anomalie = new ListViewModel<AnomaliaViewModel>
            {
                TotalCount = totalCount,
                Results = anomalie
            }
        };
    }

    public async Task<EsitoAzioneViewModel> AggiornaAzioneAsync(AggiornaAzioneInputModel aggiornaAzione)
    {
        Azione? azione = null;

        // Recupera la nota se l'Id è valido
        if (aggiornaAzione.IdAzione.HasValue && aggiornaAzione.IdAzione.Value > 0)
        {
            azione = await _dbContext.Azioni
                .FindAsync(aggiornaAzione.IdAzione.Value);
        }

        // Se non esiste, ne crea una nuova
        if (azione == null)
        {
            azione = new Azione
            {
                IdSource = aggiornaAzione.IdSource,
                Descrizione = aggiornaAzione.Descrizione ?? string.Empty,
                Nota = aggiornaAzione.Nota ?? string.Empty,
                ChiaveHash = aggiornaAzione.ChiaveHash,
                Attiva = true
            };
            _dbContext.Azioni.Add(azione);
        }

        azione.Nota = aggiornaAzione.Nota ?? string.Empty;

        if (aggiornaAzione.Risolto.HasValue)
        {
            azione.Risolta = aggiornaAzione.Risolto.Value;
        }

        azione.UltimaModifica = DateTime.Now;
        azione.Utente = _userService.GetUserName();

        await _dbContext.SaveChangesAsync();

        return new EsitoAzioneViewModel
        {
            IdAzione = azione.IdAzione,
            DataModificaFormatted = azione.UltimaModifica?.ToString("dd/MM/yyyy") ?? string.Empty
        };
    }

    public async Task<List<AnomaliaViewModel>> GetStoricoAzioniAsync(int idSource, string chiaveHash)
    {
        return await _dbContext.Azioni
            .Where(x => x.IdSource == idSource
                     && x.ChiaveHash == chiaveHash)
            .OrderByDescending(x => x.UltimaModifica)
            .Select(x => new AnomaliaViewModel
            {
                IdAzione = x.IdAzione,
                IdSource = x.IdSource,
                Descrizione = x.Descrizione,
                ChiaveHash = x.ChiaveHash,
                Nota = x.Nota,
                Risolta = x.Risolta,
                Attiva = x.Attiva,
                UltimaModifica = x.UltimaModifica,
                DataDisattivazione = x.DataDisattivazione,
                Utente = x.Utente
            })
            .ToListAsync();
    }

    public async Task<EccezioneViewModel> GetEccezioneAsync(int idSource, string chiaveHash)
    {
        var valori = new Dictionary<string, object?>();

        var colonne = await _dbContext.ColonneEccezioni
            .Where(x => x.IdSource == idSource)
            .OrderBy(x => x.Posizione)
            .ToListAsync();

        if (!colonne.Any())
        {
            throw new InvalidOperationException(
                $"Nessuna configurazione ColonneEccezioni trovata per IdSource {idSource}.");
        }

        var source = await _dbContext.Sources
            .SingleOrDefaultAsync(x => x.IdSource == idSource)
            ?? throw new InvalidOperationException(
                $"Source {idSource} non trovata.");

        if (string.IsNullOrWhiteSpace(source.Descrizione))
        {
            throw new InvalidOperationException(
                $"La descrizione della Source {idSource} è vuota.");
        }

        var sql = $@"
        SELECT TOP 1 *
        FROM [{source.NomeDatabase}].[{source.NomeSchema}].[{source.NomeOggetto}]
        WHERE CONVERT(VARCHAR(64),
            HASHBYTES(
                'SHA2_256',
                {source.Descrizione}
            ),
            2
        ) = @ChiaveHash";

        using var connection = _dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        using var command = connection.CreateCommand();

        command.CommandText = sql;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@ChiaveHash";
        parameter.Value = chiaveHash;

        command.Parameters.Add(parameter);

        try
        {
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var colonneReader = Enumerable
                    .Range(0, reader.FieldCount)
                    .Select(reader.GetName)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var colonna in colonne)
                {
                    if (string.IsNullOrWhiteSpace(colonna.ColonnaOrigine))
                        continue;

                    if (colonneReader.Contains(colonna.ColonnaOrigine))
                    {
                        valori[colonna.NomeColonna] = reader[colonna.ColonnaOrigine];
                    }
                    else
                    {
                        valori[colonna.NomeColonna] = null;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Errore durante il recupero dell'eccezione. {ex.Message}", ex);
        }

        return new EccezioneViewModel
        {
            IdSource = idSource,
            Eccezioni = source.Eccezioni,
            Colonne = colonne.Select(x => new ColonnaEccezioneViewModel
            {
                Posizione = x.Posizione,
                NomeColonna = x.NomeColonna,
                TipoDato = x.TipoDato,
                ColonnaOrigine = x.ColonnaOrigine,
                Valore = valori.TryGetValue(
                    x.NomeColonna,
                    out var valore)
                        ? valore
                        : null
            }).ToList()
        };
    }

    public async Task SalvaEccezioneAsync(IFormCollection form)
    {
        int idSource = int.Parse(form["IdSource"]);

        var source = await _dbContext.Sources
            .SingleAsync(x => x.IdSource == idSource);

        if (string.IsNullOrWhiteSpace(source.Eccezioni))
        {
            throw new InvalidOperationException(
                $"Tabella Eccezioni non configurata per Source {idSource}.");
        }

        var colonne = await _dbContext.ColonneEccezioni
            .Where(x => x.IdSource == idSource)
            .OrderBy(x => x.Posizione)
            .ToListAsync();

        var colonneInsert = new List<string>();
        var parametriInsert = new List<string>();

        using var connection = _dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        using var command = connection.CreateCommand();

        foreach (var colonna in colonne)
        {
            colonneInsert.Add($"[{colonna.NomeColonna}]");
            parametriInsert.Add($"@p{colonna.Posizione}");

            var parametro = command.CreateParameter();
            parametro.ParameterName = $"@p{colonna.Posizione}";
            parametro.Value = form[colonna.NomeColonna].ToString() ?? DBNull.Value.ToString();

            command.Parameters.Add(parametro);
        }

        command.CommandText = $@"
        INSERT INTO [{source.NomeSchema}].[{source.Eccezioni}]
        (
            {string.Join(", ", colonneInsert)}
        )
        VALUES
        (
            {string.Join(", ", parametriInsert)}
        )";

        await command.ExecuteNonQueryAsync();
    }
}