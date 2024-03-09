using Application.Models.CatchReport;
using ClosedXML.Excel;
using Infrastructure.Documents.Excel;

namespace Infrastructure.Services;

public static class ExcelService
{
    // Creates a yearly report based on user statistics
    public static XLWorkbook CreateYearlyReport(List<UserStatistic> userStatistics, int year)
    {
        // Validates input parameters
        ArgumentNullException.ThrowIfNull(userStatistics);

        var monthReports = CreateMonthReports(userStatistics);

        return AnnualReportExcel.CreateAnnualReportExcel(monthReports, year, userStatistics);
    }

    public static XLWorkbook CreateYearlyUserReport(UserStatistic userStatistic, int year)
    {
        ArgumentNullException.ThrowIfNull(userStatistic);

        return AnnualUserReportExcel.CreateUserAnnualReport(userStatistic, year);
    }

    // Creates month-wise reports based on user statistics
    private static List<MonthReport> CreateMonthReports(IReadOnlyCollection<UserStatistic> userStatistics)
    {
        var monthReports = new List<MonthReport>();

        // Iterate over each month in the enumeration
        foreach (Month month in Enum.GetValues(typeof(Month)))
        {
            // Filter catches for the current month
            var monthCatches = userStatistics
                .SelectMany(c => c.Catches
                    .Where(d => (Month)d.CatchDate.Month == month))
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
            {
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
            }

            // Add the month report to the list of month reports
            monthReports.Add(monthReport);
        }

        return monthReports;
    }
}