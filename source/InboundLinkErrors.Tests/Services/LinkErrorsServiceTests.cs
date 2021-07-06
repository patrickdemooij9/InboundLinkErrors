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
            var linkErrorRepositoryMock = new LinkErrorsMockRepository();
            var linkErrorAdapterMock = new Mock<IRedirectAdapter>();
            var linkErrorsService = new LinkErrorsService(linkErrorRepositoryMock, linkErrorAdapterMock.Object);

            linkErrorsService.Add(new LinkErrorDto(missingLink));

            Assert.AreEqual(1, linkErrorRepositoryMock.Data.Count);
            Assert.AreEqual(missingLink, linkErrorRepositoryMock.Data.FirstOrDefault()?.Url);
        }
    }
}
