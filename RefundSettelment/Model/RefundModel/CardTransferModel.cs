namespace RefundSettelment.Model.RefundModel
{
    public class CardTransferModel
    {
        public int Id { get; set; }
        public long RefundRefrenceNumber { get; set; }
        public string Status { get; set; }
        public string Rrn { get; set; }
        public long Amount { get; set; }
        public string EncryptedSourcePan { get; set; }
        public long RefrenceNumber { get; set; }
        public string UserName { get; set; }
        public string EncryptedPan { get; set; }
        public long TransactionAmount { get; set; }
        public long CommitionAmount { get; set; }
        public string RefundStatus { get; set; }
        public string TransferRrn { get; set; }
        public string Description { get; set; }
    }
}