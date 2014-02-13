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
            String orderReference = "DOTNET-LTQR-" + Guid.NewGuid().ToString();
            PaymentContext paymentContext = new PaymentContext(20000, Currency.EUR, ".NET Test", orderReference, null, 10, false, true);
            DateTime expiryDate = DateTime.Now.AddMonths(3);

            LTQRSession session = client.push(paymentContext, false, expiryDate, null);

            Assert.NotNull(session);
        }

        [Test]
        public void testPull()
        {
            string[] orderReferences = new string[] { "DOTNET-LTQR-7a9218c0-f559-4e89-9cb6-14f9a9bfe4d7" };
            string[] clientSessionIds = new string[] { };

            LTQRClientSession[] clientSessions = client.pull(orderReferences, clientSessionIds);

            Assert.NotNull(clientSessions);
            //Assert.AreEqual(1, clientSessions.Length);

            foreach (LTQRClientSession clientSession in clientSessions)
            {
                Console.WriteLine("Client session: " + clientSession.ToString());
            }
        }

        [Test]
        public void testRemove()
        {
            string[] orderReferences = new string[] { "DOTNET-LTQR-7a9218c0-f559-4e89-9cb6-14f9a9bfe4d7" };
            string[] clientSessionIds = new string[] { };

            // operate: fetch
            LTQRClientSession[] clientSessions = client.pull(orderReferences, clientSessionIds);
            Assert.NotNull(clientSessions);
            //Assert.AreEqual(2, clientSessions.Length);
            foreach (LTQRClientSession clientSession in clientSessions)
            {
                Console.WriteLine("Client session: " + clientSession.ToString());
            }

            // operate: remove
            client.remove(orderReferences, clientSessionIds);

            // operate: fetch again
            clientSessions = client.pull(orderReferences, clientSessionIds);
            Assert.AreEqual(0, clientSessions.Length);
        }
    }
}
