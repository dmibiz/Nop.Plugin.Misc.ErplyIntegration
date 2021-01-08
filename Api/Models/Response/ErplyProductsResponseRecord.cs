using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Response
{
    /// <summary>
    /// See https://learn-api.erply.com/requests/getproducts for fields descriptions
    /// </summary>
    public class ErplyProductsResponseRecord
    {
        [JsonProperty("productID")]
        public int ProductID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("longdesc")]
        public string LongDescription { get; set; }

        [JsonProperty("groupID")]
        public int GroupID { get; set; }

        [JsonProperty("warehouses")]
        public Dictionary<int, ErplyProductResponseRecordWarehouse> Warehouses { get; set; }
    }

    public class ErplyProductResponseRecordWarehouse
    {
        [JsonProperty("free")]
        public int Free { get; set; }
    }
}
