using Application.DataTransferObjects.FishingRegulation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents;

// Represents a PDF document for fishing regulations
public class FishingRulesDocument(List<FishingRegulationDto> fishingRegulations) : IDocument
{
    // Implementation of the Compose method from the IDocument interface
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            // Set page margins and size
            page.Margin(20);
            page.MarginLeft(50);
            page.MarginRight(50);
            page.Size(PageSizes.A4.Portrait());

            // Create header, content, and footer for the document
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Text($"Baden, {DateTime.Now:dd.MMMM yyyy}");
        });
    }

    // Method to compose the content section of the document
    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            var index = 1;

            // Iterate through each fishing regulation and compose a row for each
            foreach (var rule in fishingRegulations)
                column.Item().Row(row =>
                {
                    row.AutoItem().PaddingLeft(20).Text($"{index}. ").Bold();
                    row.RelativeItem().PaddingBottom(10).Text(rule.Regulation);
                    index++;
                });

            // Additional rows for club information
            column.Item().Row(row =>
            {
                row.RelativeItem().AlignCenter().PaddingTop(10).Text("FISCHERCLUB DÄTTWILERWEIHER").Bold()
                    .FontSize(20);
            });
            column.Item().Row(row => { row.RelativeItem().AlignCenter().PaddingTop(5).Text("Der Vorsitzende"); });
            column.Item().Row(row => { row.RelativeItem().AlignCenter().PaddingTop(5).Text("Fritz Wanner"); });
        });
    }

    // Method to compose the header section of the document
    private static void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            // Header content with title and introductory text
            column.Item().Text("Vorschriften").Bold().FontSize(28);
            column.Item().Text(text => text.EmptyLine());
            column.Item().Text(
                "Der Inhaber dieser Karte ist berechtigt im Dättwilerweiher vom Ufer aus und mit dem Boot" +
                " zu fischen und hat folgendes zu beachten:").FontSize(12);
            column.Item().Text(text => text.EmptyLine());
        });
    }
}