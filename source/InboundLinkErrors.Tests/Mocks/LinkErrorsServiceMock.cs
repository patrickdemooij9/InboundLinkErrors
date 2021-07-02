using System;
using System.Collections.Generic;
using System.Linq;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;
using Umbraco.Core.Models.PublishedContent;

namespace InboundLinkErrors.Tests.Mocks
{
    public class LinkErrorsServiceMock : ILinkErrorsService
    {
        public List<LinkErrorDto> Data;

        public LinkErrorsServiceMock()
        {
            Data = new List<LinkErrorDto>();
        }

        public LinkErrorDto Add(LinkErrorDto model)
        {
            Data.Add(model);
            return model;
        }

        public LinkErrorDto Update(LinkErrorDto model)
        {
            return model;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public LinkErrorDto Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LinkErrorDto> GetByUrl(params string[] urls)
        {
            return Data.Where(it => urls.Contains(it.Url));
        }

        public IEnumerable<LinkErrorDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public void SetRedirect(int linkErrorId, IPublishedContent nodeTo, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
