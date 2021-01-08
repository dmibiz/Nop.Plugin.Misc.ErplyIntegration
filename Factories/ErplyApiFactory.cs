using Nop.Plugin.Misc.ErplyIntegration.Api;
using Nop.Core.Http;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Nop.Plugin.Misc.ErplyIntegration.Factories
{
    public class ErplyApiFactory : IErplyApiFactory
    {
        #region Fields

        private readonly ErplyIntegrationSettings _erplyIntegrationSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public ErplyApiFactory(
            ErplyIntegrationSettings erplyIntegrationSettings,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _erplyIntegrationSettings = erplyIntegrationSettings;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        public ErplyApi GetApi()
        {
            ErplyApiUrl apiUrl = new ErplyApiUrl() { ClientCode = _erplyIntegrationSettings.ClientCode };
            return GetApi(
                apiUrl.ToString(),
                _erplyIntegrationSettings.ClientCode,
                _erplyIntegrationSettings.Username,
                _erplyIntegrationSettings.Password
                );
        }

        public ErplyApi GetApi(string url, string clientCode, string username, string password)
        {
            return new ErplyApi(
                url, 
                clientCode, 
                username, 
                password, 
                _httpClientFactory.CreateClient(NopHttpDefaults.DefaultHttpClient),
                _httpContextAccessor.HttpContext.Session
                );
        }
    }
}
