using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Xsl;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

public partial class HubInvDetail : System.Web.UI.Page
{
    int totalQty;
    double totalAmt;
    int limitQty = 0;
    int rsQty = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["usr"] == null)
            {
                //Response.Redirect("default.aspx");
                //re take session when it's lose.
                nUser usr = new nUser(User.Identity.Name.ToString());
                Session["usr"] = usr;
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "init", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(loadpage);", true);
            startDate.Attributes.Add("onclick", "calendar(this)");
            endDate.Attributes.Add("onclick", "calendar(this)");
            Label1.Visible = false;
            Label1.Text = "No records found....";
            ViewState["SortOrder"] = "BaanSO desc";
        }

    }
    protected void search_Click(object sender, EventArgs e)
    {
        loading();
    }
    private void loading()
    {
        GridView1.PageIndex = 0;
        GridView1.DataSource = getData();
        GridView1.DataBind();
        Label1.Visible = false;
        if (GridView1.Rows.Count <= 0)
        {
            Label1.Visible = true;
            //Label1.Text = "No records found";
        }
    }
    private void genExcelByGV()
    {
        HttpResponse response = HttpContext.Current.Response;
        string style = @"<style>td { mso-number-format:\@;}</style>";
        response.Clear();
        response.Charset = "";
        response.Buffer = true;
        response.Write(style);
        response.ContentType = "application/vnd.ms-excel";
        response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        response.AddHeader("Content-Disposition", "attachment;filename=\"hubinventorydetail.xls\"");
        response.Write("<meta http-equiv=Content-Type content=application/vnd.ms-excel;charset=utf-8>");

        GridView gv2 = new GridView();
        DataView dv = getData().DefaultView;
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                gv2.DataSource = dv;
                gv2.DataBind();
                gv2.RenderControl(htw);
                response.Write(sw.ToString());
                response.End();
            }
        }
    }
    private void genExcelByXML()
    {
        HttpContext context = HttpContext.Current;
        context.Response.Clear();
        context.Response.Charset = "";
        context.Response.AddHeader("content-disposition", "attachment;filename=hubinventorydetail.xls");
        context.Response.ContentType = "application/vnd.ms-excel";


        StreamReader sr = new StreamReader(Context.Server.MapPath("xml/excelTemp.xml"));
        string rptxml = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();

        string content = "<Row>" +
            "<Cell><Data ss:Type=\"String\">OEM</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Part#</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Project#</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">CustomerPO</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">HubInDate</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Price</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">shippedQty</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Amt</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">CS</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">ToCountry</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Plant</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">warehouse</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">CustomerCode</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">DeliveryCode</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Baan SO</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">CRF Code</Data></Cell></Row>";

        string rowxml = "<Row><Cell><Data ss:Type=\"String\">{0}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{1}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{2}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{3}</Data></Cell>" +
            "<Cell ss:StyleID=\"s23\"><Data ss:Type=\"DateTime\">{4:yyyy-MM-ddTHH:mm:ss.fff}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{5}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{6}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{7}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{8}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{9}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{10}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{11}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{12}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{13}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{14}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{15}</Data></Cell></Row>";
        DataTable dt = getData();
        if (rsQty == limitQty)
            content = "<Row>" +
                "<Cell ss:StyleID=\"s71\"><Data ss:Type=\"String\">Warning: Your downloaded result has reached the limit of the number of " + limitQty.ToString() + ". It will probably not your expected.</Data></Cell>" +
                "</Row>" + content;
        StringBuilder sb = new StringBuilder();
        sb.Append(content);
        foreach (DataRow row in dt.Rows)
        {
            sb.Append(string.Format(rowxml,
                vs(row["oem"]), vs(row["customerPartNo"]), vs(row["partNumber"]), vs(row["customerPO"]), row["hubindate"],
                vs(row["unitprice"]), vs(row["shippedQty"]), vs(row["shippedAmount"]), vs(row["cs"]), vs(row["toCountry"]),
                vs(row["plant"]), vs(row["warehouse"]), vs(row["customerCode"]), vs(row["delCode"]), vs(row["baanSO"]), vs(row["crfCode"])));
        }
        dt.Dispose();
        rptxml = rptxml.Replace("<Row />", sb.ToString());
        Response.Write(rptxml);
        Response.End();
        /*using (StringWriter sw = new StringWriter())
        {
            sw.Write(rptxml);
            context.Response.Write(sw.ToString());
        }
        context.Response.End();
        */
        /*
        foreach (DataColumn col in dt.Columns)
        {
            rowxml += col.ColumnName+"." + col.DataType.ToString() +"<br/>";
        }
        dt.Dispose();
        temp.Text = rowxml;
         */
    }
    private void genExcelByXSL()
    {
        DataTable dt = getData();
        XmlDataDocument xdd = new XmlDataDocument(dt.DataSet);
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "";
        XslTransform xt = new XslTransform();
        xt.Load(Server.MapPath("xml/excelTemp.xslt"));
        xt.Transform(xdd, null, Response.OutputStream);
        Response.End();
    }
    protected void downloadExcel_Click(object sender, EventArgs e)
    {
        genExcelByXML();
        /*
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        DataTable dt = getData();
        dt.WriteXml(sw);
        dt.Dispose();
        Response.Write(sb.ToString());
        */
    }

    private DataTable getData()
    {
        nUser usr = (nUser)Session["usr"];
        //usr.sysUserId;

        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["biconn"].ToString()))
        {
            int start = 0;
            int end = 0;
            if (startDate.Text.Trim() != "")
                start = Convert.ToInt32(DateTime.ParseExact(startDate.Text.Trim(), @"M/d/yyyy", null).ToString("yyyyMMdd"));
            if (endDate.Text.Trim() != "")
                end = Convert.ToInt32(DateTime.ParseExact(endDate.Text.Trim(), @"M/d/yyyy", null).ToString("yyyyMMdd"));

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "[sp_MKTRPT_hubInvDetail]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@oem", OEM.Text.Trim());
            cmd.Parameters.AddWithValue("@proj", partNumber.Text.Trim());
            cmd.Parameters.AddWithValue("@cusPartNo", customerPartNo.Text.Trim());
            cmd.Parameters.AddWithValue("@warehouse", warehouse.Text.Trim());
            cmd.Parameters.AddWithValue("@delveryCode", delCode.Text.Trim());
            cmd.Parameters.AddWithValue("@customerPO", customerPO.Text.Trim());
            cmd.Parameters.AddWithValue("@customerCode", customerCode.Text.Trim());
            cmd.Parameters.AddWithValue("@crfCode", crfCode.Text.Trim());
            cmd.Parameters.AddWithValue("@baanSO", baanNo.Text.Trim());
            cmd.Parameters.AddWithValue("@startDate", start);
            cmd.Parameters.AddWithValue("@endDate", end);
            cmd.Parameters.AddWithValue("@sysuid", usr.sysUserId);
            SqlParameter amtObj = cmd.Parameters.AddWithValue("@totalShipAmt", 0);
            amtObj.Direction = ParameterDirection.Output;
            SqlParameter qtyQbj = cmd.Parameters.AddWithValue("@totalShipQty", 0);
            qtyQbj.Direction = ParameterDirection.Output;
            SqlParameter ltdQty = cmd.Parameters.AddWithValue("@limitQty", 0);
            ltdQty.Direction = ParameterDirection.Output;
            if (ViewState["SortOrder"] != null)
            {
                cmd.Parameters.AddWithValue("@sort", " order by " + ViewState["SortOrder"].ToString());
            }
            SqlDataAdapter custDA = new SqlDataAdapter();
            custDA.SelectCommand = cmd;
            cmd.Connection = conn;
            DataSet ds = new DataSet();
            custDA.Fill(ds, "tmp");
            dt = ds.Tables[0];
            custDA.Dispose();

            totalAmt = Convert.ToDouble(amtObj.Value);
            totalQty = Convert.ToInt32(qtyQbj.Value);
            limitQty = Convert.ToInt32(ltdQty.Value);
            rsQty = dt.Rows.Count;
            cmd.Dispose();
        }
        return dt;
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = getData();
        GridView1.DataBind();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int cellIndex = -1;
            int idx;
            foreach (DataControlField field in GridView1.Columns)
            {
                if (field.SortExpression != "")
                {
                    idx = GridView1.Columns.IndexOf(field);
                    e.Row.Cells[idx].CssClass = "sortholdheaderstyle";
                    if (field.SortExpression == (string)ViewState["SortOrder"] || field.SortExpression + " desc" == (string)ViewState["SortOrder"])
                    {
                        cellIndex = idx;
                    }
                }
            }
            if (cellIndex > -1)
            {
                e.Row.Cells[cellIndex].CssClass =
                ViewState["SortOrder"].ToString().IndexOf(" desc") > 0? "sortascheaderstyle" : "sortdescheaderstyle";
            }
        }
        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFCD6'");
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");

    }
    private string vs(object str)
    {
        return str.ToString().Trim().Replace("\"", "&quot;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;");
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0)
        {
            Table tbl = (Table)GridView1.Controls[0];
            GridViewRow row = new GridViewRow(1, -1, DataControlRowType.Header, DataControlRowState.Normal);
            string msg = "&nbsp;";
            if (rsQty == limitQty)
                msg = "Warning: Your search result has reached the limit of the number of " + limitQty.ToString();

            TableCell th = new TableHeaderCell();
            th.ColumnSpan = 6;
            th.Text = msg;
            row.Cells.Add(th);
            TableCell thQty = new TableHeaderCell();
            thQty.Text = "Total Qty<br/>" + totalQty.ToString();
            thQty.ForeColor = System.Drawing.Color.Red;
            thQty.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(thQty);
            TableCell thAmt = new TableHeaderCell();
            thAmt.Text = "Total Amount<br/>" + totalAmt.ToString();
            row.Cells.Add(thAmt);
            thAmt.ForeColor = System.Drawing.Color.Red;
            thAmt.HorizontalAlign = HorizontalAlign.Right;
            TableCell th2 = new TableHeaderCell();
            th2.ColumnSpan = 8;
            th2.Text = "&nbsp";
            row.Cells.Add(th2);
            tbl.Rows.AddAt(1, row);
        }
    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sPage = e.SortExpression;
        if ((string)ViewState["SortOrder"] == sPage)
        {
            ViewState["SortOrder"] = sPage + " desc";
        }
        else
        {
            ViewState["SortOrder"] = e.SortExpression;
        }
        loading();
    }
}
