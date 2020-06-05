using System;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace InboundLinkErrors.Core.Models
{
    [TableName("InboundLinkErrorReferrers")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class LinkErrorReferrerEntity
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("LinkErrorId")]
        [ForeignKey(typeof(LinkErrorEntity), Column = "Id", Name = "FK_InboundLinkErrorRefferer_InboundLinkError")]
        public int LinkErrorId { get; set; }

        [Column("Referrer")]
        public string Referrer { get; set; }

        [Column("VisitCount")]
        public int VisitCount { get; set; }

        [Column("LastAccessedTime")]
        public DateTime LastAccessedTime { get; set; }
    }
}
