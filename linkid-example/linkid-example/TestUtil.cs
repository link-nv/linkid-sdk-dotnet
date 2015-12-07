using System;
using System.Web;
using System.Collections.Generic;
using safe_online_sdk_dotnet;

namespace linkid_example
{
    public class TestUtil
    {
        // linkID host to be used
        public static string LINKID_HOST = "192.168.5.14:8443";
        //public static string LINKID_HOST = "demo.linkid.be";

        // application details
        public static string APP_NAME = "example-mobile";
        //public static string APP_NAME = "test-shop";

        // set the language to be used in the linkID iFrame
        public static string language = "en";

        // username,password case HAWS binding / WS-Security Password token is used
         public static string wsUsername = "example-mobile";
        //public static string wsUsername = "test-shop";
        public static string wsPassword = "6E6C1CB7-965C-48A0-B2B0-6B65674BE19F";
        //public static string wsPassword = "5E017416-23B2-47E1-A9E0-43EE3C75A1B0";

        public static LinkIDServiceClient getClient()
        {
            return new LinkIDServiceClientImpl(LINKID_HOST, wsUsername, wsPassword);
        }
    }
}
