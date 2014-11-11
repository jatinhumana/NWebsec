// Copyright (c) Andr� N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.Tests.Unit.TestHelpers;

namespace NWebsec.Mvc.Tests.Unit.Helpers
{
    [TestFixture]
    public class CspConfigMapperTests
    {
        private CspConfigMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new CspConfigMapper();
        }

        [Test]
        public void GetCspDirectiveConfig_CommonCspDirectives_NoException([ValueSource(typeof(CspCommonDirectives), "Directives")] CspDirectives directive)
        {
            var config = new CspConfiguration();

            Assert.DoesNotThrow(() => _mapper.GetCspDirectiveConfig(config, directive));
        }

        [Test]
        public void SetCspDirectiveConfig_CommonCspDirectives_NoException([ValueSource(typeof(CspCommonDirectives), "Directives")] CspDirectives directive)
        {
            var config = new CspConfiguration();
            var directiveConfig = new CspDirectiveConfiguration();

            Assert.DoesNotThrow(() => _mapper.SetCspDirectiveConfig(config, directive, directiveConfig));
        }

        [Test]
        public void GetCspDirectiveConfig_DirectiveSet_ReturnsDirective()
        {
            var directives = CspCommonDirectives.Directives().ToArray();
            var config = new CspConfiguration(false);

            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(config, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(config, directive);
                Assert.IsNotNull(directiveConfig);
                Assert.AreEqual(directive.ToString(), directiveConfig.Nonce);
            }
        }

        [Test]
        public void GetCspDirectiveConfigCloned_NoDirective_ReturnsNull()
        {
            var config = new CspConfiguration(false);
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspDirectiveConfigCloned(config, CspDirectives.ScriptSrc);

            Assert.IsNull(clone);
        }

        [Test]
        public void GetCspDirectiveConfigCloned_DefaultDirective_ClonesElement()
        {
            var directive = new CspDirectiveConfiguration();

            var config = new CspConfiguration(false) { ScriptSrcDirective = directive };
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspDirectiveConfigCloned(config, CspDirectives.ScriptSrc);


            Assert.AreNotSame(directive, clone);
            Assert.That(clone, Is.EqualTo(directive).Using(new CspDirectiveComparer()));
        }

        [Test]
        public void GetCspDirectiveConfigCloned_Configured_ClonesElement()
        {
            var directive = new CspDirectiveConfiguration
            {
                Enabled = false,
                NoneSrc = true,
                SelfSrc = true,
                UnsafeEvalSrc = true,
                UnsafeInlineSrc = false,
                CustomSources = new[] { "https://www.nwebsec.com", "www.klings.org" }
            };

            var config = new CspConfiguration(false) { ScriptSrcDirective = directive };
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspDirectiveConfigCloned(config, CspDirectives.ScriptSrc);

            Assert.AreNotSame(directive, clone);
            Assert.That(clone, Is.EqualTo(directive).Using(new CspDirectiveComparer()));
        }

        [Test]
        public void MergeConfiguration_DirectivesNotConfigured_MergesHeaderAttributesAndInitializesDirectives()
        {
            var sourceConfig = new CspConfiguration(false) { Enabled = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.IsNotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.IsNotNull(destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeConfiguration_DirectivesConfigured_MergesHeaderAttributesAndDirectives()
        {
            var sourceConfig = new CspConfiguration { Enabled = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.AreSame(_mapper.GetCspDirectiveConfig(sourceConfig, directive), _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.AreSame(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeOverrides_HeaderAndDirectivesNotConfigured_InitializesDirectives()
        {
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsTrue(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.IsNotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.IsNotNull(destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeOverrides_HeaderConfiguredAndDirectivesNotConfigured_MergesHeaderConfigAndInitializesDirectives()
        {
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = true };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.IsNotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.IsNotNull(destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeOverrides_HeaderNotConfiguredAndDirectivesConfigured_MergesDirectives()
        {
            var directives = CspCommonDirectives.Directives().ToArray();
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = false };
            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(sourceConfig, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }
            var reportUri = new CspReportUriDirectiveConfiguration();
            sourceConfig.ReportUriDirective = reportUri;
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);
            
            Assert.IsTrue(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(destinationConfig, directive);
                Assert.IsNotNull(directiveConfig);
                Assert.AreEqual(directive.ToString(), directiveConfig.Nonce);
            }

            Assert.AreSame(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeOverrides_HeaderConfiguredAndDirectivesConfigured_MergesHeaderAndDirectives()
        {
            var directives = CspCommonDirectives.Directives().ToArray();
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = true };
            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(sourceConfig, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }
            var reportUri = new CspReportUriDirectiveConfiguration();
            sourceConfig.ReportUriDirective = reportUri;
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(destinationConfig, directive);
                Assert.IsNotNull(directiveConfig);
                Assert.AreEqual(directive.ToString(), directiveConfig.Nonce);
            }

            Assert.AreSame(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }
    }
}