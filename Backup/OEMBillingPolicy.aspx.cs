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
using System.Data.SqlClient;
public partial class OEMBillingPolicy : System.Web.UI.Page
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
            loadOEM();
    }
    private void loadOEM()
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
            cmd.CommandText = "select * from hubInventoryOEMPolicy order by warehouseId,billingPolicy,transit";
            cmd.CommandType = CommandType.Text;
            dt = db.getDataTableWithCmd(ref cmd);
            cmd.Dispose();
        }
        return dt;
    }
    private void updateOEM(string warehouseId, int billing, int transit)
    {
        using (SqlDB db = new SqlDB(ConfigurationManager.ConnectionStrings["dbconn"].ToString()))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "update hubInventoryOEMPolicy set billingpolicy=@billing, transit=@transit where warehouseId=@warehouseId";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@billing", billing);
            cmd.Parameters.AddWithValue("@transit", transit);
            cmd.Parameters.AddWithValue("@warehouseId", warehouseId);
            db.execSqlWithCmd(ref cmd);
            cmd.Dispose();
        }
    }
    protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        //loadOEM();
        if (e.CommandName == "Update")
        {
            int b = 0;
            int s = 0;
            int.TryParse(((TextBox)e.Item.FindControl("BillingPolicy")).Text.Trim(), out b);
            int.TryParse(((TextBox)e.Item.FindControl("transit")).Text.Trim(), out s);
            string warehouseId = ((Label)e.Item.FindControl("warehouseId")).Text.Trim();
            updateOEM(warehouseId, b, s);
        }
    }
    protected void ListView1_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        ListView1.EditIndex = e.NewEditIndex;
        loadOEM();
    }
    protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        ListView1.EditIndex = -1;
        loadOEM();
    }
    protected void ListView1_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {
        ListView1.EditIndex = -1;
        loadOEM();
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
        loadOEM();
    }
    protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem && e.Item.FindControl("bp") != null)
        {
            Label bp = (Label)e.Item.FindControl("bp");
            Label ts = (Label)e.Item.FindControl("ts");
            if (bp.Text == "0")
                bp.ForeColor = System.Drawing.Color.Red;
            if (ts.Text == "0")
                ts.ForeColor = System.Drawing.Color.Red;
        }
    }
}
