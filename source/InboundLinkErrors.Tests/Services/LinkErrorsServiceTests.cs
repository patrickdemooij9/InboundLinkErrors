using System.Collections.Generic;
using System.Linq;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;
using InboundLinkErrors.Core.Services;
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
    }
}
