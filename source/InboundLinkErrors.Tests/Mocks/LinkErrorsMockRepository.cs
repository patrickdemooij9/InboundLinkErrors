using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;

namespace InboundLinkErrors.Tests.Mocks
{
    public class LinkErrorsMockRepository : ILinkErrorsRepository
    {
        public List<LinkErrorDto> Data { get; set; }

        public LinkErrorsMockRepository()
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

        public IEnumerable<LinkErrorDto> UpdateOrAdd(IEnumerable<LinkErrorDto> models)
        {
            foreach (var model in models)
            {
                if (!Data.Contains(model))
                    Data.Add(model);
            }

            return models;
        }

        public void Delete(LinkErrorDto model)
        {
            Data.Remove(model);
        }

        public LinkErrorDto Get(int id)
        {
            return Data.FirstOrDefault(it => it.Id == id);
        }

        public IEnumerable<LinkErrorDto> GetByUrl(params string[] urls)
        {
            return Data.Where(it => urls.Contains(it.Url));
        }

        public LinkErrorDto GetByUrl(string url)
        {
            return Data.FirstOrDefault(it => it.Url == url);
        }

        public IEnumerable<LinkErrorDto> GetAll()
        {
            return Data;
        }
    }
}
