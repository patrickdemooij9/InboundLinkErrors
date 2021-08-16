using InboundLinkErrors.Core.Models.Data;
using Umbraco.Cms.Infrastructure.Migrations;

namespace InboundLinkErrors.Core.Migrations
{
    public class InitialMigration : MigrationBase
    {
        public InitialMigration(IMigrationContext context) : base(context)
        {
        }

        protected override void Migrate()
        {
            if (!TableExists("InboundLinkErrors"))
            {
                Create.Table<LinkErrorEntity>().Do();
            }
        }
    }
}
