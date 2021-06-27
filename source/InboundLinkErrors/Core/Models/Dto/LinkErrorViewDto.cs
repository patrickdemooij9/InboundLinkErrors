using System;
using System.Threading;

namespace InboundLinkErrors.Core.Models.Dto
{
    public class LinkErrorViewDto
    {
        private int _visitCount;

        public DateTime Date { get; }
        public int VisitCount => _visitCount;

        public LinkErrorViewDto(DateTime date, int visitCount = 0)
        {
            Date = date;
            _visitCount = visitCount;
        }

        public void Increment()
        {
            Interlocked.Increment(ref _visitCount);
        }
    }
}
