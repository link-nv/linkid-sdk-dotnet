using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using safe_online_sdk_dotnet;

namespace linkid_example
{
    public partial class PaymentMobileHaws : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkIDAuthenticationContext linkIDContext = new LinkIDAuthenticationContext();

            linkIDContext.applicationName = TestUtil.APP_NAME;

//            linkIDContext.paymentContext = new LinkIDPaymentContext(
//                new LinkIDPaymentAmount(100, LinkIDCurrency.EUR, null), "payment context", null,
//                null, 10, PaymentAddBrowser.REDIRECT, false);
            linkIDContext.paymentContext = new LinkIDPaymentContext(
                new LinkIDPaymentAmount(100, null, "urn:linkid:wallet:coin:coffee"), "payment context", null,
                null, 10, PaymentAddBrowser.REDIRECT);

            linkIDContext.store(Session);
        }
    }
}
