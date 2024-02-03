using Application.Models;

namespace Application.Interfaces;

// Interface for Swiss QR Bill generation related operations
public interface ISwissQrBillService
{
    // Create a Swiss QR Bill for a fishing license bill and return it as a byte array
    byte[] CreateFishingLicenseBill(FishingLicenseBill fishingLicenseBill);
}