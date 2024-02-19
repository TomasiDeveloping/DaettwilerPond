using Application.Models.CatchReport;
using ClosedXML.Excel;

namespace Infrastructure.Documents.Excel
{
    public class AnnualReportExcel
    {
        public static XLWorkbook CreateAnnualReportExcel(List<MonthReport> monthReports, int year, List<UserStatistic> userStatistics)
        {
            // Initializes workbook and worksheet
            var workbook = new XLWorkbook();

            var workSheet = ExcelExtensions.InitializeWorksheet(workbook, $"Fangstatistik {year}", $"Fangstatistik {year}", 18);

            // Sets up month rows and summary section
            var row = 3;
            row = SetupMonthRows(workSheet, row, monthReports);

            row = AddSummarySection(workSheet, row, year, monthReports);

            row += 2;

            AddUserSection(workSheet, row, userStatistics);

            return workbook;

        }

        private static int SetupMonthRows(IXLWorksheet worksheet, int startRow, IReadOnlyCollection<MonthReport> monthReports)
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

        private static int SetupMonthRow(IXLWorksheet worksheet, int row, Month month, IEnumerable<MonthReport> monthReports)
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

        private static int AddSummarySection(IXLWorksheet worksheet, int startRow, int year,
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
            return startRow;
        }

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

        private static void AddUserSection(IXLWorksheet worksheet, int startRow, List<UserStatistic> userStatistics)
        {
            worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            startRow++;
            worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            startRow++;
            worksheet.Cell(startRow, 1).SetValue("Mitglieder Statistik");
            worksheet.Cell(startRow, 1).Style.Font.SetBold();
            worksheet.Row(startRow).Style.Font.FontSize = 16;

            startRow++;
            startRow++;
            foreach (var userStatistic in userStatistics)
            {
                worksheet.Cell(startRow, 1).SetValue("Name");
                worksheet.Cell(startRow, 1).Style.Font.SetBold();

                worksheet.Cell(startRow, 2).SetValue("SaNa Nummer");
                worksheet.Cell(startRow, 2).Style.Font.SetBold();

                startRow++;
                worksheet.Cell(startRow, 1).Value = $"{userStatistic.FirstName} {userStatistic.LastName}";
                worksheet.Cell(startRow, 2).Value = "SANA NUMMER";
                startRow++;

                var userFishDictionary = new Dictionary<string, (int, int)>();
                var hoursSpend = 0d;
                foreach (var fishCatch in userStatistic.Catches)
                {
                    hoursSpend += fishCatch.HoursSpend;
                    foreach (var detail in fishCatch.CatchDetails)
                    {
                        var fishExists = userFishDictionary.ContainsKey(detail.FishTypeName);
                        var crabAmount = detail.HadCrabs ? 1 : 0;
                        if (fishExists)
                        {
                            // If the fish exists, update its counts
                            var amountAndCrabs = userFishDictionary[detail.FishTypeName];

                            userFishDictionary[detail.FishTypeName] = (amountAndCrabs.Item1 + detail.Amount, amountAndCrabs.Item2 + crabAmount);
                        }
                        else
                        {
                            // If the fish does not exist, add it to the dictionary with its counts
                            userFishDictionary.Add(detail.FishTypeName, (detail.Amount, crabAmount));
                        }
                    }
                }

                worksheet.Cell(startRow, 1).Value = $"Total Stunden {hoursSpend:0.00}";
                worksheet.Cell(startRow, 1).Style.Font.SetBold();
                startRow++;
                worksheet.Cell(startRow, 2).Value = "Fischart";
                worksheet.Cell(startRow, 2).Style.Font.SetBold();
                worksheet.Cell(startRow, 3).Value = "Anzahl";
                worksheet.Cell(startRow, 3).Style.Font.SetBold();
                worksheet.Cell(startRow, 4).Value = "Mageninhalt";
                worksheet.Cell(startRow, 4).Style.Font.SetBold();


                foreach (var userCatches in userFishDictionary)
                {
                    startRow++;
                    worksheet.Cell(startRow, 2).Value = userCatches.Key;
                    worksheet.Cell(startRow, 3).Value = userCatches.Value.Item1;
                    worksheet.Cell(startRow, 4).Value = userCatches.Value.Item2;
                }

                startRow++;
                worksheet.Row(startRow).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                startRow++;
                startRow++;
            }
        }
    }
}
