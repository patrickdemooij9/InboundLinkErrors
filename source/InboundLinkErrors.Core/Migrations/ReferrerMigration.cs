using InboundLinkErrors.Core.Models.Data;
using Umbraco.Cms.Infrastructure.Migrations;

namespace InboundLinkErrors.Core.Migrations
{
    public class ReferrerMigration : MigrationBase
    {
        public ReferrerMigration(IMigrationContext context) : base(context)
        {
        }

        protected override void Migrate()
        {
            if (!TableExists("InboundLinkErrorReferrers"))
            {
                Create.Table<LinkErrorReferrerEntity>().Do();
            }
        }
    }
}
