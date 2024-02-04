using Application.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents;

// Represents a PDF document for a fishing license bill
public class FishingLicenseBillDocument(byte[] qrBill, FishingLicenseBill fishingLicenseBill) : IDocument
{
    // Implementation of the Compose method from the IDocument interface
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4.Portrait());

            // Define header, content, and footer for the document
            page.Header().PaddingTop(50).PaddingLeft(50).PaddingRight(50).Element(ComposeHeader);
            page.Content().PaddingLeft(50).PaddingRight(50).Element(ComposeContent);
            page.Footer().Image(qrBill);
        });
    }

    // Method to compose the header section of the document
    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            // Header content with relevant information
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Rechnung Fischerkarte {fishingLicenseBill.LicenseYear}").FontSize(20).SemiBold()
                    .FontColor(Colors.Blue.Medium);

                // Invoice date information
                column.Item().Text(text =>
                {
                    text.Span("Rechnungsdatum: ").SemiBold();
                    text.Span($"{fishingLicenseBill.InvoiceDate:dd.MM.yyyy}");
                });

                // Payment due date information
                column.Item().Text(text =>
                {
                    text.Span("Zahlbar bis: ").SemiBold();
                    text.Span($"{fishingLicenseBill.InvoiceDate.AddDays(30):dd.MM.yyyy}");
                });
            });
        });
    }

    // Method to compose the content section of the document
    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            // Address information (sender and receiver)
            column.Item().Row(row =>
            {
                row.RelativeItem().Element(ComposeToAddress);
                row.ConstantItem(50);
                row.RelativeItem().Element(ComposeFromAddress);
            });

            // Table displaying the details of the fishing license bill
            column.Item().PaddingTop(20).Element(ComposeTable);

            // Total amount information
            column.Item().AlignRight().Text($"Rechnungsbetrag: CHF {fishingLicenseBill.Amount:##.00}").FontSize(14);

            // Comments section
            column.Item().PaddingTop(25).Element(ComposeComments);
        });
    }

    // Method to compose the comments section of the document
    private static void ComposeComments(IContainer container)
    {
        container.Background(Colors.Grey.Lighten5).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Zahlungsbedingungen: Zahlung innerhalb 30 Tagen ab Rechnungseingang");
        });
    }

    // Method to compose the table section of the document
    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            // Define columns for the table
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(2);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            // Header row of the table
            table.Header(header =>
            {
                header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).Text("Pos.");
                header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).Text("Bezeichnung");
                header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).AlignRight().Text("Menge");
                header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).AlignRight()
                    .Text("Einzelpreis");
                header.Cell().Background(Colors.Grey.Lighten3).Element(HeaderCellStyle).AlignRight()
                    .Text("Gesamtpreis");

                // Styling for the header cells
                static IContainer HeaderCellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1)
                        .BorderColor(Colors.Black);
                }
            });

            // Data row of the table
            table.Cell().Element(CellStyle).Text("1");
            table.Cell().Element(CellStyle).Text($"Fischerkarte {fishingLicenseBill.LicenseYear}");
            table.Cell().Element(CellStyle).AlignRight().Text("1");
            table.Cell().Element(CellStyle).AlignRight().Text(fishingLicenseBill.Amount.ToString("##.00"));
            table.Cell().Element(CellStyle).AlignRight().Text(fishingLicenseBill.Amount.ToString("##.00"));

            // Styling for the data cells
            static IContainer CellStyle(IContainer container)
            {
                return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }

    // Method to compose the sender's address section of the document
    private void ComposeFromAddress(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            // Sender's address information
            column.Item().BorderBottom(1).PaddingBottom(5).Text("VON").SemiBold();
            column.Item().Text(fishingLicenseBill.FishingClubName);
            column.Item().Text(fishingLicenseBill.CreditorName);
            column.Item().Text(fishingLicenseBill.CreditorAddress);
            column.Item().Text(fishingLicenseBill.CreditorCity);
        });
    }

    // Method to compose the receiver's address section of the document
    private void ComposeToAddress(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            // Receiver's address information
            column.Item().BorderBottom(1).PaddingBottom(5).Text("AN").SemiBold();
            column.Item().Text(fishingLicenseBill.DebtorName);
            column.Item().Text(fishingLicenseBill.DebtorAddress);
            column.Item().Text(fishingLicenseBill.DebtorCity);
        });
    }
}