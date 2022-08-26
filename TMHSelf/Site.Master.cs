using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace TMHSelf
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public HtmlGenericControl PagebodyAccess
        {
            get
            {
                return PageBody;
            }
            set
            {
                PageBody = value;
            }
        }

    }
}