using Application.Models.CatchReport;
using ClosedXML.Excel;

namespace Infrastructure.Documents.Excel;

public class AnnualUserReportExcel
{
    public static XLWorkbook CreateUserAnnualReport(UserStatistic userStatistic, int year)
    {
        // Create a new Excel workbook
        var workbook = new XLWorkbook();

        // Sort the catches by catch date
        userStatistic.Catches = [.. userStatistic.Catches.OrderBy(c => c.CatchDate)];

        // Initialize a new Excel worksheet with specific parameters
        var workSheet = ExcelExtensions.InitializeWorksheet(workbook,
            $"Statistik {userStatistic.FirstName} {userStatistic.LastName} {year}",
            $"Statistik {userStatistic.FirstName} {userStatistic.LastName} {year}", 18);

        // Set up the initial row for data entry
        var row = 2;

        // Add a static value to the worksheet
        workSheet.Cell(row, 1).Value = "SaNa Nr: 123456";
        workSheet.Cell(row, 1).Style.Font.SetBold();
        workSheet.Row(row).Style.Font.FontSize = 14;
        row++;
        row++;

        // Create annual summary section
        row = CreateUserAnnualSummary(workSheet, row, userStatistic);

        // Create month summary section
        row = CreateUserMonthSummary(workSheet, row, userStatistic);

        // Create detail summary section
        CreateUserDetailSummary(workSheet, row, userStatistic);

        // Return the workbook with populated data
        return workbook;
    }

    private static int CreateUserAnnualSummary(IXLWorksheet worksheet, int startRow, UserStatistic userStatistic)
    {
        // Dictionary to store fish types and their counts
        var userFishDictionary = new Dictionary<string, (int, int)>();

        // Total hours spent fishing
        var totalHours = 0d;

        // Loop through each catch in the user's statistics
        foreach (var userCatches in userStatistic.Catches)
        {
            totalHours += userCatches.HoursSpend;

            // Loop through each fish caught in the catch details
            foreach (var fishCatch in userCatches.CatchDetails)
            {
                var fishExists = userFishDictionary.ContainsKey(fishCatch.FishTypeName);
                var crabAmount = fishCatch.HadCrabs ? 1 : 0;
                if (fishExists)
                {
                    // If the fish exists, update its counts
                    var amountAndCrabs = userFishDictionary[fishCatch.FishTypeName];

                    userFishDictionary[fishCatch.FishTypeName] = (amountAndCrabs.Item1 + fishCatch.Amount,
                        amountAndCrabs.Item2 + crabAmount);
                }
                else
                {
                    // If the fish does not exist, add it to the dictionary with its counts
                    userFishDictionary.Add(fishCatch.FishTypeName, (fishCatch.Amount, crabAmount));
                }
            }
        }

        // Header for annual summary section
        worksheet.Cell(startRow, 1).Value = "Jahresübersicht";
        worksheet.Cell(startRow, 1).Style.Font.SetBold();
        worksheet.Row(startRow).Style.Font.FontSize = 14;
        startRow++;

        // Background color for header row
        worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.LightGray);
        startRow++;

        // Total hours spent fishing
        worksheet.Cell(startRow, 1).Value = $"Total Stunden {totalHours:0.00}";
        worksheet.Cell(startRow, 1).Style.Font.SetBold();
        startRow++;

        // Column headers for fish type, amount, and crab content
        worksheet.Cell(startRow, 2).Value = "Fischart";
        worksheet.Cell(startRow, 2).Style.Font.SetBold();
        worksheet.Cell(startRow, 3).Value = "Anzahl";
        worksheet.Cell(startRow, 3).Style.Font.SetBold();
        worksheet.Cell(startRow, 4).Value = "Mageninhalt";
        worksheet.Cell(startRow, 4).Style.Font.SetBold();
        startRow++;

        // Populate data for each fish type and its counts
        foreach (var userCatch in userFishDictionary)
        {
            worksheet.Cell(startRow, 2).Value = userCatch.Key;  // Fish type
            worksheet.Cell(startRow, 3).Value = userCatch.Value.Item1; // Amount
            worksheet.Cell(startRow, 4).Value = userCatch.Value.Item2; // Crab content
            startRow++;
        }

        // Total catches and catches with crab content
        worksheet.Cell(startRow, 1).Value =
            $"Total Fänge {userFishDictionary.Sum(c => c.Value.Item1)} davon {userFishDictionary.Sum(c => c.Value.Item2)} mit Mageninhalt";
        worksheet.Cell(startRow, 1).Style.Font.SetBold();
        startRow++;

        return startRow;
    }

    private static int CreateUserMonthSummary(IXLWorksheet worksheet, int startRow, UserStatistic userStatistic)
    {
        // Add some space before the month summary section
        startRow++;

        // Set background color for separation
        worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.Black);
        startRow++;
        startRow++;

        // Header for month summary section
        worksheet.Cell(startRow, 1).Value = "Monatsübersicht";
        worksheet.Cell(startRow, 1).Style.Font.SetBold();
        worksheet.Row(startRow).Style.Font.FontSize = 14;
        startRow++;

        // Loop through each month
        foreach (Month month in Enum.GetValues(typeof(Month)))
        {
            // Display the name of the month
            worksheet.Cell(startRow, 1).Value = month.ToString();
            worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            worksheet.Row(startRow).Style.Font.FontSize = 14;
            startRow++;

            // Dictionary to store fish types and their counts for the current month
            var userFishDictionary = new Dictionary<string, (int, int)>();
            var totalHours = 0d;

            // Filter catches for the current month
            var monthCatches = userStatistic.Catches.Where(c => (Month)c.CatchDate.Month == month).ToList();

            // Process catches for the current month if any
            if (monthCatches.Count != 0)
            {
                foreach (var userCatch in monthCatches)
                {
                    totalHours += userCatch.HoursSpend;
                    foreach (var fishCatch in userCatch.CatchDetails)
                    {
                        var fishExists = userFishDictionary.ContainsKey(fishCatch.FishTypeName);
                        var crabAmount = fishCatch.HadCrabs ? 1 : 0;
                        if (fishExists)
                        {
                            // If the fish exists, update its counts
                            var amountAndCrabs = userFishDictionary[fishCatch.FishTypeName];

                            userFishDictionary[fishCatch.FishTypeName] = (amountAndCrabs.Item1 + fishCatch.Amount,
                                amountAndCrabs.Item2 + crabAmount);
                        }
                        else
                        {
                            // If the fish does not exist, add it to the dictionary with its counts
                            userFishDictionary.Add(fishCatch.FishTypeName, (fishCatch.Amount, crabAmount));
                        }
                    }
                }

                // Column headers for fish type, amount, and crab content
                worksheet.Cell(startRow, 2).Value = "Fischart";
                worksheet.Cell(startRow, 2).Style.Font.SetBold();
                worksheet.Cell(startRow, 3).Value = "Anzahl";
                worksheet.Cell(startRow, 3).Style.Font.SetBold();
                worksheet.Cell(startRow, 4).Value = "Mageninhalt";
                worksheet.Cell(startRow, 4).Style.Font.SetBold();
                startRow++;

                // Populate data for each fish type and its counts for the current month
                foreach (var userCatch in userFishDictionary)
                {
                    worksheet.Cell(startRow, 2).Value = userCatch.Key;
                    worksheet.Cell(startRow, 3).Value = userCatch.Value.Item1;
                    worksheet.Cell(startRow, 4).Value = userCatch.Value.Item2;
                    startRow++;
                }
            }

            // Total hours spent fishing for the current month
            worksheet.Cell(startRow, 1).Value = $"Total Stunden {totalHours:0.00}";
            worksheet.Cell(startRow, 1).Style.Font.SetBold();
            startRow++;

            // Display total catches and catches with crab content for the current month if any
            if (userFishDictionary.Count != 0)
            {
                worksheet.Cell(startRow, 1).Value =
                    $"Total Fänge {userFishDictionary.Sum(c => c.Value.Item1)} davon {userFishDictionary.Sum(x => x.Value.Item2)} mit Mageninhalt";
                worksheet.Cell(startRow, 1).Style.Font.SetBold();
                startRow++;
            }

            // Add extra space between months
            startRow++;
        }

        // Return the next starting row index
        return startRow;
    }

    private static void CreateUserDetailSummary(IXLWorksheet worksheet, int startRow, UserStatistic userStatistic)
    {
        // Set background color for separation
        worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.Black);
        startRow++;
        startRow++;

        // Header for detail summary section
        worksheet.Cell(startRow, 1).Value = "Detailübersicht";
        worksheet.Cell(startRow, 1).Style.Font.SetBold();
        worksheet.Row(startRow).Style.Font.FontSize = 14;
        startRow++;

        // Loop through each month
        foreach (Month month in Enum.GetValues(typeof(Month)))
        {
            // Filter catches for the current month
            var monthCatches = userStatistic.Catches.Where(c => (Month)c.CatchDate.Month == month).ToList();

            // Skip if no catches for the current month
            if (monthCatches.Count == 0) continue;

            // Display the name of the month
            worksheet.Cell(startRow, 1).Value = month.ToString();
            worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            worksheet.Row(startRow).Style.Font.FontSize = 14;
            startRow++;

            // Loop through each catch in the current month
            foreach (var catchDays in monthCatches)
            {
                // Display the date and hours spent fishing for each catch day
                worksheet.Cell(startRow, 1).Value = catchDays.CatchDate.ToString("dd.MM");
                worksheet.Cell(startRow, 2).Value = $"{catchDays.HoursSpend:0.00} Stunden";
                startRow++;

                // If there are catch details, display fish type and crab content
                if (catchDays.CatchDetails.Count != 0)
                {
                    worksheet.Row(startRow).Style.Font.FontSize = 12;
                    worksheet.Cell(startRow, 4).SetValue("Fischart");
                    worksheet.Cell(startRow, 4).Style.Font.SetBold();
                    worksheet.Cell(startRow, 5).SetValue("Mageninhalt");
                    worksheet.Cell(startRow, 5).Style.Font.SetBold();
                    startRow++;
                }

                // Loop through each fish catch detail
                foreach (var fishCatch in catchDays.CatchDetails)
                {
                    worksheet.Cell(startRow, 4).Value = fishCatch.FishTypeName;
                    worksheet.Cell(startRow, 5).Value = fishCatch.HadCrabs ? "Ja" : "Nein";
                    startRow++;
                }

                // Add a border at the bottom of the catch details
                worksheet.Row(startRow).Style.Border.SetBottomBorderColor(XLColor.Black);
                worksheet.Row(startRow).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);
                startRow++;
            }
        }
    }
}