using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;
using InboundLinkErrors.Core.Processor;
using InboundLinkErrors.Core.Services;
using InboundLinkErrors.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace InboundLinkErrors.Tests.Processor
{
    [TestFixture]
    public class LinkErrorsProcessorTests
    {
        [Test]
        public void TestAddingMultipleUrls()
        {
            var missingLinks = new[]
                {"https://google.com", "https://google.com", "https://test.nl", "https://google.com/"};
            var linkErrorsService = new LinkErrorsServiceMock();
            var linkErrorsProcessor = new LinkErrorsProcessor(linkErrorsService);

            Parallel.ForEach(missingLinks, (link) => linkErrorsProcessor.AddRequest(link));
            linkErrorsProcessor.ProcessData();
            var allData = linkErrorsService.Data.ToArray();

            Assert.AreEqual(2, allData.Length);
            Assert.AreEqual(3, allData.FirstOrDefault(it => it.Url == "https://google.com")?.TotalVisits);
            Assert.AreEqual(1, allData.FirstOrDefault(it => it.Url == "https://test.nl")?.TotalVisits);
        }

        [Test]
        public void TestAddDifferentMonthUrl()
        {
            var missingLink = "https://google.com";
            var linkErrorsService = new LinkErrorsServiceMock();
            var linkErrorsProcessor = new LinkErrorsProcessor(linkErrorsService);
            linkErrorsService.Data.Add(new LinkErrorDto(missingLink){Id = 1, Views = new List<LinkErrorViewDto>()
            {
                new LinkErrorViewDto(DateTime.UtcNow.AddMonths(-1), 2)
            }});

            linkErrorsProcessor.AddRequest(missingLink);
            linkErrorsProcessor.ProcessData();

            var data = linkErrorsService.Data.First(it => it.Id == 1); 
            Assert.AreEqual(2, data.Views.Count);
            Assert.AreEqual(3, data.TotalVisits);
            Assert.AreEqual(1, data.Views.FirstOrDefault(it => it.Date.Equals(DateTime.UtcNow.Date))?.VisitCount);
        }
    }
}
