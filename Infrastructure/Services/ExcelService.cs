using System.Globalization;
using Application.Models.CatchReport;
using ClosedXML.Excel;

namespace Infrastructure.Services;

public static class ExcelService
{
    public static XLWorkbook CreateYearlyReport(List<UserStatistic> userStatistics, int year)
    {
        var workbook = new XLWorkbook();

        var monthReports = CreateMonthReports(userStatistics);

        var worksheet = workbook.Worksheets.Add($"Fangstatistik {year}");
        worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        var header = worksheet.Cell(1, 1);
        header.Value = $"Fangstatistik {year}";
        header.Style.Font.FontSize = 18;
        header.Style.Font.SetBold();
        worksheet.Cell(2, 1).Value = "";
        worksheet.Cell(2, 1).WorksheetRow().Height = 30;
        worksheet.Column("A").Width = 50;
        worksheet.Column("B").Width = 20;
        worksheet.Column("C").Width = 20;
        worksheet.Column("D").Width = 20;
        worksheet.Column("E").Width = 20;
        worksheet.Column("F").Width = 20;
        worksheet.Column("G").Width = 20;
        worksheet.Column("H").Width = 20;
        worksheet.Rows().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        var row = 3;


        for (var i = 1; i <= 12; i++)
        {
            worksheet.Cell(row, 1).SetValue(GetMonthName(i));
            worksheet.Cell(row, 1).Style.Font.SetBold();
            worksheet.Cell(row, 2).SetValue("Fischart");
            worksheet.Cell(row, 2).Style.Font.SetBold();
            worksheet.Row(row).Style.Font.FontSize = 12;
            worksheet.Cell(row, 3).SetValue("Anzahl Fänge");
            worksheet.Cell(row, 3).Style.Font.SetBold();
            worksheet.Cell(row, 4).SetValue("Mit Mageninhalt");
            worksheet.Cell(row, 4).Style.Font.SetBold();

            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.LightGray);

            row++;

            var monthCatches = monthReports.FirstOrDefault(m => m.Month == i);

            worksheet.Row(row).Style.Font.FontSize = 12;
            worksheet.Cell(row, 1).Value = $"Total Stunden: {monthCatches?.HoursSpend:0.00}";
            worksheet.Cell(row, 1).Style.Font.SetBold();

            row++;

            if (monthCatches != null)
            {
                foreach (var catchDetails in monthCatches.MonthCatches)
                {
                    worksheet.Cell(row, 2).Value = catchDetails.FishName;
                    worksheet.Cell(row, 3).Value = catchDetails.Amount;
                    worksheet.Cell(row, 4).Value = catchDetails.CrabsAmount;
                    row++;
                }
            }

            row++;
        }

        row++;
        worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.LightGray);
        row++;
        worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.LightGray);
        row++;

        var fishDictionary = new Dictionary<string, (int, int)>();
        foreach (var report in monthReports)
        {
            foreach (var catches in report.MonthCatches)
            {
                var fishExists = fishDictionary.ContainsKey(catches.FishName);
                if (fishExists)
                {
                    var amountAndCrabs = fishDictionary[catches.FishName];
                    fishDictionary[catches.FishName] = (amountAndCrabs.Item1 + catches.Amount, amountAndCrabs.Item2 + catches.CrabsAmount);
                }
                else
                {
                    fishDictionary.Add(catches.FishName, (catches.Amount, catches.CrabsAmount));
                }
            }
        }

        worksheet.Cell(row, 1).SetValue($"Zusammenfassung Jahr {year}");
        worksheet.Cell(row, 1).Style.Font.SetBold();
        worksheet.Row(row).Style.Font.FontSize = 16;
        row++;
        worksheet.Cell(row, 2).SetValue("Fischart");
        worksheet.Cell(row, 2).Style.Font.SetBold();
        worksheet.Cell(row, 3).SetValue("Anzahl");
        worksheet.Cell(row, 3).Style.Font.SetBold();
        worksheet.Cell(row, 4).SetValue("Mageninhalt");
        worksheet.Cell(row, 4).Style.Font.SetBold();
        row++;
        foreach (var fish in fishDictionary)
        {
            worksheet.Cell(row, 2).Value = fish.Key;
            worksheet.Cell(row, 3).Value = fish.Value.Item1;
            worksheet.Cell(row, 4).Value = fish.Value.Item2;
            row++;
        }

        row++;
        worksheet.Cell(row, 1).SetValue($"Total Stunden {monthReports.Sum(r => r.HoursSpend).ToString(CultureInfo.CurrentCulture)}");
        worksheet.Cell(row, 1).Style.Font.SetBold();
        worksheet.Row(row).Style.Font.FontSize = 12;
        row++;

        row++;
        worksheet.Cell(row, 1).SetValue($"Total Fänge {monthReports.Sum(r => r.MonthCatches.Sum(c => c.Amount)).ToString(CultureInfo.InvariantCulture)} davon {monthReports.Sum(r => r.MonthCatches.Sum(c => c.CrabsAmount))} mit Mageninhalt");
        worksheet.Cell(row, 1).Style.Font.SetBold();
        worksheet.Row(row).Style.Font.FontSize = 12;

        return workbook;
    }

    private static List<MonthReport> CreateMonthReports(List<UserStatistic> userStatistics)
    {
        var monthReports = new List<MonthReport>();

        for (var i = 1; i <= 12; i++)
        {
            var month = i;
            var monthCatches = userStatistics.SelectMany(c => c.Catches.Where(d => d.CatchDate.Month == month)).ToList();

            var monthReport = monthReports.FirstOrDefault(r => r.Month == month);
            if (monthReport is null)
            {
                monthReport = new MonthReport()
                {
                    HoursSpend = monthCatches.Sum(c => c.HoursSpend),
                    Month = month,
                    MonthCatches = []
                };
            }
            else
            {
                monthReport.HoursSpend += monthCatches.Sum(c => c.HoursSpend);
            }

            foreach (var monthCatch in monthCatches)
            {
                foreach (var catchDetails in monthCatch.CatchDetails)
                {
                    var report = monthReport.MonthCatches.FirstOrDefault(c => c.FishName == catchDetails.FishTypeName);
                    if (report is null)
                    {
                        monthReport.MonthCatches.Add(new MonthCatch()
                        {
                            Amount = 1,
                            CrabsAmount = catchDetails.HadCrabs ? 1 : 0,
                            FishName = catchDetails.FishTypeName
                        });
                    }
                    else
                    {
                        report.Amount++;
                        if (catchDetails.HadCrabs)
                        {
                            report.CrabsAmount++;
                        }
                    }

                }
            }
            monthReports.Add(monthReport);
        }

        return monthReports;
    }

    private static string GetMonthName(int month)
    {
        return month switch
        {
            1 => "Januar",
            2 => "Februar",
            3 => "März",
            4 => "April",
            5 => "Mai",
            6 => "Juni",
            7 => "Juli",
            8 => "August",
            9 => "September",
            10 => "Oktober",
            11 => "November",
            12 => "Dezember",
            _ => "Unbekannt"
        };
    }
}