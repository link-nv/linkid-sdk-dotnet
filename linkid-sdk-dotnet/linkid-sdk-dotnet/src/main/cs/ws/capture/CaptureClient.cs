using System;
using CaptureWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public interface CaptureClient
    {
        /// <summary>
        /// Capture a payment
        /// </summary>
        /// <param name="orderReference">order reference of the payment to capture</param>
        void capture(String orderReference);
    }
}
