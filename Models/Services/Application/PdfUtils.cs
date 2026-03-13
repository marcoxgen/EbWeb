using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

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
                        .PaddingBottom(10)
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
                            if (rev.Info == "SI" || rev.Info == "ACCESO" || rev.Info == "Indicatore non rispettato")
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

    public static byte[] GeneraTabellaBudgetPdf(List<SchedaBudgetViewModel> schedeBudget)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var gruppiPerUO = schedeBudget
            .GroupBy(x => new { x.ID_UO, x.EtichettaUO })
            .ToList();

        var document = Document.Create(container =>
        {
            foreach (var gruppo in gruppiPerUO)
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(0.5f, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(7f));

                    page.Header().PaddingBottom(5)
                        .Text(schedeBudget.FirstOrDefault()?.EtichettaData ?? "")
                        .FontSize(14).SemiBold();

                    page.Content().Column(col =>
                    {
                        col.Item().Background(Colors.Grey.Lighten2).Padding(3)
                            .Text(gruppo.Key.EtichettaUO).SemiBold().FontSize(9);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.5f);
                                for (int i = 0; i < 13; i++) columns.RelativeColumn(1f);
                            });

                            void RenderHeaderRows(string tipologia)
                            {
                                table.Cell().Element(BlueHeaderStyle).Text($"Metrica - {tipologia}");
                                table.Cell().Element(BlueHeaderStyle).Text("Dicembre AP");
                                table.Cell().Element(BlueHeaderStyle).Text("Rettifiche");
                                table.Cell().Element(BlueHeaderStyle).Text("Dicembre AP Rettificato");
                                table.Cell().Element(BlueHeaderStyle).Text("Stesso Periodo AP");
                                table.Cell().Element(BlueHeaderStyle).Text("Valore Alla Data");
                                table.Cell().Element(BlueHeaderStyle).Text("Flusso Alla Data");
                                table.Cell().Element(BlueHeaderStyle).Text("Obiettivo Annuale");
                                table.Cell().Element(BlueHeaderStyle).Text("Valore da Raggiungere Annuale");
                                table.Cell().Element(BlueHeaderStyle).Text("% Ragg.to Annuale");
                                table.Cell().Element(BlueHeaderStyle).Text("Obiettivo Mensile");
                                table.Cell().Element(BlueHeaderStyle).Text("Valore da Raggiungere Mensile");
                                table.Cell().Element(BlueHeaderStyle).Text("% Ragg.to Mensile");
                                table.Cell().Element(BlueHeaderStyle).Text("Punti");
                            }

                            table.Header(header => 
                            {
                                var primaTipologia = gruppo.FirstOrDefault()?.TipologiaDes ?? "";
                                RenderHeaderRows(primaTipologia);
                            });

                            string ultimaTipologia = null;

                            foreach (var item in gruppo)
                            {
                                if (ultimaTipologia != null && ultimaTipologia != item.TipologiaDes)
                                {
                                    RenderHeaderRows(item.TipologiaDes);
                                }
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).Text(item.MetricaDes);
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(item.ValoreFineAnnoPrecedente != 0 ? item.ValoreFineAnnoPrecedente?.ToString("N0") : "");
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(txt =>
                                    {
                                        if (item.Rettifiche != 0 && item.Rettifiche != null)
                                        {
                                            txt.Span(item.Rettifiche?.ToString("N0"))
                                            .FontColor(item.Rettifiche < 0 ? Colors.Red.Medium : Colors.Black);
                                        }
                                        else 
                                        {
                                            txt.Span("");
                                        }
                                    });
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(item.ValoreAnnoPrecedenteRettificato != 0 ? item.ValoreAnnoPrecedenteRettificato?.ToString("N0") : "");
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(item.ValoreStessoPeriodoAnnoPrecedenteRettificato != 0 ? item.ValoreStessoPeriodoAnnoPrecedenteRettificato?.ToString("N0") : "");
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(item.ValoreAllaDataRif != 0 ? item.ValoreAllaDataRif?.ToString("N0") : "");
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(txt =>
                                    {
                                        if (item.FlussoAllaData != 0 && item.FlussoAllaData != null)
                                        {
                                            txt.Span(item.FlussoAllaData?.ToString("N0"))
                                            .FontColor(item.FlussoAllaData < 0 ? Colors.Red.Medium : Colors.Black);
                                        }
                                        else 
                                        {
                                            txt.Span("");
                                        }
                                    });
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.Teal.Lighten3)).AlignRight()
                                    .Text(item.ObiettivoAnnuale != 0 ? item.ObiettivoAnnuale?.ToString("N0") : "");
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(item.ValoreDaRaggiungereAnnuale != 0 ? item.ValoreDaRaggiungereAnnuale?.ToString("N0") : "");
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White))
                                    .PaddingVertical(2)
                                    .Column(c =>
                                    {
                                        c.Item().AlignCenter().Text(item.PercentualeRaggiungimentoObiettivoAnnuale ?? "0%")
                                            .FontSize(6)
                                            .SemiBold()
                                            .FontColor(Colors.Black);

                                        c.Item().PaddingTop(2).AlignCenter().Element(conBar => 
                                            DrawDivergingBar(conBar, item.PercentualeRaggiungimentoObiettivoAnnualeIstogramma ?? 0));
                                    });
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.Cyan.Lighten3)).AlignRight()
                                    .Text(item.ObiettivoMensile != 0 ? item.ObiettivoMensile?.ToString("N0") : "");
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(item.ValoreDaRaggiungereMensile != 0 ? item.ValoreDaRaggiungereMensile?.ToString("N0") : "");
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignCenter()
                                    .Text(item.PercentualeRaggiungimentoObiettivoMensile);
                                table.Cell().Element(con => BudgetCellStyle(con, Colors.White)).AlignRight()
                                    .Text(item.Punteggio != 0 ? item.Punteggio?.ToString("N0") : "");
                                ultimaTipologia = item.TipologiaDes;
                            }
                        });

                        var nMetrichePrincipali = gruppo.Count(x => 
                            string.Equals(x.TipologiaDes, "Principali", StringComparison.OrdinalIgnoreCase) && 
                            x.AdObiettivo == 1
                        );

                        var totalePunti = gruppo.Sum(x => x.Punteggio ?? 0);

                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem();
                            row.Spacing(15);

                            row.AutoItem().Element(CardStyle).Column(c =>
                            {
                                c.Item().Text(nMetrichePrincipali.ToString())
                                    .FontSize(12).SemiBold().FontColor(Colors.Black);
                                
                                c.Item().Text("N.RO METRICHE PRINCIPALI (NO BONUS) CON % RAGG >=75%")
                                    .FontSize(6).FontColor(Colors.Grey.Darken2).SemiBold();
                            });

                            row.AutoItem().Element(CardStyle).Column(c =>
                            {
                                c.Item().Text(totalePunti.ToString("N0"))
                                    .FontSize(12).SemiBold().FontColor(Colors.Black);
                                
                                c.Item().Text("TOTALE PUNTI")
                                    .FontSize(6).FontColor(Colors.Grey.Darken2).SemiBold();
                            });
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Pagina ");
                        x.CurrentPageNumber();
                        x.Span(" di ");
                        x.TotalPages();
                    });
                });
            }
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
            .Background("#54B5CC")
            .Border(0.5f)
            .BorderColor(Colors.Cyan.Medium)
            .AlignCenter()
            .AlignMiddle();
    }

    static IContainer BlueHeaderStyle(IContainer container)
    {
        return container
            .Background(Colors.Blue.Darken4)
            .Border(0.5f)
            .BorderColor(Colors.White)
            .AlignCenter()
            .AlignMiddle()
            .DefaultTextStyle(x => x.SemiBold().FontColor(Colors.White).FontSize(7));
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

    static IContainer BudgetCellStyle(IContainer container, string bgColor)
    {
        return container
            .Background(bgColor)
            .Border(0.5f)
            .BorderColor(Colors.Cyan.Medium)
            .AlignMiddle()
            .Padding(2);
    }

    static IContainer CardStyle(IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten4)
            .BorderLeft(2)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(4)       
            .PaddingHorizontal(8);
        }

    static void DrawDivergingBar(IContainer container, double valoreDouble)
    {
        float valore = (float)valoreDouble;
        if (valore > 100) valore = 100;
        if (valore < -100) valore = -100;

        container.Height(6).Row(row =>
        {
            row.RelativeItem(1).Row(leftRow =>
            {
                if (valore < 0)
                {
                    leftRow.RelativeItem(100 + valore); 
                    leftRow.RelativeItem(Math.Abs(valore)).Background(Colors.Red.Medium);
                }
                else
                {
                    leftRow.RelativeItem(1); 
                }
            });

            row.ConstantItem(0.5f).Background(Colors.Black);

            row.RelativeItem(1).Row(rightRow =>
            {
                if (valore > 0)
                {
                    rightRow.RelativeItem(valore).Background(Colors.Green.Medium);
                    rightRow.RelativeItem(100 - valore); 
                }
                else
                {
                    rightRow.RelativeItem(1); 
                }
            });
        });
    }
}