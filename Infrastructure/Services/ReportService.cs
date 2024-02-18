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
}