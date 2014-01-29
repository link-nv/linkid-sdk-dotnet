using safe_online_sdk_dotnet;
using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using safe_online_sdk_dotnet_test.test.cs;

namespace safe_online_sdk_dotnet.test.cs
{
    /// <summary>
	/// Test class for the WS-Notification clients.
	/// </summary>
    [TestFixture]
    public class TestLTQRClient
    {
        LTQRClient client = null;

        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();

            client = new LTQRClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
        }

        [Test]
        public void testPush()
        {
            PaymentContext paymentContext = new PaymentContext(20000, Currency.EUR, ".NET Test", null, 10, false, true);
            DateTime expiryDate = DateTime.Now.AddMonths(3);

            LTQRSession session = client.push(paymentContext, false, expiryDate, null);

            Assert.NotNull(session);
        }

        [Test]
        public void testPull()
        {
            string[] sessionIds = new string[] { "c7353cd2-a3e3-41a7-9a31-d7b659d5cce1" };
            string[] clientSessionIds = new string[] { "011e9a4e-7e3c-4c49-b0fd-ae2572e1ebb3", "38e5a9f1-7094-4257-9010-515922394f65" };

            LTQRClientSession[] clientSessions = client.pull(sessionIds, clientSessionIds);

            Assert.NotNull(clientSessions);
            Assert.AreEqual(2, clientSessions.Length);

            foreach (LTQRClientSession clientSession in clientSessions)
            {
                Console.WriteLine("Client session: " + clientSession.ToString());
            }
        }

        [Test]
        public void testRemove()
        {
            string[] sessionIds = new string[] { "c7353cd2-a3e3-41a7-9a31-d7b659d5cce1" };
            string[] clientSessionIds = new string[] { "011e9a4e-7e3c-4c49-b0fd-ae2572e1ebb3", "38e5a9f1-7094-4257-9010-515922394f65" };

            // operate: fetch
            LTQRClientSession[] clientSessions = client.pull(sessionIds, clientSessionIds);
            Assert.NotNull(clientSessions);
            Assert.AreEqual(2, clientSessions.Length);

            // operate: remove
            client.remove(sessionIds, clientSessionIds);

            // operate: fetch again
            clientSessions = client.pull(sessionIds, clientSessionIds);
            Assert.AreEqual(0, clientSessions.Length);
        }
    }
}
