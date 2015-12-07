using safe_online_sdk_dotnet;
using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using safe_online_sdk_dotnet_test.test.cs;

namespace safe_online_sdk_dotnet.test.cs
{
    /// <summary>
	/// Test class for the LinkID WS client configuration operations.
	/// </summary>
    [TestFixture]
    public class TestLinkIDWSConfig
    {
        LinkIDServiceClient client = null;

        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();

            client = new LinkIDServiceClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            client.enableLogging();
        }

        [Test]
        public void testThemes()
        {
            // Setup
            String applicationName = "test-shop";

            // Operate
            LinkIDThemes themes = client.getThemes(applicationName);

            // Verify
            Assert.NotNull(themes);
            Assert.AreEqual(4, themes.themes.Count);
            foreach (LinkIDTheme theme in themes.themes)
            {
                Console.WriteLine("Theme: " + theme.name);
            }
        }

        [Test]
        public void testLocalization()
        {
            // Setup
            List<String> keys = new List<string>();
            keys.Add("urn:linkid:wallet:leaseplan");

            // Operate
            List<LinkIDLocalization> localizations = client.getLocalization(keys);

            // Verify
            Assert.NotNull(localizations);
            Assert.AreEqual(2, localizations.Count);
            foreach (LinkIDLocalization localization in localizations)
            {
                Console.WriteLine("Localization: " + localization.key + " - " + localization.keyType);
            }
        }
    }
}
