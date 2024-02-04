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
                AddressLine1 = fishingLicenseBill.CreditorAddress,
                AddressLine2 = fishingLicenseBill.CreditorCity,
                CountryCode = "CH"
            },

            // Payment data
            Amount = fishingLicenseBill.Amount,
            Currency = "CHF",

            // Debtor data
            Debtor = new Address
            {
                Name = fishingLicenseBill.DebtorName,
                AddressLine1 = string.IsNullOrEmpty(fishingLicenseBill.DebtorAddress) ? null : fishingLicenseBill.DebtorAddress,
                AddressLine2 = string.IsNullOrEmpty(fishingLicenseBill.DebtorCity) ? null : fishingLicenseBill.DebtorCity,
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
        bill.Debtor.AddressLine1 = null;
        bill.Debtor.AddressLine2 = null;
        bill.Debtor.CountryCode = null;

        return QRBill.Generate(bill);
    }
}