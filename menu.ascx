<%@ Control Language="C#" AutoEventWireup="true" Inherits="menu" Codebehind="menu.ascx.cs" %>
     <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding:0px;">
                 <tr>
                     <td >
                        <img alt="Multek" id="HeaderImage" src="images/Multek-ID-PMS.gif" style="height:47px;border-width:0px;" onclick="return HeaderImage_onclick()" />
                    </td>
                    <td align="center" width="600px">
                        <span id="Span1" class="MasterTile" style="display:inline-block;width:500px; white-space:nowrap">
                        Marketing Reports</span></td>
                    <td  
                         style="background-image: url('images/flex_curve.gif'); width: 423px; text-align: center">
                        &nbsp;
                        <span style="color:#ffffff; cursor:pointer; text-decoration:underline;" onclick="pg.html.addFavorite('Hub inventory Detail Reports');">Add To Favorite</span>
                        <br />
                        <a href="mailto:CN-HKGMultekApplications%26Business@cn.flextronics.com?subject=comments%20for%20HUB%20Inventory%20Detail%20resports" style="color:#ffffff; cursor:pointer; text-decoration:underline;">Send your comments</a>
                    </td>
                 </tr>
                <tr bgcolor="#003366" height="20px">
                    <td colspan="3"  bgcolor="#003366" height="20px" align="center">
                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr><td colspan="3">
            <ul id="channel">
                <li id='li_shipment' runat="server"><asp:HyperLink ID="HyperLink1" Text="Shipment" runat="server" NavigateUrl="shipment.aspx" /></li>
                <li id='li_backlog' runat="server"><asp:HyperLink ID="HyperLink3" Text="Backlog" runat="server" NavigateUrl="backlog.aspx" /></li>
				<li id='li_hubdetail' runat="server"><asp:HyperLink ID="HyperLink2" Text="Hub Inventory Detail" runat="server" NavigateUrl="hubInvDetail.aspx"/></li>
				<li id="li_project" runat="server"><asp:HyperLink ID="hyp_project" Text="Project Detail" runat="server" NavigateUrl="~/ProjectDetail.aspx" /></li>
				<li id="li_oem_ctrl" runat="server"><asp:HyperLink ID="oemSync" Text="OEM Billing Policy" runat="server" NavigateUrl="OEMBillingPolicy.aspx" /></li>
				<li id="li_ecm_location" runat="server"><asp:HyperLink ID="cemLocation" Text="CEM Location Setting" runat="server" NavigateUrl="CEMLocation.aspx" /></li>
				<li id="li_member_ctrl" runat="server"><asp:HyperLink ID="memberControl" runat="server" Text="Access Control" NavigateUrl="userControl.aspx" /></li>
                <li><asp:HyperLink ID="logout" runat="server" Text="Logout" NavigateUrl="~/logout.aspx" /></li>

			</ul>


                <div style="border-top:1px solid #444; width:auto; height:5px; ">&nbsp;</div>
                </td></tr>
            </table>