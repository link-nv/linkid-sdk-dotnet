using System;
using System.Collections.Generic;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLocalization
    {
        public String key { get; set; }
        public ConfigLocalizationKeyType keyType { get; set; }
        public Dictionary<String, String> values { get; set; }

        public LinkIDLocalization(String key, ConfigLocalizationKeyType keyType,
            Dictionary<String, String> values)
        {
            this.key = key;
            this.keyType = keyType;
            this.values = values;
        }
    }
}
