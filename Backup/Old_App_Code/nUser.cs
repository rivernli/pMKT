using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
/// <summary>
/// Summary description for nUser
/// </summary>
public class nUser : IDisposable
{
    private string _uid;
    public string uid { get { return _uid; } }
    public string userName;

    public int sysUserId = 0;
    public bool isAdmin = false;
    public bool isActive = false;
    public bool isSale = false;
    public bool isReportViewer = false;
    public int managerId = -1;
    public string domain;

    public string emailAddress;
    public string jobTitle;
    public string fax;
    public string tel;
    public string mobile;
    public string department;


    private bool _isDbUser = false;
    private bool _isLdapUser = false;
    public bool isDBuser { get { return _isDbUser; } }
    public bool isLdapUser { get { return _isLdapUser; } }

    public string msg;

    private static string __conn = ConfigurationManager.ConnectionStrings["biconn"].ToString();
    public nUser(string user_id, string domainName)
    {
        set_nUser(user_id, domainName);
    }
    public nUser(string ud)
    {
        string[] Users;
        Users = ud.Trim().Split('\\');
        if(Users.Length == 2)
            set_nUser(Users[0].ToString().Trim(), Users[1].ToString().Trim());

    }
    private void set_nUser(string user_id, string domainName)
    {
        domain = "asia";
        if (domainName != "")
            domain = domainName;
        LDAP ldap = new LDAP(@"LDAP://DC=" + domain + ",DC=ad,DC=flextronics,DC=com");
        if (ldap.findUser(user_id, domain))
        {
            _isLdapUser = true;
            _uid = ldap.uid;
            using (SqlDB sqldb = new SqlDB(__conn))
            {
                SqlCommand cmd = new SqlCommand("[sp_GAM_UsersGet]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uid", _uid);
                cmd.Parameters.AddWithValue("@domain", domain);
                DataTable dt = sqldb.getDataTableWithCmd(ref cmd);
                cmd.Dispose();
                if (dt.Rows.Count == 1)
                {
                    DataRow row = dt.Rows[0];
                    isActive = (bool)row["isActive"];
                    isAdmin = (bool)row["isAdmin"];
                    if (row["uGroup"].ToString().ToLower() == "admin")
                        isAdmin = true;
                    managerId = (int)row["managerId"];
                    isSale = (bool)row["isSales"];
                    isReportViewer = (bool)row["isReportViewer"];
                    sysUserId = (int)row["sysUserId"];
                    _isDbUser = true;
                }
            }
            userName = ldap.name;
            emailAddress = ldap.email;
            jobTitle = ldap.title;
            fax = ldap.fax;
            tel = ldap.tel;
            department = ldap.department;
        }
        else
        {
            _uid = "";
            msg = "User not found.";
        }
        ldap.Dispose();
    }
    public void Dispose() { }
    public static bool isWebAdmin(string uid)
    {
            return false;

    }
    public static DataTable getDbUser(string uid, string domain)
    {
        DataTable dt = new DataTable();
       
        using (SqlDB sqldb = new SqlDB(__conn))
        {
            SqlCommand cmd = new SqlCommand("sp_GAM_UsersGet");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@domain", domain);
            dt = sqldb.getDataTableWithCmd(ref cmd);
            cmd.Dispose();
        }
        return dt;
    }
    /*
    public bool addDBUser()
    {
        if (_isLdapUser && !_isDbUser)
        {
            using (SqlDB sqldb = new SqlDB(__conn))
            {
                SqlCommand cmd = new SqlCommand("sp_HubInventoryDetail_UserAdd");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.Parameters.AddWithValue("@domain", domain);
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Parameters.AddWithValue("@isActive", isActive);
                cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
                cmd.Parameters.AddWithValue("@emailAddress", emailAddress);
                cmd.Parameters.AddWithValue("@department", department);
                cmd.Parameters.AddWithValue("@jobTitle", jobTitle);
                cmd.Parameters.AddWithValue("@fax", fax);
                cmd.Parameters.AddWithValue("@tel", tel);
                sqldb.execSqlWithCmd(ref cmd);
                cmd.Dispose();
            }

        }
        return true;
    }
    public bool updateDBUser()
    {
        if (_isLdapUser && _isDbUser)
        {
            using (SqlDB sqldb = new SqlDB(__conn))
            {
                SqlCommand cmd = new SqlCommand("sp_HubInventoryDetail_UserUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.Parameters.AddWithValue("@domain", domain);
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Parameters.AddWithValue("@isActive", isActive);
                cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
                sqldb.execSqlWithCmd(ref cmd);
                cmd.Dispose();
            }

        }
        return true;
    }*/
}
