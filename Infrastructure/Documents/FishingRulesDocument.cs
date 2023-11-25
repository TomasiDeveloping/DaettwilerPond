using Application.DataTransferObjects.FishingRegulation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents;

public class FishingRulesDocument(List<FishingRegulationDto> fishingRegulations) : IDocument
{
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(20);
            page.MarginLeft(50);
            page.MarginRight(50);
            page.Size(PageSizes.A4.Portrait());
            // Create header
            page.Header().Element(ComposeHeader);
            // Create content with fishing rules
            page.Content().Element(ComposeContent);
            // Create footer with current date
            page.Footer().Text($"Baden, {DateTime.Now:dd.MMMM yyyy}");
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            var index = 1;
            foreach (var rule in fishingRegulations)
                column.Item().Row(row =>
                {
                    row.AutoItem().PaddingLeft(20).Text($"{index}. ").Bold();
                    row.RelativeItem().PaddingBottom(10).Text(rule.Regulation);
                    index++;
                });
            column.Item().Row(row =>
            {
                row.RelativeItem().AlignCenter().PaddingTop(10).Text("FISCHERCLUB DÄTTWILERWEIHER").Bold()
                    .FontSize(20);
            });
            column.Item().Row(row => { row.RelativeItem().AlignCenter().PaddingTop(5).Text("Der Vorsitzende"); });
            column.Item().Row(row => { row.RelativeItem().AlignCenter().PaddingTop(5).Text("Fritz Wanner"); });
        });
    }

    private static void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Vorschriften").Bold().FontSize(28);
            column.Item().Text(text => text.EmptyLine());
            column.Item().Text(
                "Der Inhaber dieser Karte ist berechtigt im Dättwilerweiher vom Ufer aus und mit dem Boot" +
                " zu fischen und hat folgendes zu beachten:").FontSize(12);
            column.Item().Text(text => text.EmptyLine());
        });
    }
}