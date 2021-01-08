using Nop.Data;
using Nop.Plugin.Misc.ErplyIntegration.Domains;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Misc.ErplyIntegration.Services
{
    public class ErplyMappingService : IErplyMappingService
    {
        #region Fields

        private readonly IRepository<ErplyCategoryMapping> _categoryMappingRepository;
        private readonly IRepository<ErplyProductMapping> _productMappingRepository;

        #endregion

        #region Ctor

        public ErplyMappingService(IRepository<ErplyCategoryMapping> categoryMappingRepository,
            IRepository<ErplyProductMapping> productMappingRepository)
        {
            _categoryMappingRepository = categoryMappingRepository;
            _productMappingRepository = productMappingRepository;
        }

        #endregion

        #region Methods

        public ErplyCategoryMapping GetCategoryMappingByErplyProductGroupId(int erplyProductGroupId)
        {
            var query = from cm in _categoryMappingRepository.Table
                        where cm.ErplyProductGroupID == erplyProductGroupId
                        select cm;

            return query.FirstOrDefault();
        }

        public ErplyProductMapping GetProductMappingByErplyProductId(int erplyProductId)
        {
            var query = from pm in _productMappingRepository.Table
                        where pm.ErplyProductID == erplyProductId
                        select pm;

            return query.FirstOrDefault();
        }

        public List<ErplyCategoryMapping> GetAllCategoryMappings()
        {
            return _categoryMappingRepository.Table.ToList();
        }

        public List<ErplyProductMapping> GetAllProductMappings()
        {
            return _productMappingRepository.Table.ToList();
        }

        public void InsertCategoryMapping(ErplyCategoryMapping categoryMapping)
        {
            if (categoryMapping == null)
                throw new ArgumentNullException(nameof(categoryMapping));

            _categoryMappingRepository.Insert(categoryMapping);
        }

        public void InsertProductMapping(ErplyProductMapping productMapping)
        {
            if (productMapping == null)
                throw new ArgumentNullException(nameof(productMapping));

            _productMappingRepository.Insert(productMapping);
        }

        #endregion
    }
}
