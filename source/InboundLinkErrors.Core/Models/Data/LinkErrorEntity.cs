using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace InboundLinkErrors.Core.Models.Data
{
    [TableName("InboundLinkErrors")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class LinkErrorEntity
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("Url")]
        public string Url { get; set; }

        [Column("IsHidden")]
        public bool IsHidden { get; set; }
    }
}
