using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nop.Plugin.Misc.ErplyIntegration.Api.Models.Request;
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

        public async Task<T> SendRequest<T>(ErplyRequest request)
        {
            request.ClientCode = _clientCode;

            if (request.GetType() != typeof(ErplyVerifyUserRequest))
            {
                string sessionKey = await GetSessionKey();
                request.SessionKey = sessionKey;
            }

            var requestBody = new FormUrlEncodedContent(request.ToKeyValuePairList());
            var response = _httpClient.PostAsync(_url, requestBody).Result;
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
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

                ErplyVerifyUserRequest verifyUserRequest = new ErplyVerifyUserRequest()
                {
                    Username = _username,
                    Password = _password
                };

                var verifyUserResponse = await SendRequest<ErplyUserResponse>(verifyUserRequest);
                var verifyUserResponseRecord = verifyUserResponse?.Records?.First();

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

        public async Task<ErplyProductGroupsResponse> GetProductGroups(ErplyGetProductGroupsRequest request = null)
        {
            return await SendRequest<ErplyProductGroupsResponse>(request);
        }

        public async Task<ErplyProductsResponse> GetProducts(ErplyGetProductsRequest request)
        {
            return await SendRequest<ErplyProductsResponse>(request);
        }

        #endregion
    }
}
