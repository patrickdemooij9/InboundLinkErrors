using System;
using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
using Umbraco.Core.Mapping;

namespace InboundLinkErrors.Core.Mappers
{
    public class LinkErrorsViewMapDefinition : IMapDefinition
    {
        public void DefineMaps(UmbracoMapper mapper)
        {
            mapper.Define<LinkErrorViewDto, LinkErrorViewEntity>(
                (source, context) => new LinkErrorViewEntity(),
                (source, target, context) =>
                {
                    if (!context.Items.ContainsKey("LinkErrorId"))
                        throw new ArgumentException("No LinkErrorId supplied in context!");
                    target.LinkErrorId = (int)context.Items["LinkErrorId"];
                    target.Date = source.Date;
                    target.VisitCount = source.VisitCount;
                });

            mapper.Define<LinkErrorViewEntity, LinkErrorViewDto>(
                (source, context) => new LinkErrorViewDto(source.Date, source.VisitCount),
                (source, target, context) =>
                {

                });
        }
    }
}
