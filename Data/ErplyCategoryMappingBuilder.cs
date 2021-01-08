using System.Data;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ErplyIntegration.Domains;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;

namespace Nop.Plugin.Misc.ErplyIntegration.Data
{
    public class ErplyCategoryMappingBuilder : NopEntityBuilder<ErplyCategoryMapping>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ErplyCategoryMapping.CategoryID)).AsInt32().ForeignKey<Category>(onDelete: Rule.Cascade).PrimaryKey()
                .WithColumn(nameof(ErplyCategoryMapping.ErplyProductGroupID)).AsInt32().PrimaryKey();
        }
    }
}
