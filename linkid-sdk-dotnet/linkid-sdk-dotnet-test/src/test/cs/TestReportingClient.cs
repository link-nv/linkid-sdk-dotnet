using safe_online_sdk_dotnet;
using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using safe_online_sdk_dotnet_test.test.cs;

namespace safe_online_sdk_dotnet.test.cs
{
    /// <summary>
	/// Test class for the Reporting WS client.
	/// </summary>
    [TestFixture]
    public class TestReportingClient
    {
        ReportingClient client = null;
        ReportingClient parkingClient = null;

        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();

            client = new ReportingClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            client.enableLogging();
        }

        [Test]
        public void testGetPaymentReport()
        {
            DateTime startDate = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
            DateTime endDate = DateTime.Now;

            List<PaymentTransaction> transactions = client.getPaymentReport(startDate, endDate);

            Assert.NotNull(transactions);
            foreach (PaymentTransaction txn in transactions)
            {
                Console.WriteLine("Txn: " + txn.orderReference + " - " + txn.amount);
            }
        }

        [Test]
        public void testGetPaymentReportOrderReferences()
        {
            List<String> orderReferences = new List<string>();
            orderReferences.Add("7a03effa07e1441fa0832a78f262bef2");
            orderReferences.Add("9751acef-1f3c-45bc-aea7-a5e159e7b395");

            List<PaymentTransaction> transactions = client.getPaymentReportForOrderReferences(orderReferences);

            Assert.NotNull(transactions);
            foreach (PaymentTransaction txn in transactions)
            {
                Console.WriteLine("Txn: " + txn.orderReference + " - " + txn.amount);
            }
        }

        [Test]
        public void testGetParkingSessions()
        {
            DateTime startDate = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
            DateTime endDate = DateTime.Now;

            List<ParkingSession> sessions = client.getParkingReport(startDate, endDate);

            Assert.NotNull(sessions);
            foreach (ParkingSession session in sessions)
            {
                Console.WriteLine("Session: " + session.parking + " - " + session.validated);
            }
        }

        [Test]
        public void testGetParkingSessionsByBarCodes()
        {
            List<String> barCodes = new List<string>();
            barCodes.Add("10014905552275590459");
            barCodes.Add("foo");

            List<ParkingSession> sessions = client.getParkingReportForBarCodes(barCodes);

            Assert.NotNull(sessions);
            foreach (ParkingSession session in sessions)
            {
                Console.WriteLine("Session: " + session.parking + " - " + session.validated);
            }
        }
    }
}
