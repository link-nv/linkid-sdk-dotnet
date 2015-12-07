using safe_online_sdk_dotnet;
using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using safe_online_sdk_dotnet_test.test.cs;

namespace safe_online_sdk_dotnet.test.cs
{
    /// <summary>
	/// Test class for the LTQR WS client.
	/// </summary>
    [TestFixture]
    public class TestLTQRClient
    {
        LinkIDServiceClient client = null;
        String ltqrReference = "5641b76d-806b-47df-ada3-c7cfb9d6734b";

        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();

            client = new LinkIDServiceClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
        }

        [Test]
        public void testPush()
        {
            LinkIDLTQRContent content = new LinkIDLTQRContent();
            content.authenticationMessage = "foo";
            content.finishedMessage = "bar";
            content.paymentContext = new LinkIDPaymentContext(new LinkIDPaymentAmount(20000, LinkIDCurrency.EUR, null),
                "blaat", null, null);
            content.expiryDate = DateTime.Now.AddMonths(3);

            LinkIDLTQRSession session = client.ltqrPush(content, null, LinkIDLTQRLockType.NEVER);

            Assert.NotNull(session);
        }

        [Test]
        public void testChange()
        {
            LinkIDLTQRContent content = new LinkIDLTQRContent();
            content.authenticationMessage = "foo";
            content.finishedMessage = "bar";
            content.paymentContext = new LinkIDPaymentContext(new LinkIDPaymentAmount(20000, LinkIDCurrency.EUR, null),
                "blaat", null, null);
            content.expiryDate = DateTime.Now.AddMonths(3);

            LinkIDLTQRSession session = client.ltqrChange(ltqrReference, content, null, false, false);

            Assert.NotNull(session);
        }

        [Test]
        public void testPull()
        {
            List<String> ltqrReferences = new List<string>();
            ltqrReferences.Add(ltqrReference);

            List<LinkIDLTQRClientSession> clientSessions = client.ltqrPull(ltqrReferences, null, null);

            Assert.NotNull(clientSessions);

            foreach (LinkIDLTQRClientSession clientSession in clientSessions)
            {
                Console.WriteLine("Client session: " + clientSession.ToString());
            }
        }

        [Test]
        public void testRemove()
        {
            List<String> ltqrReferences = new List<string>();
            ltqrReferences.Add(ltqrReference);

            client.ltqrRemove(ltqrReferences, null, null);
        }

        [Test]
        public void testInfo()
        {
            List<String> ltqrReferences = new List<string>();
            ltqrReferences.Add(ltqrReference);

            // operate: info
            List<LinkIDLTQRInfo> infos = client.ltqrInfo(ltqrReferences, null);

            // Verify
            Assert.NotNull(infos);
            Assert.AreEqual(ltqrReferences.Count, infos.Count);

            foreach (LinkIDLTQRInfo info in infos)
            {
                Console.WriteLine("LTQR Info: " + info);
            }
        }
    }
}
