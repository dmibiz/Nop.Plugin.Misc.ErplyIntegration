namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Request
{
    /// <summary>
    /// See https://learn-api.erply.com/requests/getproductgroups for fields descriptions
    /// </summary>
    public class ErplyGetProductGroupsRequest : ErplyRequest
    {
        public override string Request { get; } = "getProductGroups";

        public int ShowInWebshop { get; set; }
    }
}
