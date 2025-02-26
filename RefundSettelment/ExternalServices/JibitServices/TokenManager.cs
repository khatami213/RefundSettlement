using Newtonsoft.Json;
using RefundSettelment.ExternalServices.JibitServices.Models;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RefundSettelment.ExternalServices.JibitServices
{
    public static class TokenManager
    {
        private static string _accessToken;
        private static DateTime _tokenExpiration;

        public static string Token
        {
            get
            {
                if (_accessToken == null || _tokenExpiration <= DateTime.Now)
                {
                    _accessToken = GetToken().Result;
                    _tokenExpiration = GetTokenExpiration(_accessToken);
                }

                return _accessToken;
            }
            set
            {
                _accessToken = value;
            }
        }

        // IdentityServer configuration
        private static readonly string IdentityServerUrl = ConfigurationManager.AppSettings["PNAIdentityBaseAddress"]; //"http://192.168.18.39:8080/api/identity/login";
        private static readonly string Username = ConfigurationManager.AppSettings["PNAIdentityUsername"]; //"refund";
        private static readonly string Password = ConfigurationManager.AppSettings["PNAIdentityPassword"]; //"123456";
        //private static readonly string Scope = "refund.user";

        private static async Task<string> GetToken()
        {
            var token = string.Empty;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient())
            {
                var request = new
                {
                    userName = Username,
                    password = Password,
                };
                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(IdentityServerUrl, content);
                var stringResposne = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<BaseTokenResponseModel<TokenResponseModel>>(stringResposne);
                    if (result != null && !string.IsNullOrEmpty(result.Data?.AccessToken))
                    {
                        token = result.Data.AccessToken;
                    }
                }
                else
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(stringResponse))
                    {
                        var result = JsonConvert.DeserializeObject<BaseTokenResponseModel<TokenResponseModel>>(stringResponse);
                    }
                }
            }
            return token;
        }

        private static DateTime GetTokenExpiration(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            return jwtToken.ValidTo;
        }
    }
}
