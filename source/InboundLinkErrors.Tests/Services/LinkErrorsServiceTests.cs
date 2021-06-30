using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;
using InboundLinkErrors.Core.Services;
using InboundLinkErrors.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace InboundLinkErrors.Tests.Services
{
    [TestFixture]
    public class LinkErrorsServiceTests
    {
        [Test]
        public void TestAddingNonExistingUrl()
        {
            var missingLink = "https://google.nl";
            var linkErrorRepositoryMock = new Mock<ILinkErrorsRepository>();
            var linkErrorAdapterMock = new Mock<IRedirectAdapter>();
            var linkErrorsService = new LinkErrorsService(linkErrorRepositoryMock.Object, linkErrorAdapterMock.Object);

            linkErrorsService.TrackMissingLink(missingLink);
            linkErrorsService.SyncToDatabase();

            linkErrorRepositoryMock.Verify(it => it.UpdateOrAdd(It.Is<IEnumerable<LinkErrorDto>>(e => e.FirstOrDefault(l => l.Url.Equals(missingLink) && l.TotalVisits == 1) != null)));
        }

        [Test]
        public void CleanupDatabaseSyncAfterSync()
        {
            var linkErrorRepositoryMock = new Mock<ILinkErrorsRepository>();
            var linkErrorAdapterMock = new Mock<IRedirectAdapter>();
            var linkErrorsService = new LinkErrorsService(linkErrorRepositoryMock.Object, linkErrorAdapterMock.Object);

            linkErrorsService.TrackMissingLink("https://google.nl");
            linkErrorsService.SyncToDatabase();
            linkErrorsService.SyncToDatabase();

            linkErrorRepositoryMock.Verify(it => it.UpdateOrAdd(It.IsAny<IEnumerable<LinkErrorDto>>()), Times.Once);
        }

        [Test]
        public void TestAddingMultipleUrls()
        {
            var missingLinks = new[]
                {"https://google.com", "https://google.com", "https://test.nl", "https://google.com/"};
            var linkErrorRepositoryMock = new LinkErrorsMockRepository();
            var linkErrorAdapterMock = new Mock<IRedirectAdapter>();
            var linkErrorsService = new LinkErrorsService(linkErrorRepositoryMock, linkErrorAdapterMock.Object);

            Parallel.ForEach(missingLinks, (link) => linkErrorsService.TrackMissingLink(link));
            linkErrorsService.SyncToDatabase();
            var allData = linkErrorRepositoryMock.GetAll().ToArray();

            Assert.AreEqual(2, allData.Length);
            Assert.AreEqual(3, allData.FirstOrDefault(it => it.Url == "https://google.com")?.TotalVisits);
            Assert.AreEqual(1, allData.FirstOrDefault(it => it.Url == "https://test.nl")?.TotalVisits);
        }

        [Test]
        public void TestAddDifferentMonthUrl()
        {
            var missingLink = "https://google.com";
            var linkErrorRepositoryMock = new LinkErrorsMockRepository();
            var linkErrorAdapterMock = new Mock<IRedirectAdapter>();
            var linkErrorsService = new LinkErrorsService(linkErrorRepositoryMock, linkErrorAdapterMock.Object);
            linkErrorRepositoryMock.Add(new LinkErrorDto(missingLink){Id = 1, Views = new ConcurrentBag<LinkErrorViewDto>()
            {
                new LinkErrorViewDto(DateTime.UtcNow.AddMonths(-1), 2)
            }});

            linkErrorsService.TrackMissingLink(missingLink);
            linkErrorsService.SyncToDatabase();

            var data = linkErrorRepositoryMock.Get(1); 
            Assert.AreEqual(2, data.Views.Count);
            Assert.AreEqual(3, data.TotalVisits);
            Assert.AreEqual(1, data.Views.FirstOrDefault(it => it.Date.Equals(DateTime.UtcNow.Date))?.VisitCount);
        }
    }
}
