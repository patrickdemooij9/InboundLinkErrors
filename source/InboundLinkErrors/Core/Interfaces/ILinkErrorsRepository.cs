using System.Collections.Generic;
using InboundLinkErrors.Core.Models.Dto;

namespace InboundLinkErrors.Core.Interfaces
{
    public interface ILinkErrorsRepository
    {
        LinkErrorDto Add(LinkErrorDto model);
        LinkErrorDto Update(LinkErrorDto model);
        IEnumerable<LinkErrorDto> UpdateOrAdd(IEnumerable<LinkErrorDto> models);
        void Delete(LinkErrorDto model);
        LinkErrorDto Get(int id);
        IEnumerable<LinkErrorDto> GetByUrl(params string[] urls);
        IEnumerable<LinkErrorDto> GetAll();
    }
}
