<%@ Page Language="C#" AutoEventWireup="true" Inherits="CEMLocation" Codebehind="CEMLocation.aspx.cs" %>

<%@ Register src="menu.ascx" tagname="menu" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CEM Location Control</title>
    <link href="Stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:menu ID="menu1" runat="server" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Label ID="sy" Text="Synchronize Customer Code:" runat="server" />
         <asp:ImageButton ID="ImageButton3" runat="server"  AlternateText="Synchronize Customer Code"
            ImageUrl="images/action_refresh.gif" onclick="ImageButton3_Click" 
            style="height: 16px" />
        <asp:ListView ID="ListView1" runat="server"
            onitemcommand="ListView1_ItemCommand" 
            onitemediting="ListView1_ItemEditing" onitemcanceling="ListView1_ItemCanceling" 
            onitemupdating="ListView1_ItemUpdating" 
                onitemdatabound="ListView1_ItemDataBound">
            <LayoutTemplate>
                   <div style="background:url(images/background.gif);">
                       <table border="1" bordercolor="#cccccc" cellpadding="1" cellspacing="0" 
                           class="standardTable" width="560" >
                           <tr bgcolor="0CAAFF">
                               <td>Customer Code</td>
                               <td>Location</td>
                               <td></td>
                           </tr>
                           <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                       </table>
                   </div>
               </LayoutTemplate>
            <ItemTemplate>
                 <tr>
                       <td><asp:Label ID="customer_code" runat="server" Text='<%# Eval("customer_code") %>' ></asp:Label></td>
                       <td><asp:Label ID="location" runat="server" Text='<%# Eval("location") %>' /></td>
                       <td>
                           <asp:ImageButton ID="editBtn" runat="server" CommandName="Edit"
                               ImageUrl="images/edit.png" AlternateText="Modify User" 
                                />
                       </td>
                   </tr>
            </ItemTemplate>
            <EditItemTemplate>
                 <tr>
                       <td><asp:Label ID="customer_code" runat="server" Text='<%# Eval("customer_code") %>' /></td>
                       <td><asp:TextBox Width="200"  ID="location" runat="server" Text='<%# Eval("location") %>' /></td>
                       <td>
                           <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Cancel" 
                               ImageUrl="images/cancel.png" 
                               />
                           <asp:ImageButton ID="ImageButton2" runat="server" CommandName="Update" 
                               ImageUrl="images/submit.png" 
                               />
                       </td>
                   </tr>
            </EditItemTemplate>
        </asp:ListView>       
        </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
