using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;
using InboundLinkErrors.Core.Repositories;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace InboundLinkErrors.Core.Services
{
    public class LinkErrorsService
    {
        private readonly object _linkErrorLock = new object();
        private readonly object _linkViewLock = new object();
        private ConcurrentDictionary<string, LinkErrorDto> _itemsToSync;

        private readonly LinkErrorsRepository _linkErrorsRepository;
        private readonly IRedirectAdapter _redirectService;
        private readonly LinkErrorsReferrerService _linkErrorsReferrerService;
        private readonly LinkErrorsUserAgentService _linkErrorsUserAgentService;

        public LinkErrorsService(LinkErrorsRepository linkErrorsRepository, IRedirectAdapter redirectService, LinkErrorsReferrerService linkErrorsReferrerService, LinkErrorsUserAgentService linkErrorsUserAgentService)
        {
            _linkErrorsRepository = linkErrorsRepository;
            _redirectService = redirectService;
            _linkErrorsReferrerService = linkErrorsReferrerService;
            _linkErrorsUserAgentService = linkErrorsUserAgentService;

            _itemsToSync = new ConcurrentDictionary<string, LinkErrorDto>();
        }

        public LinkErrorDto Add(LinkErrorDto model)
        {
            return _linkErrorsRepository.Add(model);
        }

        public LinkErrorDto Update(LinkErrorDto model)
        {
            return _linkErrorsRepository.Update(model);
        }

        public void Delete(int id)
        {
            _linkErrorsRepository.Delete(_linkErrorsRepository.Get(id));
        }

        public LinkErrorDto Get(int id)
        {
            return _linkErrorsRepository.Get(id);
        }

        public LinkErrorDto GetByUrl(string url)
        {
            return _linkErrorsRepository.GetByUrl(url);
        }

        public IEnumerable<LinkErrorDto> GetAll()
        {
            return _linkErrorsRepository.GetAll();
        }

        public void ToggleHide(int id, bool toggle)
        {
            var dto = Get(id);
            dto.IsHidden = toggle;
            Update(dto);
        }

        public void TrackMissingLink(HttpRequest request)
        {
            var formattedUrl = request.Url.AbsoluteUri.ToLowerInvariant().TrimEnd("/");
            var linkError = GetLinkErrorByUrl(formattedUrl);
            var todayView = GetCurrentView(linkError);

            //TODO: Remove the IsDeleted logic. Just delete it
            //If a missing link is deleted, we want to "reset" it.
            /*if (linkError.IsDeleted)
            {
                todayView.VisitCount = 1;
                linkError.IsDeleted = false;
            }
            else
            {
                Interlocked.Increment(ref todayView.VisitCount);
            }*/

            todayView.Increment();

            //linkError.LastAccessedTime = DateTime.UtcNow;

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

        public void SyncToDatabase()
        {
            var copyItems = new ConcurrentDictionary<string, LinkErrorDto>(_itemsToSync);
            _itemsToSync.Clear();

            foreach (var (_, value) in copyItems)
            {
                if (value.Id == 0)
                    _linkErrorsRepository.Add(value);
                else
                    _linkErrorsRepository.Update(value);
            }
        }

        private LinkErrorDto GetLinkErrorByUrl(string url)
        {
            if (_itemsToSync.ContainsKey(url))
                return _itemsToSync[url];

            lock (_linkErrorLock)
            {
                if (_itemsToSync.ContainsKey(url))
                    return _itemsToSync[url];

                var model = _linkErrorsRepository.GetByUrl(url) ?? new LinkErrorDto(url);

                _itemsToSync.TryAdd(url, model);
                return _itemsToSync[url];
            }
        }

        private LinkErrorViewDto GetCurrentView(LinkErrorDto model)
        {
            var currentView = model.Views.FirstOrDefault(it => it.Date == DateTime.UtcNow.Date);
            if (currentView != null)
                return currentView;

            lock (_linkViewLock)
            {
                currentView = model.Views.FirstOrDefault(it => it.Date == DateTime.UtcNow.Date);
                if (currentView != null)
                    return currentView;

                currentView = new LinkErrorViewDto(DateTime.UtcNow.Date);
                model.Views.Add(currentView);

                return currentView;
            }
        }
    }
}
