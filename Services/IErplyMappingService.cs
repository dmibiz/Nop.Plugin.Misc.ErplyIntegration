using System.Collections.Generic;
using Nop.Plugin.Misc.ErplyIntegration.Domains;

namespace Nop.Plugin.Misc.ErplyIntegration.Services
{
    public interface IErplyMappingService
    {
        void InsertCategoryMapping(ErplyCategoryMapping categoryMapping);
        ErplyCategoryMapping GetCategoryMappingByErplyProductGroupId(int erplyProductGroupId);
        List<ErplyCategoryMapping> GetAllCategoryMappings();
        void InsertProductMapping(ErplyProductMapping productMapping);
        ErplyProductMapping GetProductMappingByErplyProductId(int erplyProductId);
        List<ErplyProductMapping> GetAllProductMappings();
    }
}
