namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Request
{
    /// <summary>
    /// See https://learn-api.erply.com/requests/getproducts for fields descriptions
    /// </summary>
    public class ErplyGetProductsRequest : ErplyRequest
    {
        public override string Request { get; } = "getProducts";

        public int DisplayedInWebshop { get; set; }

        public int GetStockInfo { get; set; }

        public string OrderByDir { get; set; }

        public int RecordsOnPage { get; set; }

        public int PageNo { get; set; }

        public string Status { get; set; }
    }
}
