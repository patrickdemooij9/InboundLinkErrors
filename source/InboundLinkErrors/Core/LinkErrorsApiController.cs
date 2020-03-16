using System.Collections.Generic;
using System.Web.Http;
using InboundLinkErrors.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace InboundLinkErrors.Core
{
    [PluginController("LinkErrors")]
    public class LinkErrorsApiController : UmbracoAuthorizedApiController
    {
        private readonly LinkErrorsService _linkErrorsService;

        public LinkErrorsApiController(LinkErrorsService linkErrorsService)
        {
            _linkErrorsService = linkErrorsService;
        }

        [HttpGet]
        public IEnumerable<LinkErrorDto> GetAll()
        {
            return _linkErrorsService.GetAll();
        }

        [HttpDelete]
        public LinkErrorRequestResponse Delete(int id)
        {
            if (id <= 0) return new LinkErrorRequestResponse("Id is not valid!");

            _linkErrorsService.Delete(id);
            return new LinkErrorRequestResponse();
        }

        [HttpPost]
        public LinkErrorRequestResponse SetRedirect(int linkErrorId, int nodeId, string culture)
        {
            var node = Umbraco.Content(nodeId);
            if (node is null)
                return new LinkErrorRequestResponse("Umbraco node could not be found!");

            _linkErrorsService.SetRedirect(linkErrorId, node, culture);
            return new LinkErrorRequestResponse();
        }

        [HttpPost]
        public LinkErrorRequestResponse Hide(int id, bool toggle)
        {
            _linkErrorsService.ToggleHide(id, toggle);
            return new LinkErrorRequestResponse();
        }
    }
}
