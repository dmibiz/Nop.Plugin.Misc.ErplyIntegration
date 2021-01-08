using Nop.Core;

namespace Nop.Plugin.Misc.ErplyIntegration.Domains
{
    public partial class ErplyCategoryMapping : BaseEntity
    {
        public int CategoryID { get; set; }
        public int ErplyProductGroupID { get; set; }
    }
}
