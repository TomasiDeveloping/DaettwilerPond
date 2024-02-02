namespace Domain.Entities;

public class FishingLicense
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Catch> Catches { get; set; }
    public int Year { get; set; }
    public bool IsPaid { get; set; }
    public string IssuedBy { get; set; }
    public bool IsActive { get; set; }
    public DateTime ExpiresOn { get; set; }
}