using System.Data;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ErplyIntegration.Domains;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;

namespace Nop.Plugin.Misc.ErplyIntegration.Data
{
    public class ErplyProductMappingBuilder : NopEntityBuilder<ErplyProductMapping>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ErplyProductMapping.ProductID)).AsInt32().ForeignKey<Product>(onDelete: Rule.Cascade).PrimaryKey()
                .WithColumn(nameof(ErplyProductMapping.ErplyProductID)).AsInt32().PrimaryKey();
        }
    }
}
