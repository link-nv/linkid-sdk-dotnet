/*
 * Created by SharpDevelop.
 * User: devel
 * Date: 23/12/2008
 * Time: 14:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace safe_online_sdk_dotnet
{
	/// <summary>
	/// Description of AttributeNotFoundException.
	/// </summary>
    public class ﻿InvalidPaymentContextException : System.Exception
	{
		private string messsage;
		
		public ﻿InvalidPaymentContextException()
		{
		}
		
		public ﻿InvalidPaymentContextException(string message) {
			this.messsage = message;
		}
		
		/// <summary>
		/// Constructor needed for serialization when exception propagates from a remoting server to the client.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ﻿InvalidPaymentContextException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) {
		}
		
		public string getMessage() {
			return this.messsage;
		}
	}
}
