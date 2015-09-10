<%@ Page Language="C#" AutoEventWireup="true" Inherits="HubInvDetail" Codebehind="HubInvDetail.aspx.cs" %>

<%@ Register src="menu.ascx" tagname="menu" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hub Inventory Detail Reports</title>
    <script type="text/javascript" src="http://bi.multek.com/ws/utility.js" ></script>
    <script type="text/javascript" src="http://bi.multek.com/ws/calendar.js" ></script>
    <link href="Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    table td {padding:0px 1px 0px 3px;}
    </style>
    <script type="text/javascript">

    function loadpage(){
        document.getElementById("loading").style.display = "none";
    }
    function initload(){
        document.getElementById("loading").style.display = "block";
    }
    
var getCumulativeOffset = function (obj) {
    var left, top;
    left = top = 0;
    if (obj.offsetParent) {
        do {
            left += obj.offsetLeft;
            top  += obj.offsetTop;
        } while (obj = obj.offsetParent);
    }
    return {
        x : left,
        y : top
    };
};

    function getValue(o,fil)
    {
    //return;
    
        selection = pg.html.showPanel(null,"selection","auto","auto");
        selection.style.display = "none";
        selection.style.whiteSpace='nowrap'; 
        selection.style.overflow = "auto";
        selection.innerHTML = "";

    var xy = new getCumulativeOffset(o);
        selection.style.left = (xy.x) + "px";
        selection.style.top = (xy.y + 22) + "px";
        selection.style.width = (o.clientWidth - 4) + "px";            

            
        selectionObject = o;
        if(o.value.trim() != "")
            filtering.getSelection(fil,o.value,setValue);
        return;
    }
    function setValue(obj)
    {
        var lst = eval(obj);
        selection.innerHTML = "";
        var cls = document.createElement("div");
        cls.innerHTML = "Close";
        cls.style.backgroundColor = "blue";
        cls.style.color = "#ffffff";
        cls.style.padding = "1px 2px";
        cls.style.cursor="pointer";
        cls.onclick = function(){ selection.style.display = "none";}
        selection.appendChild(cls);
        for(var i=0; i < lst.length; i++){
            var div = document.createElement("div");
            div.innerHTML = lst[i];
            div.style.cursor = "pointer";
            div.onclick = function()
            {
                selectionObject.value = this.innerText;
                selection.innerHTML = "";
                selection.style.display = "none";   
            }
            selection.appendChild(div);
        }
        selection.style.display = "block";
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/filtering.asmx" />
        </Services>
        </asp:ScriptManager>
    
        
    <uc1:menu ID="menu1" runat="server" />
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
     
     
        <asp:Label ID="temp" runat="server" />
        <div style="padding:0px 1px;">
            <table cellspacing="1" cellpadding="1" border="0">
            <tr>
                <td>OEM</td><td><asp:TextBox CssClass="txtbox_small" ID="OEM" runat="server" onkeyup="return getValue(this,'OEM');"/></td>
                <td>Proj#</td><td><asp:TextBox CssClass="txtbox_small" ID="partNumber" runat="server" onkeyup="return getValue(this,'partNumber');" /></td>
                <td>Customer Part#</td><td><asp:TextBox CssClass="txtbox_small" ID="customerPartNo" runat="server" onkeyup="return getValue(this,'customerPartNo');" /></td>
                <td>Warehouse Code</td><td><asp:TextBox CssClass="txtbox_small" ID="warehouse" runat="server" onkeyup="return getValue(this,'warehouse');" /></td>
            </tr>
            <tr>
                <td>Delivery Code</td>
                <td><asp:TextBox CssClass="txtbox_small" ID="delCode" runat="server" onkeyup="return getValue(this,'delCode');" /></td>
                <td>Customer PO</td>
                <td><asp:TextBox CssClass="txtbox_small" ID="customerPO" runat="server" onkeyup="return getValue(this,'customerPO');"/></td>
                <td>CRF#</td><td><asp:TextBox CssClass="txtbox_small" ID="crfCode" runat="server" onkeyup="return getValue(this,'crfCode');"/></td>
                <td>Customer Code</td><td><asp:TextBox CssClass="txtbox_small" ID="customerCode" runat="server"  onkeyup="return getValue(this,'customerCode');"/></td>
                
            </tr>
            <tr>
            <td>Start Date</td><td><asp:TextBox CssClass="txtbox_small" ID="startDate" runat="server" /></td>
            <td>End Date</td><td><asp:TextBox CssClass="txtbox_small" ID="endDate" runat="server" ></asp:TextBox></td>
            <td>Baan#</td><td><asp:TextBox CssClass="txtbox_small" ID="baanNo" runat="server" onkeyup="return getValue(this,'baanSO');"></asp:TextBox></td>
            <td colspan="2" align="right"><asp:Button ID="search"  runat="server" Text="Search" 
                    onclick="search_Click"   OnClientClick="initload()" />
                    <asp:Button ID="downloadExcel" runat="server" Text="Download as Excel" 
                    onclick="downloadExcel_Click" /></td>
            </tr>
            </table>
        </div>
            <div style="display:none" id="loading">
            <img src="http://bi.multek.com/ws/images/ajax-loader_6.gif" alt="loading...." />
            </div>

        <asp:GridView ID="GridView1" runat="server" AllowPaging="True"  CssClass="tablestyle"
                        onpageindexchanging="GridView1_PageIndexChanging" PageSize="30" 
                        AutoGenerateColumns="False" AllowSorting="true"
             onrowdatabound="GridView1_RowDataBound" ondatabound="GridView1_DataBound" 
             onsorting="GridView1_Sorting">
                    <Columns>
                        <asp:BoundField DataField="oem" HeaderText="OEM"  />
                        <asp:BoundField DataField="customerPartNo" HeaderText="Part#" SortExpression="customerPartNo" />
                        <asp:BoundField DataField="partNumber" HeaderText="Proj#" SortExpression="partNumber" />
                        <asp:BoundField DataField="customerPO" HeaderText="Customer PO" />
                        <asp:BoundField DataField="hubinDate" DataFormatString="{0:MMMM d, yyyy}" 
                            HeaderText="Hub in Date" SortExpression="hubInDate" />
                        <asp:BoundField DataField="unitPrice" HeaderText="Unit Price" 
                            DataFormatString="{0:n}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="shippedQty" HeaderText="Shipped Qty" 
                            DataFormatString="{0:#,###}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="shippedAmount" HeaderText="Shipped Amt" 
                            DataFormatString="{0:n}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="cs" HeaderText="CS" />
                        <asp:BoundField DataField="toCountry" HeaderText="To Country" />
                        <asp:BoundField DataField="Plant" HeaderText="Plant" />
                        <asp:BoundField DataField="warehouse" HeaderText="Warehouse" />
                        <asp:BoundField DataField="customerCode" HeaderText="Cust Code" />
                        <asp:BoundField DataField="delCode" HeaderText="Del Code" />
                        <asp:BoundField DataField="BaanSO" HeaderText="Baan SO" SortExpression="BaanSO" />
                        <asp:BoundField DataField="crfcode" HeaderText="CRF" SortExpression="crfCode" />
                    </Columns>
                </asp:GridView>
        <asp:Label ID="Label1" runat="server" ForeColor="#CC0000"></asp:Label>
     
     </ContentTemplate>
     <Triggers>
        <asp:PostBackTrigger ControlID="downloadExcel" />
     </Triggers>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
