<%@ Page Language="C#" AutoEventWireup="true" Inherits="Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Marketing Reports Login</title>
    <style type="text/css">

TD	{
	color:#000;
	font:13px Verdana,microsoft sans serif;
	}
.MasterTile {font-size:17px; color:#333333; font-weight:bold;}
span {font-size:13px;}
span	{
	color:#000;
	font:13px Verdana,microsoft sans serif;
	}


.loginDiv { position:relative; background-image:url('../B1ME/images/login_head.gif'); 
background-repeat:no-repeat;
            background-position:center top; 
width:324px;
        }
.loginDivBtm { background-image:url('../B1ME/images/login_head_btm.gif'); 
               background-position:center bottom; 
background-repeat:no-repeat;
        }
               
A:LINK	{
	color:#039;
	text-decoration:none;
	}
    </style>
    <link href="Stylesheet.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
function loadpage(){
    var ft = document.getElementById("footer");
    var bg = document.getElementById("bgScreen");
    if(ft.offsetTop >0 && bg.offsetTop > 0){
        var y = ft.offsetTop - bg.offsetTop;
        bg.style.height = y +"px";
    }else{
        bg.style.height = "500px";
    }
    var d = new Date().getMonth()+1;
    var p = "http://bi.multek.com/ws/images/bg_month/m"+d+".jpg";
    bg.style.background = "#000000 url('http://bi.multek.com/ws/images/bg_speed.png') no-repeat fixed center";
}
//window.onresize = loadpage;
//window.onload = loadpage;
</script>
</head>
<body style="margin:0px;">
    <form id="form2" runat="server" style="display:inline;">
    <div style="padding:0px; margin:0px;">
        <div>
            <table border="0" cellpadding="0" cellspacing="0" 
                style="width: 100%; padding:0px;">
                <tr>
                    <td>
                        <span style="padding-left:3px;">
                        <img id="HeaderImage" alt="Multek" src="images/Multek-ID-PMS.gif" 
                            style="height:47px;border-width:0px;" /></span>
                    </td>
                    <td align="center" width="600px">
                        <span id="Span1" class="MasterTile" style="display:inline-block;width:300px;">
                        Marketing Reports</span></td>
                    <td style="background-image: url('images/flex_curve.gif'); width: 423px; text-align: center">
                        &nbsp;<span onclick="addFavorite('Hub inventory Detail Reports');" 
                            style="color:#ffffff; cursor:pointer;">Add To Favorite</span>
                    </td>
                </tr>
                <tr bgcolor="#003366" height="7px">
                    <td bgcolor="#003366" colspan="3" height="7px">
                        &nbsp;</td>
                </tr>
            </table>
        </div>
        <div id="bgScreen" style="width:auto; height:100%; padding:0px;">
            <div id="login_panel">
                <table align="center" width="400">
                    <tr>
                        <td style="height: 60px;">
                            <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div class="loginDiv">
                                <div class="loginDivBtm">
                                    <br />
                                    <table style="width: 300px;">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="Label3" runat="server" Text="Domain:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="americasCheck" runat="server" CssClass="domainSelect" 
                                                    GroupName="domain" Text="Americas" />
                                                <asp:RadioButton ID="asiaCheck" runat="server" Checked="true" 
                                                    CssClass="domainSelect" GroupName="domain" Text="Asia" />
                                                <asp:RadioButton ID="europeCheck" runat="server" CssClass="domainSelect" 
                                                    GroupName="domain" Text="Europe" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="Label1" runat="server" Text="User:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="username" runat="server" style="width: 180px;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="password" runat="server" style="width: 180px;" 
                                                    TextMode="Password"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Login" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="color: White; font-size: 10px; text-align: left; padding: 4px 2px; border-top: 1px #fff solid;">
                                                    Please verify your domain,account and password
                                                    <br />
                                                    For example:<br />
                                                    Domain:Asia; Account:LGZjzhan ; Password:******
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <asp:Label ID="Label4" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="footer">
            <span style="padding-top: 3px; font-size: 11px;">© System Support:             <a href="mailto:peter.xu@hk.multek.com">Peter Xu</a>,
            <a href="mailto:joe.cheng@hk.multek.com">Joe Cheng</a></span>
        </div>
    </div>
    </form>
</body>
</html>
