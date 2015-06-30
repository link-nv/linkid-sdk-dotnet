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
            DateTime startDate = DateTime.Now.Subtract(new TimeSpan(90, 0, 0, 0));
            DateTime endDate = DateTime.Now;

            List<LinkIDPaymentOrder> paymentOrders = client.getPaymentReport(startDate, endDate);

            Assert.NotNull(paymentOrders);
            foreach (LinkIDPaymentOrder paymentOrder in paymentOrders)
            {
                Console.WriteLine(paymentOrder.ToString());
            }
        }

        [Test]
        public void testGetPaymentReportOrderReferences()
        {
            List<String> orderReferences = new List<string>();
            orderReferences.Add("afc766fe-f091-4f8e-9dec-eed83422ca43");
            orderReferences.Add("02a4e6c2-67d1-4f71-b262-a12b447c1435");

            List<LinkIDPaymentOrder> paymentOrders = client.getPaymentReportForOrderReferences(orderReferences);

            Assert.NotNull(paymentOrders);
            foreach (LinkIDPaymentOrder paymentOrder in paymentOrders)
            {
                Console.WriteLine(paymentOrder.ToString());
            }
        }

        [Test]
        public void testGetParkingSessions()
        {
            DateTime startDate = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
            DateTime endDate = DateTime.Now;

            List<LinkIDParkingSession> sessions = client.getParkingReport(startDate, endDate);

            Assert.NotNull(sessions);
            foreach (LinkIDParkingSession session in sessions)
            {
                Console.WriteLine(session.ToString());
            }
        }

        [Test]
        public void testGetParkingSessionsByBarCodes()
        {
            List<String> barCodes = new List<string>();
            barCodes.Add("10014905552275590459");
            barCodes.Add("foo");

            List<LinkIDParkingSession> sessions = client.getParkingReportForBarCodes(barCodes);

            Assert.NotNull(sessions);
            foreach (LinkIDParkingSession session in sessions)
            {
                Console.WriteLine(session.ToString());
            }
        }

        [Test]
        public void testWalletReportDateFilter()
        {
            String walletOrganizationId = "f508212c-9189-4402-ab76-6e26110697b4";
            DateTime startDate = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));

            List<LinkIDWalletReportTransaction> transactions = client.getWalletReport(walletOrganizationId, 
                new LinkIDReportDateFilter(startDate, null));

            Assert.NotNull(transactions);
            foreach(LinkIDWalletReportTransaction transaction in transactions)
            {
                Console.WriteLine(transaction.ToString());
            }
        }

        [Test]
        public void testWalletReportApplicationFilter()
        {
            String walletOrganizationId = "f508212c-9189-4402-ab76-6e26110697b4";
            String applicationName = "test-shop";

            List<LinkIDWalletReportTransaction> transactions = client.getWalletReport(walletOrganizationId,
                new LinkIDReportApplicationFilter(applicationName));

            Assert.NotNull(transactions);
            foreach (LinkIDWalletReportTransaction transaction in transactions)
            {
                Console.WriteLine(transaction.ToString());
            }
        }

        [Test]
        public void testWalletReportWalletFilter()
        {
            String walletOrganizationId = "f508212c-9189-4402-ab76-6e26110697b4";
            String walletId = "ff52177f-8f80-4640-9e86-558f6b1b24c3";
            String userId = "e4269366-ddfb-43dc-838d-01569a8c4c22";

            List<LinkIDWalletReportTransaction> transactions = client.getWalletReport(walletOrganizationId,
                new LinkIDReportWalletFilter(walletId, userId));

            Assert.NotNull(transactions);
            foreach (LinkIDWalletReportTransaction transaction in transactions)
            {
                Console.WriteLine(transaction.ToString());
            }
        }
    }
}
