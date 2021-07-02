using System;
using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
using Umbraco.Core.Mapping;

namespace InboundLinkErrors.Core.Mappers
{
    public class LinkErrorsReferrerMapDefinition : IMapDefinition
    {
        public void DefineMaps(UmbracoMapper mapper)
        {
            mapper.Define<LinkErrorReferrerDto, LinkErrorReferrerEntity>(
                (source, context) => new LinkErrorReferrerEntity(),
                (source, target, context) =>
                {
                    if (!context.Items.ContainsKey("LinkErrorId"))
                        throw new ArgumentException("No LinkErrorId supplied in context!");
                    target.Id = source.Id;
                    target.LinkErrorId = (int)context.Items["LinkErrorId"];
                    target.Referrer = source.Referrer;
                    target.LastAccessedTime = source.LastAccessedTime;
                });

            mapper.Define<LinkErrorReferrerEntity, LinkErrorReferrerDto>(
                (source, context) => new LinkErrorReferrerDto(source.Referrer),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.LastAccessedTime = source.LastAccessedTime;
                });
        }
    }
}
