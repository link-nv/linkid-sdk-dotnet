using System;
using System.ServiceModel;
using System.Net.Security;
using System.ServiceModel.Security;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using CaptureWSNameSpace;


namespace safe_online_sdk_dotnet
{
    public class CaptureClientImpl : CaptureClient
    {
        private CaptureServicePortClient client;

        public CaptureClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/capture20";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new CaptureServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public void capture(String orderReference)
        {
            CaptureRequest request = new CaptureRequest();

            request.orderReference = orderReference;

            // operate
            CaptureResponse response = this.client.capture(request);

            // parse response
            if (null != response.error)
            {
                throw new CaptureException(response.error.errorCode);
            }

            // all good...
        }
    }
}
