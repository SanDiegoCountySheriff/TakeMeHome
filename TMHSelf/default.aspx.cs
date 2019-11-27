using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TMHSelf
{
    public partial class _default : System.Web.UI.Page
    {
        SQLCalssLib db1 = new SQLCalssLib();

        protected void Page_Load(object sender, EventArgs e)
        {
            //lblMsg.Text = User.Identity.Name.ToString();

            if (!IsPostBack)
            {
                //PageBody.Attributes.Add("onload", "javascript:form2.txtLastName.focus()");
            }

            if (HttpContext.Current.Session["TMHPasswordAccess"] == null)
            {
                HttpContext.Current.Session["TMHPasswordAccess"] = "";
                Server.Transfer("TMHLogin.aspx");
            }

            if (HttpContext.Current.Session["TMHPasswordAccess"].ToString() != "Yes") Server.Transfer("TMHLogin.aspx");
            string strUser = HttpContext.Current.Session["TMHUser"].ToString().Trim();
            lblUser.Text = strUser;
            lblCurrDtTm.Text = DateTime.Now.ToString();
            SearchRecords();
        }
         
        //---------------------------- Method ---------------------------------------
        /// <summary>This method searches the DB for records matching the search criteria.</summary>
        protected void SearchRecords()
        {
            string strUser = HttpContext.Current.Session["TMHUser"].ToString().Trim();

            string strIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();
            if (strIPAddress.Length > 20) strIPAddress = strIPAddress.Substring(0, 20);

            //strMsg = "";

            DataSet ds0 = new DataSet();

            StringBuilder strCmd0 = new StringBuilder();

            strCmd0.Append("SELECT UserName ");
            strCmd0.Append("FROM [dbo].[TMHUserExceptions] ");
            strCmd0.Append("WHERE rtrim(ltrim(UserName)) ='" + strUser + "'");

            db1.mv_OpenConnection2(strIPAddress);
            ds0 = db1.mds_ExecuteQuery(strCmd0.ToString(), "tblUserExceptions");
            db1.mv_CloseConnection();

            int iRecordLimit = 4;

            string strMaxRecords = System.Configuration.ConfigurationManager.AppSettings["MaxRecords"].ToString().Trim();
            int iMaxRecords = Int32.Parse(strMaxRecords);

            // if user is in the exception list allow more entries defined in the web.config
            if (ds0 != null)
            {
                if (ds0.Tables["tblUserExceptions"].Rows.Count > 0) iRecordLimit = iMaxRecords;

            }




            DataSet ds1 = new DataSet();

            StringBuilder strCmd = new StringBuilder();

            strCmd.Append("SELECT A.UserId,A.Recid, B.UserName, B.Status, ");
            strCmd.Append("C.txtLastName, C.txtFirstName,C.txtMiddleName, C.txtDOB,C.ddlRace,C.ddlSex,C.ddlHeight,");
            strCmd.Append("C.txtWeight,C.ddlEye,C.ddlHair,C.SubmittedDateTime,C.id, C.FileId ");
            strCmd.Append("FROM [dbo].[TMHUserRecs] A ");
		    strCmd.Append("left outer join [dbo].[TMHUsers] B on a.UserId = b.UserId ");
		    strCmd.Append("left outer join [dbo].[TMHRec] C on a.Recid = c.id ");
            strCmd.Append("WHERE B.UserName ='" + strUser + "' and C.ddlApproved <> 'D' ");
            strCmd.Append("order by C.txtLastName, C.txtFirstName");

            //strCmd.Append("SELECT A.*, B.*, C.* ");
            //strCmd.Append("FROM TMHUserRecs A outer join TMHUser B on a.UserId = b.UserId outer join TMHRec C on a.id = c.id ");
            //strCmd.Append("WHERE B.UserName ='" + strUser + "' and C.ddlApproved <> 'D' order by C.txtLastName, C.txtFirstName");

            db1.mv_OpenConnection2(strIPAddress);
            ds1 = db1.mds_ExecuteQuery(strCmd.ToString(), "tblRecords");
            db1.mv_CloseConnection();
            // check to see if error occurred during database connection and sql execution
            if (ds1 == null)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "You have not registered anyone yet, you are allowed to register " + iRecordLimit + " persons!";
                return;
            }

            if (ds1.Tables["tblRecords"].Rows.Count == 0)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "You have not registered anyone yet.";
                pnlRecList.Visible = false;
                return;
            }

            lblLimit.Text = iRecordLimit.ToString();

            if (ds1.Tables["tblRecords"].Rows.Count > 0)
            {
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                if (ds1.Tables["tblRecords"].Rows.Count > 1)
                    lblRegCount.Text = ds1.Tables["tblRecords"].Rows.Count.ToString() + " persons ";
                else
                    lblRegCount.Text = ds1.Tables["tblRecords"].Rows.Count.ToString() + " person ";
            }

            if (ds1.Tables["tblRecords"].Rows.Count > iRecordLimit-1)
            {
                urlRegister.Visible = false;
            }

            string strFileId="", strData = "", strHeightDesc="", strEyeDesc="", strHairDesc="", strSexDesc="", strRaceDesc="";

            DataSet1TableAdapters.TMH_CodesTableAdapter taTMHDesc = new DataSet1TableAdapters.TMH_CodesTableAdapter();

            for (int i = 0; i < ds1.Tables["tblRecords"].Rows.Count; i++ )
            {
                strEyeDesc = ds1.Tables["tblRecords"].Rows[i]["ddlEye"].ToString();
                if (strEyeDesc.Length > 0) strEyeDesc = taTMHDesc.GetDescription("EYE", strEyeDesc);

                strHeightDesc = ds1.Tables["tblRecords"].Rows[i]["ddlHeight"].ToString();
                if (strHeightDesc.Length > 0) strHeightDesc = taTMHDesc.GetDescription("HEIGHT", strHeightDesc);

                strHairDesc = ds1.Tables["tblRecords"].Rows[i]["ddlHair"].ToString();
                if (strHairDesc.Length > 0) strHairDesc = taTMHDesc.GetDescription("HAIR", strHairDesc);

                strSexDesc = ds1.Tables["tblRecords"].Rows[i]["ddlSex"].ToString();
                if (strSexDesc.Length > 0) strSexDesc = taTMHDesc.GetDescription("SEX", strSexDesc);

                strRaceDesc = ds1.Tables["tblRecords"].Rows[i]["ddlRace"].ToString();
                if (strRaceDesc.Length > 0) strRaceDesc = taTMHDesc.GetDescription("RACE", strRaceDesc);

                DateTime dob = Convert.ToDateTime(ds1.Tables["tblRecords"].Rows[i]["txtDOB"]);

                strFileId = ds1.Tables["tblRecords"].Rows[i]["FileId"].ToString();
                strData += "<div class='col-md-6'>";
                strData +=      "<div class='panel'>";
                strData +=          "<h3>";
                strData +=             "<span>"+ ds1.Tables["tblRecords"].Rows[i]["txtFirstName"].ToString() + "</span> ";
                strData +=              "<span>" + ds1.Tables["tblRecords"].Rows[i]["txtMiddleName"].ToString() + "</span> ";
                strData +=              "<span>" + ds1.Tables["tblRecords"].Rows[i]["txtLastName"].ToString() + "</span>";
                strData +=          "</h3>";
                strData +=          "<div class='row'>";
                strData +=              "<div class='col-sm-4'>";
                strData +=                  "<span id=\"lblImage\"><img id=\"image\" class=\"img-responsive img-thumbnail\" alt=\"Responsive image\" src=TMHPhoto.aspx?Data=" + strFileId + "></span>";
                strData +=              "</div>";
                strData +=              "<div class='col-sm-8 tmhdetails'>";     
                strData +=                  "<label>DoB:</label><span> " + dob.ToString("MM/dd/yyyy") + "</span><br/>";
                strData +=                  "<label>Race:</label><span> " + strRaceDesc + "</span><br/>";
                strData +=                  "<label>Sex:</label><span> " + strSexDesc + "</span><br/>";
                strData +=                  "<label>Height:</label><span> " + strHeightDesc + "</span><br/>";
                strData +=                  "<label>Weight:</label><span> " + ds1.Tables["tblRecords"].Rows[i]["txtWeight"].ToString() + " lbs</span><br/>";
                strData +=                  "<label>Eye:</label><span> " + strEyeDesc + "</span><br/>";
                strData +=                  "<label>Hair:</label><span> " + strHairDesc + "</span><br/>";
                strData +=                  "<label>Enrolled:</label><span> " + ds1.Tables["tblRecords"].Rows[i]["SubmittedDateTime"].ToString() + "</span><br/>";
                strData +=              "</div>";
                strData +=          "</div>";
                strData +=          "<a id='urlEdt' name='urlEdt' runat='server' href='Register.aspx?Operation=Edt&id=" + ds1.Tables["tblRecords"].Rows[i]["id"].ToString() + "&FileId=" + ds1.Tables["tblRecords"].Rows[i]["FileId"].ToString() + "' class='btn btn-primary'>Edit</a>";
                strData +=          "<a id='urlDel' name='urlDel' runat='server' href='Register.aspx?Operation=Del&id=" + ds1.Tables["tblRecords"].Rows[i]["id"].ToString() + "&FileId=" + ds1.Tables["tblRecords"].Rows[i]["FileId"].ToString() + "' class='btn btn-primary' OnClick=\"javascript:return confirm('Are you sure you want to delete this person from the TMH system?');\">Delete</a>";
                strData +=      "</div>";
                strData += "</div>";
            }

            lblRecords.Text = strData;
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