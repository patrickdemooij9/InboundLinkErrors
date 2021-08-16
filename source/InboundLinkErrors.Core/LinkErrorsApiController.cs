using System;
using System.Linq;
using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace InboundLinkErrors.Core
{
    [PluginController("LinkErrors")]
    public class LinkErrorsApiController : UmbracoAuthorizedApiController
    {
        private readonly ILinkErrorsService _linkErrorsService;
        private readonly ILinkErrorConfigurationProvider _configurationProvider;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public LinkErrorsApiController(ILinkErrorsService linkErrorsService, ILinkErrorConfigurationProvider configurationProvider, IUmbracoContextFactory umbracoContextFactory)
        {
            _linkErrorsService = linkErrorsService;
            _configurationProvider = configurationProvider;
            _umbracoContextFactory = umbracoContextFactory;
        }

        [HttpGet]
        public LinkErrorViewModel GetAll()
        {
            return new LinkErrorViewModel
            {
                Items = _linkErrorsService.GetAll().Select(it => new LinkErrorItemViewModel
                {
                    Id = it.Id,
                    Url = it.Url,
                    RelativeUrl = new Uri(it.Url).AbsolutePath,
                    IsMedia = it.IsMedia,
                    IsHidden = it.IsHidden,
                    Views = it.Views.ToDictionary(v => v.Date, v => v.VisitCount),
                    Referrers = it.Referrers.Select(r => r.Referrer).ToArray(),
                    UserAgents = it.UserAgents.Select(u => u.UserAgent).ToArray()
                }).ToArray(),
                AllowMedia = _configurationProvider.GetConfiguration().TrackMedia
            };
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
            using var ctx = _umbracoContextFactory.EnsureUmbracoContext();
            var node = ctx.UmbracoContext.Content.GetById(nodeId);
            if (node is null)
                return new LinkErrorRequestResponse("Umbraco node could not be found!");

            _linkErrorsService.SetRedirect(linkErrorId, node, culture);
            return new LinkErrorRequestResponse();
        }

        [HttpPost]
        public LinkErrorRequestResponse Hide(int id, bool toggle)
        {
            var model = _linkErrorsService.Get(id);
            if (model is null)
                return new LinkErrorRequestResponse("Could not find item");

            model.IsHidden = toggle;
            _linkErrorsService.Update(model);
            return new LinkErrorRequestResponse();
        }
    }
}
