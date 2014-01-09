﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentMobile.aspx.cs" Inherits="linkid_example.PaymentMobile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>linkID Mobile Payment Demo</title>
    
    <script type="text/javascript" id="linkid-login-script" 
        src="https://192.168.5.14:8443/linkid-static/js/linkid-min.js"></script>

    <style type="text/css">
        .linkid-login {
            cursor: pointer;
        }
    </style>
    
</head>
<body>

    <h1>linkID Mobile Payment Demo</h1>

    <div class="qr-demo">
        <div>
            <iframe id="linkid" style="display: none;"></iframe>
        </div>
        <div>
            <a class="linkid-login" data-login-href="./LinkIDLogin.aspx?force=true" 
                    data-mobile-minimal="linkid" data-completion-href="./LoggedIn.aspx">
                Start
            </a>        
        </div>
    </div>
</body>
</html>
