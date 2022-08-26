using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TMHSelf
{
    public partial class ForgotPass : System.Web.UI.Page
    {
        SQLCalssLib db1 = new SQLCalssLib();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "Please use this form to receive a temporary password.";
            if (!IsPostBack)
            {
                Master.PagebodyAccess.Attributes.Add("onload", "javascript:form1.useremail.focus()");
            }
        }

        protected void btnSendPass_Click(object sender, EventArgs e)
        {

            string strUser = strSafeSqlLiteral(useremail.Text.ToString(),255);
            //string strUser = strSafeSqlLiteral(useremail.Text.ToString(), 255).Replace("_", "");
            string strPIN = strSafeSqlLiteral(userPIN.Text.ToString(), 4);

            if ((strUser == "") || (strPIN.Length != 4))
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "E-Mail and PIN are required!";
                return;
            }

            if ((strUser.Length > 0) && (!Regex.IsMatch(strUser, @"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b")))
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "Invalid User.";
                return;
            }

            string strIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();
            if (strIPAddress.Length > 20) strIPAddress = strIPAddress.Substring(0, 20);

            DataSet ds1 = new DataSet();

            StringBuilder strCmd = new StringBuilder();

            strCmd.Append("SELECT * ");
            strCmd.Append("FROM TMHUsers ");
            strCmd.Append("WHERE [UserName] ='" + strUser + "' AND [PIN] = '" + strPIN + "'");

            db1.mv_OpenConnection2(strIPAddress);
            ds1 = db1.mds_ExecuteQuery(strCmd.ToString(), "tblUser");
            db1.mv_CloseConnection();
            // check to see if error occurred during database connection and sql execution
            if (ds1 == null)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "Your email and PIN do not match or not found!";
                return;
            }

            if (ds1.Tables["tblUser"].Rows.Count == 0)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "No user account found, or PIN number not matched with our file, Please try again.";
                return;
            }

            if (strUser.IndexOf("@") > 0)
            {
                string strNewPass = getRandomPassword();
                SmtpClient sc = new SmtpClient(Properties.Settings.Default.SMTP_Server);
                MailMessage mm = new MailMessage("netwebmaster@sdsheriff.org", strUser);
                mm.Subject = "San Diego Sheriff TMH Self-Registry Account Information.";
                mm.Body = "This email was auto-generated, please do not reply to this email message. ";
                mm.Body = mm.Body + (char)13 + (char)12;
                mm.Body = mm.Body + "Your Take Me Home Self-Registry password has been reset. Your new password is: " + strNewPass + (char)13 + (char)12;
                mm.Body = mm.Body + "This password will allow you to login and then require you to set a new password. ";
                //mm.Body = mm.Body + "Login at: " + Properties.Settings.Default.TMHUrl;
                mm.Body = mm.Body + "Login at: https://apps.sdsheriff.net/TMHSelf/TMHLogin.aspx";
                mm.Body = mm.Body + (char)13 + (char)12;
                mm.Body = mm.Body + "Copy and paste the password from this email into the login form. ";
                mm.Body = mm.Body + "You will then be asked to change your password and you should paste the ";
                mm.Body = mm.Body + "same password from this email in the old password field. ";
                mm.Body = mm.Body + "You can pick a new password and enter it into the new password and confirm fields. ";
                mm.Body = mm.Body + "" + (char)13 + (char)12;
                mm.Body = mm.Body + "The new password you choose cannot repeat a previous password, cannot be any part of your lastname, firstname, userid, ";
                mm.Body = mm.Body + "must be at least 8 characters long, contain at least 1 numeric, 1 uppercase letter ";
                mm.Body = mm.Body + "and 1 lowercase letter. " + (char)13 + (char)12;
                sc.Send(mm);

                SDSheriffUtils.SDSheriffCrypto SDSheriffCrypto1 = new SDSheriffUtils.SDSheriffCrypto();

                string sPassEnc = SDSheriffCrypto1.Encrypt(strNewPass);
                //string sPassDec = SDSheriffCrypto1.Decrypt(sPassEnc).ToString();
                try
                {
                    // reset user password with the random generated password
                    StringBuilder strCmd2 = new StringBuilder();

                    strCmd2.Append("UPDATE TMHUsers SET [PassWord] = " + "'" + sPassEnc + "', ");
                    strCmd2.Append("[LastActivityDate] = GETDATE(), [Status] = 'X', ");
                    strCmd2.Append("[IP] = '" + strIPAddress + "' ");
                    strCmd2.Append("WHERE [UserName] = '" + strUser + "' AND [PIN] = '" + strPIN + "'");
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
                    lblMsg.Text = "Error Occurred, new password not set or emailed to you!";
                    return;
                }
                msgVal.Attributes["class"] = "alert alert-success";
                lblMsg.Text = "Your password has been reset and the new password has been emailed to your email address on file.";
            }
            else
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "The confirmation code does not match our records. Please contact your administrator.";
            }
        }

        //---------------------------- method -----------------------------
        private string getRandomPassword()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            string newPassword = "";
            int x = r.Next(26);
            newPassword = newPassword + Convert.ToChar(Convert.ToInt32('A') + x);
            for (int i = 0; i < 7; i++)
            {
                x = r.Next(26);
                newPassword = newPassword + Convert.ToChar(Convert.ToInt32('a') + x);
            }
            x = r.Next(10);
            newPassword = newPassword + x.ToString();
            return newPassword;
        }

        //---------------------------- Method ---------------------------------------
        protected void btnCancel_Click(object sender, EventArgs e)
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

        //---------------------------- Method ---------------------------------------
        /// <summary>This method removes sql literal characters from an input field.</summary>
        public string strDecodeLiteral(string strInputField)
        {
            if (strInputField == "") return "";

            string strTemp = strInputField;
            strTemp = Server.HtmlDecode(strTemp);
            strTemp = strTemp.Replace("''", "'");
            return strTemp;
        }



    }
}