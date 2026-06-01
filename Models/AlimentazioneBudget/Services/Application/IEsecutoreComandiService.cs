using EbWeb.Models.AlimentazioneBudget.Services.Infrastructure;

namespace EbWeb.Models.AlimentazioneBudget.Services.Application
{
    public interface IEsecutoreComandiService
    {
        Task<SqlExecutionResult> EseguiComandoDinamicoAsync(string dbName, string sqlComando);
    }
}