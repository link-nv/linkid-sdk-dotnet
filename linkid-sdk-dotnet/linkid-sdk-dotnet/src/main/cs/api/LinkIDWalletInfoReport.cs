using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletInfoReport
    {
        public String walletId { get; set; }
        public DateTime created { get; set; }
        public DateTime removed { get; set; }
        public String userId { get; set; }
        public String organizationId { get; set; }
        public String organization { get; set; }
        public double balance { get; set; }

        public LinkIDWalletInfoReport(String walletId, DateTime created, DateTime removed, String userId,
            String organizationId, String organization, double balance)
        {
            this.walletId = walletId;
            this.created = created;
            this.removed = removed;
            this.userId = userId;
            this.organizationId = organizationId;
            this.organization = organization;
            this.balance = balance;
        }
    }
}
