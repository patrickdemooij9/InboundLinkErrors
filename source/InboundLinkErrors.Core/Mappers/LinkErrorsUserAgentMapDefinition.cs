using System;
using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
using Umbraco.Cms.Core.Mapping;

namespace InboundLinkErrors.Core.Mappers
{
    public class LinkErrorsUserAgentMapDefinition : IMapDefinition
    {
        public void DefineMaps(IUmbracoMapper mapper)
        {
            mapper.Define<LinkErrorUserAgentDto, LinkErrorUserAgentEntity>(
                (source, context) => new LinkErrorUserAgentEntity(),
                (source, target, context) =>
                {
                    if (!context.Items.ContainsKey("LinkErrorId"))
                        throw new ArgumentException("No LinkErrorId supplied in context!");
                    target.Id = source.Id;
                    target.LinkErrorId = (int)context.Items["LinkErrorId"];
                    target.UserAgent = source.UserAgent;
                    target.LastAccessedTime = source.LastAccessedTime;
                });

            mapper.Define<LinkErrorUserAgentEntity, LinkErrorUserAgentDto>(
                (source, context) => new LinkErrorUserAgentDto(source.UserAgent),
                (source, target, context) =>
                {
                    target.Id = source.Id;
                    target.LastAccessedTime = source.LastAccessedTime;
                });
        }
    }
}
