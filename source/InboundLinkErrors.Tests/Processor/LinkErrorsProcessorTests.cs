using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InboundLinkErrors.Core.Models.Dto;
using InboundLinkErrors.Core.Processor;
using InboundLinkErrors.Tests.Mocks;
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
            linkErrorsService.Data.Add(new LinkErrorDto(missingLink)
            {
                Id = 1,
                Views = new List<LinkErrorViewDto>()
            {
                new LinkErrorViewDto(DateTime.UtcNow.AddMonths(-1), 2)
            }
            });

            linkErrorsProcessor.AddRequest(missingLink);
            linkErrorsProcessor.ProcessData();

            var data = linkErrorsService.Data.First(it => it.Id == 1);
            Assert.AreEqual(2, data.Views.Count);
            Assert.AreEqual(3, data.TotalVisits);
            Assert.AreEqual(1, data.Views.FirstOrDefault(it => it.Date.Equals(DateTime.UtcNow.Date))?.VisitCount);
        }

        [Test]
        public void TestAddingExistingAndNewUrls()
        {
            var missingLinks = new[] { "https://google.com", "https://budgetboardgaming.com" };
            var linkErrorsService = new LinkErrorsServiceMock();
            var linkErrorsProcessor = new LinkErrorsProcessor(linkErrorsService);
            linkErrorsService.Data.Add(new LinkErrorDto("https://budgetboardgaming.com")
            {
                Id = 1,
                Views = new List<LinkErrorViewDto> { new LinkErrorViewDto(DateTime.UtcNow.Date, 1) }
            });

            foreach (var missingLink in missingLinks)
            {
                linkErrorsProcessor.AddRequest(missingLink);
            }
            linkErrorsProcessor.ProcessData();

            Assert.AreEqual(2, linkErrorsService.Data.Count);
            Assert.AreEqual(2, linkErrorsService.Data.FirstOrDefault(it => it.Id == 1)?.TotalVisits);
            Assert.AreEqual(1, linkErrorsService.Data.FirstOrDefault(it => it.Id != 1)?.TotalVisits);
        }

        [Test]
        public void TestAddingNewUserAgent()
        {
            var missingLink = "https://budgetboardgaming.com";
            var userAgent = "Test user agent";
            var linkErrorsService = new LinkErrorsServiceMock();
            var linkErrorsProcessor = new LinkErrorsProcessor(linkErrorsService);

            linkErrorsProcessor.AddRequest(missingLink, userAgent: userAgent);
            linkErrorsProcessor.ProcessData();

            Assert.AreEqual(userAgent, linkErrorsService.Data.First().UserAgents.First().UserAgent);
        }

        [Test]
        public void TestAddingExistingUserAgent()
        {
            var missingLink = "https://budgetboardgaming.com";
            var userAgent = "Test user agent";
            var linkErrorsService = new LinkErrorsServiceMock();
            var linkErrorsProcessor = new LinkErrorsProcessor(linkErrorsService);
            linkErrorsService.Add(new LinkErrorDto(missingLink)
            {
                Id = 1,
                UserAgents = new List<LinkErrorUserAgentDto> { new LinkErrorUserAgentDto(userAgent) { LastAccessedTime = DateTime.UtcNow.AddMonths(-1) } }
            });

            linkErrorsProcessor.AddRequest(missingLink, userAgent: userAgent);
            linkErrorsProcessor.ProcessData();

            Assert.AreEqual(1, linkErrorsService.Data.First().UserAgents.Count);
            Assert.AreEqual(DateTime.UtcNow.Date, linkErrorsService.Data.First().UserAgents.First().LastAccessedTime);
        }

        [Test]
        public void TestAddingNewReferrer()
        {
            var missingLink = "https://budgetboardgaming.com";
            var referrer = "https://test.nl/";
            var linkErrorsService = new LinkErrorsServiceMock();
            var linkErrorsProcessor = new LinkErrorsProcessor(linkErrorsService);

            linkErrorsProcessor.AddRequest(missingLink, referrer: referrer);
            linkErrorsProcessor.ProcessData();

            Assert.AreEqual(referrer, linkErrorsService.Data.First().Referrers.First().Referrer);
        }

        [Test]
        public void TestAddingExistingReferrer()
        {
            var missingLink = "https://budgetboardgaming.com";
            var referrer = "https://test.nl/";
            var linkErrorsService = new LinkErrorsServiceMock();
            var linkErrorsProcessor = new LinkErrorsProcessor(linkErrorsService);
            linkErrorsService.Add(new LinkErrorDto(missingLink)
            {
                Id = 1,
                Referrers = new List<LinkErrorReferrerDto> { new LinkErrorReferrerDto(referrer) { LastAccessedTime = DateTime.UtcNow.AddMonths(-1) } }
            });

            linkErrorsProcessor.AddRequest(missingLink, referrer: referrer);
            linkErrorsProcessor.ProcessData();

            Assert.AreEqual(1, linkErrorsService.Data.First().Referrers.Count);
            Assert.AreEqual(DateTime.UtcNow.Date, linkErrorsService.Data.First().Referrers.First().LastAccessedTime);
        }
    }
}
