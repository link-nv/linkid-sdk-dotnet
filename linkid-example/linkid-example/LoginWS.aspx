<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginWS.aspx.cs" Inherits="linkid_example.LoginWS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>linkID Login Example</title>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        function pollLinkID() {
        
            (function poll() {

                console.log("poll...");

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "LoginWS.aspx/pollLinkID",
                    data: null,
                    dataType: "json",
                    success: function(result) {
                        var jsonData = $.parseJSON(result.d);
                        console.log("Info: " + jsonData.info);
                        if (!jsonData.finished) {
                            setTimeout("pollLinkID()", 2000);
                        } 

                    },

                    error: function(result) {
                        alert("Error");
                    }

                });

                /*
                var pollResult = PageMethods.pollLinkID();
                console.log("poll result" . pollResult);
                if (pollResult.hideQR) {
                console.log("hide QR...");
                } else {
                console.log("show QR...");
                }
                setTimeout("pollLinkID()", 2000);
                */
            })();
        
        };
    </script>
    <script type="text/javascript">
        $(document).ready(function() {
            pollLinkID();
            console.log("document loaded");
        });
    </script>
</head>
<body>
    <div>
    
        <img id="qr" runat="server" alt="linkID QR Code" src="" />  
        
        <pre>
            <asp:Label ID="state" runat="server" Text=""></asp:Label>
        </pre>

        <form id="form1" runat="server">
            <asp:ScriptManager ID='ScriptManager1' runat='server' EnablePageMethods='true' />        
            <asp:LinkButton ID="restart" runat="server" Text="Restart" OnClick="onRestart" />        
        </form>
    </div>
</body>
</html>
