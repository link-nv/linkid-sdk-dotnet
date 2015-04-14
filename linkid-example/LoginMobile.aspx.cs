using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using safe_online_sdk_dotnet;

namespace linkid_example
{
    public partial class Mobile : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkIDAuthenticationContext linkIDContext = new LinkIDAuthenticationContext();

            linkIDContext.applicationName = TestUtil.APP_NAME;

            // device context
            linkIDContext.authenticationMessage = ".NET Authn Msg";
            linkIDContext.finishedMessage = ".NET Finished Msg";

            // Identity profiles
            List<String> identityProfiles = new List<String>();
            identityProfiles.Add("linkid_basic");
            identityProfiles.Add("linkid_payment");
            linkIDContext.identityProfiles = identityProfiles;

            // attribute suggestions
            Dictionary<string, List<Object>> attributeSuggestions = new Dictionary<string, List<object>>();
            attributeSuggestions.Add("test.attribute.string", new List<Object> { "test" });
            attributeSuggestions.Add("test.attribute.multi.date", new List<Object> { DateTime.Now });
            attributeSuggestions.Add("test.attribute.boolean", new List<Object> { true });
            attributeSuggestions.Add("test.attribute.integer", new List<Object> { 69 });
            attributeSuggestions.Add("test.attribute.double", new List<Object> { 3.14159 });
            linkIDContext.attributeSuggestions = attributeSuggestions;

            linkIDContext.store(Session);
        }
    }
}
