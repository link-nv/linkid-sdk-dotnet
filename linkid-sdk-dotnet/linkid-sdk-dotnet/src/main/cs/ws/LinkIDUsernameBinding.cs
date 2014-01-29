/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace safe_online_sdk_dotnet
{
    /// <summary>
    /// LinkIDBinding.
    /// 
    /// This custom binding provides both transport security as adds the WS-Security username token in the SOAP header.
    /// </summary>
    public class LinkIDUsernameBinding : Binding
    {
        private BindingElementCollection bindingElements;

        public LinkIDUsernameBinding(String username, String password)
        {
			
			HttpsTransportBindingElement httpsTransport = new HttpsTransportBindingElement();
            httpsTransport.MaxReceivedMessageSize = 2147483647;
			TextMessageEncodingBindingElement encoding = new TextMessageEncodingBindingElement();
			encoding.MessageVersion = MessageVersion.Soap11;

            /*
            AsymmetricSecurityBindingElement securityBinding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); 
			securityBinding.AllowSerializedSigningTokenOnReply = true;
			securityBinding.SecurityHeaderLayout = SecurityHeaderLayout.Lax;
            securityBinding.EnableUnsecuredResponse = true;
            */
			this.bindingElements = new BindingElementCollection();
			//this.bindingElements.Add(securityBinding);
			this.bindingElements.Add(encoding);
			this.bindingElements.Add(httpsTransport);
		}
		
		public override BindingElementCollection CreateBindingElements() {
			return this.bindingElements.Clone();
		}
		
		public override string Scheme {
			get { return "https"; }
		}
		
		public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingParameterCollection parameters)
		{
			Console.WriteLine("build channel factory");
			return null;
		}
		
		public override bool CanBuildChannelFactory<TChannel>(BindingParameterCollection parameters)
		{
			Console.WriteLine("can build channel factory");
			return true;
		}

    }
}
