/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using AttributeWSNamespace;
using System.Security.Cryptography.X509Certificates;

namespace safe_online_sdk_dotnet
{
    /// <summary>
    /// Saml2AuthUtil
    /// 
    /// This utility class generates a SAML v2.0 AuthenticationRequest with HTTP Browser Post binding
    /// and validates the returned SAML v2.0 Response.
    /// </summary>
    public class Saml2AuthUtil
    {
        public Saml2AuthUtil()
        {
        }

        /// <summary>
        /// Generates a SAML v2.0 Authentication Request with HTTP Browser Post Binding. 
        /// The return string containing the request is NOT Base64 encoded.
        /// </summary>
        /// <param name="linkIDContext">the linkID authentication/payment configuration</param>
        /// <returns>SAML request</returns>
        public static AuthnRequestType generateAuthnRequest(LinkIDAuthenticationContext linkIDContext)
        {
            AuthnRequestType authnRequest = new AuthnRequestType();
            authnRequest.ForceAuthn = true;
            authnRequest.ID = Guid.NewGuid().ToString();
            authnRequest.Version = "2.0";
            authnRequest.IssueInstant = DateTime.UtcNow;

            NameIDType issuer = new NameIDType();
            issuer.Value = linkIDContext.applicationName;
            authnRequest.Issuer = issuer;

            NameIDPolicyType nameIdPolicy = new NameIDPolicyType();
            nameIdPolicy.AllowCreate = true;
            nameIdPolicy.AllowCreateSpecified = true;
            authnRequest.NameIDPolicy = nameIdPolicy;

            Dictionary<string, string> deviceContextMap = linkIDContext.getDeviceContextMap();
            DeviceContextType deviceContext = null;
            if (null != deviceContextMap && deviceContextMap.Count > 0)
            {
                deviceContext = new DeviceContextType();
                List<AttributeType> attributes = new List<AttributeType>();
                foreach (string deviceContextKey in deviceContextMap.Keys)
                {
                    string deviceContextValue = deviceContextMap[deviceContextKey];
                    AttributeType attribute = new AttributeType();
                    attribute.Name = deviceContextKey;
                    attribute.AttributeValue = new object[] { deviceContextValue };
                    attributes.Add(attribute);
                    deviceContext.Items = attributes.ToArray();
                }
            }
            SubjectAttributesType subjectAttributes = null;
            if (null != linkIDContext.attributeSuggestions && linkIDContext.attributeSuggestions.Count > 0)
            {
                subjectAttributes = new SubjectAttributesType();
                List<AttributeType> attributes = new List<AttributeType>();
                foreach (string attributeName in linkIDContext.attributeSuggestions.Keys)
                {
                    List<object> values = linkIDContext.attributeSuggestions[attributeName];

                    AttributeType attribute = new AttributeType();
                    attribute.Name = attributeName;
                    attribute.AttributeValue = values.ToArray();
                    attributes.Add(attribute);
                    subjectAttributes.Items = attributes.ToArray();
                }
            }

            PaymentContextType paymentContextType = null;
            if (null != linkIDContext.paymentContext)
            {
                Dictionary<String, String> paymentContextDict = linkIDContext.paymentContext.toDictionary();
                paymentContextType = new PaymentContextType();
                List<AttributeType> attributes = new List<AttributeType>();
                foreach (string paymentContextKey in paymentContextDict.Keys)
                {
                    string value = paymentContextDict[paymentContextKey];
                    AttributeType attribute = new AttributeType();
                    attribute.Name = paymentContextKey;
                    attribute.AttributeValue = new object[] { value };
                    attributes.Add(attribute);
                    paymentContextType.Items = attributes.ToArray();
                }
            }

            CallbackType callbackType = null;
            if (null != linkIDContext.callback)
            {
                Dictionary<String, String> callbackDict = linkIDContext.callback.toDictionary();
                callbackType = new CallbackType();
                List<AttributeType> attributes = new List<AttributeType>();
                foreach (string callbackKey in callbackDict.Keys)
                {
                    string value = callbackDict[callbackKey];
                    AttributeType attribute = new AttributeType();
                    attribute.Name = callbackKey;
                    attribute.AttributeValue = new object[] { value };
                    attributes.Add(attribute);
                    callbackType.Items = attributes.ToArray();
                }
            }


            if (null != deviceContext || null != subjectAttributes || null != paymentContextType || null != callbackType)
            {
                ExtensionsType extensions = new ExtensionsType();
                List<XmlElement> extensionsList = new List<XmlElement>();
                if (null != subjectAttributes)
                    extensionsList.Add(toXmlElement(subjectAttributes));
                if (null != deviceContext)
                    extensionsList.Add(toXmlElement(deviceContext));
                if (null != paymentContextType)
                    extensionsList.Add(toXmlElement(paymentContextType));
                if (null != callbackType)
                    extensionsList.Add(toXmlElement(callbackType));
                extensions.Any = extensionsList.ToArray();
                authnRequest.Extensions = extensions;
            }

            return authnRequest;
        }

        public static XmlDocument toXmlDocument(AuthnRequestType authnRequest)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("samlp", Saml2Constants.SAML2_PROTOCOL_NAMESPACE);
            ns.Add("saml", Saml2Constants.SAML2_ASSERTION_NAMESPACE);

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "AuthnRequest";
            xRoot.Namespace = Saml2Constants.SAML2_PROTOCOL_NAMESPACE;
            XmlSerializer serializer = new XmlSerializer(typeof(AuthnRequestType), xRoot);
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            serializer.Serialize(xmlTextWriter, authnRequest, ns);

            XmlDocument document = new XmlDocument();
            memoryStream.Seek(0, SeekOrigin.Begin);
            document.Load(memoryStream);
            xmlTextWriter.Close();

            return document;
        }

        private static XmlElement toXmlElement(CallbackType callback)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("samlp", Saml2Constants.SAML2_PROTOCOL_NAMESPACE);
            ns.Add("saml", Saml2Constants.SAML2_ASSERTION_NAMESPACE);
            ns.Add("xs", "http://www.w3.org/2001/XMLSchema");

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Callback";
            xRoot.Namespace = Saml2Constants.SAML2_ASSERTION_NAMESPACE;
            XmlSerializer serializer = new XmlSerializer(typeof(CallbackType), xRoot);
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            serializer.Serialize(xmlTextWriter, callback, ns);

            XmlDocument document = new XmlDocument();
            memoryStream.Seek(0, SeekOrigin.Begin);
            document.Load(memoryStream);

            foreach (XmlNode node in document.ChildNodes)
            {
                if (node is XmlElement)
                    return (XmlElement)node;
            }

            return null;
        }

        private static XmlElement toXmlElement(PaymentContextType paymentContext)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("samlp", Saml2Constants.SAML2_PROTOCOL_NAMESPACE);
            ns.Add("saml", Saml2Constants.SAML2_ASSERTION_NAMESPACE);
            ns.Add("xs", "http://www.w3.org/2001/XMLSchema");

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "PaymentContext";
            xRoot.Namespace = Saml2Constants.SAML2_ASSERTION_NAMESPACE;
            XmlSerializer serializer = new XmlSerializer(typeof(PaymentContextType), xRoot);
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            serializer.Serialize(xmlTextWriter, paymentContext, ns);

            XmlDocument document = new XmlDocument();
            memoryStream.Seek(0, SeekOrigin.Begin);
            document.Load(memoryStream);

            foreach (XmlNode node in document.ChildNodes)
            {
                if (node is XmlElement)
                    return (XmlElement)node;
            }

            return null;
        }

        private static XmlElement toXmlElement(DeviceContextType deviceContext)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("samlp", Saml2Constants.SAML2_PROTOCOL_NAMESPACE);
            ns.Add("saml", Saml2Constants.SAML2_ASSERTION_NAMESPACE);
            ns.Add("xs", "http://www.w3.org/2001/XMLSchema");

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "DeviceContext";
            xRoot.Namespace = Saml2Constants.SAML2_ASSERTION_NAMESPACE;
            XmlSerializer serializer = new XmlSerializer(typeof(DeviceContextType), xRoot);
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            serializer.Serialize(xmlTextWriter, deviceContext, ns);

            XmlDocument document = new XmlDocument();
            memoryStream.Seek(0, SeekOrigin.Begin);
            document.Load(memoryStream);

            foreach (XmlNode node in document.ChildNodes)
            {
                if (node is XmlElement)
                    return (XmlElement)node;
            }

            return null;
        }

        private static XmlElement toXmlElement(SubjectAttributesType subjectAttributes)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("samlp", Saml2Constants.SAML2_PROTOCOL_NAMESPACE);
            ns.Add("saml", Saml2Constants.SAML2_ASSERTION_NAMESPACE);
            ns.Add("xs", "http://www.w3.org/2001/XMLSchema");

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "SubjectAttributes";
            xRoot.Namespace = Saml2Constants.SAML2_ASSERTION_NAMESPACE;
            XmlSerializer serializer = new XmlSerializer(typeof(SubjectAttributesType), xRoot);
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            serializer.Serialize(xmlTextWriter, subjectAttributes, ns);

            XmlDocument document = new XmlDocument();
            memoryStream.Seek(0, SeekOrigin.Begin);
            document.Load(memoryStream);

            foreach (XmlNode node in document.ChildNodes)
            {
                if (node is XmlElement)
                    return (XmlElement)node;
            }

            return null;
        }

        public static LinkIDAuthnResponse parse(ResponseType response)
        {
            String userId = null;
            Dictionary<String, List<LinkIDAttribute>> attributes = new Dictionary<string, List<LinkIDAttribute>>();
            foreach (object item in response.Items)
            {
                AssertionType assertion = (AssertionType)item;
                SubjectType subject = assertion.Subject;
                NameIDType nameId = (NameIDType)subject.Items[0];
                userId = nameId.Value;

                foreach (StatementAbstractType statement in assertion.Items)
                {
                    if (statement is AttributeStatementType)
                    {
                        AttributeStatementType attributeStatement = (AttributeStatementType)statement;
                        attributes = getAttributes(attributeStatement);
                    }
                }

            }

            LinkIDPaymentResponse paymentResponse = findPaymentResponse(response);
            LinkIDExternalCodeResponse externalCodeResponse = findExternalCodeResponse(response);

            return new LinkIDAuthnResponse(userId, attributes, paymentResponse, externalCodeResponse);
        }

        private static LinkIDPaymentResponse findPaymentResponse(ResponseType response)
        {
            if (null == response.Extensions || null == response.Extensions.Any)
                return null;
            if (0 == response.Extensions.Any.Length)
                return null;

            XmlElement xmlElement = response.Extensions.Any[0];
            if (xmlElement.LocalName.Equals(LinkIDPaymentResponse.LOCAL_NAME))
            {
                PaymentResponseType paymentResponseType = deserializePaymentResponse(xmlElement);
                return LinkIDPaymentResponse.fromSaml(paymentResponseType);
            }

            return null;
        }

        private static LinkIDExternalCodeResponse findExternalCodeResponse(ResponseType response)
        {
            if (null == response.Extensions || null == response.Extensions.Any)
                return null;
            if (0 == response.Extensions.Any.Length)
                return null;

            XmlElement xmlElement = response.Extensions.Any[0];
            if (xmlElement.LocalName.Equals(LinkIDExternalCodeResponse.LOCAL_NAME))
            {
                ExternalCodeResponseType externalCodeResponseType = deserializeExternalCodeResponse(xmlElement);
                return LinkIDExternalCodeResponse.fromSaml(externalCodeResponseType);
            }

            return null;
        }

        private static Dictionary<string, List<LinkIDAttribute>> getAttributes(AttributeStatementType attributeStatement)
        {
            Dictionary<string, List<LinkIDAttribute>> attributeMap = new Dictionary<string, List<LinkIDAttribute>>();

            foreach (object item in attributeStatement.Items)
            {
                AttributeType attributeType = (AttributeType)item;
                LinkIDAttribute attribute = getAttribute(attributeType);

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
            return attributeMap;
        }

        public static LinkIDAttribute getAttribute(AttributeType attributeType)
        {
            Boolean multivalued = isMultivalued(attributeType);
            String attributeId = getAttributeId(attributeType);
            String attributeName = attributeType.Name;

            LinkIDAttribute attribute = new LinkIDAttribute(attributeId, attributeName);

            if (attributeType.AttributeValue.Length == 0) return attribute;

            if (attributeType.AttributeValue[0] is XmlNode[])
            {
                // compound
                List<LinkIDAttribute> compoundMembers = new List<LinkIDAttribute>();
                foreach (XmlNode attributeNode in (XmlNode[])attributeType.AttributeValue[0])
                {
                    if (isAttributeElement(attributeNode))
                    {
                        LinkIDAttribute member = new LinkIDAttribute(attributeId,
                            getXmlAttribute(attributeNode, WebServiceConstants.ATTRIBUTE_NAME_ATTRIBUTE));
                        member.value = attributeNode.InnerText;
                        compoundMembers.Add(member);
                    }
                }
                attribute.value = new LinkIDCompound(compoundMembers);
            }
            else if (attributeType.AttributeValue[0] is AttributeType)
            {
                // also compound but through WS
                AttributeType compoundValue = (AttributeType)attributeType.AttributeValue[0];
                List<LinkIDAttribute> compoundMembers = new List<LinkIDAttribute>();
                foreach (Object memberObject in compoundValue.AttributeValue)
                {
                    AttributeType memberType = (AttributeType)memberObject;
                    LinkIDAttribute member = new LinkIDAttribute(attributeId, memberType.Name, memberType.AttributeValue[0]);
                    compoundMembers.Add(member);
                }
                attribute.value = new LinkIDCompound(compoundMembers);
            }
            else
            {
                // single/multi valued
                attribute.value = attributeType.AttributeValue[0];
            }
            return attribute;
        }

        private static Boolean isAttributeElement(XmlNode node)
        {
            return node.NamespaceURI.Equals("urn:oasis:names:tc:SAML:2.0:assertion") && node.LocalName.Equals("Attribute");
        }

        private static String getXmlAttribute(XmlNode attributeNode, String localName)
        {
            if (null == attributeNode.Attributes) return null;

            foreach (XmlAttribute attribute in attributeNode.Attributes)
            {
                if (attribute.LocalName.Equals(localName))
                {
                    return attribute.Value;
                }
            }
            return null;
        }

        private static String getAttributeId(AttributeType attribute)
        {
            String attributeId = null;
            XmlAttribute[] xmlAttributes = attribute.AnyAttr;
            if (null != xmlAttributes)
            {
                foreach (XmlAttribute xmlAttribute in xmlAttributes)
                {
                    if (xmlAttribute.LocalName.Equals(WebServiceConstants.ATTRIBUTE_ID_ATTRIBUTE))
                    {
                        attributeId = xmlAttribute.Value;
                    }
                }
            }
            return attributeId;
        }

        private static bool isMultivalued(AttributeType attribute)
        {
            bool multivalued = false;
            XmlAttribute[] xmlAttributes = attribute.AnyAttr;
            if (null != xmlAttributes)
            {
                foreach (XmlAttribute xmlAttribute in xmlAttributes)
                {
                    if (xmlAttribute.LocalName.Equals(WebServiceConstants.MUTLIVALUED_ATTRIBUTE))
                    {
                        multivalued = Boolean.Parse(xmlAttribute.Value);
                    }
                }
            }
            return multivalued;
        }

        private static PaymentResponseType deserializePaymentResponse(XmlElement xmlElement)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "PaymentResponse";
            xRoot.Namespace = Saml2Constants.SAML2_ASSERTION_NAMESPACE;

            XmlSerializer serializer = new XmlSerializer(typeof(PaymentResponseType), xRoot);
            return (PaymentResponseType)serializer.Deserialize(new XmlTextReader(new StringReader(xmlElement.OuterXml)));
        }

        private static ExternalCodeResponseType deserializeExternalCodeResponse(XmlElement xmlElement)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ExternalCodeResponse";
            xRoot.Namespace = Saml2Constants.SAML2_ASSERTION_NAMESPACE;

            XmlSerializer serializer = new XmlSerializer(typeof(ExternalCodeResponseType), xRoot);
            return (ExternalCodeResponseType)serializer.Deserialize(new XmlTextReader(new StringReader(xmlElement.OuterXml)));
        }
    }
}
