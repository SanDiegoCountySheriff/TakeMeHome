using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TMHSelf
{
    public partial class TMHLogin : System.Web.UI.Page
    {
        SQLCalssLib db1 = new SQLCalssLib();

        protected void Page_Load(object sender, EventArgs e)
        {
           HttpCookie LoginCookie = Context.Request.Cookies["TMHUserId"];

            if (!IsPostBack)
            {
                
                if (LoginCookie != null)
                {
                    useremail.Text = LoginCookie["UserName"].ToString();
                    PageBody.Attributes.Add("onload", "javascript:form1.userpassword.focus()");
                }
                
                else PageBody.Attributes.Add("onload", "javascript:form1.useremail.focus()");

                lblMsg.Text = "";
                HttpContext.Current.Session["TMHPasswordAccess"] = "";
                HttpContext.Current.Session["TMHUser"] = "";
                HttpContext.Current.Session["TMHUserId"] = "";
                HttpContext.Current.Session["TMHUserPIN"] = "";
                lblMsg.Text = "Your session timeout is: " + HttpContext.Current.Session.Timeout.ToString() + " minutes.";
                //default session timeout is 180 minutes, 3 hours
                //HttpContext.Current.Session.Timeout = 240;
                //lblMsg.Text = "User Identity: " + User.Identity.Name.ToString();

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            string strUser = strSafeSqlLiteral(useremail.Text.ToString(),255);
            string strPass = strSafeSqlLiteral(userpassword.Text.ToString(), 127);

            if ((strUser.Length > 0) && (!Regex.IsMatch(strUser, @"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b")))
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-100, Invalid User.";
                return;
            }

            if ((strPass.Length < 8) || (strUser.Length < 5))
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-110, No user account found, or bad password.";
                return;
            }

            string strIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();
            if (strIPAddress.Length > 20) strIPAddress = strIPAddress.Substring(0, 20);

            SDSheriffUtils.SDSheriffCrypto SDSheriffCrypto1 = new SDSheriffUtils.SDSheriffCrypto();

            string sPassEnc = SDSheriffCrypto1.Encrypt(strPass);
            string sPassDec = SDSheriffCrypto1.Decrypt(sPassEnc).ToString();
            //if (strPass != sPassDec) lblMsg.Text = "Enc/Dec not working";
            //else lblMsg.Text = "Success";

            DataSet ds1 = new DataSet();

            StringBuilder strCmd = new StringBuilder();

            strCmd.Append("SELECT * ");
            strCmd.Append("FROM TMHUsers ");
            strCmd.Append("WHERE UserName ='" + strUser + "' AND PassWord = '" + sPassEnc + "' AND STATUS in ('A', 'X')");

            db1.mv_OpenConnection2(strIPAddress);
            ds1 = db1.mds_ExecuteQuery(strCmd.ToString(), "tblUser");
            db1.mv_CloseConnection();
            // check to see if error occurred during database connection and sql cmd execution
            if (ds1 == null)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-120, Error Occurred - (1) Database Error";
                return;
            }

            if (ds1.Tables["tblUser"].Rows.Count == 0)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-130, Bad user account or password!";
                return;
            }

            if (ds1.Tables["tblUser"].Rows.Count > 1)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-140, Multiple user accounts found - Error.";
                return;
            }

            HttpContext.Current.Session["TMHPasswordAccess"] = "Yes";
            HttpContext.Current.Session["TMHUser"] = strUser;

            // need internal user id saved in the session variable for inserting new TMH cases for the user
            HttpContext.Current.Session["TMHUserId"] = ds1.Tables["tblUser"].Rows[0]["UserId"].ToString().Trim();
            HttpContext.Current.Session["TMHUserPIN"] = ds1.Tables["tblUser"].Rows[0]["PIN"].ToString().Trim();

            try
            {
                // reset user IP address with the current IP address
                StringBuilder strCmd2 = new StringBuilder();

                strCmd2.Append("UPDATE TMHUsers SET [IP] = '" + strIPAddress + "', ");
                strCmd2.Append("[LastActivityDate] = GETDATE() ");
                strCmd2.Append("WHERE [UserName] = '" + strUser + "'");
                db1.mv_OpenConnection2(strIPAddress);
                db1.mv_InsertOrUpdate(strCmd2.ToString());
                db1.mv_CloseConnection();
                // check to see if error occurred during database connection and sql command execution
            }
            catch (Exception ex)
            {
                string strErr = ex.Message.ToString();
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-150, Login Error, Take Me Home is not available at this time, Please try later.";
                return;
            }

            string strUserStatus = ds1.Tables["tblUser"].Rows[0]["Status"].ToString().Trim();
            
            if (userremember.Checked)
            {
                HttpCookie LoginCookie = new HttpCookie("TMHUserId");
                LoginCookie.Values.Add("UserName", useremail.Text.ToString().Trim());
                LoginCookie.Expires = DateTime.Now.AddYears(100);
                Response.Cookies.Add(LoginCookie);
            }

            // Create the forms authetication ticket
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,useremail.Text,DateTime.Now
                ,DateTime.Now.AddMinutes(300),false,String.Empty,FormsAuthentication.FormsCookiePath);
            
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            // Create a cookie and add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName,encryptedTicket);

            // Add the cookie to the outgoing cookies collection.					
            Response.Cookies.Add(authCookie);
            
            if (Request.QueryString["ReturnUrl"] != null)
            {
                FormsAuthentication.RedirectFromLoginPage(useremail.Text, false);
                //Response.Redirect("default.aspx");
            }
            
            if (strUserStatus == "X") Server.Transfer("ChangePass.aspx");
            else Server.Transfer("default.aspx");
        }

        protected void LoginReset(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            useremail.Text = "";
            userpassword.Text = "";
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