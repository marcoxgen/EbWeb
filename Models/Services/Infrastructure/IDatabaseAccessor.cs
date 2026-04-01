using System.Data;

namespace EbWeb.Models.Services.Infrastructure;

public interface IDatabaseAccessor
{
    Task<DataSet> QueryAsync(string connectionName, FormattableString query);
    Task<T> QueryScalarAsync<T>(string connectionName, FormattableString query);
    Task<int> CommandAsync(string connectionName, FormattableString cmd);
}