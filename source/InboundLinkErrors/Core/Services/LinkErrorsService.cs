using System;
using System.Collections.Generic;
using System.Web;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
using InboundLinkErrors.Core.Repositories;
using Umbraco.Core.Mapping;
using Umbraco.Core.Models.PublishedContent;

namespace InboundLinkErrors.Core.Services
{
    public class LinkErrorsService
    {
        private readonly LinkErrorsRepository _linkErrorsRepository;
        private readonly IRedirectAdapter _redirectService;
        private readonly UmbracoMapper _umbracoMapper;
        private readonly LinkErrorsReferrerService _linkErrorsReferrerService;
        private readonly LinkErrorsUserAgentService _linkErrorsUserAgentService;

        public LinkErrorsService(LinkErrorsRepository linkErrorsRepository, UmbracoMapper umbracoMapper, IRedirectAdapter redirectService, LinkErrorsReferrerService linkErrorsReferrerService, LinkErrorsUserAgentService linkErrorsUserAgentService)
        {
            _linkErrorsRepository = linkErrorsRepository;
            _redirectService = redirectService;
            _umbracoMapper = umbracoMapper;
            _linkErrorsReferrerService = linkErrorsReferrerService;
            _linkErrorsUserAgentService = linkErrorsUserAgentService;
        }

        public LinkErrorDto Add(LinkErrorDto model)
        {
            return _umbracoMapper.Map<LinkErrorEntity, LinkErrorDto>(_linkErrorsRepository.Add(_umbracoMapper.Map<LinkErrorDto, LinkErrorEntity>(model)));
        }

        public LinkErrorDto Update(LinkErrorDto model)
        {
            return _umbracoMapper.Map<LinkErrorEntity, LinkErrorDto>(_linkErrorsRepository.Update(_umbracoMapper.Map<LinkErrorDto, LinkErrorEntity>(model)));
        }

        public void Delete(int id)
        {
            _linkErrorsRepository.Delete(_linkErrorsRepository.Get(id));
        }

        public LinkErrorDto Get(int id)
        {
            return _umbracoMapper.Map<LinkErrorEntity, LinkErrorDto>(_linkErrorsRepository.Get(id));
        }

        public LinkErrorDto GetByUrl(string url)
        {
            return _umbracoMapper.Map<LinkErrorEntity, LinkErrorDto>(_linkErrorsRepository.GetByUrl(url));
        }

        public IEnumerable<LinkErrorDto> GetAll()
        {
            return _umbracoMapper.MapEnumerable<LinkErrorEntity, LinkErrorDto>(_linkErrorsRepository.GetAll());
        }

        public void ToggleHide(int id, bool toggle)
        {
            var dto = Get(id);
            dto.IsHidden = toggle;
            Update(dto);
        }

        public void TrackMissingLink(HttpRequest request)
        {
            var formattedUrl = request.Url.AbsoluteUri.ToLowerInvariant();
            var linkError = GetByUrl(formattedUrl) ?? new LinkErrorDto(formattedUrl);

            //If a missing link is deleted, we want to "reset" it.
            if (linkError.IsDeleted)
            {
                linkError.TimesAccessed = 1;
                linkError.IsDeleted = false;
            }
            else
            {
                linkError.TimesAccessed++;
            }

            linkError.LastAccessedTime = DateTime.UtcNow;

            linkError = linkError.Id == 0 ? Add(linkError) : Update(linkError);

            if (!string.IsNullOrWhiteSpace(request.UrlReferrer?.AbsoluteUri))
            {
                //_linkErrorsReferrerService.TrackReferrer(request.UrlReferrer.AbsoluteUri, linkError.Id);
            }

            if (!string.IsNullOrWhiteSpace(request.UserAgent))
            {
                //_linkErrorsUserAgentService.TrackUserAgent(request.UserAgent, linkError.Id);
            }
        }

        public void SetRedirect(int linkErrorId, IPublishedContent nodeTo, string culture)
        {
            var linkError = Get(linkErrorId);
            _redirectService.AddRedirect(linkError.Url, nodeTo, culture);
            Delete(linkErrorId);
        }
    }
}
