using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Seo;
using Nop.Plugin.Misc.ErplyIntegration.Factories;
using Nop.Plugin.Misc.ErplyIntegration.Domains;
using Nop.Plugin.Misc.ErplyIntegration.Api;
using Nop.Plugin.Misc.ErplyIntegration.Api.Models.Response;

namespace Nop.Plugin.Misc.ErplyIntegration.Services
{
    public class ErplyImportManager : IErplyImportManager
    {
        #region Fields

        private readonly IErplyApiFactory _erplyApiFactory;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IErplyMappingService _erplyMappingService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Ctor

        public ErplyImportManager(
            IErplyApiFactory erplyApiFactory, 
            ICategoryService categoryService,
            IProductService productService,
            IErplyMappingService mappingService,
            IUrlRecordService urlRecordService,
            CatalogSettings catalogSettings)
        {
            _erplyApiFactory = erplyApiFactory;
            _categoryService = categoryService;
            _productService = productService;
            _erplyMappingService = mappingService;
            _urlRecordService = urlRecordService;
            _catalogSettings = catalogSettings;
        }

        #endregion

        #region Methods

        public async Task ImportCategories()
        {
            var erplyApi = _erplyApiFactory.GetApi();
            var erplyProductGroups = await erplyApi.GetProductGroups();
            var categoryMappings = _erplyMappingService.GetAllCategoryMappings();
            var allCategories = _categoryService.GetAllCategories(showHidden: true);

            SaveCategoriesRecursive(erplyProductGroups.Records, categoryMappings, allCategories);
        }

        private void SaveCategoriesRecursive(
            List<ErplyProductGroupsResponseRecord> erplyProductGroups, 
            List<ErplyCategoryMapping> categoryMappings,
            IList<Category> allCategories,
            int parentCategoryId = 0)
        {
            foreach (var erplyProductGroup in erplyProductGroups)
            {
                var categoryMapping = categoryMappings.FirstOrDefault(mapping => mapping.ErplyProductGroupID == erplyProductGroup.ProductGroupID);

                Category category = allCategories.FirstOrDefault(c => c.Id == categoryMapping?.CategoryID);
                bool isNewCategory = category == null;
                category ??= new Category();
                category = GetUpdatedCategoryFromErplyProductGroup(category, erplyProductGroup, parentCategoryId, isNewCategory);

                if (isNewCategory)
                {
                    _categoryService.InsertCategory(category);
                    ErplyCategoryMapping newMapping = new ErplyCategoryMapping()
                    {
                        CategoryID = category.Id,
                        ErplyProductGroupID = erplyProductGroup.ProductGroupID
                    };
                    _erplyMappingService.InsertCategoryMapping(newMapping);
                } else
                {
                    _categoryService.UpdateCategory(category);
                }

                _urlRecordService.SaveSlug(category, _urlRecordService.ValidateSeName(category, "", category.Name, true), 0);

                if (erplyProductGroup.SubGroups.Count > 0)
                {
                    SaveCategoriesRecursive(erplyProductGroup.SubGroups, categoryMappings, allCategories, category.Id);
                }
            }
        }

        private Category GetUpdatedCategoryFromErplyProductGroup(Category category, ErplyProductGroupsResponseRecord erplyProductGroup, int parentCategoryId, bool isNew)
        {
            if (isNew)
            {
                category.CreatedOnUtc = DateTime.UtcNow;
            }
            category.UpdatedOnUtc = DateTime.UtcNow;
            category.Name = erplyProductGroup.Name;
            category.ParentCategoryId = parentCategoryId;
            category.DisplayOrder = erplyProductGroup.PositionNo;
            category.PageSize = _catalogSettings.DefaultCategoryPageSize;
            category.PageSizeOptions = _catalogSettings.DefaultCategoryPageSizeOptions;
            category.AllowCustomersToSelectPageSize = true;
            return category;
        }

        public async Task ImportProducts()
        {
            int apiRequestPageNo = 1;
            var erplyApi = _erplyApiFactory.GetApi();
            List<string> erplyProductCodes = new List<string>();
            List<ErplyProductsResponseRecord> erplyProducts = new List<ErplyProductsResponseRecord>();
            var categoryMappings = _erplyMappingService.GetAllCategoryMappings();

            ErplyProductsResponse erplyProductsResponse;
            do
            {
                erplyProductsResponse = await erplyApi.GetProducts(true, ErplyApi.SortDirectionAscending, 100, apiRequestPageNo, ErplyApi.ProductStatusActive);

                foreach (var erplyProduct in erplyProductsResponse.Records)
                {
                    erplyProductCodes.Add(erplyProduct.Code);
                    erplyProducts.Add(erplyProduct);
                }

                apiRequestPageNo++;
            } while (erplyProductsResponse.Records.Count > 0);

            var allProductsBySku = _productService.GetProductsBySku(erplyProductCodes.ToArray());
            var allProductsCategoryIds = _categoryService.GetProductCategoryIds(allProductsBySku.Select(p => p.Id).ToArray());

            foreach (var erplyProduct in erplyProducts)
            {
                var product = allProductsBySku.FirstOrDefault(p => p.Sku == erplyProduct.Code);
                var isNewProduct = product == null;
                product ??= new Product();
                product = GetUpdatedProductFromErplyProduct(product, erplyProduct, isNewProduct);

                if (isNewProduct)
                {
                    _productService.InsertProduct(product);
                } else
                {
                    _productService.UpdateProduct(product);
                }

                _urlRecordService.SaveSlug(product, _urlRecordService.ValidateSeName(product, "", product.Name, true), 0);

                var productCategories = isNewProduct || !allProductsCategoryIds.ContainsKey(product.Id) ? Array.Empty<int>() : allProductsCategoryIds[product.Id];
                var categoryMapping = categoryMappings.FirstOrDefault(mapping => mapping.ErplyProductGroupID == erplyProduct.GroupID);

                if (!productCategories.Any(c => c == categoryMapping?.CategoryID))
                {
                    var productCategory = new ProductCategory
                    {
                        ProductId = product.Id,
                        CategoryId = categoryMapping.CategoryID,
                        IsFeaturedProduct = false,
                        DisplayOrder = 1
                    };
                    _categoryService.InsertProductCategory(productCategory);
                }
            }
        }

        private Product GetUpdatedProductFromErplyProduct(Product product, ErplyProductsResponseRecord erplyProduct, bool isNew)
        {
            if (isNew)
            {
                product.CreatedOnUtc = DateTime.UtcNow;
                product.ProductType = ProductType.SimpleProduct;
                product.VisibleIndividually = true;
            }
            product.UpdatedOnUtc = DateTime.UtcNow;
            product.Sku = erplyProduct.Code;
            product.Name = erplyProduct.Name;
            product.Price = erplyProduct.Price;
            product.ShortDescription = erplyProduct.Description;
            product.FullDescription = erplyProduct.LongDescription;
            product.StockQuantity = GetQuantityFromErplyProduct(erplyProduct);
            return product;
        }

        private int GetQuantityFromErplyProduct(ErplyProductsResponseRecord erplyProduct)
        {
            int quantity = 0;
            foreach (var warehouse in erplyProduct.Warehouses.Values)
            {
                quantity += warehouse.Free;
            }
            return quantity;
        }

        #endregion
    }
}
