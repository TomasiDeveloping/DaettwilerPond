using ClosedXML.Excel;

namespace Application.Interfaces;

public interface IReportService
{
    Task<XLWorkbook> CreateYearlyExcelReportAsync(int year);
}