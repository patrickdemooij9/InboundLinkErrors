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
    public class LinkErrorsService : ILinkErrorsService
    {
        private readonly ILinkErrorsRepository _linkErrorsRepository;
        private readonly IRedirectAdapter _redirectService;

        public LinkErrorsService(ILinkErrorsRepository linkErrorsRepository, IRedirectAdapter redirectService)
        {
            _linkErrorsRepository = linkErrorsRepository;
            _redirectService = redirectService;
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

        public IEnumerable<LinkErrorDto> GetByUrl(params string[] urls)
        {
            return _linkErrorsRepository.GetByUrl(urls);
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

        public void SetRedirect(int linkErrorId, IPublishedContent nodeTo, string culture)
        {
            var linkError = Get(linkErrorId);
            _redirectService.AddRedirect(linkError.Url, nodeTo, culture);
            Delete(linkErrorId);
        }
    }
}
