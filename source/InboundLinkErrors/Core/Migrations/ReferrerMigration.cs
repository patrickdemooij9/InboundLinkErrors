using InboundLinkErrors.Core.Models;
using Umbraco.Core.Migrations;

namespace InboundLinkErrors.Core.Migrations
{
    public class ReferrerMigration : MigrationBase
    {
        public ReferrerMigration(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            if (!TableExists("InboundLinkErrorReferrers"))
            {
                Create.Table<LinkErrorReferrerEntity>().Do();
            }
        }
    }
}
