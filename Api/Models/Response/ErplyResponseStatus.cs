using Newtonsoft.Json;

namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Response
{
    public class ErplyResponseStatus
    {
        [JsonProperty("request")]
        public string Request { get; set; }

        [JsonProperty("requestUnixTime")]
        public int RequestUnixTime { get; set; }

        [JsonProperty("responseStatus")]
        public string ResponseStatus { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("generationTime")]
        public float GenerationTime { get; set; }

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("recordsInResponse")]
        public float RecordsInResponse { get; set; }
    }
}
