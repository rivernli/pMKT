<%@ Page Language="C#" AutoEventWireup="true" Inherits="userControl" Codebehind="userControl.aspx.cs" %>

<%@ Register src="menu.ascx" tagname="menu" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Control</title>
    <link href="Stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:menu ID="menu1" runat="server" />
    </div>
    </form>
    <p>
        Please go to <a href='http://bi.multek.com/gam/acl.aspx' target="_blank" >bi.multek.com/gam</a> for user config</p>
</body>
</html>
