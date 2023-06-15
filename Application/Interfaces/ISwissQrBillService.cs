using Application.Models;

namespace Application.Interfaces;

public interface ISwissQrBillService
{
    byte[] CreateFishingLicenseBill(FishingLicenseBill fishingLicenseBill);
}