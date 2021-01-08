using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Response
{
    /// <summary>
    /// See https://learn-api.erply.com/requests/getproductgroups for fields descriptions
    /// </summary>
    public class ErplyProductGroupsResponseRecord
    {
        [JsonProperty("productGroupID")]
        public int ProductGroupID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("positionNo")]
        public int PositionNo { get; set; }

        [JsonProperty("parentGroupID")]
        public int ParentGroupID { get; set; }

        [JsonProperty("subGroups")]
        public List<ErplyProductGroupsResponseRecord> SubGroups { get; set; }
    }
}
