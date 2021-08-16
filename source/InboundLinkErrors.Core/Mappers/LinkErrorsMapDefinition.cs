using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
using Umbraco.Cms.Core.Mapping;

namespace InboundLinkErrors.Core.Mappers
{
    public class LinkErrorsMapDefinition : IMapDefinition
    {
        public LinkErrorsMapDefinition()
        {
        }

        public void DefineMaps(IUmbracoMapper mapper)
        {
            mapper.Define<LinkErrorEntity, LinkErrorDto>(
                (source, context) => new LinkErrorDto(source.Url),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.IsHidden = source.IsHidden;
                });

            mapper.Define<LinkErrorDto, LinkErrorEntity>(
                (source, context) => new LinkErrorEntity(),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.IsHidden = source.IsHidden;
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
