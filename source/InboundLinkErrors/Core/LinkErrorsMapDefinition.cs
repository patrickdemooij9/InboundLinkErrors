using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
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
                });

            mapper.Define<LinkErrorDto, LinkErrorEntity>(
                (source, context) => new LinkErrorEntity(),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.IsHidden = source.IsHidden;
                    target.IsDeleted = source.IsDeleted;
                    target.Url = source.Url;
                });


            mapper.Define<LinkErrorReferrerEntity, LinkErrorReferrerDto>(
                (source, context) => new LinkErrorReferrerDto(source.Referrer),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                });

            mapper.Define<LinkErrorReferrerDto, LinkErrorReferrerEntity>(
                (source, context) => new LinkErrorReferrerEntity(),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.Referrer = source.Referrer;
                });

            mapper.Define<LinkErrorUserAgentEntity, LinkErrorUserAgentDto>(
                (source, context) => new LinkErrorUserAgentDto(source.UserAgent),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                });

            mapper.Define<LinkErrorUserAgentDto, LinkErrorUserAgentEntity>(
                (source, context) => new LinkErrorUserAgentEntity(),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.UserAgent = source.UserAgent;
                });
        }
    }
}
