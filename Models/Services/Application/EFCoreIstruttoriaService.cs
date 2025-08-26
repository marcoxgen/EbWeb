using EbWeb.Models.Entities;
using EbWeb.Models.InputModels;
using EbWeb.Models.Services.Infrastructure;
using EbWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Models.Services.Application
{
    public class EFCoreIstruttoriaService : IIstruttoriaService
    {
        private readonly IstruttoriaDbContext dbContext;

        public EFCoreIstruttoriaService(IstruttoriaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ListViewModel<IstruttoriaViewModel>> GetIstruttorieAsync(IstruttoriaListInputModel model)
        {
            IQueryable<Istruttoria> baseQuery = dbContext.Istruttorie;

            switch(model.OrderBy)
            {
                case "Istruttore":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(istruttoria => istruttoria.Istruttore);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(istruttoria => istruttoria.Istruttore);
                    }
                    break;
                case "Nag":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(istruttoria => istruttoria.Nag);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(istruttoria => istruttoria.Nag);
                    }
                    break;
                case "Cluster_Pratica":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(istruttoria => istruttoria.Cluster_Pratica);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(istruttoria => istruttoria.Cluster_Pratica);
                    }
                    break;
            }

            IQueryable<IstruttoriaViewModel> queryLinq = baseQuery
                .Where(istruttoria => istruttoria.Intestazione.Contains(model.Search))
                .AsNoTracking()
                .Select(istruttoria => IstruttoriaViewModel.FromEntity(istruttoria)); //Usando metodi statici come FromEntity, la query potrebbe essere inefficiente. Mantenere il mapping nella lambda oppure usare un extension method personalizzato

            List<IstruttoriaViewModel> courses = await queryLinq
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToListAsync(); //La query al database viene inviata qui, quando manifestiamo l'intenzione di voler leggere i risultati

            int totalCount = await queryLinq.CountAsync();

            ListViewModel<IstruttoriaViewModel> result = new ListViewModel<IstruttoriaViewModel>
            {
                Results = courses,
                TotalCount = totalCount
            };

            return result;
        }
    }
}