using System;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace InboundLinkErrors.Core.Models
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

        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }

        [Column("TimesAccessed")]
        public int TimesAccessed { get; set; }

        [Column("LastTimeAccessed")]
        public DateTime LastAccessedTime { get; set; }
    }
}
