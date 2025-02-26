using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RefundSettelment.ExternalServices
{
    public class CallExternalServices
    {
        private readonly string _baseUrl;
        private readonly RestClient _client;

        public CallExternalServices(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new RestClient(baseUrl);
        }

        public async Task<T> CallPostApi<T>(string methodName, string jsonBody, string token) where T : class
        {
            try
            {
                var result = default(T);
                var requestData = new RestRequest($"{methodName}", Method.POST);
                requestData.AddHeader("Authorization", $"Bearer {token}");
                requestData.AddHeader("Content-Type", "application/json");
                requestData.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resp = await _client.ExecuteAsync(requestData);
                var responseJson = resp.Content;

                if (resp.IsSuccessful)
                {
                    result = JsonConvert.DeserializeObject<T>(responseJson);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception($"{_baseUrl}/{methodName} : {resp.StatusCode}");
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception($"{_baseUrl}/{methodName} : {resp.StatusCode}");
                //else if (resp.StatusCode == HttpStatusCode.BadRequest)
                //    throw new Exception($"{_baseUrl}/{methodName} -- RequestBody : {jsonBody} : {resp.StatusCode}");
                else
                {
                    if (!string.IsNullOrEmpty(responseJson))
                        result = JsonConvert.DeserializeObject<T>(responseJson);
                    else
                        throw new Exception(resp.StatusCode.ToString());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> CallPostApi<T>(string methodName, string jsonBody, string username, string password) where T : class
        {
            try
            {
                var result = default(T);
                var base64Auth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
                var requestData = new RestRequest($"{methodName}", Method.POST);
                requestData.AddHeader("Authorization", $"Basic {base64Auth}");
                requestData.AddHeader("Content-Type", "application/json");
                requestData.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resp = await _client.ExecuteAsync(requestData);
                var responseJson = resp.Content;

                if (resp.IsSuccessful)
                {
                    result = JsonConvert.DeserializeObject<T>(responseJson);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception($"{_baseUrl}/{methodName} : {resp.StatusCode}");
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception($"{_baseUrl}/{methodName} : {resp.StatusCode}");
                //else if (resp.StatusCode == HttpStatusCode.BadRequest)
                //    throw new Exception($"{_baseUrl}/{methodName} -- RequestBody : {jsonBody} : {resp.StatusCode}");
                else
                {
                    if (!string.IsNullOrEmpty(responseJson))
                        result = JsonConvert.DeserializeObject<T>(responseJson);
                    else
                        throw new Exception(resp.StatusCode.ToString());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> CallGetApi<T>(string methodName, string token, Dictionary<string, string> queryParams = null)
        {
            try
            {
                var result = default(T);
                var requestData = new RestRequest($"{methodName}", Method.GET);
                requestData.AddHeader("Authorization", $"Bearer {token}");
                requestData.AddHeader("Content-Type", "application/json");
                var parameters = new List<string>();
                var paramsString = string.Empty;
                if (queryParams != null && queryParams.Count() != 0)
                {
                    foreach (var param in queryParams)
                    {
                        requestData.AddQueryParameter(param.Key, param.Value.ToString());
                        parameters.Add($"{param.Key}:{param.Value}");
                    }
                    paramsString = string.Join("|", parameters);
                }

                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resp = await _client.ExecuteAsync(requestData);
                var responseJson = resp.Content;

                if (resp.IsSuccessful)
                {
                    result = JsonConvert.DeserializeObject<T>(responseJson);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception($"{_baseUrl}/{methodName} : {resp.StatusCode}");
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception($"{_baseUrl}/{methodName} : {resp.StatusCode}");
                //else if (resp.StatusCode == HttpStatusCode.BadRequest)
                //    throw new Exception($"{_baseUrl}/{methodName} -- Parameter : {paramsString} : {resp.StatusCode}");
                else
                {
                    if (!string.IsNullOrEmpty(responseJson))
                        result = JsonConvert.DeserializeObject<T>(responseJson);
                    else
                        throw new Exception(resp.StatusCode.ToString());
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> CallGetApi<T>(string methodName, string username, string password, Dictionary<string, string> queryParams = null)
        {
            try
            {
                var result = default(T);
                var base64Auth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
                var requestData = new RestRequest($"{methodName}", Method.GET);
                requestData.AddHeader("Authorization", $"Basic {base64Auth}");
                requestData.AddHeader("Content-Type", "application/json");

                if (queryParams != null && queryParams.Count() != 0)
                {
                    foreach (var param in queryParams)
                        requestData.AddQueryParameter(param.Key, param.Value.ToString());
                }

                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resp = await _client.ExecuteAsync(requestData);
                var responseJson = resp.Content;

                if (resp.IsSuccessful)
                {
                    result = JsonConvert.DeserializeObject<T>(responseJson);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception($"{_baseUrl}/{methodName} : {resp.StatusCode}");
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception($"{_baseUrl}/{methodName} : {resp.StatusCode}");
                //else if (resp.StatusCode == HttpStatusCode.BadRequest)
                //    throw new Exception($"{_baseUrl}/{methodName} -- Parameter : {queryParams} : {resp.StatusCode}");
                else
                {
                    if (!string.IsNullOrEmpty(responseJson))
                        result = JsonConvert.DeserializeObject<T>(responseJson);
                    else
                        throw new Exception(resp.StatusCode.ToString());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
