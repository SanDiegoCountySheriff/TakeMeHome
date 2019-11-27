using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TMHSelf
{
    public partial class signout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session["TMHPasswordAccess"] = null;
            HttpContext.Current.Session["TMHUser"] = null;
            HttpContext.Current.Session["TMHUserId"] = null;
            HttpContext.Current.Session["PhotoID"] = null;
            HttpContext.Current.Session["SelectedID"] = null;
            HttpContext.Current.Session["TMHUserPIN"] = null;
            HttpContext.Current.Session["NewRecord"] = null;

            Server.Transfer("default.aspx");
        }
    }
}