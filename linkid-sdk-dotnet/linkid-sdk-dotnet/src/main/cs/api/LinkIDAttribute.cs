using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDAttribute
    {
        public String attributeId { get; set; }
        public String attributeName { get; set; }

        public Boolean unavailable { get; set; }

        public Object value { get; set; }

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

        public override string ToString()
        {
            String output = "";

            output += "     Attribute: " + attributeName + "(ID=" + attributeId + ")\n";
            if (value is LinkIDCompound)
            {
                LinkIDCompound compound = (LinkIDCompound)value;
                output += "       Compound:\n";
                output += compound.ToString();
            }
            else
            {
                output += "       value=" + value + "\n";
            }

            return output;
        }
    }
}
