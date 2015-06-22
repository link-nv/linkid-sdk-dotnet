using System;
using LTQRWSNameSpace;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRInfo
    {
        public String ltqrReference { get; private set; }
        public String sessionId { get; private set; }
        public DateTime created { get; private set; }
        //
        public byte[] qrCodeImage { get; private set; }
        public String qrCodeURL { get; private set; }
        //
        public String authenticationMessage { get; private set; }
        public String finishedMessage { get; private set; }
        //
        public bool oneTimeUse { get; private set; }
        //
        public DateTime expiryDate { get; private set; }
        public Nullable<long> expiryDuration { get; private set; }
        //
        public LinkIDPaymentContext paymentContext { get; private set; }
        public LinkIDCallback callback { get; private set; }
        public String[] identityProfiles { get; private set; }
        //
        public Nullable<long> sessionExpiryOverride { get; private set; }
        public String theme { get; private set; }
        //
        public String mobileLandingSuccess { get; private set; }
        public String mobileLandingError{ get; private set; }
        public String mobileLandingCancel { get; private set; }

        public LinkIDLTQRInfo(String ltqrReference, String sessionId, DateTime created,
            byte[] qrCodeImage, String qrCodeURL, String authenticationMessage, String finishedMessage,
            bool oneTimeUse, DateTime expiryDate, Nullable<long> expiryDuration,
            LinkIDPaymentContext paymentContext, LinkIDCallback callback, String[] identityProfiles,
            Nullable<long> sessionExpiryOverride, String theme,
            String mobileLandingSuccess, String mobileLandingError, String mobileLandingCancel)
        {
            this.ltqrReference = ltqrReference;
            this.sessionId = sessionId;
            this.created = created;
            this.qrCodeImage = qrCodeImage;
            this.qrCodeURL = qrCodeURL;
            this.authenticationMessage = authenticationMessage;
            this.finishedMessage = finishedMessage;
            this.oneTimeUse = oneTimeUse;
            this.expiryDate = expiryDate;
            this.expiryDuration = expiryDuration;
            this.paymentContext = paymentContext;
            this.callback = callback;
            this.identityProfiles = identityProfiles;
            this.sessionExpiryOverride = sessionExpiryOverride;
            this.theme = theme;
            this.mobileLandingSuccess = mobileLandingSuccess;
            this.mobileLandingError = mobileLandingError;
            this.mobileLandingCancel = mobileLandingCancel;
        }

        public override String ToString()
        {
            String output = "";

            output += "LTQR info\n";
            output += "  * ltqrReference: " + ltqrReference + "\n";
            output += "  * sessionId: " + sessionId + "\n";
            output += "  * created: " + created + "\n";

            return output;
        }

    }
}
