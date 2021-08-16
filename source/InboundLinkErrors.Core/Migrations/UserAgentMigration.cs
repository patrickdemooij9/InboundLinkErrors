using InboundLinkErrors.Core.Models.Data;
using Umbraco.Cms.Infrastructure.Migrations;

namespace InboundLinkErrors.Core.Migrations
{
    public class UserAgentMigration : MigrationBase
    {
        public UserAgentMigration(IMigrationContext context) : base(context)
        {
        }

        protected override void Migrate()
        {
            if (!TableExists("InboundLinkErrorUserAgent"))
            {
                Create.Table<LinkErrorUserAgentEntity>().Do();
            }
        }
    }
}
