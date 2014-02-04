/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using safe_online_sdk_dotnet_test.test.cs;
using AttributeWSNamespace;

namespace safe_online_sdk_dotnet.test.cs
{
    [TestFixture]
    public class TestHawsClient
    {
        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();
        }

        [Test]
        public void TestPullNotFound()
        {
            HawsClient client = new HawsClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            try
            {
                client.pull("foo");
                Assert.Fail();
            }
            catch (HawsPullException e)
            {
                // should get here...
                Console.WriteLine("Response not found: " + e.errorCode + " - " + e.info);
            }
        }

        [Test]
        public void TestPull()
        {
            HawsClient client = new HawsClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            try
            {
                ResponseType response = client.pull("5b9d8654-25f0-4246-b43a-b19e555d3289");
                Console.WriteLine("Response: " + response.ID);
            }
            catch (HawsPullException e)
            {
                // should get here...
                Console.WriteLine("Response not found: " + e.errorCode + " - " + e.info);
                Assert.Fail();
            }
        }

    }
}
