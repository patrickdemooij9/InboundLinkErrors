using System;

namespace InboundLinkErrors.Core.Models.Dto
{
    public class LinkErrorViewDto
    {
        public DateTime Date { get; }
        public int VisitCount { get; set; }


        public LinkErrorViewDto(DateTime date, int visitCount = 0)
        {
            Date = date;
            VisitCount = visitCount;
        }
    }
}
