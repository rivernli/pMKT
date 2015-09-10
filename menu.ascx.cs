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
using System.Text;
using System.Text.RegularExpressions;

public partial class menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["usr"] != null)
            {
                nUser Me = (nUser)Session["usr"];
                if (!Me.isAdmin)
                {
                    /*
                    oemSync.Enabled =
                    cemLocation.Enabled =
                    memberControl.Enabled =
                    hyp_project.Enabled = false;
                    */
                    li_project.Visible = li_oem_ctrl.Visible =
                        li_ecm_location.Visible = li_member_ctrl.Visible = false;

                }
            }

            string p = Parent.Page.Request.Path.ToLower();//.FilePath.ToLower();
            MatchCollection matchs = Regex.Matches(p, "/(?<key>[a-z0-9]*).aspx");
            if (matchs.Count > 0)
            {
                GroupCollection gc = matchs[0].Groups;
                p = gc["key"].Value;
            }
            switch (p)
            {
                case "hubinvdetail":
                    li_hubdetail.Attributes.Add("class", "selected");
                    break;
                case "oembillingpolicy":
                    li_oem_ctrl.Attributes.Add("class", "selected");
                    break;
                case "usercontrol":
                    li_member_ctrl.Attributes.Add("class", "selected");
                    break;
                case "backlog":
                    li_backlog.Attributes.Add("class", "selected");
                    break;
                case "shipment":
                    li_shipment.Attributes.Add("class", "selected");
                    break;
                case "cemlocation":
                    li_ecm_location.Attributes.Add("class", "selected");
                    break;
                case "projectdetail":
                    li_project.Attributes.Add("class", "selected");
                    break;
            } 

        }

    }
}
