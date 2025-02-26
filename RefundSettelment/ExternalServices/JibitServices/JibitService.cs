using Logging;
using Newtonsoft.Json;
using RefundSettelment.ExternalServices.JibitServices.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace RefundSettelment.ExternalServices.JibitServices
{
    public class JibitService
    {
        private readonly string _baseUrl;
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(JibitService));
        public JibitService()
        {
            _baseUrl = ConfigurationManager.AppSettings["JibitBaseAddress"]; //"http://192.168.18.85:1007/v1/jibit/";
        }

        public async Task<TransferResponseModel> Transfer(TransferRequestModel request)
        {
            var externalServices = new CallExternalServices(_baseUrl);
            try
            {
                var token = TokenManager.Token;
                var result = new TransferResponseModel();
                var jsonBody = JsonConvert.SerializeObject(request);
                result = await externalServices.CallPostApi<TransferResponseModel>("transfer", jsonBody, token);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Fatal($"transfer Service Failed : {ex.Message}");
                throw;
            }
        }

        public async Task<GetBalanceResponseModel> GetBalance()
        {
            var externalServices = new CallExternalServices(_baseUrl);
            try
            {
                var token = TokenManager.Token;
                var result = new GetBalanceResponseModel();
                result = await externalServices.CallGetApi<GetBalanceResponseModel>("get-balance", token);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Fatal($"get-balance Service Failed : {ex.Message}");
                throw;
            }
        }

    }
}
