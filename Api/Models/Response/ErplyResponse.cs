using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Response
{
    public abstract class ErplyResponse<T>
    {
        [JsonProperty("status")]
        public ErplyResponseStatus Status { get; set; }

        [JsonProperty("records")]
        public List<T> Records { get; set;}
    }
}
