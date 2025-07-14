using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application
{
    public static class PdfUtils
    {
        public static byte[] CreaLegendaPdf(List<RevisioneViewModel> revisioni)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(6));

                    page.Header()
                        .ShowOnce()
                        .Element(container => container
                            .PaddingBottom(10) // Spazio dopo l'header
                            .Text(revisioni.First().Filtro)
                            .FontSize(14)
                        );
                        
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderStyle).Text("Indicatori").FontColor(Colors.White).SemiBold();
                            header.Cell().Element(HeaderStyle).Text("Info").FontColor(Colors.White).SemiBold();
                            header.Cell().Element(HeaderStyle).Text("Note Istruttore").FontColor(Colors.White).SemiBold();
                        });

                        bool isEven = false;
                        foreach (var rev in revisioni)
                        {
                            var bgColor = rev.Tipo_Colonna == 2
                                ? Colors.Yellow.Medium
                                : isEven ? Colors.Blue.Lighten5 : Colors.White;
                            isEven = !isEven;

                            table.Cell().Element(container => CellStyle(container, bgColor)).Text(rev.NomeColonna);
                            table.Cell().Element(container => CellStyle(container, bgColor)).Text(text =>
                            {
                                if (rev.Info == "SI" || rev.Info == "ACCESO")
                                {
                                    text.DefaultTextStyle(x => x.FontColor(Colors.Red.Medium).SemiBold());
                                }

                                text.Span(rev.Info);
                            });
                            table.Cell().Element(container => CellStyle(container, bgColor)).Text(rev.NoteIstruttore);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Pagina ");
                            text.CurrentPageNumber();
                            text.Span(" di ");
                            text.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        }

        public static byte[] UnisciPdf(params byte[][] pdfFiles)
        {
            using var outputDocument = new PdfDocument();

            foreach (var pdf in pdfFiles)
            {
                using var inputStream = new MemoryStream(pdf);
                using var inputDocument = PdfReader.Open(inputStream, PdfDocumentOpenMode.Import);
                for (int i = 0; i < inputDocument.PageCount; i++)
                    outputDocument.AddPage(inputDocument.Pages[i]);
            }

            using var outputStream = new MemoryStream();
            outputDocument.Save(outputStream);
            return outputStream.ToArray();
        }

        static IContainer HeaderStyle(IContainer container)
        {
            return container
                //.Background(Colors.Blue.Lighten2)
                .Background("#54B5CC")
                .Border(0.5f)
                .BorderColor(Colors.Cyan.Medium)
                .AlignCenter()
                .AlignMiddle();
        }

        static IContainer CellStyle(IContainer container, string bgColor)
        {
            return container
                .Background(bgColor)
                .Border(0.5f)
                .BorderColor(Colors.Cyan.Medium)
                .AlignLeft()
                .AlignMiddle()
                .Padding(2);
        }
    }
}