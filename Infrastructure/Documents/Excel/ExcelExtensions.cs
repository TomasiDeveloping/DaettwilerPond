using ClosedXML.Excel;

namespace Infrastructure.Documents.Excel;

public static class ExcelExtensions
{
    public static IXLWorksheet InitializeWorksheet(IXLWorkbook workbook, string sheetName, string title, int fontSize)
    {
        // Adds a new worksheet to the workbook
        var worksheet = workbook.Worksheets.Add(sheetName);

        // Sets the page orientation of the worksheet to landscape
        worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;

        // Sets up the header cell
        var header = worksheet.Cell(1, 1);
        header.Value = title;
        header.Style.Font.FontSize = fontSize;
        header.Style.Font.SetBold();

        // Adds an empty row as a spacer between the header and the data
        worksheet.Cell(2, 1).Value = "";
        worksheet.Cell(2, 1).WorksheetRow().Height = 30;

        // Sets the column widths of the worksheet
        SetColumnWidths(worksheet);

        // Centers the text horizontally in all rows of the worksheet
        worksheet.Rows().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        return worksheet;
    }

    private static void SetColumnWidths(IXLWorksheet worksheet)
    {
        // Define column widths as key-value pairs, where the key represents the column letter and the value represents the width
        var columnWidths = new Dictionary<string, double>
        {
            { "A", 50 },
            { "B", 20 },
            { "C", 20 },
            { "D", 20 },
            { "E", 20 },
            { "F", 20 },
            { "G", 20 },
            { "H", 20 }
        };
        // Iterate over each key-value pair in the dictionary
        foreach (var kvp in columnWidths)
            // Set the width of the column specified by the key to the value specified
            worksheet.Column(kvp.Key).Width = kvp.Value;
    }
}