using Application.Interfaces;
using Codecrete.SwissQRBill.Generator;

namespace Infrastructure.Services;

public class SwissQrBillService : ISwissQrBillService
{
    public byte[] CreateFishingLicenseBill()
    {
        var bill = new Bill
        {
            // creditor data
            Account = "CH2400232232514607M1P",
            Creditor = new Address
            {
                Name = "Friedrich Alexander Wanner",
                AddressLine1 = "Obere Kehlstrasse 10",
                AddressLine2 = "5400 Baden",
                CountryCode = "CH"
            },

            // payment data
            Amount = 500.00m,
            Currency = "CHF",

            // debtor data
            Debtor = new Address
            {
                Name = "Tomasi Patrick",
                AddressLine1 = "Bühlstrasse 190",
                AddressLine2 = "4468 Kienberg",
                CountryCode = "CH"
            },

            // more payment data
            UnstructuredMessage = "Fischerkarte für 2023",


            // output format
            Format = new BillFormat
            {
                Language = Language.DE,
                GraphicsFormat = GraphicsFormat.PNG,
                OutputSize = OutputSize.QrBillExtraSpace
            }
        };

        return QRBill.Generate(bill);
    }
}