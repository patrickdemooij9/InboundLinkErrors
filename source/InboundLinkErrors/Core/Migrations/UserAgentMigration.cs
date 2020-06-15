using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Models.Data;
using Umbraco.Core.Migrations;

namespace InboundLinkErrors.Core.Migrations
{
    public class UserAgentMigration : MigrationBase
    {
        public UserAgentMigration(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            if (!TableExists("InboundLinkErrorUserAgent"))
            {
                Create.Table<LinkErrorUserAgentEntity>().Do();
            }
        }
    }
}
