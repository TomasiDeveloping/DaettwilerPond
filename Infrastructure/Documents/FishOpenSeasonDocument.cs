using Application.DataTransferObjects.FishType;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents;

public class FishOpenSeasonDocument : IDocument
{
    private readonly List<FishTypeDto> _fishTypes;

    public FishOpenSeasonDocument(List<FishTypeDto> fishTypes)
    {
        _fishTypes = fishTypes;
    }

    private static TextStyle TableHeaderTextStyle => TextStyle
        .Default
        .FontSize(14)
        .Bold();

    private static TextStyle TableBodyTextStyle => TextStyle
        .Default
        .FontSize(11);

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(20);
            page.Size(PageSizes.A4.Landscape());
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Text($"Baden, {DateTime.Now:dd.MMMM yyyy}");
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });
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

            foreach (var fishType in _fishTypes)
            {
                table.Cell().Element(TableBodyStyle).Text(fishType.Name).Style(TableBodyTextStyle);
                table.Cell().Element(TableBodyStyle).Text($"{fishType.MinimumSize} cm").Style(TableBodyTextStyle);
                if (fishType.HasClosedSeason)
                    table.Cell().Element(TableBodyStyle)
                        .Text(
                            $"{fishType.ClosedSeasonFromDay}.{fishType.ClosedSeasonFromMonth} - {fishType.ClosedSeasonToDay}.{fishType.ClosedSeasonToMonth}");
                else
                    table.Cell().Element(TableBodyStyle).Text("-").Style(TableBodyTextStyle);


                static IContainer TableBodyStyle(IContainer container)
                {
                    return container.Border(1).MinHeight(30).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(1)
                        .AlignCenter().AlignMiddle();
                }
            }
        });
    }

    private static void ComposeHeader(IContainer container)
    {
        container.Column(columns =>
        {
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