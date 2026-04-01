using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EbWeb.Models.Services.Application;

public class AuditUserInterceptor : DbCommandInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuditUserInterceptor(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        InjectUserContext(command);
        return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        InjectUserContext(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    private void InjectUserContext(DbCommand command)
    {
        string sql = command.CommandText.TrimStart().ToUpper();

        if (sql.StartsWith("SELECT"))
        {
            return;
        }

        var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(user))
        {
            user = "USER_NOT_FOUND_IN_VSC";
        }

        var userParam = command.CreateParameter();
        userParam.ParameterName = "@AuditUser";
        userParam.Value = user;
        command.Parameters.Add(userParam);

        command.CommandText = $"EXEC sp_set_session_context 'UtenteAD', @AuditUser; {command.CommandText}";
    }
}