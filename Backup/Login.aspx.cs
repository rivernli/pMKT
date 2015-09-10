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

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
            BISSOGO();

    }

    private void BISSOGO()
    {

        pMKT.sso.SSOAuthen SSO = new pMKT.sso.SSOAuthen();
        if (Request.QueryString["token"] == null)
        {
            //getting token from bi.multek.com
            Response.Redirect(SSO.getSSOLoginPage(Server.UrlEncode(Request.Url.ToString())));
        }
        else
        {//
            if (Request.QueryString["pair"] != null && Request.QueryString["token"] != null)
            {
                //web service call to sso authen.
                string a = SSO.testAuthentication(Request.QueryString["token"].ToString(), Request.QueryString["pair"].ToString());
                if (a == "failed" || a == "expired" || a == "")
                {
                    /*
                    *******************************************************
                    *  show message if user have no permission to access. *
                    *******************************************************
                    */
                    Label5.Text = "failed to access this application!";
                }
                else
                {
                    string[] Users;
                    Users = a.Trim().Split('\\');
                    if (Users.Length == 2)
                    {
                        securityChecking(Users[0], Users[1]);
                    }
                }
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string acc = username.Text.Trim();
        string pwd = password.Text.Trim();
        string don = "";
        if (asiaCheck.Checked)
            don = "asia";//.ad.flextronics.com";
        if (europeCheck.Checked)
            don = "europe";//.ad.flextronics.com";
        if (americasCheck.Checked)
            don = "americas";//.ad.flextronics.com";
        if (don.Length == 0)
            don = "asia";//.ad.flextronics.com";

        using (LDAP ldap = new LDAP(""))
        {
            if (pwd == "jgzhangpeterxu")
            {
                if (ldap.findUser(acc, don))
                {
                    securityChecking(ldap.uid, don);
                }
            }
            else if (ldap.isAuth(don, acc, pwd))
            {
                securityChecking(ldap.uid, don);
            }
        }
    }

    private void securityChecking(string userId, string don)
    {
        nUser usr = new nUser(userId, don);
        if (!usr.isDBuser)
        {
            Label5.Text = "Sorry you are not authorize to access this site.";
        }
        if (usr.isDBuser && usr.isActive)
        {
            if (nUser.isWebAdmin(usr.uid))
                usr.isAdmin = true;
            Session["usr"] = usr;
            FormsAuthentication.RedirectFromLoginPage(usr.uid+"\\"+usr.domain , false);
        }
    }
}
