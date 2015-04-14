using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using safe_online_sdk_dotnet;

namespace linkid_example
{
    public partial class PaymentMobile : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkIDAuthenticationContext linkIDContext = new LinkIDAuthenticationContext();

            linkIDContext.applicationName = TestUtil.APP_NAME;

            // payment context
            linkIDContext.paymentContext = new LinkIDPaymentContext(100, LinkIDCurrency.EUR, "payment context", null, 
                null, 10, PaymentAddBrowser.REDIRECT, false);
        }
    }
}
