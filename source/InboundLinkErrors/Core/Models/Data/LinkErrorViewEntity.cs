using System;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace InboundLinkErrors.Core.Models.Data
{
    [TableName("InboundLinkErrorView")]
    [ExplicitColumns]
    [PrimaryKey(new[] { "LinkErrorId", "Date" })]
    public class LinkErrorViewEntity
    {
        [Column("LinkErrorId")]
        [PrimaryKeyColumn(AutoIncrement = false, OnColumns = "LinkErrorId, Date")]
        [ForeignKey(typeof(LinkErrorEntity), Column = "Id", Name = "FK_InboundLinkErrorView_InboundLinkError")]
        public int LinkErrorId { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }

        [Column("VisitCount")]
        public int VisitCount { get; set; }
    }
}
