using static EbWeb.Models.AlimentazioneBudget.Services.Application.AdoNetEsecutoreComandiService;

namespace EbWeb.Models.AlimentazioneBudget.Services.Application
{
    public interface IEsecutoreComandiService
    {
        Task<SqlExecutionResult> EseguiComandoDinamicoAsync(string dbName, string sqlComando);
    }
}