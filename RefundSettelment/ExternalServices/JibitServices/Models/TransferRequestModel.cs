using Newtonsoft.Json;
using System.Collections.Generic;

namespace RefundSettelment.ExternalServices.JibitServices.Models
{
    public class TransferRequestModel
    {
        [JsonProperty("batchID")]
        public string BatchId { get; set; }

        [JsonProperty("submissionMode")]
        public string SubmissionMode { get; set; }

        [JsonProperty("transfers")]
        public List<TransferInfo> Transfers { get; set; }
    }

    public class TransferInfo
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("transferID")]
        public long TransferID { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }
    }
}
