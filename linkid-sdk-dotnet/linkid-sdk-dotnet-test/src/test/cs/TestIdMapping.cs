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

namespace safe_online_sdk_dotnet.test.cs
{
	[TestFixture]
	public class TestIdMapping
	{
        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();
        }

        [Test]
        public void TestGetUserIdUsername()
        {
            IdMappingClient idMappingClient =
                new IdMappingClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            String userId = idMappingClient.getUserId(TestConstants.loginAttribute, TestConstants.testLogin);
            Console.WriteLine("admin userId: " + userId);
        }
    }
}
