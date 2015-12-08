using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentReport
    {
        public long total { get; set; }
        public List<LinkIDPaymentOrder> paymentOrders { get; set; }

        public LinkIDPaymentReport(long total, List<LinkIDPaymentOrder> paymentOrders)
        {
            this.total = total;
            this.paymentOrders = paymentOrders;
        }
    }
}
