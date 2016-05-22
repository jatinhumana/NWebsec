﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using NWebsec.AspNetCore.Core.Helpers;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
//using NWebsec.Csp;
//using NWebsec.Helpers;
using NWebsec.AspNetCore.Mvc.Helpers;

namespace NWebsec.AspNetCore.Mvc.Tests.Helpers
{
    [TestFixture]
    public class HeaderOverrideHelperTests
    {
        private Mock<IContextConfigurationHelper> _contextHelper;
        private Mock<IHeaderConfigurationOverrideHelper> _configurationOverrideHelper;
        private Mock<IHeaderGenerator> _headerGenerator;
        private Mock<IHeaderResultHandler> _headerResultHandler;
        private Mock<ICspConfigurationOverrideHelper> _cspConfigurationOverrideHelper;
        //private Mock<ICspReportHelper> _reportHelper;
        private HeaderOverrideHelper _overrideHelper;
        private HttpContext _mockContext;
        private HeaderResult _expectedHeaderResult;
        //TODO deal with the reporthelper

        [SetUp]
        public void Setup()
        {
            _contextHelper = new Mock<IContextConfigurationHelper>(MockBehavior.Strict);
            _configurationOverrideHelper = new Mock<IHeaderConfigurationOverrideHelper>(MockBehavior.Strict);
            _headerGenerator = new Mock<IHeaderGenerator>(MockBehavior.Strict);

            _expectedHeaderResult = new HeaderResult(HeaderResult.ResponseAction.Set, "ExpectedHeader", "ninjavalue");
            _headerResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);
            _headerResultHandler.Setup(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult));

            _cspConfigurationOverrideHelper = new Mock<ICspConfigurationOverrideHelper>(MockBehavior.Strict);
            //_reportHelper = new Mock<ICspReportHelper>(MockBehavior.Strict);

            _overrideHelper = new HeaderOverrideHelper(_contextHelper.Object,
                _configurationOverrideHelper.Object,
                _headerGenerator.Object,
                _headerResultHandler.Object,
                _cspConfigurationOverrideHelper.Object);
            //_reportHelper.Object);

            _mockContext = new Mock<HttpContext>().Object;
        }

        [Test]
        public void SetXRobotsTagHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XRobotsTagConfiguration();
            _contextHelper.Setup(h => h.GetXRobotsTagConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXRobotsTagWithOverride(It.IsAny<HttpContext>())).Returns((XRobotsTagConfiguration)null);

            _overrideHelper.SetXRobotsTagHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXRobotsTagResult(It.IsAny<XRobotsTagConfiguration>(), It.IsAny<XRobotsTagConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Test]
        public void SetXRobotsTagHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XRobotsTagConfiguration();
            var overrideConfig = new XRobotsTagConfiguration();
            _contextHelper.Setup(h => h.GetXRobotsTagConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXRobotsTagWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXRobotsTagResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXRobotsTagHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXFrameoptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XFrameOptionsConfiguration();
            _contextHelper.Setup(h => h.GetXFrameOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXFrameoptionsWithOverride(It.IsAny<HttpContext>())).Returns((XFrameOptionsConfiguration)null);

            _overrideHelper.SetXFrameoptionsHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXfoResult(It.IsAny<XFrameOptionsConfiguration>(), It.IsAny<XFrameOptionsConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Test]
        public void SetXFrameoptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XFrameOptionsConfiguration();
            var overrideConfig = new XFrameOptionsConfiguration();
            _contextHelper.Setup(h => h.GetXFrameOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXFrameoptionsWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXfoResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXFrameoptionsHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXContentTypeOptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXContentTypeOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXContentTypeOptionsWithOverride(It.IsAny<HttpContext>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetXContentTypeOptionsHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXContentTypeOptionsResult(It.IsAny<SimpleBooleanConfiguration>(), It.IsAny<SimpleBooleanConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Test]
        public void SetXContentTypeOptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            var overrideConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXContentTypeOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXContentTypeOptionsWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXContentTypeOptionsResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXContentTypeOptionsHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXDownloadOptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXDownloadOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXDownloadOptionsWithOverride(It.IsAny<HttpContext>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetXDownloadOptionsHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXDownloadOptionsResult(It.IsAny<SimpleBooleanConfiguration>(), It.IsAny<SimpleBooleanConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Test]
        public void SetXDownloadOptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            var overrideConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXDownloadOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXDownloadOptionsWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXDownloadOptionsResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXDownloadOptionsHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXXssProtectionHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XXssProtectionConfiguration();
            _contextHelper.Setup(h => h.GetXXssProtectionConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXXssProtectionWithOverride(It.IsAny<HttpContext>())).Returns((XXssProtectionConfiguration)null);

            _overrideHelper.SetXXssProtectionHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXXssProtectionResult(It.IsAny<XXssProtectionConfiguration>(), It.IsAny<XXssProtectionConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Test]
        public void SetXXssProtectionHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XXssProtectionConfiguration();
            var overrideConfig = new XXssProtectionConfiguration();
            _contextHelper.Setup(h => h.GetXXssProtectionConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXXssProtectionWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXXssProtectionResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXXssProtectionHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetNoCacheHeaders_NoOverride_DoesNothing()
        {
            //Get ASP.NET stuff in order
            //var cachePolicy = new Mock<HttpCachePolicyBase>();
            var responseHeaders = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            //response.Setup(r => r.Cache).Returns(cachePolicy.Object);
            response.Setup(r => r.Headers).Returns(responseHeaders);
            Mock.Get(_mockContext).Setup(c => c.Response).Returns(response.Object);

            _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<HttpContext>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetNoCacheHeaders(_mockContext);

            Assert.IsEmpty(responseHeaders);
            
            //headers.CacheControl.
            //cachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
            //cachePolicy.Verify(c => c.SetNoStore(), Times.Never());
            //cachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
            //Assert.IsEmpty(responseHeaders);
        }

        [Test]
        public void SetNoCacheHeaders_OverrideAndDisabled_DoesNothing()
        {
            //Get ASP.NET stuff in order
            //var cachePolicy = new Mock<HttpCachePolicyBase>();
            var responseHeaders = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            //response.Setup(r => r.Cache).Returns(cachePolicy.Object);
            response.Setup(r => r.Headers).Returns(responseHeaders);
            Mock.Get(_mockContext).Setup(c => c.Response).Returns(response.Object);

            var overrideConfig = new SimpleBooleanConfiguration { Enabled = false };
            _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);

            _overrideHelper.SetNoCacheHeaders(_mockContext);

            //TODO cleanup
            //cachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
            //cachePolicy.Verify(c => c.SetNoStore(), Times.Never());
            //cachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
            Assert.IsEmpty(responseHeaders);
        }

        [Test]
        public void SetNoCacheHeaders_OverrideAndEnabled_SetsCacheHeaders()
        {
            //Get ASP.NET stuff in order
            //var cachePolicy = new Mock<HttpCachePolicyBase>();
            var responseHeaders = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            //response.Setup(r => r.Cache).Returns(cachePolicy.Object);
            response.Setup(r => r.Headers).Returns(responseHeaders);
            Mock.Get(_mockContext).Setup(c => c.Response).Returns(response.Object);

            var overrideConfig = new SimpleBooleanConfiguration { Enabled = true };
            _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);

            _overrideHelper.SetNoCacheHeaders(_mockContext);

            var headers = response.Object.GetTypedHeaders();
            var cachePolicy = headers.CacheControl;
            var expiresHeader = responseHeaders["Expires"].Single();
            var pragmaHeader = responseHeaders["Pragma"].Single();

            Assert.IsTrue(cachePolicy.NoCache);
            Assert.IsTrue(cachePolicy.NoStore);
            Assert.IsTrue(cachePolicy.MustRevalidate);
            Assert.AreEqual("-1", expiresHeader);
            Assert.AreEqual("no-cache", pragmaHeader);
            //cachePolicy.Verify(c => c.SetCacheability(HttpCacheability.NoCache), Times.Once());
            //cachePolicy.Verify(c => c.SetNoStore(), Times.Once());
            //cachePolicy.Verify(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches), Times.Once());
        }

        [Test]
        public void SetCspHeaders_NoOverride_DoesNothing([Values(false, true)] bool reportOnly)
        {
            //Get ASP.NET stuff in order
            var request = new Mock<HttpRequest>();
            request.SetupAllProperties();
            Mock.Get(_mockContext).Setup(c => c.Request).Returns(request.Object);

            var contextConfig = new CspConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContext>(), reportOnly)).Returns(contextConfig);
            _cspConfigurationOverrideHelper.Setup(h => h.GetCspConfigWithOverrides(It.IsAny<HttpContext>(), reportOnly)).Returns((CspConfiguration)null);

            _overrideHelper.SetCspHeaders(_mockContext, reportOnly);

            _headerGenerator.Verify(g => g.CreateCspResult(It.IsAny<ICspConfiguration>(), reportOnly, It.IsAny<string>(), It.IsAny<ICspConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Test]
        public void SetCspHeaders_Override_CreatesAndHandlesHeaderResult([Values(false, true)] bool reportOnly)
        {
            //Get ASP.NET stuff in order
            var request = new Mock<HttpRequest>();
            request.SetupAllProperties();
            Mock.Get(_mockContext).Setup(c => c.Request).Returns(request.Object);

            //const string reportUri = "/cspreport";

            var contextConfig = new CspConfiguration();
            var overrideConfig = new CspConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContext>(), reportOnly)).Returns(contextConfig);
            _cspConfigurationOverrideHelper.Setup(h => h.GetCspConfigWithOverrides(It.IsAny<HttpContext>(), reportOnly)).Returns(overrideConfig);
            //_reportHelper.Setup(h => h.GetBuiltInCspReportHandlerRelativeUri()).Returns(reportUri);
            //TODO reporthelper
            _headerGenerator.Setup(g => g.CreateCspResult(overrideConfig, reportOnly, null, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetCspHeaders(_mockContext, reportOnly);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }
    }
}