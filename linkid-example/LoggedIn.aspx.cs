using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using safe_online_sdk_dotnet;

namespace linkid_example
{
    public partial class LoggedIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            AuthenticationProtocolContext authContext =
                (AuthenticationProtocolContext)Session[TestUtil.SESSION_AUTH_CONTEXT];
            if (null != authContext)
            {
                this.OutputLabel.Text = TestUtil.dumpAuthenticationContext(authContext);
            }
        }
    }
}
