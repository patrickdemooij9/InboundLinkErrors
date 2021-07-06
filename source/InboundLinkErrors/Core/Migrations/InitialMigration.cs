using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Models.Data;
using Umbraco.Core.Migrations;

namespace InboundLinkErrors.Core.Migrations
{
    public class InitialMigration : MigrationBase
    {
        public InitialMigration(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            if (!TableExists("InboundLinkErrors"))
            {
                Create.Table<LinkErrorEntity>().Do();
            }
        }
    }
}
