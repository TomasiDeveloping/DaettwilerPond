using Application.DataTransferObjects.User;
using Infrastructure.Documents.Resources;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents;

public class MemberDocument : IDocument
{
    private readonly List<UserWithAddressDto> _usersWithAddresses;

    public MemberDocument(List<UserWithAddressDto> usersWithAddresses)
    {
        _usersWithAddresses = usersWithAddresses;
    }

    private static TextStyle TableBodyTextStyle => TextStyle
        .Default
        .FontSize(11);

    private static TextStyle TableHeaderTextStyle => TextStyle
        .Default
        .FontSize(14)
        .Bold();

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            // Document settings
            page.Margin(20);
            page.Size(PageSizes.A4.Landscape());
            // Create Document Header
            page.Header().Element(ComposeHeader);
            // Create Document Content Table
            page.Content().Element(ComposeContent);
            // Create Document Footer
            page.Footer().AlignCenter().Text($"Stand: {DateTime.Now:dd.MM.yyyy}");
        });
    }

    private static void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.AutoItem().AlignMiddle().Text("Mitglieder Fischerclub Dättwiler Weiher").Bold().FontSize(20);
            row.AutoItem().AlignRight().Width(150).Image(Resource1.logo).FitArea();
        });
    }

    private void ComposeContent(IContainer container)
    {
        // Table Column Definition
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                // LatsName
                columns.ConstantColumn(100);
                // FirstName
                columns.ConstantColumn(100);
                // Email
                columns.ConstantColumn(150);
                // Address and HouseNumber
                columns.ConstantColumn(150);
                // PostalCode and City
                columns.ConstantColumn(100);
                // Phone
                columns.ConstantColumn(100);
                // Mobile
                columns.ConstantColumn(100);
            });
            // Configure Table Header 
            table.Header(header =>
            {
                header.Cell().Element(TableHeaderStyle).Text("Name").Style(TableHeaderTextStyle);
                header.Cell().Element(TableHeaderStyle).Text("Vorname").Style(TableBodyTextStyle);
                header.Cell().Element(TableHeaderStyle).Text("E-Mail").Style(TableHeaderTextStyle);
                header.Cell().Element(TableHeaderStyle).Text("Adresse").Style(TableHeaderTextStyle);
                header.Cell().Element(TableHeaderStyle).Text("Ort").Style(TableHeaderTextStyle);
                header.Cell().Element(TableHeaderStyle).Text("Telefon").Style(TableHeaderTextStyle);
                header.Cell().Element(TableHeaderStyle).Text("Natel").Style(TableHeaderTextStyle);

                static IContainer TableHeaderStyle(IContainer container)
                {
                    return container.Border(1).Background(Colors.Grey.Lighten1).Padding(1).AlignCenter();
                }
            });
            // Create Table Body Content
            foreach (var user in _usersWithAddresses)
            {
                // LastName
                table.Cell().Element(TableBodyStyle).Text(user.LastName).Style(TableBodyTextStyle);
                // FirstName
                table.Cell().Element(TableBodyStyle).Text(user.FirstName).Style(TableBodyTextStyle);
                // Email
                table.Cell().Element(TableBodyStyle).Hyperlink($"mailto:{user.Email}").Text(user.Email)
                    .Style(TableBodyTextStyle)
                    .FontColor(Colors.Blue.Accent3).Underline();
                // Street and HouseNumber
                table.Cell().Element(TableBodyStyle).Text($"{user.Address.Street} {user.Address.HouseNumber}")
                    .Style(TableBodyTextStyle);
                // PostalCode and City
                table.Cell().Element(TableBodyStyle).Text($"{user.Address.PostalCode} {user.Address.City}")
                    .Style(TableBodyTextStyle);
                // Phone
                table.Cell().Element(TableBodyStyle).Text(user.Address.Phone).Style(TableBodyTextStyle);
                // Mobile
                table.Cell().Element(TableBodyStyle).Text(user.Address.Mobile).Style(TableBodyTextStyle);

                static IContainer TableBodyStyle(IContainer container)
                {
                    return container.Border(1).MinHeight(30).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(1)
                        .AlignLeft().AlignMiddle();
                }
            }
        });
    }
}