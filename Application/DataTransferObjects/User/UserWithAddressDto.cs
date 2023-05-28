using Application.DataTransferObjects.Address;

namespace Application.DataTransferObjects.User;

public class UserWithAddressDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; }
    public AddressDto Address { get; set; }
}