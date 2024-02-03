namespace Application.DataTransferObjects.FishingLicense
{
    // Data Transfer Object (DTO) representing the data for creating a fishing license bill
    public class CreateFishingLicenseBillDto
    {
        // Array of user identifiers for whom the fishing license bill is created
        public Guid[] UserIds { get; set; }

        // License year for which the fishing license bill is generated
        public int LicenseYear { get; set; }

        // Email message associated with the fishing license bill
        public string EmailMessage { get; set; }

        // Indicates whether to create the actual fishing license
        public bool CreateLicense { get; set; }
    }
}