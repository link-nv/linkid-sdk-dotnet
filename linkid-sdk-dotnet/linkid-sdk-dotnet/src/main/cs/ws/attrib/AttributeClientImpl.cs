/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Net;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using AttributeWSNamespace;

namespace safe_online_sdk_dotnet
{
    /// <summary>
    /// Client implementation for the linkID Attribute Web Service.
    /// </summary>
    public class AttributeClientImpl : AttributeClient
    {
        private SAMLAttributePortClient client;

        public AttributeClientImpl(string location, string username, string password)
		{
            string address = "https://" + location + "/linkid-ws-username/attrib";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new SAMLAttributePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public List<LinkIDAttribute> getAttributes(String userId, String attributeName)
        {
            Dictionary<String, List<LinkIDAttribute>> attributeMap = new Dictionary<string, List<LinkIDAttribute>>();
            AttributeQueryType request = getAttributeQuery(userId, new string[] { attributeName });
            ResponseType response = getResponse(request);
            checkStatus(response);
            getAttributeValues(response, attributeMap);
            return attributeMap[attributeName];
        }

        public void getAttributes(String userId, Dictionary<String, List<LinkIDAttribute>> attributeMap)
        {
            String[] attributeNames = new String[attributeMap.Keys.Count];
            attributeMap.Keys.CopyTo(attributeNames, 0);
            AttributeQueryType request = getAttributeQuery(userId, attributeNames);
            ResponseType response = getResponse(request);
            checkStatus(response);
            getAttributeValues(response, attributeMap);
        }

        public Dictionary<String, List<LinkIDAttribute>> getAttributes(String userId)
        {
            Dictionary<String, List<LinkIDAttribute>> attributeMap = new Dictionary<string, List<LinkIDAttribute>>();
            AttributeQueryType request = getAttributeQuery(userId, new String[] { });
            ResponseType response = getResponse(request);
            checkStatus(response);
            getAttributeValues(response, attributeMap);
            return attributeMap;
        }

        private AttributeQueryType getAttributeQuery(string userId, string[] attributeNames)
        {
            AttributeQueryType attributeQuery = new AttributeQueryType();
            SubjectType subject = new SubjectType();
            NameIDType subjectName = new NameIDType();
            subjectName.Value = userId;
            subject.Items = new Object[] { subjectName };
            attributeQuery.Subject = subject;

            if (null != attributeNames)
            {
                List<AttributeType> attributes = new List<AttributeType>();
                foreach (string attributeName in attributeNames)
                {
                    AttributeType attribute = new AttributeType();
                    attribute.Name = attributeName;
                    attributes.Add(attribute);
                }
                attributeQuery.Attribute = attributes.ToArray();
            }
            return attributeQuery;
        }

        private ResponseType getResponse(AttributeQueryType request)
        {
            return this.client.AttributeQuery(request);
        }

        private void checkStatus(ResponseType response)
        {
            StatusType status = response.Status;
            StatusCodeType statusCode = status.StatusCode;
            if (!statusCode.Value.Equals(Saml2Constants.SAML2_STATUS_SUCCESS))
            {
                Console.WriteLine("status code: " + statusCode.Value);
                Console.WriteLine("status message: " + status.StatusMessage);
                StatusCodeType secondLevelStatusCode = statusCode.StatusCode;
                if (null != secondLevelStatusCode)
                {
                    if (secondLevelStatusCode.Value.Equals(Saml2Constants.SAML2_STATUS_INVALID_ATTRIBUTE_NAME_OR_VALUE))
                    {
                        throw new AttributeNotFoundException();
                    }
                    else if (secondLevelStatusCode.Value.Equals(Saml2Constants.SAML2_STATUS_REQUEST_DENIED))
                    {
                        throw new RequestDeniedException();
                    }
                    else if (secondLevelStatusCode.Value.Equals(Saml2Constants.SAML2_STATUS_ATTRIBUTE_UNAVAILABLE))
                    {
                        throw new AttributeUnavailableException();
                    }
                    Console.WriteLine("second level status code: " + secondLevelStatusCode.Value);
                }

                throw new RuntimeException("error: " + statusCode.Value);
            }
        }

        private static void getAttributeValues(ResponseType response, Dictionary<String, List<LinkIDAttribute>> attributeMap)
        {

            if (null == response.Items || response.Items.Length == 0)
            {
                throw new RuntimeException("No assertions in response");
            }
            AssertionType assertion = (AssertionType)response.Items[0];
            if (null == assertion.Items || assertion.Items.Length == 0)
            {
                throw new RuntimeException("No statements in response assertion");
            }
            AttributeStatementType attributeStatement = (AttributeStatementType)assertion.Items[0];

            foreach (Object attributeObject in attributeStatement.Items)
            {
                AttributeType attributeType = (AttributeType)attributeObject;
                LinkIDAttribute attribute = Saml2AuthUtil.getAttribute(attributeType);

                List<LinkIDAttribute> attributes;
                if (!attributeMap.ContainsKey(attribute.attributeName))
                {
                    attributes = new List<LinkIDAttribute>();
                }
                else
                {
                    attributes = attributeMap[attribute.attributeName];
                }
                attributes.Add(attribute);
                attributeMap.Remove(attribute.attributeName);
                attributeMap.Add(attribute.attributeName, attributes);
            }
        }
    }
}
