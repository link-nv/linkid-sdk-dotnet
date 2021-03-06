/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Net;
using System.Net.Security;

using safe_online_sdk_dotnet;

namespace safe_online_sdk_dotnet_test.test.cs
{
	public sealed class TestConstants
	{
        public static readonly string workDir = "C:\\cygwin\\home\\devel\\keystores";

        // constants for WS-Security PasswordToken profile
//        public static readonly string testWsUsername = "example-mobile";
//        public static readonly string testWsPassword = "6E6C1CB7-965C-48A0-B2B0-6B65674BE19F";
        public static readonly string testWsUsername = "test-shop";
        public static readonly string testWsPassword = "5E017416-23B2-47E1-A9E0-43EE3C75A1B0";

//        public static readonly string linkidHost = "demo.linkid.be";
        public static readonly string linkidHost = "192.168.5.14";
//        public static readonly string wsLocation = linkidHost;
        public static readonly string wsLocation = linkidHost + ":8443";
        public static readonly string linkidAuthEntry = "https://" + wsLocation + "/linkid-auth/entry";
        public static readonly string linkidLogoutEntry = "https://" + wsLocation + "/linkid-auth/logoutentry";

        public static readonly string localhost = "192.168.5.20";

		public static readonly string testMultiStringAttribute = "urn:test:multi:string";
		public static readonly string testMultiDateAttribute = "urn:test:multi:date";
		public static readonly string testCompoundAttribute = "urn:test:compound";
		public static readonly string testSingleStringAttribute = "urn:test:single:string";
		public static readonly string testSingleDateAttribute = "urn:test:single:date";
		
		public static readonly string linkidTopicRemoveUser = "urn:net:lin-k:safe-online:topic:user:remove";
		public static readonly string linkidTopicUnsubscribeUser = "urn:net:lin-k:safe-online:topic:user:unsubscribe";

        public static void initForDevelopment()
        {
            // Allow any SSL certficate
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(WCFUtil.AnyCertificateValidationCallback);
        }

		private TestConstants()
		{
		}
	}
}
