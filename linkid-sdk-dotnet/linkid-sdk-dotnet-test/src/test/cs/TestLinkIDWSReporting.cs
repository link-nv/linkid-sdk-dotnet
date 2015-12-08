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
        LinkIDServiceClient client;

        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();
            client = new LinkIDServiceClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            client.enableLogging();
        }

        [Test]
        public void testGetPaymentReport()
        {
            DateTime startDate = DateTime.Now.Subtract(new TimeSpan(90, 0, 0, 0));
            DateTime endDate = DateTime.Now;
            LinkIDReportDateFilter dateFilter = new LinkIDReportDateFilter(startDate, endDate);

            LinkIDPaymentReport report = client.getPaymentReport(dateFilter, null);

            Assert.NotNull(report);
            foreach (LinkIDPaymentOrder paymentOrder in report.paymentOrders)
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

            LinkIDPaymentReport report = client.getPaymentReportForOrderReferences(orderReferences, null);

            Assert.NotNull(report);
            foreach (LinkIDPaymentOrder paymentOrder in report.paymentOrders)
            {
                Console.WriteLine(paymentOrder.ToString());
            }
        }

        [Test]
        public void testGetParkingSessions()
        {
            DateTime startDate = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
            DateTime endDate = DateTime.Now;

            LinkIDReportDateFilter dateFilter = new LinkIDReportDateFilter(startDate, endDate);

            LinkIDParkingReport report = client.getParkingReport(dateFilter, null);

            Assert.NotNull(report);
            foreach (LinkIDParkingSession session in report.parkingSessions)
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

            LinkIDParkingReport report = client.getParkingReportForBarCodes(barCodes, null);

            Assert.NotNull(report);
            foreach (LinkIDParkingSession session in report.parkingSessions)
            {
                Console.WriteLine(session.ToString());
            }
        }

        [Test]
        public void testWalletReport()
        {
            String walletOrganizationId = "urn:linkid:wallet:leaseplan";
            LinkIDReportDateFilter dateFilter = new LinkIDReportDateFilter(DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0)), null);
            LinkIDReportApplicationFilter applicationFilter = new LinkIDReportApplicationFilter("test-shop");
            LinkIDReportWalletFilter walletFilter = new LinkIDReportWalletFilter("123b1c22-e6c5-4ebc-9255-e59b72db5abf");

            LinkIDWalletReport report = client.getWalletReport(null, walletOrganizationId, applicationFilter,
                walletFilter, dateFilter, null);

            Assert.NotNull(report);
            foreach(LinkIDWalletReportTransaction transaction in report.walletTransactions)
            {
                Console.WriteLine(transaction.ToString());
            }
        }

        [Test]
        public void testWalletInfoReport()
        {
            List<String> walletIds = new List<string>();
            walletIds.Add("123b1c22-e6c5-4ebc-9255-e59b72db5abf");

            List<LinkIDWalletInfoReport> result = client.getWalletInfoReport("en", walletIds);

            Assert.NotNull(result);
            Assert.AreEqual(walletIds.Count, result.Count);
            foreach(LinkIDWalletInfoReport infoReport in result)
            {
                Console.WriteLine(infoReport.ToString());
            }
        }
    }
}
