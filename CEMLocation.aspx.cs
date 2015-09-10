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
using System.Data.SqlClient;

public partial class CEMLocation : System.Web.UI.Page
{
    nUser Me;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usr"] != null)
        {
            Me = (nUser)Session["usr"];
            if (!Me.isAdmin)
                Response.Redirect("default.aspx");
        }
        else
        {
            Response.Redirect("default.aspx");
        }
        if (!IsPostBack)
            loadCUS();
    }
    private void loadCUS()
    {
        ListView1.DataSource = loadDT();
        ListView1.DataBind();
    }

    private DataTable loadDT()
    {
        DataTable dt = new DataTable();
        using (SqlDB db = new SqlDB(ConfigurationManager.ConnectionStrings["dbconn"].ToString()))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from hubInventoryCEMLocation order by customer_code";
            cmd.CommandType = CommandType.Text;
            dt = db.getDataTableWithCmd(ref cmd);
            cmd.Dispose();
        }
        return dt;
    }
    private void updateCUS(string customer_code, string location)
    {
        using (SqlDB db = new SqlDB(ConfigurationManager.ConnectionStrings["dbconn"].ToString()))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "update hubInventoryCEMLocation set location=@location where customer_code=@customer_code";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@customer_code", customer_code);
            db.execSqlWithCmd(ref cmd);
            cmd.Dispose();
        }
    }
    protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        //loadOEM();
        if (e.CommandName == "Update")
        {
            string customer_code = ((Label)e.Item.FindControl("customer_code")).Text.Trim();
            string location = ((TextBox)e.Item.FindControl("location")).Text.Trim();
            updateCUS(customer_code, location);
        }
    }
    protected void ListView1_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        ListView1.EditIndex = e.NewEditIndex;
        loadCUS();
    }
    protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        ListView1.EditIndex = -1;
        loadCUS();
    }
    protected void ListView1_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {
        ListView1.EditIndex = -1;
        loadCUS();
    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        using (SqlDB db = new SqlDB(ConfigurationManager.ConnectionStrings["dbconn"].ToString()))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_HubInv_SyncPolicy";
            cmd.CommandType = CommandType.StoredProcedure;
            db.execSqlWithCmd(ref cmd);
            cmd.Dispose();
        }
        loadCUS();
    }
    protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
    }
}
