using System;
using System.ServiceModel;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
			string address = "https://" + location + "/linkid-ws-username/capture";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new CaptureServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public CaptureClientImpl(string location, X509Certificate2 appCertificate, X509Certificate2 linkidCertificate)
        {
            string address = "https://" + location + "/linkid-ws/mandate";
            EndpointAddress remoteAddress = new EndpointAddress(address);

            this.client = new CaptureServicePortClient(new LinkIDBinding(linkidCertificate), remoteAddress);


            this.client.ClientCredentials.ClientCertificate.Certificate = appCertificate;
            this.client.ClientCredentials.ServiceCertificate.DefaultCertificate = linkidCertificate;
            // To override the validation for our self-signed test certificates
            this.client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            this.client.Endpoint.Contract.ProtectionLevel = ProtectionLevel.Sign;

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
