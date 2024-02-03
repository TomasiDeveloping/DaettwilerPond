namespace Application.DataTransferObjects.Address;

// Data Transfer Object (DTO) representing address information
public class AddressDto
{
    // Unique identifier for the address
    public Guid Id { get; set; }

    // Unique identifier of the user associated with the address
    public Guid UserId { get; set; }

    // Street name of the address
    public string Street { get; set; }

    // House number of the address
    public string HouseNumber { get; set; }

    // City of the address
    public string City { get; set; }

    // Country of the address
    public string Country { get; set; }

    // Postal code of the address
    public string PostalCode { get; set; }

    // Phone number associated with the address
    public string Phone { get; set; }

    // Mobile number associated with the address
    public string Mobile { get; set; }
}