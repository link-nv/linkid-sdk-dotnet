using System;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRPollingConfiguration
    {
        public int pollAttempts { get; private set; }
        public int pollInterval { get; private set; }
        public int paymentPollAttempts { get; private set; }
        public int paymentPollInterval { get; private set; }

        public LinkIDLTQRPollingConfiguration(int pollAttempts, int pollInterval,
            int paymentPollAttempts, int paymentPollInterval)
        {
            this.pollAttempts = pollAttempts;
            this.pollInterval = pollInterval;
            this.paymentPollAttempts = paymentPollAttempts;
            this.paymentPollInterval = paymentPollInterval;
        }
    }
}
