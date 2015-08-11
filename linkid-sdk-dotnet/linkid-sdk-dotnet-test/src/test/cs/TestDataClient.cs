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
        public void TestDataWSUsername1()
        {
            DataClient dataClient = new DataClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            dataClient.enableLogging();

            String userId = "e4269366-ddfb-43dc-838d-01569a8c4c22";
            String attributeName = "<your-data-attribute>";

            // Get
            List<LinkIDAttribute> attributes = dataClient.getAttributes(userId, attributeName);
            Assert.AreEqual(0, attributes.Count);
        }

        [Test]
        public void TestDataWSUsername2()
        {
            DataClient dataClient = new DataClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            dataClient.enableLogging();

            String userId = "e4269366-ddfb-43dc-838d-01569a8c4c22";
            String attributeName = "<your-data-attribute>";
            String value = "Value 1";
            String value2 = "Value 2";

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
