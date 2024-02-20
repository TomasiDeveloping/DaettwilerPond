using ClosedXML.Excel;

namespace Application.Interfaces;

public interface IReportService
{
    Task<XLWorkbook> CreateYearlyExcelReportAsync(int year);

    Task<XLWorkbook> CreateYearlyUserExcelReportAsync(Guid userId, int year);
}