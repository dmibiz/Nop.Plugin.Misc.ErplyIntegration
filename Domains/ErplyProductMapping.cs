using Nop.Core;

namespace Nop.Plugin.Misc.ErplyIntegration.Domains
{
    public class ErplyProductMapping : BaseEntity
    {
        public int ProductID { get; set; }
        public int ErplyProductID { get; set; }
    }
}
