using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundSettelment.Model.RefundModel
{
    public class IBANTransferModel
    {
        public long Id { get; set; }
        public long RefundRefrenceNumber { get; set; }
        public string Status { get; set; }
        public string TransferId { get; set; }
        public long Amount { get; set; }
        public long RefrenceNumber { get; set; }
        public string UserName { get; set; }
        public string EncryptedIBAN { get; set; }
        public long TransactionAmount { get; set; }
        public long InquiryCommision { get; set; }
        public string RefundStatus { get; set; }
        public string TransferRrn { get; set; }
        public string Description { get; set; }
    }
}
