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

public partial class ProjectDetail : System.Web.UI.Page
{
    bool isWebAdmin = false;
    nUser Me;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usr"] != null)
        {
            Me = (nUser)Session["usr"];
            if (!Me.isAdmin)
                Response.Redirect("default.aspx");
            isWebAdmin = nUser.isWebAdmin(Me.uid);
        }
        else
        {
            Response.Redirect("default.aspx");
        } 
        
        if (!IsPostBack)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "init", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(loadpage);", true);
            startDate.Attributes.Add("onclick", "calendar(this)");
            endDate.Attributes.Add("onclick", "calendar(this)");
            Label1.Visible = false;
            Label1.Text = "No records found....";
        }
    }
    protected void search_Click(object sender, EventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.DataSource = getData();
        GridView1.DataBind();
        Label1.Visible = false;
        if (GridView1.Rows.Count <= 0)
        {
            Label1.Visible = true;
        }

    }
    protected void downloadExcel_Click(object sender, EventArgs e)
    {
        genExcelByXML();

    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = getData();
        GridView1.DataBind();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFCD6'");
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");

    }

    private DataTable getData()
    {
        DataTable dt = new DataTable();
        using (SqlDB sqldb = new SqlDB(ConfigurationManager.ConnectionStrings["biconn"].ToString()))
        {
            int start = 0;
            int end = 0;
            if (startDate.Text.Trim() != "")
                start = Convert.ToInt32(DateTime.ParseExact(startDate.Text.Trim(), @"M/d/yyyy", null).ToString("yyyyMMdd"));
            if (endDate.Text.Trim() != "")
                end = Convert.ToInt32(DateTime.ParseExact(endDate.Text.Trim(), @"M/d/yyyy", null).ToString("yyyyMMdd"));


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "[sp_MKTRPT_projectDetail]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@oem", OEM.Text.Trim());
            cmd.Parameters.AddWithValue("@plant", Plant.Text.Trim());
            cmd.Parameters.AddWithValue("@project ", project.Text.Trim());
            cmd.Parameters.AddWithValue("@layer", layer.Text.Trim());
            cmd.Parameters.AddWithValue("@tech", tech.Text.Trim());
            cmd.Parameters.AddWithValue("@surf", surf.Text.Trim());
            cmd.Parameters.AddWithValue("@startDate", start);
            cmd.Parameters.AddWithValue("@endDate", end);
            dt = sqldb.getDataTableWithCmd(ref cmd);
            cmd.Dispose();
        }
        return dt;
    }
    private void genExcelByXML()
    {
        HttpContext context = HttpContext.Current;
        context.Response.Clear();
        context.Response.Charset = "";
        context.Response.AddHeader("content-disposition", "attachment;filename=projectDetail.xls");
        context.Response.ContentType = "application/vnd.ms-excel";
        StreamReader sr = new StreamReader(Context.Server.MapPath("xml/excelTemp.xml"));
        string rptxml = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();
        string content = "<Row>" +
            "<Cell><Data ss:Type=\"String\">OEM</Data></Cell><Cell><Data ss:Type=\"String\">Plant</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Project#</Data></Cell><Cell><Data ss:Type=\"String\">Part#</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Layer</Data></Cell><Cell><Data ss:Type=\"String\">Technical</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Surface</Data></Cell><Cell><Data ss:Type=\"String\">Date</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">ASP</Data></Cell><Cell><Data ss:Type=\"String\">SQFT</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Price</Data></Cell><Cell><Data ss:Type=\"String\">QTY</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">Amount</Data></Cell><Cell><Data ss:Type=\"String\">Period</Data></Cell></Row>";
        string rowxml = "<Row><Cell><Data ss:Type=\"String\">{0}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{1}</Data></Cell><Cell><Data ss:Type=\"String\">{2}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{3}</Data></Cell><Cell><Data ss:Type=\"Number\">{4}</Data></Cell>" +
            "<Cell><Data ss:Type=\"String\">{5}</Data></Cell><Cell><Data ss:Type=\"String\">{6}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{7}</Data></Cell><Cell><Data ss:Type=\"Number\">{8}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{9}</Data></Cell><Cell><Data ss:Type=\"Number\">{10}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{11}</Data></Cell><Cell><Data ss:Type=\"Number\">{12}</Data></Cell>" +
            "<Cell><Data ss:Type=\"Number\">{13}</Data></Cell></Row>";
        DataTable dt = getData();
        if (dt.Rows.Count == 50000)
            content = "<Row>" +
                "<Cell ss:StyleID=\"s71\"><Data ss:Type=\"String\">Warning: Your downloaded result has reached the limit of the number of 50000. It will probably not your expected.</Data></Cell>" +
                "</Row>" + content;

        StringBuilder sb = new StringBuilder();
        sb.Append(content);
        foreach (DataRow row in dt.Rows)
        {
            sb.Append(string.Format(rowxml, row["oem"].ToString().Trim().Replace("&", "&amp;"),
                row["plant"],row["item"].ToString().Trim().Replace("&", "&amp;"),
                row["cus_part_no"].ToString().Trim().Replace("&","&amp;"),
                row["layer"], row["tech"],row["surf"], row["gamDate"], row["asp"], row["sqft"], row["price"], row["qty"], row["amount"], row["iperiod"]));
        }
        dt.Dispose();
        rptxml = rptxml.Replace("<Row />", sb.ToString());
        Response.Write(rptxml);
        Response.End();
    }
}
