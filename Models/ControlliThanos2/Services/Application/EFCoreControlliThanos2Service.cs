using Dapper;
using EbWeb.Models.Common.Services.Application;
using EbWeb.Models.ControlliThanos2.Services.Infrastructure;
using EbWeb.Models.ControlliThanos2.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace EbWeb.Models.ControlliThanos2.Services.Application;

public class EFCoreControlloThanos2Service : IControlloThanos2Service
{
    private readonly ControlliDWHDbContext _dbContext;
    private readonly IUserService _userService;

    public EFCoreControlloThanos2Service(ControlliDWHDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<List<ControlloThanos2ViewModel>> GetControlliThanos2Async()
    {
        var sources = await _dbContext.Sources
            .AsNoTracking()
            .Where(x => x.Abilitato)
            .OrderBy(x => x.Priorita)
            .ThenBy(x => x.Anomalia)
            .ToListAsync();

        var connection = _dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var result = new List<ControlloThanos2ViewModel>();

        foreach (var source in sources)
        {
            /*var sql = $"SELECT COUNT(DISTINCT {source.Descrizione}) FROM [{source.NomeSchema}].[{source.NomeOggetto}]";

            if (!string.IsNullOrWhiteSpace(source.Filtro))
            {
                sql += $" WHERE {source.Filtro}";
            }

            long count = 0;
            var sw = Stopwatch.StartNew();

            try
            {
                count = await connection.ExecuteScalarAsync<long>(
                    new CommandDefinition(sql, commandTimeout: 5));
            }
            catch (SqlException ex) when (ex.Number == -2)
            {
                count = 0;
            }

            sw.Stop();*/

            if (source.NumeroRecord >= 0)
            {
                result.Add(new ControlloThanos2ViewModel
                {
                    IdSource = source.IdSource,
                    Anomalia = source.Anomalia,
                    Priorita = source.Priorita,
                    NumeroRecord = source.NumeroRecord,
                    UltimoAggiornamento = source.UltimoAggiornamento
                });
            }
        }

        return result;
    }
}