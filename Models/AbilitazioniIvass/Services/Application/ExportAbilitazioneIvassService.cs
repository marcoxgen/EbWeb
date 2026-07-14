using ClosedXML.Excel;
using EbWeb.Models.AbilitazioniIvass.ViewModels;

namespace EbWeb.Models.AbilitazioniIvass.Services.Application;

public class ExportAbilitazioneIvassService : IExportAbilitazioneIvassService
{
    public byte[] GenerateAbilitazioniExcel(IEnumerable<AbilitazioneIvassDetailViewModel> viewModel)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Abilitazioni IVASS");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Matricola";
            worksheet.Cell(1, 3).Value = "Intestazione";
            worksheet.Cell(1, 4).Value = "Unità Organizzativa";
            worksheet.Cell(1, 5).Value = "Codice Fiscale";
            worksheet.Cell(1, 6).Value = "Ruolo";
            worksheet.Cell(1, 7).Value = "Formato MiFID";
            worksheet.Cell(1, 8).Value = "Abilitazione Finanza";
            worksheet.Cell(1, 9).Value = "Abilitazione Operatività IVASS";
            worksheet.Cell(1, 10).Value = "Data Abilitazione IVASS";
            worksheet.Cell(1, 11).Value = "Data Fine Abilitazione IVASS";
            worksheet.Cell(1, 12).Value = "Data Esame";
            worksheet.Cell(1, 13).Value = "Data Abilitazione Operativa";
            worksheet.Cell(1, 14).Value = "Data Fine Abilitazione Operativa";
            worksheet.Cell(1, 15).Value = "Data Sospensione";
            worksheet.Cell(1, 16).Value = "Data Termine Sospensione";
            worksheet.Cell(1, 17).Value = "Formazione 2021";
            worksheet.Cell(1, 18).Value = "Formazione 2022";
            worksheet.Cell(1, 19).Value = "Formazione 2023";
            worksheet.Cell(1, 20).Value = "Formazione 2024";
            worksheet.Cell(1, 21).Value = "Formazione 2025";
            worksheet.Cell(1, 22).Value = "Formazione 2026";
            worksheet.Cell(1, 23).Value = "Note";
            worksheet.Cell(1, 24).Value = "Data Ultimo Aggiornamento";

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

            int currentRow = 2;
            foreach (var item in viewModel)
            {
                worksheet.Cell(currentRow, 1).Value = item.Id;
                worksheet.Cell(currentRow, 2).Value = item.Matricola;
                worksheet.Cell(currentRow, 3).Value = item.Intestazione;
                worksheet.Cell(currentRow, 4).Value = item.DescrUO;
                worksheet.Cell(currentRow, 5).Value = item.CodiceFiscale;
                worksheet.Cell(currentRow, 6).Value = item.Ruolo;
                worksheet.Cell(currentRow, 7).Value = item.FlagFormatoMifid == true ? "SI" : "NO";
                worksheet.Cell(currentRow, 8).Value = item.FlagAbilitatoFinanza == true ? "SI" : "NO";
                worksheet.Cell(currentRow, 9).Value = item.AbilitatoOperativitaIvass == true ? "SI" : "NO";
                worksheet.Cell(currentRow, 10).Value = item.DataAbilitazioneIvass?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 11).Value = item.DataFineAbilitazioneIvass?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 12).Value = item.DataEsame?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 13).Value = item.DataAbilitazioneOperativa?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 14).Value = item.DataFineAbilitazioneOperativa?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 15).Value = item.DataSospensione?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 16).Value = item.DataTermineSospensione?.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 17).Value = item.Formazione2021;
                worksheet.Cell(currentRow, 18).Value = item.Formazione2022;
                worksheet.Cell(currentRow, 19).Value = item.Formazione2023;
                worksheet.Cell(currentRow, 20).Value = item.Formazione2024;
                worksheet.Cell(currentRow, 21).Value = item.Formazione2025;
                worksheet.Cell(currentRow, 22).Value = item.Formazione2026;
                worksheet.Cell(currentRow, 23).Value = item.Note;
                worksheet.Cell(currentRow, 24).Value = item.DataUltimoAggiornamento?.ToString("dd/MM/yyyy");

                currentRow++;
            }
            
            worksheet.Range(1, 1, currentRow - 1, 24).SetAutoFilter();        
            worksheet.SheetView.FreezeRows(1);
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}