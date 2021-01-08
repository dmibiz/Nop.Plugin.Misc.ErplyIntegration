using Newtonsoft.Json;

namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Response
{
    /// <summary>
    /// See https://learn-api.erply.com/requests/verifyuser for fields descriptions
    /// </summary>
    public class ErplyUserResponseRecord
    {
        [JsonProperty("sessionKey")]
        public string SessionKey { get; set; }

        [JsonProperty("sessionLength")]
        public int SessionLength { get; }
    }
}
