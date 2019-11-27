using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TMHSelf
{
    public partial class ChangePass : System.Web.UI.Page
    {
        SQLCalssLib db1 = new SQLCalssLib();
        //string strRefUrl = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString();
        //string strRefUrl = HttpContext.Current.Request.UrlReferrer.ToString();

        //---------------------------- Method ---------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (HttpContext.Current.Session["TMHPasswordAccess"] == null)
            {
                Session["TMHPasswordAccess"] = "";
                Server.Transfer("TMHLogin.aspx");
            }

            if (HttpContext.Current.Session["TMHPasswordAccess"].ToString() != "Yes") Server.Transfer("TMHLogin.aspx");

            if (!IsPostBack)
            {
                PageBody.Attributes.Add("onload", "javascript:form1.userpasswordOld.focus()");
            }

            lblMsg.Text = "Please fill out the form to change your password.";

            useremail.Text = HttpContext.Current.Session["TMHUser"].ToString();
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
        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            string strUser = strSafeSqlLiteral(useremail.Text.ToString(),255);

            string strPassOld = strSafeSqlLiteral(userpasswordOld.Text.ToString(),127);
            string strPassNew = strSafeSqlLiteral(userpasswordNew.Text.ToString(),127);
            string strPassNewC = strSafeSqlLiteral(userpasswordNewC.Text.ToString(),127);

            if (!IsComplexPassword(strPassNew))
            {
                // at least change the message and write to app event log
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-300, Error: Make sure the new password contains numeric, upper and lower case characters and is at least 8 characters long.";
                return;
            }

            if (strPassNew.Length < 8)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-310, Error: New password length is less than 8 characters long, Please try again.";
                return;
            }

            if (strPassNew != strPassNewC)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-320, Error: New password and confirm password do not match, Please try again.";
                return;
            }

            string strIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();
            if (strIPAddress.Length > 20) strIPAddress = strIPAddress.Substring(0, 20);

            SDSheriffUtils.SDSheriffCrypto SDSheriffCrypto1 = new SDSheriffUtils.SDSheriffCrypto();

            string sPassEncOld = SDSheriffCrypto1.Encrypt(strPassOld);
            string sPassDecOld = SDSheriffCrypto1.Decrypt(sPassEncOld).ToString();
            //if (strPass != sPassDec) lblMsg.Text = "Enc/Dec not working";
            //else lblMsg.Text = "Success";

            DataSet ds1 = new DataSet();

            StringBuilder strCmd = new StringBuilder();

            strCmd.Append("SELECT * ");
            strCmd.Append("FROM TMHUsers ");
            strCmd.Append("WHERE [UserName] ='" + strUser + "' AND [PassWord] = '" + sPassEncOld + "' AND STATUS in ('A', 'X')");

            db1.mv_OpenConnection2(strIPAddress);
            ds1 = db1.mds_ExecuteQuery(strCmd.ToString(), "tblUser");
            db1.mv_CloseConnection();
            // check to see if error occurred during database connection and sql execution
            if (ds1 == null)
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-330, Error: Database Error";
                return;
            }

            if (ds1.Tables["tblUser"].Rows.Count == 0)
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "TMHSelf-340, Error: No user account found, or old password bad, Please try again.";
                return;
            }

            string sPassEncNew = SDSheriffCrypto1.Encrypt(strPassNew);
            try
            {
                // change user password with the new password selected by the user
                StringBuilder strCmd2 = new StringBuilder();

                strCmd2.Append("UPDATE TMHUsers SET [PassWord] = " + "'" + sPassEncNew + "', ");
                strCmd2.Append("[LastActivityDate] = GETDATE(), [Status] = 'A', ");
                strCmd2.Append("[IP] = '" + strIPAddress + "' ");
                strCmd2.Append("WHERE [UserName] = '" + strUser + "' AND [PassWord] = '" + sPassEncOld + "'");
                db1.mv_OpenConnection2(strIPAddress);
                db1.mv_InsertOrUpdate(strCmd2.ToString());
                db1.mv_CloseConnection();
                // check to see if error occurred during database connection and sql command execution
            }
            catch (Exception ex)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                string strErr = ex.Message.ToString();
                lblMsg.Text = "TMHSelf-350, Error: Your Password was not changed!";
                return;
            }


            //lblMsg.ForeColor = System.Drawing.Color.Black;
            msgVal.Attributes["class"] = "alert alert-success";
            lblMsg.Text = "Your password has been changed.";
            pnlChangePass.Visible = false;
            pnlindexPage.Visible = true;
        }

        //---------------------------- Method ---------------------------------------
        protected void btnChangePassCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("default.aspx");
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