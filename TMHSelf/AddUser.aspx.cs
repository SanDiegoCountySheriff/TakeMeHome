using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TMHSelf
{
    public partial class index : System.Web.UI.Page
    {
        SQLCalssLib db1 = new SQLCalssLib();

        //---------------------------- Method ---------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageBody.Attributes.Add("onload", "javascript:form1.useremail.focus()");
            }

            lblMsg.Text = "Please fill out the form to create a new account.";
        }

        //---------------------------- Method ---------------------------------------
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            string strUser = strSafeSqlLiteral(useremail.Text.ToString().Trim(),255);
            string strpin = strSafeSqlLiteral(userpin.Text.ToString().Trim(),4);
            string strPass = strSafeSqlLiteral(userpassword.Text.ToString().Trim(),127);
            string strPassC = strSafeSqlLiteral(userpasswordC.Text.ToString().Trim(),127);

            if (!IsValidPIN(strpin))
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-200, Error: Invalid PIN, 4 digits please!";
                return;
            }

            if (!IsComplexPassword(strPass))
            {
                // at least change the message and write to app event log
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-210, Error: Make sure the new password contains numeric, upper and lower case characters and is at least 8 characters long.";
                return;
            }


            if ((strPass != strPassC) || (strPass == "") || (strPass.Length < 8) || (strpin.Length != 4) || (strUser.Length < 5))
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-220, Error: Required fields are missing, or Password and Password Confirmation do not match, or bad PIN, try again!";
                return;
            }

            if ((strUser.Length > 0) && (!Regex.IsMatch(strUser, @"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b")))
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-230, Error: Invalid User.";
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
            strCmd.Append("WHERE UserName ='" + strUser + "'");

            db1.mv_OpenConnection2(strIPAddress);
            ds1 = db1.mds_ExecuteQuery(strCmd.ToString(), "tblUser");
            db1.mv_CloseConnection();
            // check to see if error occurred during database connection and sql command execution
            if (ds1 == null)
            {
                lblMsg.Text = "TMHSelf-240, Error: Error Occurred - (1) Database Error";
                return;
            }

            if (ds1.Tables["tblUser"].Rows.Count != 0)
            {
                lblMsg.Text = "TMHSelf-250, Error: A user account already exist for the email address you entered.";
                return;
            }

            else
            {
                try
                {
                    // register new user account
                    StringBuilder strCmd2 = new StringBuilder();

                    strCmd2.Append("INSERT INTO TMHUsers ([UserName],[PassWord],[CreatedWhen],[LastActivityDate],[Status],[PIN],[IP])");
                    strCmd2.Append("VALUES ('" + strUser + "','" + sPassEnc + "', GETDATE(), GETDATE(),'A','" + strpin + "','" + strIPAddress + "')");
                    db1.mv_OpenConnection2(strIPAddress);
                    db1.mv_InsertOrUpdate(strCmd2.ToString());
                    db1.mv_CloseConnection();
                    // check to see if error occurred during database connection and sql command execution
                }
                catch (Exception ex)
                {
                    string strErr = ex.Message.ToString();
                    lblMsg.Text = "TMHSelf-260, Error: Error Occurred - (1) User Account not Created.";
                    return;
                }
            }

            msgVal.Attributes["class"] = "alert alert-info";
            lblMsg.Text = "Your user account has been created, and you are logged in.";
            pnlAddUser.Visible = false;
            pnlindexPage.Visible = true;

            HttpContext.Current.Session["TMHPasswordAccess"] = "Yes";
            HttpContext.Current.Session["TMHUser"] = strUser;
            HttpContext.Current.Session["TMHUserPIN"] = strpin;

            // need internal user id saved in the session variable for inserting new TMH cases for the user
            HttpContext.Current.Session["TMHUserId"] = strGetUserId(strUser);

            // Create the forms authetication ticket
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, strUser, DateTime.Now
                , DateTime.Now.AddMinutes(300), false, String.Empty, FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            // Create a cookie and add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            // Add the cookie to the outgoing cookies collection.					
            Response.Cookies.Add(authCookie);

            if (Request.QueryString["ReturnUrl"] != null)
            {
                FormsAuthentication.RedirectFromLoginPage(strUser, false);
                //Response.Redirect("default.aspx");
            }
        }

        protected string strGetUserId(string strUser)
        {
            string strIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();
            if (strIPAddress.Length > 20) strIPAddress = strIPAddress.Substring(0, 20);

            DataSet ds2 = new DataSet();

            StringBuilder strCmd2 = new StringBuilder();

            strCmd2.Append("SELECT UserId ");
            strCmd2.Append("FROM TMHUsers ");
            strCmd2.Append("WHERE UserName ='" + strUser + "'");

            db1.mv_OpenConnection2(strIPAddress);
            ds2 = db1.mds_ExecuteQuery(strCmd2.ToString(), "tblUser2");
            db1.mv_CloseConnection();
            // check to see if error occurred during database connection and sql command execution
            if (ds2 == null)
            {
                lblMsg.Text = "Error Occurred - (1) Database Error";
                return "";
            }

            if (ds2.Tables["tblUser2"].Rows.Count == 0)
            {
                lblMsg.Text = "User account not found.";
                return "";
            }

            return ds2.Tables["tblUser2"].Rows[0]["UserId"].ToString().Trim();

        }

        //---------------------------- Method ---------------------------------------
        protected void btnAddUserCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("default.aspx");
        }

        //---------------------------- Method ---------------------------------------
        /// <summary>
        /// Checks that password has a numeric, upper case, lower case and is between 8 and 99 characters long
        /// </summary>
        /// <param name="password"></param>
        /// <returns>boolean</returns>
        public bool IsComplexPassword(string password)
        {
            Regex passwordRegex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,99}$");
            if (passwordRegex.IsMatch(password)) return true;
            else return false;
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