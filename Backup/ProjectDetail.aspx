<%@ Page Language="C#" AutoEventWireup="true" Inherits="ProjectDetail" Codebehind="ProjectDetail.aspx.cs" %>

<%@ Register src="menu.ascx" tagname="menu" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Detail</title>
    <script type="text/javascript" src="http://bi.multek.com/ws/utility.js" ></script>
    <script type="text/javascript" src="http://bi.multek.com/ws/calendar.js" ></script>
    <link href="Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    table td {padding:0px 1px 0px 3px;}
    </style>
    <script type="text/javascript">
    
    function loadpage(){
        document.getElementById("loading").style.display = "none";
        if(document.getElementById("selection") != undefined)
            document.getElementById("selection").style.display = "none";
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
            filtering.getProjectSelect(fil,o.value,setValue);
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

                    <div style="padding:0px 1px;">
                <table cellspacing="1" cellpadding="1" border="0">
                <tr>
                    <td>OEM</td><td>
                    <asp:TextBox CssClass="txtbox_small" ID="OEM" runat="server" onkeyup="return getValue(this,'OEM');" /></td>
                    <td>Plant</td><td>
                    <asp:TextBox CssClass="txtbox_small" ID="Plant" runat="server" /></td>
                    <td>Proj#</td><td>
                    <asp:TextBox CssClass="txtbox_small" ID="project" runat="server" onkeyup="return getValue(this,'item');" /></td>
                    <td>Layer</td><td>
                    <asp:TextBox CssClass="txtbox_small" ID="layer" runat="server" /></td>
                    <td>Technical</td><td>
                    <asp:TextBox CssClass="txtbox_small" ID="tech" runat="server" onkeyup="return getValue(this,'tech');" /></td>
                </tr>
                <tr>
                <td>Surface</td><td>
                    <asp:TextBox CssClass="txtbox_small" ID="surf" runat="server" onkeyup="return getValue(this,'surf');"/></td>
                <td>Start Date</td><td>
                    <asp:TextBox CssClass="txtbox_small" ID="startDate" runat="server" /></td>
                <td>End Date</td><td>
                    <asp:TextBox CssClass="txtbox_small" ID="endDate" runat="server" ></asp:TextBox></td>
                <td colspan="4">
                <div style="text-align:right">
                    <asp:Button ID="search"   OnClientClick="initload()"
                        runat="server" Text="Search" 
                        onclick="search_Click" />
                        <asp:Button ID="downloadExcel" runat="server" Text="Download as Excel" 
                        onclick="downloadExcel_Click" />                </div>
                
                    </td>
                </tr>
                </table>
            </div>
            <div style="display:none" id="loading">
            <img src="http://bi.multek.com/ws/images/ajax-loader_6.gif" alt="loading...." />
            </div>        

            <asp:GridView ID="GridView1" runat="server" AllowPaging="True"  CssClass="standardTable"
                        onpageindexchanging="GridView1_PageIndexChanging" PageSize="30" 
                        AutoGenerateColumns="False" 
             onrowdatabound="GridView1_RowDataBound" ondatabound="GridView1_DataBound">
                    <Columns>
                        <asp:BoundField DataField="oem" HeaderText="OEM" />
                        <asp:BoundField DataField="plant" HeaderText="Plant" />
                        <asp:BoundField DataField="item" HeaderText="Proj#" />
                        <asp:BoundField DataField="cus_part_no" HeaderText="Part#" />
                        <asp:BoundField DataField="layer" HeaderText="Layer" />
                        <asp:BoundField DataField="tech" HeaderText="Technical" />
                        <asp:BoundField DataField="surf" HeaderText="Surface Finish" />
                        <asp:BoundField DataField="gamDate" HeaderText="Date" />
                        <asp:BoundField DataField="asp" HeaderText="ASP" 
                            DataFormatString="{0:n4}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="sqft" HeaderText="SQFT" 
                            DataFormatString="{0:n4}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="price" HeaderText="Price" 
                            DataFormatString="{0:n4}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="qty" HeaderText="Qty" 
                            DataFormatString="{0:n0}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="amount" HeaderText="Amount" 
                            DataFormatString="{0:n4}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="iperiod" HeaderText="Period" />
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
