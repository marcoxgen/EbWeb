using EbWeb.Models.Exceptions.Application;
using EbWeb.Models.InputModels;
using EbWeb.Models.Options;
using EbWeb.Models.Results;
using EbWeb.Models.Services.Infrastructure;
using EbWeb.Models.ValueObjects;
using EbWeb.Models.ViewModels;
using Microsoft.Extensions.Options;
using System.Data;

namespace EbWeb.Models.Services.Application;

public class AdoNetRevisioneService : IRevisioneService
{
    private readonly IDatabaseAccessor db;
    private readonly IOptionsMonitor<RevisioniOptions> revisioniOptions;
    private readonly IWebHostEnvironment environment;

    public AdoNetRevisioneService(IDatabaseAccessor db, IOptionsMonitor<RevisioniOptions> revisioniOptions, IWebHostEnvironment environment)
    {
        this.db = db;
        this.revisioniOptions = revisioniOptions;
        this.environment = environment;
    }

    public async Task<ListViewModel<RevisioneViewModel>> GetRevisioniAsync(RevisioneListInputModel model)
    {
        string orderby = model.OrderBy;
        string direction = model.Ascending ? "ASC" : "DESC";

        FormattableString query = $@"SELECT DISTINCT Nag_Affidato, Intestazione, Info, Filtro FROM REV.Revisioni_Semplificate_BI WHERE NomeColonna LIKE '[0-9][0-9].[0-9][0-9] - Revisione semplificata' AND Filtro LIKE {"%" + model.Search + "%"} ORDER BY {(Sql)orderby} {(Sql)direction} OFFSET {model.Offset} ROWS FETCH NEXT {model.Limit} ROWS ONLY;
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
        var colonneObbligatorie = revisioniOptions.CurrentValue.ColonneObbligatorie;
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

    public async Task<CreatePdfResult> CreatePdfAsync(int nag)
    {
        List<RevisioneViewModel> revisione = await GetRevisioneAsync(nag);

        var obbligatorie = revisioniOptions.CurrentValue.ColonneObbligatorie;

        var campiMancanti = revisione
            .Where(r =>
                obbligatorie.Any(c =>
                    r.NomeColonna.StartsWith(c, StringComparison.OrdinalIgnoreCase))
                && string.IsNullOrWhiteSpace(r.NoteIstruttore))
            .Select(r => r.NomeColonna)
            .ToList();

        if (campiMancanti.Any())
        {
            return new CreatePdfResult
            {
                Success = false,
                CampiMancanti = campiMancanti
            };
        }

        var pdfLegenda = PdfUtils.CreaLegendaPdf(revisione);

        var path = Path.Combine(
            environment.WebRootPath,
            "pdf",
            "Legenda report.pdf");

        var pdfEsistente = await File.ReadAllBytesAsync(path);

        var pdfFinale = PdfUtils.UnisciPdf(
            pdfLegenda,
            pdfEsistente);

        return new CreatePdfResult
        {
            Success = true,
            Pdf = pdfFinale
        };
    }
}