using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundSettelment.ExternalServices.JibitServices.Models
{
    public class TransferResponseModel
    {
        public TransferResponseModel()
        {
            Errors = new List<Error>();
        }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }

        [JsonProperty("submittedCount")]
        public int? SubmittedCount { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("totalAmountTransferred")]
        public long? TotalAmountTransferred { get; set; }
    }
}
