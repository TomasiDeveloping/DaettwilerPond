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
            // creditor data
            Account = fishingLicenseBill.Account,
            Creditor = new Address
            {
                Name = fishingLicenseBill.CreditorName,
                AddressLine1 = fishingLicenseBill.CreditorAddress,
                AddressLine2 = fishingLicenseBill.CreditorCity,
                CountryCode = "CH"
            },

            // payment data
            Amount = fishingLicenseBill.Amount,
            Currency = "CHF",

            // debtor data
            Debtor = new Address
            {
                Name = fishingLicenseBill.DebtorName,
                AddressLine1 = string.IsNullOrEmpty(fishingLicenseBill.DebtorAddress) ? null : fishingLicenseBill.DebtorAddress,
                AddressLine2 = string.IsNullOrEmpty(fishingLicenseBill.DebtorCity) ? null : fishingLicenseBill.DebtorCity,
                CountryCode = "CH"
            },

            // more payment data
            UnstructuredMessage = fishingLicenseBill.ReferenceMessage,


            // output format
            Format = new BillFormat
            {
                Language = Language.DE,
                GraphicsFormat = GraphicsFormat.PNG,
                OutputSize = OutputSize.QrBillExtraSpace
            }
        };
        if (!string.IsNullOrWhiteSpace(fishingLicenseBill.DebtorAddress) &&
            !string.IsNullOrWhiteSpace(fishingLicenseBill.DebtorCity)) return QRBill.Generate(bill);
        bill.Debtor.Name = null;
        bill.Debtor.AddressLine1 = null;
        bill.Debtor.AddressLine2 = null;
        bill.Debtor.CountryCode = null;

        return QRBill.Generate(bill);
    }
}