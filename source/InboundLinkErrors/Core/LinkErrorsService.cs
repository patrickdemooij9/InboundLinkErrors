using System;
using System.Collections.Generic;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models;
using Umbraco.Core.Mapping;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsService
    {
        private readonly LinkErrorsRepository _linkErrorsRepository;
        private readonly IRedirectAdapter _redirectService;
        private readonly UmbracoMapper _umbracoMapper;

        private readonly object _lock = new object();

        public LinkErrorsService(LinkErrorsRepository linkErrorsRepository, UmbracoMapper umbracoMapper, IRedirectAdapter redirectService)
        {
            _linkErrorsRepository = linkErrorsRepository;
            _redirectService = redirectService;
            _umbracoMapper = umbracoMapper;
        }

        public void Add(LinkErrorDto model)
        {
            _linkErrorsRepository.Add(_umbracoMapper.Map<LinkErrorDto, LinkErrorEntity>(model));
        }

        public void Update(LinkErrorDto model)
        {
            _linkErrorsRepository.Update(_umbracoMapper.Map<LinkErrorDto, LinkErrorEntity>(model));
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

        public void TrackMissingLink(string url)
        {
            var formattedUrl = url.ToLowerInvariant();
            lock (_lock)
            {
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

                if (linkError.Id == 0)
                {
                    Add(linkError);
                }
                else
                {
                    Update(linkError);
                }
            }
        }

        public void SetRedirect(int linkErrorId, string urlTo)
        {
            var linkError = Get(linkErrorId);
            _redirectService.AddRedirect(linkError.Url, urlTo);
            Delete(linkErrorId);
        }
    }
}
