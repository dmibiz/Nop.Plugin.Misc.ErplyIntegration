using FluentMigrator;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.ErplyIntegration.Domains;

namespace Nop.Plugin.Misc.ErplyIntegration.Migrations
{
    [SkipMigrationOnUpdate]
    [NopMigration("2021/01/04 00:00:00:0000000", "Nop.Plugin.Misc.ErplyIntegration schema")]
    public class SchemaMigration : AutoReversingMigration
    {
        private readonly IMigrationManager _migrationManager;

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            _migrationManager.BuildTable<ErplyCategoryMapping>(Create);
            //_migrationManager.BuildTable<ErplyProductMapping>(Create);
        }
    }
}
