using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundSettelment.ExternalServices.JibitServices.Models
{
    public class BaseTokenResponseModel<T> where T : class
    {
        public BaseTokenResponseModel()
        {
            Errors = new List<string>();
        }

        [JsonProperty("successful")]
        public bool Successful { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }

    public class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("httpStatusCode")]
        public int HttpStatusCode { get; set; }
    }
}
