using Application.Models.CatchReport;
using ClosedXML.Excel;

// ReSharper disable UnusedMember.Local

namespace Infrastructure.Services;

public static class ExcelService
{
    // Creates a yearly report based on user statistics
    public static XLWorkbook CreateYearlyReport(List<UserStatistic> userStatistics, int year)
    {
        // Validates input parameters
        ArgumentNullException.ThrowIfNull(userStatistics);

        // Initializes workbook and worksheet
        var workbook = new XLWorkbook();
        var monthReports = CreateMonthReports(userStatistics);
        var worksheet = InitializeWorksheet(workbook, year);

        // Sets up month rows and summary section
        var row = 3;
        row = SetupMonthRows(worksheet, row, monthReports);
        AddSummarySection(worksheet, row, year, monthReports);

        return workbook;
    }

    // Creates month-wise reports based on user statistics
    private static List<MonthReport> CreateMonthReports(IReadOnlyCollection<UserStatistic> userStatistics)
    {
        var monthReports = new List<MonthReport>();

        // Iterate over each month in the enumeration
        foreach (Month month in Enum.GetValues(typeof(Month)))
        {
            // Filter catches for the current month
            var monthCatches = userStatistics.SelectMany(c => c.Catches.Where(d => (Month)d.CatchDate.Month == month))
                .ToList();

            // Creates or updates month-wise report data
            var monthReport = monthReports.FirstOrDefault(monthReport => (Month)monthReport.Month == month);

            // If no report exists for the month, create a new one
            if (monthReport is null)
                monthReport = new MonthReport
                {
                    HoursSpend = monthCatches.Sum(c => c.HoursSpend),
                    Month = (int)month,
                    MonthCatches = []
                };
            else
                // If report exists, update the total hours spent
                monthReport.HoursSpend += monthCatches.Sum(c => c.HoursSpend);

            // Iterate over each catch detail for the month
            foreach (var monthCatch in monthCatches)
            foreach (var catchDetails in monthCatch.CatchDetails)
            {
                // Updates fish catch details
                var report = monthReport.MonthCatches.FirstOrDefault(c => c.FishName == catchDetails.FishTypeName);
                if (report is null)
                {
                    // If no catch detail exists for the fish, add a new one
                    monthReport.MonthCatches.Add(new MonthCatch
                    {
                        Amount = 1,
                        CrabsAmount = catchDetails.HadCrabs ? 1 : 0,
                        FishName = catchDetails.FishTypeName
                    });
                }
                else
                {
                    // If catch detail exists, update the counts
                    report.Amount++;
                    if (catchDetails.HadCrabs) report.CrabsAmount++;
                }
            }

            // Add the month report to the list of month reports
            monthReports.Add(monthReport);
        }

        return monthReports;
    }

    // Initializes the worksheet with appropriate formatting
    private static IXLWorksheet InitializeWorksheet(IXLWorkbook workbook, int year)
    {
        // Adds a new worksheet to the workbook
        var worksheet = workbook.Worksheets.Add($"Fangstatistik {year}");

        // Sets the page orientation of the worksheet to landscape
        worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;

        // Sets up the header cell
        var header = worksheet.Cell(1, 1);
        header.Value = $"Fangstatistik {year}";
        header.Style.Font.FontSize = 18;
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

    // Sets up rows for all months in the worksheet
    private static int SetupMonthRows(IXLWorksheet worksheet, int startRow,
        IReadOnlyCollection<MonthReport> monthReports)
    {
        // Iterate over each month in the enumeration
        foreach (Month month in Enum.GetValues(typeof(Month)))
        {
            // Set up rows for each month and update the startRow variable
            startRow = SetupMonthRow(worksheet, startRow, month, monthReports);
            startRow++;
        }

        worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.LightGray);
        startRow++;
        worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.LightGray);
        // Move to the next row after the extra space
        startRow++;

        return startRow;
    }

    // Adds summary section to the worksheet
    private static void AddSummarySection(IXLWorksheet worksheet, int startRow, int year,
        List<MonthReport> monthReports)
    {
        // Calculates fish dictionary for summary
        var fishDictionary = CalculateFishDictionary(monthReports);

        // Set up header for the summary section
        worksheet.Cell(startRow, 1).SetValue($"Zusammenfassung Jahr {year}");
        worksheet.Cell(startRow, 1).Style.Font.SetBold();
        worksheet.Row(startRow).Style.Font.FontSize = 16;
        startRow++;
        worksheet.Cell(startRow, 2).SetValue("Fischart");
        worksheet.Cell(startRow, 2).Style.Font.SetBold();
        worksheet.Cell(startRow, 3).SetValue("Anzahl");
        worksheet.Cell(startRow, 3).Style.Font.SetBold();
        worksheet.Cell(startRow, 4).SetValue("Mageninhalt");
        worksheet.Cell(startRow, 4).Style.Font.SetBold();
        startRow++;

        // Adds fish summary data
        foreach (var fish in fishDictionary)
        {
            worksheet.Cell(startRow, 2).Value = fish.Key;
            worksheet.Cell(startRow, 3).Value = fish.Value.Item1;
            worksheet.Cell(startRow, 4).Value = fish.Value.Item2;
            startRow++;
        }

        startRow++;
        worksheet.Cell(startRow, 1).SetValue($"Total Stunden {CalculateTotalHours(monthReports)}");
        worksheet.Cell(startRow, 1).Style.Font.SetBold();
        worksheet.Row(startRow).Style.Font.FontSize = 12;
        startRow++;

        startRow++;
        worksheet.Cell(startRow, 1).SetValue(
            $"Total Fänge {CalculateTotalCatches(monthReports)} davon {CalculateTotalCrabs(monthReports)} mit Mageninhalt");
        worksheet.Cell(startRow, 1).Style.Font.SetBold();
        worksheet.Row(startRow).Style.Font.FontSize = 12;
    }

    // Sets column widths for the worksheet
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

    // Sets up rows for each month in the worksheet
    private static int SetupMonthRow(this IXLWorksheet worksheet, int row, Month month,
        IEnumerable<MonthReport> monthReports)
    {
        // Sets up header row for each month
        worksheet.Cell(row, 1).SetValue(month.ToString());
        worksheet.Cell(row, 1).Style.Font.SetBold();
        worksheet.Cell(row, 2).SetValue("Fischart");
        worksheet.Cell(row, 2).Style.Font.SetBold();
        worksheet.Row(row).Style.Font.FontSize = 12;
        worksheet.Cell(row, 3).SetValue("Anzahl Fänge");
        worksheet.Cell(row, 3).Style.Font.SetBold();
        worksheet.Cell(row, 4).SetValue("Mit Mageninhalt");
        worksheet.Cell(row, 4).Style.Font.SetBold();
        worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.LightGray);

        // Retrieves month catches data
        var monthCatches = monthReports.FirstOrDefault(m => (Month)m.Month == month);

        row++;

        // Sets up total hours row for each month
        worksheet.Row(row).Style.Font.FontSize = 12;
        worksheet.Cell(row, 1).Value = $"Total Stunden: {monthCatches?.HoursSpend:0.00}";
        worksheet.Cell(row, 1).Style.Font.SetBold();

        row++;

        // Adds catch details for each month
        if (monthCatches == null) return row;
        foreach (var catchDetails in monthCatches.MonthCatches)
        {
            worksheet.Cell(row, 2).Value = catchDetails.FishName;
            worksheet.Cell(row, 3).Value = catchDetails.Amount;
            worksheet.Cell(row, 4).Value = catchDetails.CrabsAmount;
            row++;
        }

        return row;
    }

    // Calculates fish dictionary for summary
    private static Dictionary<string, (int, int)> CalculateFishDictionary(List<MonthReport> monthReports)
    {
        var fishDictionary = new Dictionary<string, (int, int)>();

        // Iterate over each month report
        foreach (var report in monthReports)
        foreach (var catches in report.MonthCatches)
        {
            // Updates fish summary data
            var fishExists = fishDictionary.ContainsKey(catches.FishName);
            if (fishExists)
            {
                // If the fish exists, update its counts
                var amountAndCrabs = fishDictionary[catches.FishName];
                fishDictionary[catches.FishName] = (amountAndCrabs.Item1 + catches.Amount,
                    amountAndCrabs.Item2 + catches.CrabsAmount);
            }
            else
            {
                // If the fish does not exist, add it to the dictionary with its counts
                fishDictionary.Add(catches.FishName, (catches.Amount, catches.CrabsAmount));
            }
        }

        return fishDictionary;
    }

    // Calculates total hours spent
    private static double CalculateTotalHours(IEnumerable<MonthReport> monthReports)
    {
        return monthReports.Sum(r => r.HoursSpend);
    }

    // Calculates total catches
    private static int CalculateTotalCatches(IEnumerable<MonthReport> monthReports)
    {
        return monthReports.Sum(r => r.MonthCatches.Sum(c => c.Amount));
    }

    // Calculates total catches with crabs
    private static int CalculateTotalCrabs(IEnumerable<MonthReport> monthReports)
    {
        return monthReports.Sum(r => r.MonthCatches.Sum(c => c.CrabsAmount));
    }

    // Enumeration for months
    private enum Month
    {
        Januar = 1,
        Februar,
        März,
        April,
        Mai,
        Juni,
        Juli,
        August,
        September,
        Oktober,
        November,
        Dezember
    }
}