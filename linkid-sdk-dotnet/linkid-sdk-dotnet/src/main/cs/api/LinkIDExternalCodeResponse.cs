using System;
using AttributeWSNamespace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDExternalCodeResponse
    {
        public static readonly String LOCAL_NAME = "ExternalCodeResponse";

        public static readonly String REFERENCE_KEY = "ExternalCodeResponseDO.reference";
        public static readonly String TYPE_KEY = "ExternalCodeResponseDO.type";

        public String reference { get; set; }
        public LinkIDExternalCodeType type { get; set; }

        public LinkIDExternalCodeResponse(String reference, LinkIDExternalCodeType type)
        {
            this.reference = reference;
            this.type = type;
        }

        public override string ToString()
        {
            String output = "";

            output += "reference=" + reference + "\n";
            output += "type=" + type + "\n";

            return output;
        }

        public static LinkIDExternalCodeResponse fromSaml(ExternalCodeResponseType externalCodeResponseType)
        {
            String reference = null;
            String typeString = null;
            foreach (object item in externalCodeResponseType.Items)
            {
                AttributeType attributeType = (AttributeType)item;
                if (attributeType.Name.Equals(REFERENCE_KEY) && null != attributeType.AttributeValue)
                    reference = (String)attributeType.AttributeValue[0];
                if (attributeType.Name.Equals(TYPE_KEY) && null != attributeType.AttributeValue)
                    typeString = (String)attributeType.AttributeValue[0];
            }

            return new LinkIDExternalCodeResponse(reference, parse(typeString));
        }

        public static LinkIDExternalCodeType parse(String typeString)
        {
            if (typeString.Equals(LinkIDExternalCodeType.PARKO.ToString()))
                return LinkIDExternalCodeType.PARKO;
            if (typeString.Equals(LinkIDExternalCodeType.PARKO_TEST.ToString()))
                return LinkIDExternalCodeType.PARKO_TEST;
            if (typeString.Equals(LinkIDExternalCodeType.LTQR.ToString()))
                return LinkIDExternalCodeType.LTQR;

            throw new RuntimeException("Unsupported external code type! " + typeString);
        }


    }
}
