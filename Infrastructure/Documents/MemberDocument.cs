using Application.Interfaces;
using Infrastructure.Documents.Resources;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents;

public class MemberDocument : IDocument
{
    private readonly IUserRepository _userRepository;
    private const int TableHeaderPadding = 1;
    private const string TableHeaderColor = Colors.Grey.Lighten1;
    // 
    private const int TableBodyPadding = 1;


    public MemberDocument(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(20);
            page.Size(PageSizes.A4.Landscape());

            page.Header().Element(ComposeHeader);

            page.Content().Element(ComposeContent);

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
        var usersWithAddresses = _userRepository.GetUsersWithAddressesAsync().GetAwaiter().GetResult();
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
            table.Header(header =>
            {
                header.Cell().Border(1).Background(TableHeaderColor).Padding(TableHeaderPadding).AlignCenter().Text("Name")
                    .FontSize(14).Bold();
                header.Cell().Border(1).Background(TableHeaderColor).Padding(TableHeaderPadding).AlignCenter().Text("Vorname")
                    .FontSize(14).Bold();
                header.Cell().Border(1).Background(TableHeaderColor).Padding(TableHeaderPadding).AlignCenter().Text("E-Mail")
                    .FontSize(14).Bold();
                header.Cell().Border(1).Background(TableHeaderColor).Padding(TableHeaderPadding).AlignCenter().Text("Adresse")
                    .FontSize(14).Bold();
                header.Cell().Border(1).Background(TableHeaderColor).Padding(TableHeaderPadding).AlignCenter().Text("Ort")
                    .FontSize(14).Bold();
                header.Cell().Border(1).Background(TableHeaderColor).Padding(TableHeaderPadding).AlignCenter().Text("Telefon")
                    .FontSize(14).Bold();
                header.Cell().Border(1).Background(TableHeaderColor).Padding(TableHeaderPadding).AlignCenter().Text("Natel")
                    .FontSize(14).Bold();
            });
            foreach (var user in usersWithAddresses)
            {
                table.Cell().Border(1).MinHeight(30).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(TableBodyPadding)
                    .AlignLeft().AlignMiddle().Text(user.LastName).FontSize(11);
                table.Cell().Border(1).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(TableBodyPadding).AlignLeft()
                    .AlignMiddle().Text(user.FirstName).FontSize(11);
                table.Cell().Border(1).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(TableBodyPadding).AlignLeft()
                    .AlignMiddle().Hyperlink($"mailto:{user.Email}").Text(user.Email).FontSize(11)
                    .FontColor(Colors.Blue.Accent3).Underline();
                table.Cell().Border(1).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(TableBodyPadding).AlignLeft()
                    .AlignMiddle().Text($"{user.Address.Street} {user.Address.HouseNumber}").FontSize(11);
                table.Cell().Border(1).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(TableBodyPadding).AlignLeft()
                    .AlignMiddle().Text($"{user.Address.PostalCode} {user.Address.City}").FontSize(11);
                table.Cell().Border(1).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(TableBodyPadding).AlignLeft()
                    .AlignMiddle().Text(user.Address.Phone).FontSize(11);
                table.Cell().Border(1).Background(Colors.Grey.Lighten5).PaddingLeft(2).Padding(TableBodyPadding).AlignLeft()
                    .AlignMiddle().Text(user.Address.Mobile).FontSize(11);
            }
        });
    }
}