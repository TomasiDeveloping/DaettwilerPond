using Application.DataTransferObjects.Catch;

namespace Application.Models.CatchReport;

public class UserStatistic
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string SaNaNumber { get; set; }

    public ICollection<UserCatch> Catches { get; set; } = new List<UserCatch>();
}