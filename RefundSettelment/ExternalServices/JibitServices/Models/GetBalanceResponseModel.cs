using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundSettelment.ExternalServices.JibitServices.Models
{
    public class GetBalanceResponseModel
    {
        public GetBalanceResponseModel()
        {
            Balances = new List<BalanceInfo>();
        }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("balance")]
        public long? Balance { get; set; }

        [JsonProperty("settleableBalance")]
        public long? SettleableBalance { get; set; }

        [JsonProperty("balances")]
        public List<BalanceInfo> Balances { get; set; }
    }

    public class BalanceInfo
    {
        [JsonProperty("balanceType")]
        public string BalanceType { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("amount")]
        public long? Amount { get; set; }
    }
}
