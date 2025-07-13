using System.Data;
using EbWeb.Models.Services.Infrastructrure;
using EbWeb.Models.ValueObjects;
using Microsoft.Data.SqlClient;
using MyCourse.Models.Exceptions.Infrastructure;

namespace EbWeb.Models.Services.Infrastructure;

public class SqlDatabaseAccessor : IDatabaseAccessor
{
    private readonly ILogger<SqlDatabaseAccessor> logger;
    private readonly IConfiguration configuration;

    public SqlDatabaseAccessor(ILogger<SqlDatabaseAccessor> logger, IConfiguration configuration)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task<int> CommandAsync(string connectionName, FormattableString formattableCommand)
    {
        try
        {
            string? connectionString = configuration.GetConnectionString(connectionName);
            using SqlConnection conn = await GetOpenedConnection(connectionString);
            using SqlCommand cmd = GetCommand(formattableCommand, conn);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            return affectedRows;
        }
        catch (SqlException exc) when (exc.ErrorCode == 19)
        {
            throw new ConstraintViolationException(exc);
        }
    }

    public async Task<DataSet> QueryAsync(string connectionName, FormattableString formattableQuery)
    {
        logger.LogInformation(formattableQuery.Format, formattableQuery.GetArguments());

        string? connectionString = configuration.GetConnectionString(connectionName);
        using SqlConnection conn = await GetOpenedConnection(connectionString);
        using SqlCommand cmd = GetCommand(formattableQuery, conn);

        //Invio la query al database e ottengo un SqlDataReader
        //per leggere i risultati
        
        try
        {
            using var reader = await cmd.ExecuteReaderAsync();
            var dataSet = new DataSet();

            //Creo tanti DataTable per quante sono le tabelle
            //di risultati trovate dal SqlDataReader
            do
            {
                var dataTable = new DataTable();
                dataSet.Tables.Add(dataTable);
                dataTable.Load(reader);
            } while (!reader.IsClosed);

            return dataSet;
        }
        catch (SqlException exc) when (exc.ErrorCode == 19)
        {
            throw new ConstraintViolationException(exc);
        }
    }

    public async Task<T> QueryScalarAsync<T>(string connectionName, FormattableString formattableQuery)
    {
        try
        {
            string? connectionString = configuration.GetConnectionString(connectionName);
            using SqlConnection conn = await GetOpenedConnection(connectionString);
            using SqlCommand cmd = GetCommand(formattableQuery, conn);
            object result = await cmd.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }
        catch (SqlException exc) when (exc.ErrorCode == 19)
        {
            throw new ConstraintViolationException(exc);
        }
    }

    private static SqlCommand GetCommand(FormattableString formattableQuery, SqlConnection conn)
    {
        //Creo dei SqlParameter a partire dalla FormattableString
        var queryArguments = formattableQuery.GetArguments();
        var sqlParameters = new List<SqlParameter>();
        for (var i = 0; i < queryArguments.Length; i++)
        {
            if (queryArguments[i] is Sql)
            {
                continue;
            }
            var parameter = new SqlParameter(i.ToString(), queryArguments[i]);
            sqlParameters.Add(parameter);
            queryArguments[i] = "@" + i;
        }
        string query = formattableQuery.ToString();

        var cmd = new SqlCommand(query, conn);
        //Aggiungiamo i SqliteParameters al SqliteCommand
        cmd.Parameters.AddRange(sqlParameters.ToArray());
        return cmd;
    }

    private async Task<SqlConnection> GetOpenedConnection(string connectionString)
    {
        //Mi collego al database Sql, invio la query e leggio i risultati
        var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        return conn;
    }
}