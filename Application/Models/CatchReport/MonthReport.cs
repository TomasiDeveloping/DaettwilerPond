namespace Application.Models.CatchReport;

public class MonthReport
{
    public int Month { get; set; }
    public double HoursSpend { get; set; }

    public ICollection<MonthCatch> MonthCatches { get; set; } = [];
}