/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using safe_online_sdk_dotnet_test.test.cs;

namespace safe_online_sdk_dotnet.test.cs
{
    [TestFixture]
    public class TestDataClient
    {
        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();
        }

        [Test]
        public void TestDataWSUsername()
        {
            IdMappingClient idMappingClient =
                new IdMappingClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            DataClient dataClient = new DataClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            dataClient.enableLogging();

            dataWSTest(idMappingClient, dataClient);
        }

        private void dataWSTest(IdMappingClient idMappingClient, DataClient dataClient)
        {
            String attributeName = "profile.familyName";
            String value = "Family Name";
            String value2 = "Family Name 2";


            // first fetch userId
            String userId = idMappingClient.getUserId(TestConstants.loginAttribute, TestConstants.testLogin);
            Console.WriteLine("admin userId: " + userId);

            // Remove old attribute if any
            dataClient.removeAttributes(userId, attributeName); 

            // Create
            LinkIDAttribute linkIDAttribute = new LinkIDAttribute(null, attributeName, value);
            dataClient.createAttribute(userId, linkIDAttribute);

            // Get
            List<LinkIDAttribute> attributes = dataClient.getAttributes(userId, attributeName);
            Assert.AreEqual(1, attributes.Count);
            Assert.AreEqual(value, (String)attributes[0].value);

            // Set
            linkIDAttribute.value = value2;
            dataClient.setAttributeValue(userId, linkIDAttribute);

            // Get
            attributes = dataClient.getAttributes(userId, attributeName);
            Assert.AreEqual(1, attributes.Count);
            Assert.AreEqual(value2, (String)attributes[0].value);

            // Delete
            dataClient.removeAttributes(userId, attributeName);

            // Get
            attributes = dataClient.getAttributes(userId, attributeName);
            Assert.AreEqual(1, attributes.Count);
        }
    }
}
