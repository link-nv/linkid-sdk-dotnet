using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using safe_online_sdk_dotnet;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;


namespace linkid_example
{
    public partial class PaymentStateChanged : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String transactionId = Request[RequestConstants.PAYMENT_CHANGED_ID_PARAM];
            if (null == transactionId)
            {
                return;
            }

            // get the latesst state of this payment transaction from linkID
            PaymentClient paymentClient = new PaymentClientImpl(LinkIDLogin.LINKID_HOST);

            PaymentState paymentState = paymentClient.getStatus(transactionId);

            this.OutputLabel.Text = "<h1>Payment Status</h1>";

            this.OutputLabel.Text += "  * Transaction ID : " + transactionId + "<br />";
            this.OutputLabel.Text += "  * Payment State  : " + paymentState + "<br />";

        }
    }
}
