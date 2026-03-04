using System.Data;
using EbWeb.Models.Exceptions.Application;
using EbWeb.Models.InputModels;
using EbWeb.Models.Options;
using EbWeb.Models.Services.Infrastructrure;
using EbWeb.Models.ValueObjects;
using EbWeb.Models.ViewModels;
using Microsoft.Extensions.Options;

namespace EbWeb.Models.Services.Application
{
    public class AdoNetRevisioneService : IRevisioneService
    {
        private readonly IDatabaseAccessor db;
        private readonly IOptionsMonitor<RevisioniOptions> revisioniOptions;

        public AdoNetRevisioneService(IDatabaseAccessor db, IOptionsMonitor<RevisioniOptions> revisioniOptions)
        {
            this.db = db;
            this.revisioniOptions = revisioniOptions;
        }

        public async Task<ListViewModel<RevisioneViewModel>> GetRevisioniAsync(RevisioneListInputModel model)
        {
            string orderby = model.OrderBy;
            string direction = model.Ascending ? "ASC" : "DESC";

            FormattableString query = $@"SELECT DISTINCT Nag_Affidato, Intestazione, Filtro FROM REV.Revisioni_Semplificate_BI WHERE Filtro LIKE {"%" + model.Search + "%"} ORDER BY {(Sql)orderby} {(Sql)direction} OFFSET {model.Offset} ROWS FETCH NEXT {model.Limit} ROWS ONLY; 
            SELECT COUNT(DISTINCT Nag_Affidato) FROM REV.Revisioni_Semplificate_BI WHERE Filtro LIKE {"%" + model.Search + "%"}";
            DataSet dataSet = await db.QueryAsync("Processo_Credito", query);
            var dataTable = dataSet.Tables[0];
            var revisioneList = new List<RevisioneViewModel>();
            foreach (DataRow revisioneRow in dataTable.Rows)
            {
                RevisioneViewModel revisioneViewModel = RevisioneViewModel.FromDataRowSearch(revisioneRow);
                revisioneList.Add(revisioneViewModel);
            }

            ListViewModel<RevisioneViewModel> result = new ListViewModel<RevisioneViewModel>
            {
                Results = revisioneList,
                TotalCount = Convert.ToInt32(dataSet.Tables[1].Rows[0][0])
            };

            return result;
        }

        public async Task<List<RevisioneViewModel>> GetRevisioneAsync(int nag)
        {
            FormattableString query = $"SELECT * FROM REV.Revisioni_Semplificate_BI WHERE Nag_Affidato={nag} ORDER BY NomeColonna";
            DataSet dataSet = await db.QueryAsync("Processo_Credito", query);
            var dataTable = dataSet.Tables[0];
            var revisioneList = new List<RevisioneViewModel>();
            foreach (DataRow revisioneRow in dataTable.Rows)
            {
                RevisioneViewModel revisioneViewModel = RevisioneViewModel.FromDataRow(revisioneRow);
                revisioneList.Add(revisioneViewModel);
            }
            return revisioneList;
        }

        public async Task<int> EditNoteAsync(int id, string note)
        {
            FormattableString cmd = $"UPDATE REV.Revisioni_Semplificate_BI SET [Note_Istruttore]={note} WHERE Id={id}";
            int affectedRows = await db.CommandAsync("Processo_Credito", cmd);
            if (affectedRows == 0)
            {
                FormattableString query = $"SELECT COUNT(*) FROM REV.Revisioni_Semplificate_BI WHERE Id={id}";
                bool lessonExists = await db.QueryScalarAsync<bool>("Processo_Credito", query);
                if (lessonExists)
                {
                    throw new OptimisticConcurrencyException();
                }
                else
                {
                    throw new RevisioneNotFoundException(id);
                }
            }
            return affectedRows;
        }
    }
}