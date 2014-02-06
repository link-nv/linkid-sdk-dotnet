﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginRedirect.aspx.cs" Inherits="linkid_example.LoginRedirect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>linkID Redirect Login Demo</title>
    
    <script type="text/javascript" id="linkid-login-script" 
        src="https://demo.linkid.be/linkid-static/js/linkid-min.js"></script>

    <style type="text/css">
        .linkid-login {
            cursor: pointer;
        }
    </style>

</head>
<body>
    <h1>linkID Redirect Login Demo</h1>

    <div>
        <a class="linkid-login" data-mode="redirect" data-login-href="./LinkIDLogin.aspx" 
                data-completion-href="./LoggedIn.aspx">
            Start
        </a>        
    </div>
</body>
</html>
