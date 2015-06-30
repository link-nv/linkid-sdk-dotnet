using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentMenu
    {
        public String menuResultSuccess { get; set; }
        public String menuResultCanceled{ get; set; }
        public String menuResultPending { get; set; }
        public String menuResultError { get; set; }

        public LinkIDPaymentMenu(String menuResultSuccess, String menuResultCanceled,
            String menuResultPending, String menuResultError)
        {
            this.menuResultSuccess = menuResultSuccess;
            this.menuResultCanceled = menuResultCanceled;
            this.menuResultPending = menuResultPending;
            this.menuResultError = menuResultError;
        }
    }
}
