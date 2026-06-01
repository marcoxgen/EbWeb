using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using EbWeb.Models.Options;
using EbWeb.Models.AlimentazioneBudget.Services.Infrastructure;

namespace EbWeb.Models.AlimentazioneBudget.Services.Application;

public class AdoNetEsecutoreComandiService : IEsecutoreComandiService
{
    private readonly IOptionsMonitor<ConnectionStringsOptions> _connectionStringOptions;

    public AdoNetEsecutoreComandiService(IOptionsMonitor<ConnectionStringsOptions> connectionStringOptions)
    {
        _connectionStringOptions = connectionStringOptions;
    }

    public async Task<SqlExecutionResult> EseguiComandoDinamicoAsync(string dbName, string sqlComando)
    {
        if (string.IsNullOrWhiteSpace(dbName) || string.IsNullOrWhiteSpace(sqlComando))
            throw new ArgumentException("Database o Comando SQL mancanti.");

        var connectionStringBase = _connectionStringOptions.CurrentValue.Alimentazione_Budget;
        var builder = new SqlConnectionStringBuilder(connectionStringBase) { InitialCatalog = dbName };
        var risultato = new SqlExecutionResult();

        try
        {
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                // Intercettiamo i PRINT e i messaggi in tempo reale
                connection.FireInfoMessageEventOnUserErrors = true;
                connection.InfoMessage += (s, e) => {
                    if (!string.IsNullOrEmpty(e.Message)) risultato.Messages.Add(e.Message);
                };

                await connection.OpenAsync().ConfigureAwait(false);

                using (var command = new SqlCommand(sqlComando, connection) { CommandTimeout = 120 })
                {
                    var adapter = new SqlDataAdapter(command);
                    var ds = new DataSet();

                    // Riempie il DataSet gestendo da solo record multipli, tabelle vuote e tipi di dato complessi
                    adapter.Fill(ds);

                    int setIndex = 0;
                    foreach (DataTable table in ds.Tables)
                    {
                        table.TableName = $"ResultSet_{setIndex++}";
                        risultato.ResultSets.Add(table);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Qualsiasi errore (SQL o generico) finisce nei messaggi visibili a schermo
            risultato.Messages.Add($"[ERRORE] {ex.Message}");
        }

        return risultato;
    }
}