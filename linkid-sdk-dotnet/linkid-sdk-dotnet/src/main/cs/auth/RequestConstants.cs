/*
 * Gemaakt met SharpDevelop.
 * Gebruiker: Tester
 * Datum: 20-10-2009
 * Tijd: 13:23
 * 
 * Dit sjabloon wijzigen: Extra | Opties |Coderen | Standaard kop bewerken.
 */
using System;

namespace safe_online_sdk_dotnet
{
	/// <summary>
	/// Contains some request parameter names
	/// </summary>
	public sealed class RequestConstants
	{
        // Payment state changed page order reference param
        public static readonly String PAYMENT_CHANGED_ORDER_REF_PARAM = "orderRef";

        // Long term QR notification page parameters
        public static readonly String ﻿﻿LTQR_REF_PARAM = "ltqrRef";
        public static readonly String LTQR_PAYMENT_ORDER_REF_PARAM = "paymentOrderRef";
        public static readonly String ﻿﻿LTQR_CLIENT_SESSION_ID_PARAM = "clientSessionId";

		private RequestConstants()
		{
		}
	}
}
