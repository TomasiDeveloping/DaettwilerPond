using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents
{
    public class FishingLicenseBillDocument : IDocument
    {
        private readonly byte[] _qrBill;

        public FishingLicenseBillDocument(byte[] qrBill)
        {
            _qrBill = qrBill;
        }
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                //page.Margin(50);
                page.Size(PageSizes.A4.Portrait());

                page.Header().PaddingTop(50).PaddingLeft(50).PaddingRight(50).Element(ComposeHeader);
                page.Content().PaddingLeft(50).PaddingRight(50).Element(ComposeContent);
                page.Footer().Image(_qrBill);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Rechnung Fischerkarte 2023").FontSize(20).SemiBold()
                        .FontColor(Colors.Blue.Medium);

                    column.Item().Text(text =>
                    {
                        text.Span("Rechnungsdatum: ").SemiBold();
                        text.Span($"{DateTime.Now:dd.MM.yyyy}");
                    });
                    column.Item().Text(text =>
                    {
                        text.Span("Zahlbar bis: ").SemiBold();
                        text.Span($"{(DateTime.Now).AddDays(30):dd.MM.yyyy}");
                    });
                });
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(ComposeToAddress);
                    row.ConstantItem(50);
                    row.RelativeItem().Element(ComposeFromAddress);
                });

                column.Item().PaddingTop(20).Element(ComposeTable);

                column.Item().AlignRight().Text("Rechnungsbetrag: CHF 500.00").FontSize(14);

                column.Item().PaddingTop(25).Element(ComposeComments);
            });
        }

        private void ComposeComments(IContainer container)
        {
            container.Background(Colors.Grey.Lighten5).Padding(10).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Zahlungsbedingungen: Zahlung innerhalb 30 Tagen ab Rechnungseingang");
            });
        }

        private void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).Text("Pos.");
                    header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).Text("Bezeichnung");
                    header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).AlignRight().Text("Menge");
                    header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).AlignRight().Text("Einzelpreis");
                    header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).AlignRight().Text("Gesamtpreis");

                    static IContainer HeaderCellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                table.Cell().Element(CellStyle).Text("1");
                table.Cell().Element(CellStyle).Text("Fischerkarte 2023");
                table.Cell().Element(CellStyle).AlignRight().Text("1");
                table.Cell().Element(CellStyle).AlignRight().Text("500.00");
                table.Cell().Element(CellStyle).AlignRight().Text("500.00");
                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            });
        }

        private void ComposeFromAddress(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().BorderBottom(1).PaddingBottom(5).Text("VON").SemiBold();

                column.Item().Text("Fischerclub Dättwilerweiher");
                column.Item().Text("Friedrich Alexander Wanner");
                column.Item().Text("Obere Kehlstrasse 10");
                column.Item().Text("5400 Baden");
            });
        }

        private void ComposeToAddress(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().BorderBottom(1).PaddingBottom(5).Text("AN").SemiBold();

                column.Item().Text("Tomasi Patrick");
                column.Item().Text("Bühlstrasse 190");
                column.Item().Text("4468 Kienberg");
            });
        }
    }
}
