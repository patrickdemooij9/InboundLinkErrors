using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Services;
using Umbraco.Core.Mapping;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsMapDefinition : IMapDefinition
    {
        public LinkErrorsMapDefinition()
        {
        }

        public void DefineMaps(UmbracoMapper mapper)
        {
            mapper.Define<LinkErrorEntity, LinkErrorDto>(
                (source, context) => new LinkErrorDto(source.Url),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.IsHidden = source.IsHidden;
                    target.IsDeleted = source.IsDeleted;
                    target.TimesAccessed = source.TimesAccessed;
                    target.LastAccessedTime = source.LastAccessedTime;
                    target.LastReferrer = source.LatestReferrer;
                });

            mapper.Define<LinkErrorDto, LinkErrorEntity>(
                (source, context) => new LinkErrorEntity(),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.IsHidden = source.IsHidden;
                    target.IsDeleted = source.IsDeleted;
                    target.Url = source.Url;
                    target.TimesAccessed = source.TimesAccessed;
                    target.LastAccessedTime = source.LastAccessedTime;
                });

            mapper.Define<LinkErrorReferrerEntity, LinkErrorReferrerDto>(
                (source, context) => new LinkErrorReferrerDto(),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.Referrer = source.Referrer;
                    target.VisitCount = source.VisitCount;
                });

            mapper.Define<LinkErrorReferrerDto, LinkErrorReferrerEntity>(
                (source, context) => new LinkErrorReferrerEntity(),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.Referrer = source.Referrer;
                    target.VisitCount = source.VisitCount;
                });
        }
    }
}
