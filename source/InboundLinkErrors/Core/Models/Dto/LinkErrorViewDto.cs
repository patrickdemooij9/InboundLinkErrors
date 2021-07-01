using System;

namespace InboundLinkErrors.Core.Models.Dto
{
    public class LinkErrorViewDto
    {
        private int _visitCount;

        public DateTime Date { get; }
        public int VisitCount { get; set; }


        public LinkErrorViewDto(DateTime date, int visitCount = 0)
        {
            Date = date;
            _visitCount = visitCount;
        }
    }
}
