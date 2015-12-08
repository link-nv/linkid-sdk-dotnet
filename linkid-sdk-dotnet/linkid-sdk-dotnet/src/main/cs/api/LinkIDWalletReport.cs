using System;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletReport
    {
        public long total { get; set; }
        public List<LinkIDWalletReportTransaction> walletTransactions { get; set; }

        public LinkIDWalletReport(long total, List<LinkIDWalletReportTransaction> walletTransactions)
        {
            this.total = total;
            this.walletTransactions = walletTransactions;
        }
    }
}
