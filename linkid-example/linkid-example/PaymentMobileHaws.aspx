<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentMobileHaws.aspx.cs" Inherits="linkid_example.PaymentMobileHaws" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">

    <title>linkID Mobile Payment over HAWS Demo</title>
    
    <script type="text/javascript" id="linkid-login-script" 
        src="https://demo.linkid.be/linkid-static/js/linkid.js"></script>

    <style type="text/css">
        .linkid-login {
            cursor: pointer;
        }
    </style>
    
</head>
<body>

    <h1>linkID Mobile Payment over HAWS Demo</h1>

    <div class="qr-demo">
        <div>
            <iframe id="linkid" style="display: none;"></iframe>
        </div>
        <div>
            <a class="linkid-login" data-login-href="./LinkIDLoginHaws.aspx?force=true" data-protocol="HAWS" 
                    data-mobile-minimal="linkid" data-completion-href="./LoggedIn.aspx">
                Start
            </a>        
        </div>
    </div>
</body>
</html>
