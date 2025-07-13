using EbWeb.Models.Services.Infrastructure;
using EbWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Application
{
    public class EFCoreAnomaliaService : IAnomaliaService
    {
        private readonly DB_AnagrafeDbContext dbContext;
        public EFCoreAnomaliaService(DB_AnagrafeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AnagraficaViewModel> GetAnagraficaAsync(int id)
        {
            AnagraficaViewModel viewModel = await dbContext.AnagAnagrafeGenerales
                .AsNoTracking()
                .Where(anomalie => anomalie.Nag == id)
                .Select(anomalie => new AnagraficaViewModel
                {
                    Nag = anomalie.Nag,
                    Intestazione = anomalie.Intestazione
                })
                .SingleAsync();

            return viewModel;
        }

        public Task<List<AnomaliaRegistrazioneViewModel>> GetAnomaliaRegistrazioniAsync()
        {
            throw new NotImplementedException();
        }

        public Task ReportAnomalieAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}