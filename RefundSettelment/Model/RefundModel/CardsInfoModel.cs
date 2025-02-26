namespace RefundSettelment.Model.RefundModel
{
    public class CardsInfoModel
    {
        public string Id { get; set; }
        public string Pan { get; set; }
        public string Cvv2 { get; set; }
        public string Pin2 { get; set; }
        public string ExpDate { get; set; }
        public long? Balance { get; set; }
    }
}