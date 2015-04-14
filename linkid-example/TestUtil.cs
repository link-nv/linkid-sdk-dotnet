using System;
using System.Web;
using System.Collections.Generic;
using safe_online_sdk_dotnet;

namespace linkid_example
{
    public class TestUtil
    {
        // linkID host to be used
        //public static string LINKID_HOST = "192.168.0.198:8443";
        public static string LINKID_HOST = "demo.linkid.be";

        // application details
        public static string APP_NAME = "example-mobile";

        // set the language to be used in the linkID iFrame
        public static string language = "en";

        // username,password case HAWS binding / WS-Security Password token is used
        public static string wsUsername = "example-mobile";
        public static string wsPassword = "6E6C1CB7-965C-48A0-B2B0-6B65674BE19F";

        /*
         * linkID authentication context session attribute
         * 
         * After a successfull authentication with linkID this will hold the returned   
         * AuthenticationProtocolContext object which contains the linkID user ID,
         * used authentication device(s) and optionally the returned linkID attributes
         * for the application.
         */
        public static string SESSION_AUTH_CONTEXT = "linkID.authContext";
    }
}
