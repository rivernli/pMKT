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

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["usr"] != null)
            {
                nUser nx = (nUser)Session["usr"];
                nx.Dispose();
                Session["usr"] = null;
            }
        }
        FormsAuthentication.SignOut();
        //Response.Redirect("default.aspx");
        /*replaced below section*/
        pMKT.sso.SSOAuthen ssoauth = new pMKT.sso.SSOAuthen();
        string lo = ssoauth.getSSOLogoutPage();
        ssoauth.Dispose();
        Response.Redirect(lo);

    }
}
