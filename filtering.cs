using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for filtering
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class filtering : System.Web.Services.WebService
{

    public filtering()
    {
    }
    private string urlString(string str)
    {
        return HttpUtility.HtmlEncode(str);
    }
    [WebMethod]
    public String getSelection(string fieldName, string filterValue)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("{rs:[");
        
        using (SqlDB db = new SqlDB(ConfigurationManager.ConnectionStrings["dbconn"].ToString()))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "[sp_HubInventoryDetail_getSelectItems]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@objectField", fieldName);
            cmd.Parameters.AddWithValue("@keyword", filterValue);
            DataTable dt = db.getDataTableWithCmd(ref cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                sb.Append("\"" + urlString(dt.Rows[0][0].ToString()) + "\"");
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    sb.Append(",\"" + urlString(dt.Rows[i][0].ToString()) + "\"");
                }
            }
        }
        sb.Append("]}");
        return sb.ToString();
    }

    [WebMethod]
    public String getProjectSelect(string fieldName, string filterValue)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("{rs:[");

        using (SqlDB db = new SqlDB(ConfigurationManager.ConnectionStrings["biconn"].ToString()))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "[sp_MKTRPT_projectDetail_ajax]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@objectField", fieldName);
            cmd.Parameters.AddWithValue("@keyword", filterValue);
            DataTable dt = db.getDataTableWithCmd(ref cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                sb.Append("\"" + urlString(dt.Rows[0][0].ToString()) + "\"");
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    sb.Append(",\"" + urlString(dt.Rows[i][0].ToString()) + "\"");
                }
            }
        }
        sb.Append("]}");
        return sb.ToString();
    }

}

