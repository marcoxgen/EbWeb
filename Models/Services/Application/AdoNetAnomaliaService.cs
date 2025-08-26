using System.Data;
using EbWeb.Models.Exceptions.Application;
using EbWeb.Models.Services.Infrastructrure;
using EbWeb.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Security.Principal;

namespace EbWeb.Models.Services.Application
{
    public class AdoNetAnomaliaService : IAnomaliaService
    {
        private readonly ILogger<AdoNetAnomaliaService> logger;
        private readonly IDatabaseAccessor db;
        private readonly IOptionsMonitor<ConnectionStringsOptions> connectionStringOptions;
        private readonly IHttpContextAccessor httpContextAccessor;
        
        public AdoNetAnomaliaService(ILogger<AdoNetAnomaliaService> logger,
                                     IDatabaseAccessor db,
                                     IOptionsMonitor<ConnectionStringsOptions> connectionStringOptions,
                                     IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.db = db;
            this.connectionStringOptions = connectionStringOptions;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<AnagraficaViewModel> GetAnagraficaAsync(int id)
        {
            logger.LogInformation("Nag {id} requested", id);

            FormattableString query = $"SELECT NAG, Intestazione FROM Anag_Anagrafe_Generale WHERE NAG={@id}";
            DataSet dataSet = await db.QueryAsync("Anagrafe", query);

            var dataTable = dataSet.Tables[0];
            if (dataTable.Rows.Count != 1) {
                logger.LogWarning("Nag {id} not found", id);
                throw new AnomaliaNotFoundException(id);
            }
            var dataRow = dataTable.Rows[0];
            var anagrafeViewModel = AnagraficaViewModel.FromDataRow(dataRow);

            return anagrafeViewModel;
        }
        
        public async Task ForzaAnomaliaAsync(int id)
        {
            int nag = id;
            // string utente = Environment.UserName;
            string utente = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonimo";
            DateTime dataForzatura = DateTime.Now;

            try
            {
                FormattableString query = $"INSERT INTO Attr.Anomalie_Registrazione_Forzature (NAG, Utente, Data_Forzatura) VALUES ({nag}, {utente}, {dataForzatura})";
                DataSet dataSet = await db.QueryAsync("AppXteDb", query);
            }
            catch (SqlException exc) when (exc.Number == 2601)
            {
                throw new AnomaliaNagDuplicateException(nag, exc);
            }
        }

        public async Task<List<AnomaliaRegistrazioneViewModel>> GetAnomalieRegistrazioniAsync()
        {
            FormattableString query = $"SELECT NAG, codice_fiscale, INTESTAZIONE, Id_Socio, Anomalia_Des FROM XTE.Anomalie_Registrazione_Forzabili";
            DataSet dataSet = await db.QueryAsync("AppXteDb", query);
            var dataTable = dataSet.Tables[0];
            var anomaliaRegistrazioneList = new List<AnomaliaRegistrazioneViewModel>();
            foreach(DataRow anomaliaRegistrazioneRow in dataTable.Rows) {
                AnomaliaRegistrazioneViewModel anomaliaRegistrazioneViewModel = AnomaliaRegistrazioneViewModel.FromDataRow(anomaliaRegistrazioneRow);
                anomaliaRegistrazioneList.Add(anomaliaRegistrazioneViewModel);
            }
            return anomaliaRegistrazioneList;
        }
    }
}