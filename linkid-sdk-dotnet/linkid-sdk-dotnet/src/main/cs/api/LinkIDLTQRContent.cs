using System;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRContent
    {
        public String authenticationMessage { get; set; }
        public String finishedMessage { get; set; }
        public LinkIDPaymentContext paymentContext { get; set; }
        public LinkIDCallback callback { get; set; }
        public String identityProfile { get; set; }
        public long sessionExpiryOVerride { get; set; }
        public String theme { get; set; }
        public String mobileLandingSuccess { get; set; }
        public String mobileLandingError { get; set; }
        public String mobileLandingCancel { get; set; }
        public LinkIDLTQRPollingConfiguration pollingConfiguration { get; set; }
        public String ltqrStatusLocation { get; set; }
        public DateTime expiryDate { get; set; }
        public Nullable<long> expiryDuration { get; set; }
        public bool waitForUnblock { get; set; }
        public LinkIDFavoritesConfiguration favoritesConfiguration { get; set; }

        public LinkIDLTQRContent()
        {
        }

        public LinkIDLTQRContent(String authenticationMessage, String finishedMessage,
            LinkIDPaymentContext paymentContext, LinkIDCallback callback, String identityProfile,
            long sessionExpiryOverride, String theme, String mobileLandingSuccess,
            String mobileLandingError, String mobileLandingCancel, LinkIDLTQRPollingConfiguration pollingConfiguration,
            String ltqrStatusLocation, DateTime expiryDate, Nullable<long> expiryDuration, bool waitForUnblock,
            LinkIDFavoritesConfiguration favoritesConfiguration)
        {
            this.authenticationMessage = authenticationMessage;
            this.finishedMessage = finishedMessage;
            this.paymentContext = paymentContext;
            this.callback = callback;
            this.identityProfile = identityProfile;
            this.sessionExpiryOVerride = sessionExpiryOVerride;
            this.theme = theme;
            this.mobileLandingSuccess = mobileLandingSuccess;
            this.mobileLandingError = mobileLandingError;
            this.mobileLandingCancel = mobileLandingCancel;
            this.pollingConfiguration = pollingConfiguration;
            this.ltqrStatusLocation = ltqrStatusLocation;
            this.expiryDate = expiryDate;
            this.expiryDuration = expiryDuration;
            this.waitForUnblock = waitForUnblock;
            this.favoritesConfiguration = favoritesConfiguration;
        }
    }
}
