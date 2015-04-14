using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDAttribute
    {
        private String attributeId;
        private String attributeName;

        private Boolean unavailable;

        protected Object value;

        public LinkIDAttribute(String attributeId, String attributeName, Object value)
        {
            this.attributeId = attributeId;
            this.attributeName = attributeName;
            this.value = value;
        }

        public LinkIDAttribute(String attributeId, String attributeName)
        {
            this.attributeId = attributeId;
            this.attributeName = attributeName;
        }

        public String getAttributeId()
        {
            return attributeId;
        }

        public void setAttributeId(String attributeId)
        {
            this.attributeId = attributeId;
        }

        public String getAttributeName()
        {
            return this.attributeName;
        }

        public void setAttributeName(String attributeName)
        {
            this.attributeName = attributeName;
        }

        public Object getValue()
        {
            return value;
        }

        public void setValue(Object value)
        {
            this.value = value;
        }

        public Boolean isUnavailable()
        {
            return this.unavailable;
        }

        public void setUnavailable(Boolean unavailable)
        {
            this.unavailable = unavailable;
        }

    }
}
