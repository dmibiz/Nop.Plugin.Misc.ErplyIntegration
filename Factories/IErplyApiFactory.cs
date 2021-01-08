using System.Net.Http;
using Nop.Plugin.Misc.ErplyIntegration.Api;

namespace Nop.Plugin.Misc.ErplyIntegration.Factories
{
    public interface IErplyApiFactory
    {
        /// <summary>
        /// Get Erply API using plugin's settings
        /// </summary>
        /// <returns>Erply API object</returns>
        ErplyApi GetApi();

        /// <summary>
        /// Get Erply API using given settings
        /// </summary>
        /// <param name="url">Erply API url</param>
        /// <param name="clientCode">Erply client code</param>
        /// <param name="username">Erply account username</param>
        /// <param name="password">Erply account password</param>
        /// <returns>Erply API object</returns>
        ErplyApi GetApi(string url, string clientCode, string username, string password);

    }
}
