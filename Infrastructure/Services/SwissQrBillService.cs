using Application.Interfaces;
using Application.Models;
using Codecrete.SwissQRBill.Generator;

namespace Infrastructure.Services;

public class SwissQrBillService : ISwissQrBillService
{
    public byte[] CreateFishingLicenseBill(FishingLicenseBill fishingLicenseBill)
    {
        var bill = new Bill
        {
            // Creditor data
            Account = fishingLicenseBill.Account,
            Creditor = new Address
            {
                Name = fishingLicenseBill.CreditorName,
                HouseNo = fishingLicenseBill.CreditorHouseNumber,
                PostalCode = fishingLicenseBill.CreditorPostalCode,
                Street = fishingLicenseBill.CreditorAddress,
                Town = fishingLicenseBill.CreditorCity,
                CountryCode = "CH"
            },

            // Payment data
            Amount = fishingLicenseBill.Amount,
            Currency = "CHF",

            // Debtor data
            Debtor = new Address
            {
                Name = fishingLicenseBill.DebtorName,
                HouseNo = fishingLicenseBill.DebtorHouseNumber,
                PostalCode = fishingLicenseBill.DebtorPostalCode,
                Street = fishingLicenseBill.DebtorAddress,
                Town = fishingLicenseBill.DebtorCity,
                CountryCode = "CH"
            },

            // More payment data
            UnstructuredMessage = fishingLicenseBill.ReferenceMessage,


            // Output format
            Format = new BillFormat
            {
                Language = Language.DE,
                GraphicsFormat = GraphicsFormat.PNG,
                OutputSize = OutputSize.QrBillExtraSpace
            }
        };

        // Check for null or empty values in Debtor Address and City
        if (!string.IsNullOrWhiteSpace(fishingLicenseBill.DebtorAddress) &&
            !string.IsNullOrWhiteSpace(fishingLicenseBill.DebtorCity)) return QRBill.Generate(bill);
        bill.Debtor.Name = null;
        bill.Debtor.CountryCode = null;
        bill.Debtor.HouseNo = null;
        bill.Debtor.PostalCode = null;
        bill.Debtor.Street = null;
        bill.Debtor.Town = null;

        return QRBill.Generate(bill);
    }
}