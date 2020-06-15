using System;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace InboundLinkErrors.Core.Models.Data
{
    [TableName("InboundLinkErrorUserAgent")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class LinkErrorUserAgentEntity
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("LinkErrorId")]
        [ForeignKey(typeof(LinkErrorEntity), Column = "Id", Name = "FK_InboundLinkErrorUserAgent_InboundLinkError")]
        public int LinkErrorId { get; set; }

        [Column("UserAgent")]
        public string UserAgent { get; set; }

        [Column("VisitCount")]
        public int VisitCount { get; set; }

        [Column("LastAccessedTime")]
        public DateTime LastAccessedTime { get; set; }
    }
}
