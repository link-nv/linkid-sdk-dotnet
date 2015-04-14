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
using System.Collections.Generic;
using DataWSNamespace;


namespace safe_online_sdk_dotnet
{
    /// <summary>
    /// Client implementation of the linkID Data Web Service.
    /// 
    /// This is only a proof-of-concept implementation.
    /// </summary>
    public class DataClientImpl : DataClient
    {
        private DataServicePortClient client;


        public DataClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/data";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new DataServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public void setAttributeValue(string userId, LinkIDAttribute attribute)
        {
            List<LinkIDAttribute> attributes = new List<LinkIDAttribute>();
            attributes.Add(attribute);
            setAttributeValue(userId, attributes);
        }

        public void setAttributeValue(String userId, List<LinkIDAttribute> attributes)
        {
            setTargetIdentity(userId);

            ModifyType modify = new ModifyType();
            List<ModifyItemType> modifyItems = new List<ModifyItemType>();
            foreach (LinkIDAttribute attribute in attributes)
            {
                modifyItems.Add(getModifyItem(attribute));
            }
            modify.ModifyItem = modifyItems.ToArray();

            ModifyResponseType response = this.client.Modify(modify);

            validateStatus(response.Status);
        }

        public List<LinkIDAttribute> getAttributes(String userId, String attributeName)
        {
            setTargetIdentity(userId);

            QueryType query = new QueryType();
            QueryItemType queryItem = new QueryItemType();
            queryItem.objectType = DataServiceConstants.ATTRIBUTE_OBJECT_TYPE;
            query.QueryItem = new QueryItemType[] { queryItem };

            SelectType select = new SelectType();
            select.Value = attributeName;
            queryItem.Select = select;

            QueryResponseType response = this.client.Query(query);

            validateStatus(response.Status);

            // parse attributes
            List<LinkIDAttribute> attributes = new List<LinkIDAttribute>();

            foreach (DataType data in response.Data)
            {
                attributes.Add(getAttribute(data.Attribute));
            }
            return attributes;

        }

        public void createAttribute(String userId, LinkIDAttribute attribute)
        {
            List<LinkIDAttribute> attributes = new List<LinkIDAttribute>();
            attributes.Add(attribute);
            createAttribute(userId, attributes);
        }

        public void createAttribute(String userId, List<LinkIDAttribute> attributes)
        {
            setTargetIdentity(userId);

            CreateType create = new CreateType();
            List<CreateItemType> createItems = new List<CreateItemType>();
            foreach (LinkIDAttribute attribute in attributes)
            {
                createItems.Add(getCreateItem(attribute));
            }
            create.CreateItem = createItems.ToArray();

            CreateResponseType response = this.client.Create(create);

            validateStatus(response.Status);
        }

        public void removeAttributes(String userId, String attributeName)
        {
            List<LinkIDAttribute> attributes = new List<LinkIDAttribute>();
            attributes.Add(new LinkIDAttribute(null, attributeName));
            removeAttributes(userId, attributes);
        }

        public void removeAttribute(String userId, String attributeName, String attributeId)
        {
            List<LinkIDAttribute> attributes = new List<LinkIDAttribute>();
            attributes.Add(new LinkIDAttribute(attributeId, attributeName));
            removeAttributes(userId, attributes);
        }

        public void removeAttribute(String userId, LinkIDAttribute attribute)
        {
            List<LinkIDAttribute> attributes = new List<LinkIDAttribute>();
            attributes.Add(attribute);
            removeAttributes(userId, attributes);
        }

        public void removeAttributes(String userId, List<LinkIDAttribute> attributes)
        {

            setTargetIdentity(userId);

            DeleteType delete = new DeleteType();
            List<DeleteItemType> deleteItems = new List<DeleteItemType>();
            foreach (LinkIDAttribute attribute in attributes)
            {
                deleteItems.Add(getDeleteItem(attribute));
            }

            DeleteResponseType response = this.client.Delete(delete);
            validateStatus(response.Status);
        }

        private void setTargetIdentity(string userId)
        {
            // Add TargetIdentity SOAP header
            this.client.Endpoint.Behaviors.RemoveAll<TargetIdentityBehavior>();
            this.client.Endpoint.Behaviors.Add(new TargetIdentityBehavior(userId));
        }

        private static ModifyItemType getModifyItem(LinkIDAttribute attribute)
        {
            ModifyItemType modifyItem = new ModifyItemType();
            modifyItem.objectType = DataServiceConstants.ATTRIBUTE_OBJECT_TYPE;

            SelectType select = new SelectType();
            select.Value = attribute.attributeName;
            modifyItem.Select = select;

            AppDataType newData = new AppDataType();
            newData.Attribute = getAttributeType(attribute);
            modifyItem.NewData = newData;

            return modifyItem;
        }

        private static CreateItemType getCreateItem(LinkIDAttribute attribute)
        {

            CreateItemType createItem = new CreateItemType();
            createItem.objectType = DataServiceConstants.ATTRIBUTE_OBJECT_TYPE;

            AppDataType newData = new AppDataType();
            newData.Attribute = getAttributeType(attribute);
            createItem.NewData = newData;
            return createItem;
        }

        private static DeleteItemType getDeleteItem(LinkIDAttribute attribute)
        {

            DeleteItemType deleteItem = new DeleteItemType();
            deleteItem.objectType = DataServiceConstants.ATTRIBUTE_OBJECT_TYPE;
            SelectType select = new SelectType();
            select.Value = attribute.attributeName;
            if (null != attribute.attributeId)
                setAttributeId(select, attribute.attributeId);

            return deleteItem;
        }

        public static LinkIDAttribute getAttribute(AttributeType attributeType)
        {
            Boolean multivalued = isMultivalued(attributeType);
            String attributeId = getAttributeId(attributeType);
            String attributeName = attributeType.Name;

            LinkIDAttribute attribute = new LinkIDAttribute(attributeId, attributeName);

            if (attributeType.AttributeValue.Length == 0) return attribute;

            if (attributeType.AttributeValue[0] is AttributeType)
            {
                // compound
                AttributeType compoundValue = (AttributeType)attributeType.AttributeValue[0];
                List<LinkIDAttribute> compoundMembers = new List<LinkIDAttribute>();
                foreach (Object memberObject in compoundValue.AttributeValue)
                {
                    AttributeType memberType = (AttributeType)memberObject;
                    LinkIDAttribute member = new LinkIDAttribute(attributeId, memberType.Name, 
                        memberType.AttributeValue[0]);
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

        public static String getAttributeId(AttributeType attribute)
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

        public static bool isMultivalued(AttributeType attribute)
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

        private static AttributeType getAttributeType(LinkIDAttribute attribute)
        {

            AttributeType attributeType = new AttributeType();
            attributeType.Name = attribute.attributeName;
            if (null != attribute.attributeId)
                setAttributeId(attributeType, attribute.attributeId);

            if (null != attribute.value)
            {
                if (attribute.value is LinkIDCompound)
                {
                    // wrap members
                    AttributeType compoundValueAttribute = new AttributeType();
                    attributeType.AttributeValue = new AttributeType[] { compoundValueAttribute };

                    // compounded
                    LinkIDCompound compound = (LinkIDCompound)attribute.value;
                    List<AttributeType> members = new List<AttributeType>();
                    foreach (LinkIDAttribute member in compound.members)
                    {
                        AttributeType memberAttributeType = new AttributeType();
                        memberAttributeType.Name = member.attributeName;
                        memberAttributeType.AttributeValue = new Object[] { member.value };
                        members.Add(memberAttributeType);
                    }
                    compoundValueAttribute.AttributeValue = members.ToArray();
                }
                else
                {
                    // single/multi valued
                    attributeType.AttributeValue = new Object[] { attribute.value };
                }
            }
            return attributeType;
        }

        private static void setAttributeId(AttributeType attribute, string id)
        {
            XmlDocument d = new XmlDocument();
            attribute.AnyAttr = new XmlAttribute[1];
            attribute.AnyAttr[0] = d.CreateAttribute(WebServiceConstants.SAFE_ONLINE_SAML_PREFIX,
                                                     WebServiceConstants.ATTRIBUTE_ID_ATTRIBUTE,
                                                     WebServiceConstants.SAFE_ONLINE_SAML_NAMESPACE);
            attribute.AnyAttr[0].Value = id;
        }

        private static void setAttributeId(SelectType select, string id)
        {
            XmlDocument d = new XmlDocument();
            select.AnyAttr = new XmlAttribute[1];
            select.AnyAttr[0] = d.CreateAttribute(WebServiceConstants.SAFE_ONLINE_SAML_PREFIX,
                                                     WebServiceConstants.ATTRIBUTE_ID_ATTRIBUTE,
                                                     WebServiceConstants.SAFE_ONLINE_SAML_NAMESPACE);
            select.AnyAttr[0].Value = id;
        }

        private void validateStatus(StatusType status)
        {
            Console.WriteLine("status: " + status.code);
            if (!status.code.Equals(DataServiceConstants.TOPLEVEL_STATUS_CODE_OK))
            {
                if (null != status.Status && status.Status.Length > 0)
                {
                    StatusType secondLevelStatus = status.Status[0];
                    Console.WriteLine("second level status: " + secondLevelStatus.code);
                    Console.WriteLine("second level status comment: " + secondLevelStatus.comment);
                    if (secondLevelStatus.code.Equals(DataServiceConstants.SECONDLEVEL_STATUS_CODE_INVALID_DATA))
                    {
                        throw new RuntimeException("attribute value type incorrect");
                    }
                    else if (secondLevelStatus.code.Equals(DataServiceConstants.SECONDLEVEL_STATUS_CODE_DOES_NOT_EXIST))
                    {
                        throw new AttributeNotFoundException();
                    }
                }
                Console.WriteLine("status comment: " + status.comment);
                throw new RuntimeException("could not set the attribute");
            }

        }
    }
}
