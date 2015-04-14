using System;
using System.Text;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDCallback
    {
        public static readonly String ﻿KEY_LOCATION = "Callback.location";
        public static readonly String ﻿KEY_APP_SESSION_ID = "Callback.appSessionId";
        public static readonly String ﻿KEY_IN_APP = "Callback.inApp";

        ﻿// location the linkID client will load when finished
        public String location { get; set; }

        ﻿// optional sessionId a SP can provide to load in session state before linkID was started
        public String appSessionId { get; set; }

        ﻿// display the location inApp (webView) or via the client's browser
        public Boolean inApp { get; set; }

        public LinkIDCallback(String location, String appSessionId, bool inApp)
        {
            if (null == location)
            {
                throw new InvalidCallbackException("Invalid callback, location must not be null");
            }

            this.location = location;
            this.appSessionId = appSessionId;
            this.inApp = inApp;
        }

        public Dictionary<string, string> toDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(KEY_LOCATION, location.ToString());

            if (null != appSessionId)
            {
                dictionary.Add(KEY_APP_SESSION_ID, appSessionId.ToString());
            }

            dictionary.Add(KEY_IN_APP, inApp.ToString());

            return dictionary;
        }
    }
}
