using Application.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Services;

public class ReportService(IFishingLicenseRepository fishingLicenseRepository): IReportService
{
    public async Task<XLWorkbook> CreateYearlyExcelReportAsync(int year)
    {
        var userStatistics = await fishingLicenseRepository.GetDetailYearlyCatchReportAsync(year);

        var workBook = ExcelService.CreateYearlyReport(userStatistics, year);

        return workBook;
    }

    public async Task<XLWorkbook> CreateYearlyUserExcelReportAsync(Guid userId, int year)
    {
        var userStatistic = await fishingLicenseRepository.GetYearlyUserCatchReportAsync(userId, year);

        var workBook = ExcelService.CreateYearlyUserReport(userStatistic, year);

        return workBook;
    }
}