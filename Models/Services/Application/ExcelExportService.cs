using ClosedXML.Excel;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

public class ExcelExportService : IExcelExportService
{
    public byte[] GenerateAbilitazioniExcel(IEnumerable<AbilitazioneMifidDetailViewModel> viewModel)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Abilitazioni MiFID");

            worksheet.Cell(1, 1).Value = "Matricola";
            worksheet.Cell(1, 2).Value = "Intestazione";
            worksheet.Cell(1, 3).Value = "Unità Organizzativa";
            worksheet.Cell(1, 4).Value = "Codice Fiscale";
            worksheet.Cell(1, 5).Value = "Ruolo";
            worksheet.Cell(1, 6).Value = "Titolo di Studio";
            worksheet.Cell(1, 7).Value = "Titolo di Studio Mifid";
            worksheet.Cell(1, 8).Value = "Mesi Periodo Supervisione";
            worksheet.Cell(1, 9).Value = "Data Conseguimento Titolo di Studio";
            worksheet.Cell(1, 10).Value = "Data Abilitazione Mifid";
            worksheet.Cell(1, 11).Value = "Data Sospensione";
            worksheet.Cell(1, 12).Value = "Data Termine Sospensione";
            worksheet.Cell(1, 13).Value = "Necessario Assessment";
            worksheet.Cell(1, 14).Value = "Data Superamento Assessment";
            worksheet.Cell(1, 15).Value = "Data Abilitazione Titoli";
            worksheet.Cell(1, 16).Value = "Anni Esperienza Adeguata";
            worksheet.Cell(1, 17).Value = "Data Inizio Supervisione";
            worksheet.Cell(1, 18).Value = "Data Fine Supervisione";
            worksheet.Cell(1, 19).Value = "Matricola Supervisore";
            worksheet.Cell(1, 20).Value = "Intestazione Supervisore";
            worksheet.Cell(1, 21).Value = "Matricola Sostituto Supervisore";
            worksheet.Cell(1, 22).Value = "Intestazione Sostituto Supervisore";
            worksheet.Cell(1, 23).Value = "Formazione 2024";
            worksheet.Cell(1, 24).Value = "Formazione 2025";
            worksheet.Cell(1, 25).Value = "Note";
            worksheet.Cell(1, 26).Value = "Abilitato Finance/WMP";
            worksheet.Cell(1, 27).Value = "Escluso";
            worksheet.Cell(1, 28).Value = "Data Ultimo Aggiornamento";
            worksheet.Cell(1, 29).Value = "Nota Log";

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

            int currentRow = 2;
            foreach (var item in viewModel)
            {
                worksheet.Cell(currentRow, 1).Value = item.Matricola;
                worksheet.Cell(currentRow, 2).Value = item.Intestazione;
                worksheet.Cell(currentRow, 3).Value = item.DescrUO;
                worksheet.Cell(currentRow, 4).Value = item.CodiceFiscale;
                worksheet.Cell(currentRow, 5).Value = item.Ruolo;
                worksheet.Cell(currentRow, 6).Value = item.TitoloStudio;
                worksheet.Cell(currentRow, 7).Value = item.TitoloStudioMifid;
                worksheet.Cell(currentRow, 8).Value = item.MesiPeriodoSupervisione;
                worksheet.Cell(currentRow, 9).Value = item.DataConseguimentoTitoloStudio?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 10).Value = item.DataAbilitazioneMifid?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 11).Value = item.DataSospensione?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 12).Value = item.DataTermineSospensione?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 13).Value = item.NecessarioAssessment == true ? "SI" : "NO";
                worksheet.Cell(currentRow, 14).Value = item.DataSuperamentoAssessment?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 15).Value = item.DataAbilitazioneTitoli?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 16).Value = item.AnniEsperienzaAdeguata;
                worksheet.Cell(currentRow, 17).Value = item.DataInizioSupervisione?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 18).Value = item.DataFineSupervisione?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 19).Value = item.MatricolaSupervisore;
                worksheet.Cell(currentRow, 20).Value = item.IntestazioneSupervisore;
                worksheet.Cell(currentRow, 21).Value = item.MatricolaSostitutoSupervisore;
                worksheet.Cell(currentRow, 22).Value = item.IntestazioneSostitutoSupervisore;
                worksheet.Cell(currentRow, 23).Value = item.Formazione2024;
                worksheet.Cell(currentRow, 24).Value = item.Formazione2025;
                worksheet.Cell(currentRow, 25).Value = item.Note;
                worksheet.Cell(currentRow, 26).Value = item.AbilitatoFinanceWMP == true ? "SI" : "NO";
                worksheet.Cell(currentRow, 27).Value = item.Escluso == true ? "SI" : "NO";
                worksheet.Cell(currentRow, 28).Value = item.DataUltimoAggiornamento?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 29).Value = item.NotaLog;

                currentRow++;
            }
            
            worksheet.Range(1, 1, currentRow - 1, 29).SetAutoFilter();        
            worksheet.SheetView.FreezeRows(1);
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}