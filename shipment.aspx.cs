using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

public partial class shipment : System.Web.UI.Page
{
    long Amt = 0;
    int Qty = 0;
    long Sqft = 0;
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
            DateTime td = DateTime.Today;
            td = td.AddDays(1 - td.Day);
            //startDate.Text = td.ToShortDateString();
            //endDate.Text = td.AddMonths(1).AddDays(-1).ToShortDateString();

            startDate.Attributes.Add("onclick", "calendar(this)");
            endDate.Attributes.Add("onclick", "calendar(this)");
            Label1.Visible = false;
            Label1.Text = "No records found....";
        }
    }
    protected void downloadExcel_Click(object sender, EventArgs e)
    {
        genExcelByXML();

    }
    protected void search_Click(object sender, EventArgs e)
    {
        loading();
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0)
        {
            Table tbl = (Table)GridView1.Controls[0];
            GridViewRow row = new GridViewRow(1, -1, DataControlRowType.Header, DataControlRowState.Normal);
            string msg = "";
            if (rsQty == limitQty)
                msg = "Warning: Your search result has reached the limit of the number of "+ limitQty.ToString();
            addCell(msg, row, 10);
            addCell("Total:", row, 1);
            addCell(Qty.ToString("n0"), row, 1);
            addCell(Sqft.ToString("n0"), row, 1);
            addCell(Amt.ToString("n0"), row, 1);
            addCell("", row, 17);
           
            tbl.Rows.AddAt(1, row);
        }
    }
    private void addCell(string text, GridViewRow row, int span)
    {
        TableCell cell = new TableHeaderCell();
        cell.Text = text;
        row.Cells.Add(cell);
        cell.ForeColor = System.Drawing.Color.Red;
        cell.HorizontalAlign = HorizontalAlign.Right;
        if (span > 1)
            cell.ColumnSpan = span;
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
                ViewState["SortOrder"].ToString().IndexOf(" desc") > 0 ? "sortascheaderstyle" : "sortdescheaderstyle";
            }
        }
        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFCD6'");
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
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

            cmd.CommandText = "[sp_MKTRPT_shipment_new]";
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@site", site.SelectedValue);
            cmd.Parameters.AddWithValue("@sysuid", usr.sysUserId);
            cmd.Parameters.AddWithValue("@oem", OEM.Text.Trim());
            cmd.Parameters.AddWithValue("@cem", CEM.Text.Trim());
            cmd.Parameters.AddWithValue("@project ", partNumber.Text.Trim());
            cmd.Parameters.AddWithValue("@cust_part_no ", customerPartNo.Text.Trim());
            cmd.Parameters.AddWithValue("@warehouse", warehouse.Text.Trim());
            cmd.Parameters.AddWithValue("@deliver", delCode.Text.Trim());
            cmd.Parameters.AddWithValue("@cust_po", customerPO.Text.Trim());
            cmd.Parameters.AddWithValue("@order_type", order_Type.Text.Trim());
            cmd.Parameters.AddWithValue("@Baan_so", baan_so.Text.Trim());
            cmd.Parameters.AddWithValue("ast_so", crfCode.Text.Trim());
            cmd.Parameters.AddWithValue("@cust_code", customerCode.Text.Trim());
            cmd.Parameters.AddWithValue("@invoice", invoice_no.Text.Trim());
            cmd.Parameters.AddWithValue("@startDate", start);
            cmd.Parameters.AddWithValue("@endDate", end);
            cmd.Parameters.AddWithValue("@pdName", productName.Text.Trim());
            SqlParameter tAmt = cmd.Parameters.AddWithValue("@totalShipAmt", 0);
            tAmt.Direction = ParameterDirection.Output;
            tAmt.DbType = DbType.Int64;
            SqlParameter tQty = cmd.Parameters.AddWithValue("@totalShipQty", 0);
            tQty.Direction = ParameterDirection.Output;
            SqlParameter tSqft = cmd.Parameters.AddWithValue("@totalShipSqft", 0);
            tSqft.Direction = ParameterDirection.Output;
            SqlParameter ltdQty = cmd.Parameters.AddWithValue("@limitQty", 0);
            ltdQty.Direction = ParameterDirection.Output;
            if (ViewState["SortOrder"] != null)
            {
                cmd.Parameters.AddWithValue("@sort", " order by " + ViewState["SortOrder"].ToString());
            }

            SqlDataAdapter custDA = new SqlDataAdapter();
            custDA.SelectCommand = cmd;
            //cmd.CommandTimeout = 3600;
            cmd.Connection = conn;
            DataSet ds = new DataSet();
            custDA.Fill(ds, "tmp");
            dt = ds.Tables[0];
            custDA.Dispose();
            Amt = Convert.ToInt64(tAmt.Value);
            Qty = Convert.ToInt32(tQty.Value);
            Sqft = Convert.ToInt64(tSqft.Value);
            limitQty = Convert.ToInt32(ltdQty.Value);
            rsQty = dt.Rows.Count;
            cmd.Dispose();
        }
        return dt;
    }
    private void genExcelByXML()
    {
        HttpContext context = HttpContext.Current;
        context.Response.Clear();
        context.Response.Charset = "";
        context.Response.AddHeader("content-disposition", "attachment;filename=shipment.xls");
        context.Response.ContentType = "application/vnd.ms-excel";
        StreamReader sr = new StreamReader(Context.Server.MapPath("xml/excelTemp.xml"));
        string rptxml = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();
        string content = "<Row>" +
            "<Cell><Data ss:Type=\"String\">OEM</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Part#</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Project#</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Customer PO</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">PO Date</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Cust request Date</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Invoice Date</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Currency</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Unit Price</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Qty</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Sqft</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Amount</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">To Country</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Order Type</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Sale Type</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Invoice#</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">BaaN SO</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Site</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Warehouse</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">CRF#</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Product Name</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Cust Code</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Unit/Set</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Delivery Code#</Data></Cell>"+
            "<Cell><Data ss:Type=\"String\">Proejct Type</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Plant</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Special Price</Data></Cell>"+
            "<Cell><Data ss:Type=\"String\">CEM</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Ship To Customer</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Sales Man</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Segment</Data></Cell>" +
            "</Row>";
        string rowxml = "<Row><Cell><Data ss:Type=\"String\">{0}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{1}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{2}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{3}</Data></Cell>" +
            "<Cell ss:StyleID=\"s23\"><Data ss:Type=\"DateTime\">{4:yyyy-MM-ddTHH:mm:ss.fff}</Data></Cell>" +
            "<Cell ss:StyleID=\"s23\"><Data ss:Type=\"DateTime\">{5:yyyy-MM-ddTHH:mm:ss.fff}</Data></Cell>" +
            "<Cell ss:StyleID=\"s23\"><Data ss:Type=\"DateTime\">{6:yyyy-MM-ddTHH:mm:ss.fff}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{7}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{8}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{9}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{10}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{11}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{12}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{13}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{14}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{15}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{16}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{17}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{18}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{19}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{20}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{21}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{22}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{23}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{24}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{25}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{26}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{27}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{28}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{29}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{30}</Data></Cell>" +
            "</Row>";
        DataTable dt = getData();
        //you can download no more than 10000 records. But now you may exceeded.
        //Warning: Your downloaded result has reached the limit of the number of " + limitQty.ToString() + "
        if (rsQty == limitQty)
            content = "<Row>" +
                "<Cell ss:StyleID=\"s71\"><Data ss:Type=\"String\">Warning: Your downloaded result has reached the limit of the number of " + limitQty.ToString() + ". It will probably not your expected.</Data></Cell>" +
                "</Row>" + content; 

        StringBuilder sb = new StringBuilder();
        sb.Append(content);
        foreach (DataRow row in dt.Rows)
        {
            sb.Append(string.Format(rowxml, vs(row["oem_name"]),vs(row["part_no"]),
                vs(row["prj_no"]),vs(row["po_no"]),row["po_date"],
                row["cust_req_date"],row["invoice_date"],vs(row["currency"]),vs(row["unitPrice"]),vs(row["del_qty"]),
                vs(row["del_sqft"]),vs(row["del_amt_usd"]),vs(row["ship_to_cty"]),vs(row["order_type"]),
                vs(row["sale_type"]),vs(row["invoice_no"]),vs(row["baan_so"]),vs(row["site"]),vs(row["warehouse"]),vs(row["ast_so"]),vs(row["product_name"]),
                vs(row["cust_code"]),vs(row["unit_panel"]),vs(row["del_code"]),vs(row["project_type"]),vs(row["plant"]), 
                vs(row["SpecialPrice"]),vs(row["cem_name"]),vs(row["ship_to_customer"]),vs(row["salesman"]),vs(row["segment1"])
                ));
        }
        dt.Dispose();
        rptxml = rptxml.Replace("<Row />", sb.ToString());
        Response.Write(rptxml);
        Response.End();
    }
    private string vs(object str)
    {
        return str.ToString().Trim().Replace("\"", "&quot;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;");
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
}
