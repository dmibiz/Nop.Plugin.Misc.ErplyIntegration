using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nop.Plugin.Misc.ErplyIntegration.Api.Models.Response;
using Nop.Plugin.Misc.ErplyIntegration.Api.Models.Session;
using Nop.Plugin.Misc.ErplyIntegration.Extensions;
using Newtonsoft.Json;

namespace Nop.Plugin.Misc.ErplyIntegration.Api
{
    public class ErplyApi
    {
        #region Fields

        public const string ProductStatusActive = "ACTIVE";
        public const string ProductStatusNoLongerOrdered = "NO_LONGER_ORDERED";
        public const string ProductStatusNotForSale = "NOT_FOR_SALE";
        public const string ProductStatusArchived = "ARCHIVED";
        public const string ProductStatusAllExceptArchived = "ALL_EXCEPT_ARCHIVED";

        public const string SortDirectionAscending = "asc";
        public const string SortDirectionDescending = "desc";

        private readonly string _url;
        private readonly string _clientCode;
        private readonly string _username;
        private readonly string _password;
        private readonly HttpClient _httpClient;
        
        public ISession Session { get; set; }

        #endregion

        #region Ctor

        public ErplyApi(
            string url, 
            string clientCode, 
            string username, 
            string password, 
            HttpClient httpClient,
            ISession session = null
            )
        {
            _url = url;
            _clientCode = clientCode;
            _username = username;
            _password = password;
            _httpClient = httpClient;
            Session = session;
        }

        #endregion

        #region Methods

        public async Task<string> SendRequest(
            string requestName, 
            IEnumerable<KeyValuePair<string, string>> requestParameters = null)
        {
            var additionalParameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("request", requestName),
                new KeyValuePair<string, string>("clientCode", _clientCode),
                new KeyValuePair<string, string>("version", "1.0")
            };

            if (requestName != "verifyUser")
            {
                string sessionKey = await GetSessionKey();
                additionalParameters.Add(new KeyValuePair<string, string>("sessionKey", sessionKey));
            }
            

            var requestBody = new FormUrlEncodedContent(requestParameters.Concat(additionalParameters));
            var response = _httpClient.PostAsync(_url, requestBody).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetSessionKey()
        {
            List<ErplySessionItem> erplySessionItems = Session?.GetObject<List<ErplySessionItem>>("ErplySession") ?? new List<ErplySessionItem>();
            ErplySessionItem currentSession = erplySessionItems?.FirstOrDefault(item => item.ClientCode == _clientCode);
            if (currentSession?.SessionKey == null || 
                currentSession?.SessionKeyExpires == null ||
                currentSession?.SessionKeyExpires > DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                erplySessionItems.RemoveAll(item => item.ClientCode == _clientCode);

                var verifyUserResponse = await SendRequest(
                    "verifyUser",
                    new[]
                    {
                        new KeyValuePair<string, string>("username", _username),
                        new KeyValuePair<string, string>("password", _password)
                    }
                );
                var verifyUserResponseRecord = JsonConvert.DeserializeObject<ErplyUserResponse>(verifyUserResponse)?.Records?.First();

                string sessionKey = verifyUserResponseRecord?.SessionKey;

                if (sessionKey != null)
                {
                    if (Session == null) return sessionKey;

                    ErplySessionItem newSessionItem = new ErplySessionItem()
                    {
                        ClientCode = _clientCode,
                        SessionKey = sessionKey,
                        SessionKeyExpires = (int) DateTimeOffset.Now.ToUnixTimeSeconds() + verifyUserResponseRecord.SessionLength - 30
                    };

                    erplySessionItems.Add(newSessionItem);
                    Session.SetObject("ErplySession", erplySessionItems);
                }
            }
            return Session?.GetObject<List<ErplySessionItem>>("ErplySession")?.FirstOrDefault(item => item.ClientCode == _clientCode)?.SessionKey;
        }

        public async Task<ErplyProductGroupsResponse> GetProductGroups()
        {
            var erplyProductGroupsResponse = await SendRequest(
                "getProductGroups",
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("showInWebShop", "1"),
                }
            );
            return JsonConvert.DeserializeObject<ErplyProductGroupsResponse>(erplyProductGroupsResponse);
        }

        public async Task<ErplyProductsResponse> GetProducts(bool getStockInfo, string orderByDir, int recordsOnPage, int pageNo, string status)
        {
            var erplyProductsResponse = await SendRequest(
                "getProducts",
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("displayedInWebshop", "1"),
                    new KeyValuePair<string, string>("getStockInfo", getStockInfo ? "1" : "0"),
                    new KeyValuePair<string, string>("orderByDir", orderByDir),
                    new KeyValuePair<string, string>("recordsOnPage", recordsOnPage.ToString()),
                    new KeyValuePair<string, string>("pageNo", pageNo.ToString()),
                    new KeyValuePair<string, string>("status", status)
                }
            );
            return JsonConvert.DeserializeObject<ErplyProductsResponse>(erplyProductsResponse);
        }

        #endregion
    }
}
