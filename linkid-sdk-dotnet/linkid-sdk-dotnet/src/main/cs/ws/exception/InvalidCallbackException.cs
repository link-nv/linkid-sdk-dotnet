using System;

namespace safe_online_sdk_dotnet
{
    public class ﻿InvalidCallbackException : System.Exception
	{
		private string messsage;
		
		public ﻿InvalidCallbackException()
		{
		}
		
		public ﻿InvalidCallbackException(string message) {
			this.messsage = message;
		}
		
		/// <summary>
		/// Constructor needed for serialization when exception propagates from a remoting server to the client.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
        protected ﻿InvalidCallbackException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
		}
		
		public string getMessage() {
			return this.messsage;
		}
	}
}
