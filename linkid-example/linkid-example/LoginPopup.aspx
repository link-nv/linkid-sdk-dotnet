﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPopup.aspx.cs" Inherits="linkid_example.LoginPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">

    <title>linkID Popup Login Demo</title>
    
    <script type="text/javascript" id="linkid-login-script" 
        src="https://demo.linkid.be/linkid-static/js/linkid-min.js"></script>

    <style type="text/css">
        .linkid-login {
            cursor: pointer;
        }
    </style>

</head>
<body>
    <h1>linkID Popup Login Demo</h1>

    <div>
        <a class="linkid-login" data-mode="popup" data-login-href="./LinkIDLogin.aspx" 
                data-completion-href="./LoggedIn.aspx">
            Start
        </a>        
    </div>
</body>
</html>
