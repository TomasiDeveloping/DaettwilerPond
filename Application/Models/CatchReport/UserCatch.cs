namespace Application.Models.CatchReport;

public class UserCatch
{
    public DateTime CatchDate { get; set; }
    public double HoursSpend { get; set; }

    public ICollection<UserCatchDetail> CatchDetails { get; set; } = new List<UserCatchDetail>();
}