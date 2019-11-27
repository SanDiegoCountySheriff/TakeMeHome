using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TMHSelf
{
    public partial class MyAccount : System.Web.UI.Page
    {
        SQLCalssLib db1 = new SQLCalssLib();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageBody.Attributes.Add("onload", "javascript:form1.useremail.focus()");
                useremail1.Text = HttpContext.Current.Session["TMHUser"].ToString();
                userpin1.Text = HttpContext.Current.Session["TMHUserPIN"].ToString();
            }

            lblMsg.Text = "Please use this form to update your profile.";
        }


        //---------------------------- Method ---------------------------------------
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            string strOldUser = HttpContext.Current.Session["TMHUser"].ToString();
            string strOldPIN = HttpContext.Current.Session["TMHUserPIN"].ToString();

            string strUserNew = strSafeSqlLiteral(useremail.Text.ToString().Trim(), 255);
            string strpinNew = strSafeSqlLiteral(userpin.Text.ToString().Trim(), 4);

            if ((strUserNew == "") && (strpinNew == ""))
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-400, No updates entered.";
                return;
            }

            if (strpinNew.Length > 0)
            {
                if (!IsValidPIN(strpinNew))
                {
                    //lblMsg.ForeColor = System.Drawing.Color.Red;
                    msgVal.Attributes["class"] = "alert alert-danger";
                    lblMsg.Text = "TMHSelf-410, Error: Invalid PIN, 4 digits please!";
                    return;
                }
            }

            if ((strpinNew.Length > 0) && (strpinNew.Length != 4))
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-420, Error: Bad 4 digit PIN selected, try again!";
                return;
            }

            if ((strUserNew.Length > 0) && (strUserNew.Length < 5))
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-430, Error: Invalid User selected, try again!";
                return;
            }

            if ((strUserNew.Length > 0) && (!Regex.IsMatch(strUserNew, @"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b")))
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-440, Error: Invalid User.";
                return;
            }

            if (strUserNew.Length > 0)
            {
                if (bChkNewUserExists(strUserNew))
                {
                    msgVal.Attributes["class"] = "alert alert-danger";
                    lblMsg.Text = "TMHSelf-450, Error: The email address you entered is not available, please try another one.";
                    return;
                }
            }

            string strIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();
            if (strIPAddress.Length > 20) strIPAddress = strIPAddress.Substring(0, 20);

            try
            {
                // update the user account
                StringBuilder strCmd2 = new StringBuilder();

                strCmd2.Append("UPDATE TMHUsers set [LastActivityDate]=GETDATE(), [IP] = '" + strIPAddress + "' ");
                if (strUserNew != "") strCmd2.Append(",[UserName]='" + strUserNew + "' ");
                if (strpinNew != "") strCmd2.Append(",[PIN]='" + strpinNew + "' ");
                strCmd2.Append("Where [UserName] = '" + strOldUser + "'");
                db1.mv_OpenConnection2(strIPAddress);
                db1.mv_InsertOrUpdate(strCmd2.ToString());
                db1.mv_CloseConnection();

                msgVal.Attributes["class"] = "alert alert-info";
                lblMsg.Text = "Your user account is updated.";

                if (strUserNew != "")
                {
                    HttpContext.Current.Session["TMHUser"] = strUserNew;

                    // Create the forms authetication ticket
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, strUserNew, DateTime.Now
                        , DateTime.Now.AddMinutes(300), false, String.Empty, FormsAuthentication.FormsCookiePath);

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    // Create a cookie and add the encrypted ticket to the cookie as data.
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    // Add the cookie to the outgoing cookies collection.					
                    Response.Cookies.Add(authCookie);

                    if (Request.QueryString["ReturnUrl"] != null)
                    {
                        FormsAuthentication.RedirectFromLoginPage(strUserNew, false);
                        //Response.Redirect("default.aspx");
                    }

                }

                if (strpinNew != "") HttpContext.Current.Session["TMHUserPIN"] = strpinNew;

                Server.Transfer("default.aspx");

            }

            catch (Exception ex)
            {
                string strErr = ex.Message.ToString();
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-460, Error Occurred - User Account not Updated.";
                return;
            }
        }

        protected bool bChkNewUserExists(string strUserNew)
        {
            string strIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();
            if (strIPAddress.Length > 20) strIPAddress = strIPAddress.Substring(0, 20);

            DataSet ds1 = new DataSet();

            StringBuilder strCmd = new StringBuilder();

            strCmd.Append("SELECT * ");
            strCmd.Append("FROM TMHUsers ");
            strCmd.Append("WHERE UserName ='" + strUserNew + "'");

            db1.mv_OpenConnection2(strIPAddress);
            ds1 = db1.mds_ExecuteQuery(strCmd.ToString(), "tblUser");
            db1.mv_CloseConnection();

            if (ds1 == null) return true;
            if (ds1.Tables["tblUser"].Rows.Count != 0) return true;

            return false;
        }

        //---------------------------- Method ---------------------------------------
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("default.aspx");
        }

        //---------------------------- Method ---------------------------------------
        public bool IsValidPIN(string strPIN)
        {
            Regex pinRegex = new Regex(@"(^\d{4}$)");
            if (pinRegex.IsMatch(strPIN)) return true;
            else return false;
        }

        //---------------------------- Method ---------------------------------------
        /// <summary>This method removes sql literal characters from an input field.</summary>
        protected string strSafeSqlLiteral(string strInputField, int iFieldLength)
        {
            if (strInputField == "") return "";

            string strTemp = strInputField.Trim();
            strTemp = Server.HtmlEncode(strTemp);
            strTemp = strTemp.Replace("'", "''");
            strTemp = strTemp.Replace("*", "");
            strTemp = strTemp.Replace("%", "");

            if (strTemp.Length > iFieldLength) strTemp = strTemp.Substring(0, iFieldLength);
            return strTemp;
        }

    }
}