using System;
using InboundLinkErrors.Core.Models.Data;
using Umbraco.Core.Migrations;
using Umbraco.Web.Models.ContentEditing;

namespace InboundLinkErrors.Core.Migrations
{
    public class BatchingMigration : MigrationBase
    {
        public BatchingMigration(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            if (!TableExists("InboundLinkErrorView"))
            {
                Create.Table<LinkErrorViewEntity>().Do();
            }

            if (ColumnExists("InboundLinkErrors", "TimesAccessed") && ColumnExists("InboundLinkErrors", "LastTimeAccessed"))
            {
                foreach (var data in Database.Fetch<dynamic>(
                    Database.SqlContext.Sql("select Id,TimesAccessed,LastTimeAccessed from InboundLinkErrors")))
                {
                    var viewEntity = new LinkErrorViewEntity()
                    {
                        LinkErrorId = data.Id,
                        VisitCount = data.TimesAccessed,
                        Date = ((DateTime)data.LastTimeAccessed).Date
                    };

                    Database.Insert(viewEntity);
                }
            }

            if (ColumnExists("InboundLinkErrors", "TimesAccessed"))
                Delete.Column("TimesAccessed").FromTable("InboundLinkErrors").Do();

            if (ColumnExists("InboundLinkErrors", "LastTimeAccessed"))
                Delete.Column("LastTimeAccessed").FromTable("InboundLinkErrors").Do();

            if (ColumnExists("InboundLinkErrors", "IsDeleted"))
                Delete.Column("IsDeleted").FromTable("InboundLinkErrors").Do();
        }
    }
}
