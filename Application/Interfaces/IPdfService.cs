using Application.DataTransferObjects.FishingLicense;

namespace Application.Interfaces;

// Interface for PDF-related operations
public interface IPdfService
{
    // Create a PDF document for member information
    Task<byte[]> CreateMemberPdfAsync();

    // Create a PDF document for fishing rules
    Task<byte[]> CreateFishingRulesPdfAsync();

    // Create a PDF document for fish open season information
    Task<byte[]> CreateFishOpenSeasonPdfAsync();

    // Send a fishing license bill and return whether the operation was successful
    Task<bool> SendFishingLicenseBillAsync(CreateFishingLicenseBillDto createFishingLicenseBillDto, string creatorEmail);

    // Get a user's fishing license invoice as a PDF
    Task<byte[]> GetUserFishingLicenseInvoiceAsync(Guid fishingLicenseId);
}