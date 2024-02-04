using Application.DataTransferObjects.FishType;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents;

// Represents a PDF document for fish open season information
public class FishOpenSeasonDocument(List<FishTypeDto> fishTypes) : IDocument
{
    // Text style for the header cells in the table
    private static TextStyle TableHeaderTextStyle => TextStyle
        .Default
        .FontSize(14)
        .Bold();

    // Text style for the body cells in the table
    private static TextStyle TableBodyTextStyle => TextStyle
        .Default
        .FontSize(11);

    // Implementation of the Compose method from the IDocument interface
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            // Set page margins and size
            page.Margin(20);
            page.Size(PageSizes.A4.Landscape());

            // Create header, content, and footer for the document
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Text($"Baden, {DateTime.Now:dd.MMMM yyyy}");
        });
    }

    // Method to compose the content section of the document
    private void ComposeContent(IContainer container)
    {
        container.Table(table =>
        {
            // Define columns for the table
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            // Header row of the table
            table.Header(header =>
            {
                header.Cell().Element(TableHeaderStyle).Text("Fischart").Style(TableHeaderTextStyle);
                header.Cell().Element(TableHeaderStyle).Text("Schonmass").Style(TableHeaderTextStyle);
                header.Cell().Element(TableHeaderStyle).Text("Schonzeit").Style(TableHeaderTextStyle);
            });

            static IContainer TableHeaderStyle(IContainer container)
            {
                return container.Border(1).Background(Colors.Grey.Lighten3).Padding(1).AlignCenter();
            }

            // Data rows for each fish type
            foreach (var fishType in fishTypes)
            {
                table.Cell().Element(TableBodyStyle).Text(fishType.Name).Style(TableBodyTextStyle);
                table.Cell().Element(TableBodyStyle).Text($"{fishType.MinimumSize} cm").Style(TableBodyTextStyle);
                if (fishType.HasClosedSeason)
                    table.Cell().Element(TableBodyStyle)
                        .Text(
                            $"{fishType.ClosedSeasonFromDay}.{fishType.ClosedSeasonFromMonth} - {fishType.ClosedSeasonToDay}.{fishType.ClosedSeasonToMonth}");
                else
                    table.Cell().Element(TableBodyStyle).Text("-").Style(TableBodyTextStyle);


                // Styling for the table cells in the body
                static IContainer TableBodyStyle(IContainer container)
                {
                    return container.Border(1).MinHeight(30).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(1)
                        .AlignCenter().AlignMiddle();
                }
            }
        });
    }

    // Method to compose the header section of the document
    private static void ComposeHeader(IContainer container)
    {
        container.Column(columns =>
        {
            // Header content with club name and document title
            columns.Item().Row(row =>
            {
                row.RelativeItem().PaddingBottom(5).AlignCenter().Text("FISCHERCLUB DÄTTWILERWEIHER").Bold()
                    .FontSize(24);
            });
            columns.Item().Row(row =>
            {
                row.RelativeItem().PaddingBottom(50).AlignCenter().Text("Schonmasse und Schonzeiten").Bold()
                    .FontSize(20);
            });
        });
    }
}