using System.Collections.Generic;
using InboundLinkErrors.Core.Models.Dto;
using Umbraco.Core.Models.PublishedContent;

namespace InboundLinkErrors.Core.Interfaces
{
    public interface ILinkErrorsService
    {
        LinkErrorDto Add(LinkErrorDto model);
        LinkErrorDto Update(LinkErrorDto model);
        void Delete(int id);
        LinkErrorDto Get(int id);
        IEnumerable<LinkErrorDto> GetByUrl(params string[] urls);
        IEnumerable<LinkErrorDto> GetAll();
        void TrackMissingLink(string requestUrl, string referrer = "", string userAgent = "");
        void SetRedirect(int linkErrorId, IPublishedContent nodeTo, string culture);
    }
}
