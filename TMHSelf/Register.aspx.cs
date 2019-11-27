using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TMHSelf
{
    public partial class Register : System.Web.UI.Page
    {
        //============================ Properties ====================================
        DataSet1.TMHRecDataTable TMHRecDT = null;

        bool bNewRecord; // sets to true if user clicked on Add New Record
        string strMsg = ""; // system wide message displayed in the TMH messages box
        string strDOB; // Person's Date of Birth
        int iPersonAge = 0; // Person's Age, Computed from DOB
        const string js = "TADDScript.js"; // java script code that handles the type ahead lookup in lists

        //---------------------------- Method ---------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            form2.DefaultButton = btnSubmit1.UniqueID;

            lblCurrDtTm.Text = DateTime.Now.ToString();

            if (HttpContext.Current.Session["TMHPasswordAccess"] == null)
            {
                HttpContext.Current.Session["TMHPasswordAccess"] = "";
                HttpContext.Current.Server.Transfer("TMHLogin.aspx");
            }

            if (HttpContext.Current.Session["TMHPasswordAccess"].ToString() != "Yes") Server.Transfer("TMHLogin.aspx");
            string strUser = HttpContext.Current.Session["TMHUser"].ToString().Trim();
            lblUser.Text = strUser;

            if (!IsPostBack)
            {
                PageBody.Attributes.Add("onload", "javascript:form2.txtLastName.focus()");
                vPopulateAllLists(); // populate all of the drop down list boxes on all tabs from DB table TMH_Codes
                vSetTypeAheadAttrib(); // set the type ahead lookup attribute for all of the lists


                string strTask = Request["Operation"].ToString();
                string strID = "";
                if (Request["id"] != null) strID = Request["id"].ToString();
                string strFileId = "";
                if (Request["FileId"] != null) strFileId = Request["FileId"].ToString();
                if (strTask == "Edt")
                {
                    HttpContext.Current.Session["NewRecord"] = false;
                    Guid gid = new Guid(strID);
                    Guid gPhotoGuid = new Guid(strFileId);
                    HttpContext.Current.Session["PhotoID"] = gPhotoGuid;
                    HttpContext.Current.Session["SelectedID"] = gid;

                    EdtRecord(gid);
                    lblMsg.Text = "Your session timeout is: " + HttpContext.Current.Session.Timeout.ToString() + " minutes.";
  
                }

                else if (strTask == "Del")
                {
                    Guid gid = new Guid(strID);
                    Guid gPhotoGuid = new Guid(strFileId);
                    DelRecord(gid, gPhotoGuid);
                }

                else
                {
                    AddRecord();
                    lblMsg.Text = "Your session timeout is: " + HttpContext.Current.Session.Timeout.ToString() + " minutes.";
                }

            }
        }

        //---------------------------- Method ---------------------------------------
        protected void AddRecord()
        {
            // this method clears all the input fields on input screen, grabs a new guid id for the record
            // pre populates a few fields with default values
            // and sends the user back to the input screens.

            ClearFields("ADDNEW");
            //ResetFields(TabContainer1.Controls);
            TMHRecDT = null;

            // pre populate some fields

            ddlState.SelectedValue = "CA";
            ddlCounty.SelectedValue = "SAN DIEGO";
            //ddlApproved.SelectedValue = "N";
            //txtEnrollDate.Text = DateTime.Now.ToString();

            bNewRecord = true;
            HttpContext.Current.Session["NewRecord"] = true;
            HttpContext.Current.Session["PhotoID"] = Guid.Empty;
            Guid gid = System.Guid.NewGuid();
            HttpContext.Current.Session["SelectedID"] = gid;
            //lblMsg.Text = "Please provide all the required fields.";
        }

        //---------------------------- Method ---------------------------------------
        protected void DelRecord(Guid gid, Guid gPhotoGuid)
        {
            pnlMain.Visible = false;
            pnlRecordDeleted.Visible = true;

            // delete the record, mark the ddlApproved field with 'D', keep the record in the table
            DataSet1TableAdapters.TMHRecTableAdapter taTMHRec = new DataSet1TableAdapters.TMHRecTableAdapter();
            // see if the record is in our database
            DataSet1.TMHRecDataTable TMHRecDT = taTMHRec.GetDataBy(gid);

            if (TMHRecDT.Rows.Count < 1)
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "Error - Could not retrieve the record from TMH database, an Error Occurred.";
                return;
            }

            TMHRecDT[0].ddlApproved = "D";
            TMHRecDT[0].txtEnrollDate = DateTime.Now;
            string strUserID = "TMHSelf"; // User.Identity.Name.ToString().Trim();
            TMHRecDT[0].txtUserID = strUserID;

            bool beMugDel = true;
            string strDTSubmitted = TMHRecDT[0].SubmittedDateTime.ToString().Trim();
            if (strDTSubmitted != "1/1/1900 12:00:00 AM")
            {
                // record was sent to SDLaw eMug TMH
                beMugDel = false;
                // submit the delete request to eMug

                string[] strFieldTags = new string[] {
                    "SAN_DIEGO_APPROVED", //0
                    "SAN_DIEGO_TMH_OPERATOR_ID",
                    "SAN_DIEGO_TMH_ENROLLMENT_DATETIME"
                };

                string[] strFieldVals = new string[3];
                for (int i = 0; i < 3; i++) { strFieldVals[i] = ""; }

                strFieldVals[0] = "D";
                strFieldVals[1] = strUserID;
                strFieldVals[2] = strDTSubmitted;

                // consume the web service provided by DataWorks at https://vemugiis003.sdlaw.us/takemehome/takemehome.asmx
                ws_takemehome.TakeMeHome ws1 = new ws_takemehome.TakeMeHome();
                string strReturnMsg = "";
                // submit the record first, returns 0 - success, 1 - Fail
                int iResult1 = ws1.SaveRecord(gid.ToString(), gPhotoGuid.ToString(), strFieldTags, strFieldVals, ref strReturnMsg);
                if (iResult1 == 1)
                {
                    msgVal.Attributes["class"] = "alert alert-danger";
                    strMsg = strMsg + "<li>Error - The Delete record was not submitted to the Sheriff’s Department, an Error Occurred.</li>";
                    strMsg = strMsg + "<li>The error message from the Sheriff’s Department: " + strReturnMsg + "</li>";
                    /*
                    for (int i = 0; i < 3; i++)
                    {
                         strMsg += "<li>" + strFieldTags[i] + " = " + strFieldVals[i] + "</li>";
                    }
                    */
                    msgVal.Attributes["class"] = "alert alert-danger";
                    lblMsg.Text = strMsg;
                }
                else beMugDel = true;
            }

            if (beMugDel)
            {
                //if SDLaw eMug record deleted, then delete local record in internet SQL database
                taTMHRec.Update(TMHRecDT);
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "Record Submitted to TMH for Deletion!";
            }

            taTMHRec.Connection.Close();
            taTMHRec = null;

        }

        //---------------------------- Method ---------------------------------------
        // The user wants to view/edit detail for one record.
        // we want to grab all of the data from the database and populate the fields
        // with data and present it to the user for view or edit
        protected void EdtRecord(Guid gid)
        {
            ClearFields("");
            //ResetFields(UpdatePanel1.Controls);

            bNewRecord = false;
            HttpContext.Current.Session["NewRecord"] = false;

            HttpContext.Current.Session["SelectedID"] = gid;
            DataSet1.TMHRecDataTable dtResults = new DataSet1.TMHRecDataTable();
            DataSet1TableAdapters.TMHRecTableAdapter taTMHRec = new DataSet1TableAdapters.TMHRecTableAdapter();

            // get the record's data from the DB table and populate all fields into data table dtResults
            dtResults = taTMHRec.GetDataBy(gid);
            if (dtResults.Rows.Count < 1)
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "\r\nNo Record was found.";
                return;
            }

            DataSet1.TMHUserRecsDataTable dtResults2 = new DataSet1.TMHUserRecsDataTable();
            DataSet1TableAdapters.TMHUserRecsTableAdapter taTMHRec2 = new DataSet1TableAdapters.TMHUserRecsTableAdapter();

            // get the record's data from the DB table and populate all fields into data table dtResults
            dtResults2 = taTMHRec2.GetDataBy1(gid);
            if (dtResults2.Rows.Count < 1)
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "\r\nNo Address Confirmation was found.";
                ddlAddress.SelectedValue = "";
                //return;
            }

            else ddlAddress.SelectedValue = dtResults2[0].ddlAddress.ToString().Trim();

            // Populate the page fields with values from the data table columns
            // person fields

            //ddlApproved.SelectedValue = dtResults[0].ddlApproved.ToString().Trim();

            // multi select fields are stored as comma delimited in the database table, read it and set the selected flag to true
            // for the display of the field in the list box
            string lbxDiagnosistmp = "";
            if (dtResults[0].lbxDiagnosis.Length > 0)
            {
                lbxDiagnosistmp = dtResults[0].lbxDiagnosis;

                foreach (ListItem item in lbxDiagnosis.Items)
                {
                    if (item.Value.Length > 0 && lbxDiagnosistmp.IndexOf("," + item.Value) > -1) item.Selected = true;
                }
            }

            txtLastName.Text = strDecodeLiteral(dtResults[0].txtLastName.ToString().Trim());
            txtFirstName.Text = strDecodeLiteral(dtResults[0].txtFirstName.ToString().Trim());
            txtMiddleName.Text = strDecodeLiteral(dtResults[0].txtMiddleName.ToString().Trim());
            SUFFIX_NAME.Text = dtResults[0].SUFFIX_NAME.ToString().Trim();
            txtNameToCallMe.Text = strDecodeLiteral(dtResults[0].txtNameToCallMe.ToString().Trim());
            txtHomePhone.Text = strDecodeLiteral(dtResults[0].txtHomePhone.ToString().Trim());
            txtAddressNumber.Text = strDecodeLiteral(dtResults[0].txtAddressNumber.ToString().Trim());
            txtAddressStreet.Text = strDecodeLiteral(dtResults[0].txtAddressStreet.ToString().Trim());
            txtCity.Text = strDecodeLiteral(dtResults[0].txtCity.ToString().Trim());
            ddlCounty.SelectedValue = dtResults[0].ddlCounty.ToString().Trim();
            ddlState.SelectedValue = dtResults[0].ddlState.ToString().Trim();
            txtZipCode.Text = strDecodeLiteral(dtResults[0].txtZipCode.ToString().Trim());

            // Physical Description fields
            //txtDOB.Text = strDecodeLiteral(dtResults[0].txtDOB.ToShortDateString().ToString().Trim());
            txtDOB.Text = strDecodeLiteral(dtResults[0].txtDOB.ToString("MM/dd/yyyy").Trim());

            txtAge.Text = dtResults[0].txtAge.ToString().Trim();
            ddlRace.SelectedValue = dtResults[0].ddlRace.ToString().Trim();
            ddlSex.SelectedValue = dtResults[0].ddlSex.ToString().Trim();
            ddlHeight.SelectedValue = dtResults[0].ddlHeight.ToString().Trim();
            txtWeight.Text = strDecodeLiteral(dtResults[0].txtWeight.ToString().Trim());
            ddlEye.SelectedValue = dtResults[0].ddlEye.ToString().Trim();
            ddlHair.SelectedValue = dtResults[0].ddlHair.ToString().Trim();

            // Special Fields
            ddlHomeType.SelectedValue = dtResults[0].ddlHomeType.ToString().Trim();
            ddlWander.SelectedValue = dtResults[0].ddlWander.ToString().Trim();

            string ddlCommunicationtmp = "";
            if (dtResults[0].ddlCommunication.Length > 0)
            {
                ddlCommunicationtmp = dtResults[0].ddlCommunication;

                foreach (ListItem item in ddlCommunication.Items)
                {
                    if (item.Value.Length > 0 && ddlCommunicationtmp.IndexOf("," + item.Value) > -1) item.Selected = true;
                }
            }

            ddlMedication.SelectedValue = dtResults[0].ddlMedication.ToString().Trim();
            txtLanguages.Text = strDecodeLiteral(dtResults[0].txtLanguages.ToString().Trim());
            txtMedical.Text = strDecodeLiteral(dtResults[0].txtMedical.ToString().Trim());
            txtWornItems.Text = strDecodeLiteral(dtResults[0].txtWornItems.ToString().Trim());
            txtApproach.Text = strDecodeLiteral(dtResults[0].txtApproach.ToString().Trim());
            txtBehaviors.Text = strDecodeLiteral(dtResults[0].txtBehaviors.ToString().Trim());

            string lbxSpecialtmp = "";
            if (dtResults[0].lbxSpecial.Length > 0)
            {
                lbxSpecialtmp = dtResults[0].lbxSpecial;

                foreach (ListItem item in lbxSpecial.Items)
                {
                    if (item.Value.Length > 0 && lbxSpecialtmp.IndexOf("," + item.Value) > -1) item.Selected = true;
                }
            }

            // txtEnrollDate.Text = strDecodeLiteral(dtResults[0].txtEnrollDate.ToString().Trim());
            ddlEnrollingAgency.SelectedValue = dtResults[0].ddlEnrollingAgency.ToString().Trim();

            // Contact Fields
            ddlContactRelationship.SelectedValue = dtResults[0].ddlContactRelationship.ToString().Trim();
            txtContactFullName.Text = strDecodeLiteral(dtResults[0].txtContactFullName.ToString().Trim());
            txtContactAddress.Text = strDecodeLiteral(dtResults[0].txtContactAddress.ToString().Trim());
            txtContactCity.Text = strDecodeLiteral(dtResults[0].txtContactCity.ToString().Trim());
            ddlContactState.SelectedValue = dtResults[0].ddlContactState.ToString().Trim();
            txtContactZip.Text = strDecodeLiteral(dtResults[0].txtContactZip.ToString().Trim());
            txtContactHPhone.Text = strDecodeLiteral(dtResults[0].txtContactHPhone.ToString().Trim());
            txtContactMPhone.Text = strDecodeLiteral(dtResults[0].txtContactMPhone.ToString().Trim());
            txtContactOPhone.Text = strDecodeLiteral(dtResults[0].txtContactOPhone.ToString().Trim());
            txtContactEMail.Text = strDecodeLiteral(dtResults[0].txtContactEMail.ToString().Trim());

            // Contact2 Fields
            ddlContactRelationship2.SelectedValue = dtResults[0].ddlContactRelationship2.ToString().Trim();
            txtContactFullName2.Text = strDecodeLiteral(dtResults[0].txtContactFullName2.ToString().Trim());
            txtContactAddress2.Text = strDecodeLiteral(dtResults[0].txtContactAddress2.ToString().Trim());
            txtContactCity2.Text = strDecodeLiteral(dtResults[0].txtContactCity2.ToString().Trim());
            ddlContactState2.SelectedValue = dtResults[0].ddlContactState2.ToString().Trim();
            txtContactZip2.Text = strDecodeLiteral(dtResults[0].txtContactZip2.ToString().Trim());
            txtContactHPhone2.Text = strDecodeLiteral(dtResults[0].txtContactHPhone2.ToString().Trim());
            txtContactMPhone2.Text = strDecodeLiteral(dtResults[0].txtContactMPhone2.ToString().Trim());
            txtContactOPhone2.Text = strDecodeLiteral(dtResults[0].txtContactOPhone2.ToString().Trim());
            txtContactEMail2.Text = strDecodeLiteral(dtResults[0].txtContactEMail2.ToString().Trim());

            // Vehicle Fields
            ddlVehType.SelectedValue = dtResults[0].ddlVehType.ToString().Trim();
            ddlVehYear.SelectedValue = dtResults[0].ddlVehYear.ToString().Trim();
            ddlVehMake.SelectedValue = dtResults[0].ddlVehMake.ToString().Trim();
            txtVehModel.Text = strDecodeLiteral(dtResults[0].txtVehModel.ToString().Trim());
            ddlVehColor.SelectedValue = dtResults[0].ddlVehColor.ToString().Trim();
            txtVehVIN.Text = strDecodeLiteral(dtResults[0].txtVehVIN.ToString().Trim());
            txtVehLic.Text = strDecodeLiteral(dtResults[0].txtVehLic.ToString().Trim());
            ddlVehLicState.SelectedValue = dtResults[0].ddlVehLicState.ToString().Trim();
            //ddlVehLicYear.SelectedValue = dtResults[0].ddlVehLicYear.ToString().Trim();

            // Photo Fields
            if ((dtResults[0].SubmittedDateTime.ToString().Length > 0) && (dtResults[0].SubmittedDateTime.ToString().Trim() != "1/1/1900 12:00:00 AM"))
                txtSubmittedDateTime.Text = strDecodeLiteral(dtResults[0].SubmittedDateTime.ToString().Trim());

            lblPhoto.Text = dtResults[0].FileName.ToString().Trim();

            AGE_IN_PHOTOStart.Text = strDecodeLiteral(dtResults[0].AGE_IN_PHOTOStart.ToString().Trim());

            //if (txtPhotoDate.Text.ToString().Trim().Length > 0)
            if ((dtResults[0].txtPhotoDate.ToString().Trim().Length > 0) && (dtResults[0].txtPhotoDate.ToString().Trim() != "1/1/1900 12:00:00 AM"))
            {
                string[] strPhotoDate = dtResults[0].txtPhotoDate.ToString("MM/dd/yyyy").Trim().Split(' ');
                txtPhotoDate.Text = strDecodeLiteral(strPhotoDate[0]);
            }

            Guid FileGuid2 = dtResults[0].FileId;
            HttpContext.Current.Session["PhotoID"] = FileGuid2;

            //lblImage.Text = "<img border=\"0\" src=TMHPhoto.aspx?data=" + FileGuid2 + ">";
            //<img id="image" src="Content/placeholder.jpg" class="img-responsive center-block" alt="Responsive image">
            if (FileGuid2.ToString() == "00000000-0000-0000-0000-000000000000") lblImage.Text = "<img id=\"image\" class=\"img-responsive center-block\" alt=\"Responsive image\" src=\"Content/placeholder.jpg\">";
            else 
            {
                lblImage.Text = "<img id=\"image\" class=\"img-responsive center-block\" alt=\"Responsive image\" src=TMHPhoto.aspx?Data=" + FileGuid2 + ">";
                Guid Guid1 = new Guid(HttpContext.Current.Session["PhotoID"].ToString());
                DataSet1TableAdapters.TMHFilesTableAdapter taTMHFiles = new DataSet1TableAdapters.TMHFilesTableAdapter();
                DataSet1.TMHFilesDataTable dtTMHFiles = taTMHFiles.GetDataBy(Guid1);

                if (dtTMHFiles.Rows.Count > 0)
                {
                    int iFileSize = dtTMHFiles[0].FileImage.Length;

                    if (iFileSize > 1024 * 1024)
                    {
                        iFileSize = (iFileSize / (1024 * 1024));
                        filesize.Text = "Size: " + iFileSize + " MB (Large File!)";
                    }
                    else
                    {
                        iFileSize = iFileSize / 1024;
                        filesize.Text = "Size: " + iFileSize + " KB";
                    }
                }
                taTMHFiles.Connection.Close();
                taTMHFiles.Dispose();
            }

            //else lblImage.Text = "<img border=\"0\" src=TMHPhoto.aspx?Data=" + FileGuid2 + ">";

            //vTabStatus(true);
            lblMsg.Text = "";
            //TabContainer1.ActiveTabIndex = 1;

            taTMHRec.Connection.Close();
            taTMHRec = null;
        }

        //---------------------------- Method ---------------------------------------
        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Server.Transfer("default.aspx");
        }

        //---------------------------- Method ---------------------------------------
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("default.aspx");
        }

        //---------------------------- Method ---------------------------------------
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool bReqFields = bChkRequiredFields();

            if (!bReqFields)
            {
                // missing required fields, display the message and go back to edit screens
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "<ul type=\"disc\"> " + strMsg + "</ul>";
                return;
            }

            // All required fields are collected, process the record
            msgVal.Attributes["class"] = "alert alert-success";
            lblMsg.Text = "Thank you, trying to save your record...";

            bool bFinalResult = SaveTMHRecord();

            if (bFinalResult) Server.Transfer("default.aspx");

            return;
        }

        //---------------------------- Method ---------------------------------------
        /// <summary>This method is called to save the TMH record.</summary>
        /// <param name="strMode"></param>
        /// <returns>true or false</returns>
        bool SaveTMHRecord()
        {
            bNewRecord = (bool) HttpContext.Current.Session["NewRecord"];

            //=========================================== Person Tab Fields ==================================

            DataSet1TableAdapters.TMHRecTableAdapter taTMHRec = new DataSet1TableAdapters.TMHRecTableAdapter();
            DataSet1TableAdapters.TMHUserRecsTableAdapter taTMHUserRecs = new DataSet1TableAdapters.TMHUserRecsTableAdapter();

            Guid gid = (Guid) HttpContext.Current.Session["SelectedID"];
            Guid gPhotoID = (Guid) HttpContext.Current.Session["PhotoID"];

            if (bNewRecord)
            {
                long lUserId = Convert.ToInt64(HttpContext.Current.Session["TMHUserId"].ToString());
                // insert a blank record into the table, if new record
                taTMHRec.Insert(gid, "", Guid.Empty, "", "",
                Convert.ToDateTime("1900-01-01 00:00:00"), "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                Convert.ToDateTime("1900-01-01 00:00:00"), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                Convert.ToDateTime("1900-01-01 00:00:00"), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                DateTime.Now, "", "", Convert.ToDateTime("1900-01-01 00:00:00"));

                taTMHUserRecs.InsertItem(lUserId, gid, "");

                HttpContext.Current.Session["NewRecord"] = false;
                bNewRecord = false;
            }

            // see if the record is in our database
            DataSet1.TMHRecDataTable TMHRecDT = taTMHRec.GetDataBy(gid);

            if (TMHRecDT.Rows.Count < 1)
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "Error TMH-800, Could not retrieve the record from TMH database, an Error Occurred.";
                return false;
            }

            DataSet1.TMHUserRecsDataTable TMHRecDT2 = taTMHUserRecs.GetDataBy1(gid);
            TMHRecDT2[0].ddlAddress = ddlAddress.SelectedValue.ToString().Trim();

            TMHRecDT[0].ddlApproved = "N"; // ddlApproved.SelectedValue.ToString().Trim();

            TMHRecDT[0].lbxDiagnosis = "";
            foreach (ListItem item in lbxDiagnosis.Items)
            {
                if (item.Selected) TMHRecDT[0].lbxDiagnosis = TMHRecDT[0].lbxDiagnosis + "," + item.Value;
            }
            if (TMHRecDT[0].lbxDiagnosis.ToString().Length > 2)
            {
                if (TMHRecDT[0].lbxDiagnosis.ToString().Substring(0, 2) == ",,") TMHRecDT[0].lbxDiagnosis = TMHRecDT[0].lbxDiagnosis.ToString().Replace(",,", ",");
            }

            TMHRecDT[0].txtLastName = strSafeSqlLiteral(txtLastName.Text.ToString(), 15);
            TMHRecDT[0].txtFirstName = strSafeSqlLiteral(txtFirstName.Text.ToString(), 14);
            TMHRecDT[0].txtMiddleName = strSafeSqlLiteral(txtMiddleName.Text.ToString(), 14);
            TMHRecDT[0].SUFFIX_NAME = SUFFIX_NAME.Text.ToString().Trim();
            TMHRecDT[0].txtNameToCallMe = strSafeSqlLiteral(txtNameToCallMe.Text.ToString(), 50);
            TMHRecDT[0].txtHomePhone = strSafeSqlLiteral(txtHomePhone.Text.ToString(), 12);

            TMHRecDT[0].txtAddressNumber = strSafeSqlLiteral(txtAddressNumber.Text.ToString(), 10);

            TMHRecDT[0].txtAddressStreet = strSafeSqlLiteral(txtAddressStreet.Text.ToString(), 30);
            TMHRecDT[0].txtCity = txtCity.Text.ToString().Trim();
            TMHRecDT[0].ddlCounty = ddlCounty.SelectedValue.ToString().Trim();
            TMHRecDT[0].ddlState = ddlState.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtZipCode = strSafeSqlLiteral(txtZipCode.Text.ToString(), 10);

            // physical description
            if (strDOB != "") TMHRecDT[0].txtDOB = Convert.ToDateTime(strDOB);
            TMHRecDT[0].txtAge = iPersonAge.ToString().Trim();
            TMHRecDT[0].ddlRace = ddlRace.SelectedValue.ToString().Trim();
            TMHRecDT[0].ddlSex = ddlSex.SelectedValue.ToString().Trim();
            TMHRecDT[0].ddlHeight = ddlHeight.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtWeight = strSafeSqlLiteral(txtWeight.Text.ToString(), 3);
            TMHRecDT[0].ddlEye = ddlEye.SelectedValue.ToString().Trim();
            TMHRecDT[0].ddlHair = ddlHair.SelectedValue.ToString().Trim();
            //=========================================== Special Tab Fields ================================
            TMHRecDT[0].ddlHomeType = ddlHomeType.SelectedValue.ToString().Trim();
            TMHRecDT[0].ddlWander = ddlWander.SelectedValue.ToString().Trim();

            TMHRecDT[0].ddlCommunication = "";
            foreach (ListItem item in ddlCommunication.Items)
            {
                if (item.Selected) TMHRecDT[0].ddlCommunication = TMHRecDT[0].ddlCommunication + "," + item.Value;
            }
            if (TMHRecDT[0].ddlCommunication.ToString().Length > 2)
            {
                if (TMHRecDT[0].ddlCommunication.ToString().Substring(0, 2) == ",,") TMHRecDT[0].ddlCommunication = TMHRecDT[0].ddlCommunication.ToString().Replace(",,", ",");
            }

            TMHRecDT[0].ddlMedication = ddlMedication.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtLanguages = strSafeSqlLiteral(txtLanguages.Text.ToString(), 250);
            TMHRecDT[0].txtMedical = strSafeSqlLiteral(txtMedical.Text.ToString(), 250);
            TMHRecDT[0].txtWornItems = strSafeSqlLiteral(txtWornItems.Text.ToString(), 250);
            TMHRecDT[0].txtApproach = strSafeSqlLiteral(txtApproach.Text.ToString(), 250);
            TMHRecDT[0].txtBehaviors = strSafeSqlLiteral(txtBehaviors.Text.ToString(), 250);
            TMHRecDT[0].ddlApproved = "N"; // ddlApproved.SelectedValue.ToString().Trim();
            TMHRecDT[0].lbxSpecial = "";
            foreach (ListItem item in lbxSpecial.Items)
            {
                if (item.Selected) TMHRecDT[0].lbxSpecial = TMHRecDT[0].lbxSpecial + "," + item.Value;
            }
            if (TMHRecDT[0].lbxSpecial.ToString().Length > 2)
            {
                if (TMHRecDT[0].lbxSpecial.ToString().Substring(0, 2) == ",,") TMHRecDT[0].lbxSpecial = TMHRecDT[0].lbxSpecial.ToString().Replace(",,", ",");
            }

            /*
            //=========================================== ID Info Tab Fields ================================
            TMHRecDT[0].txtBracelet = strSafeSqlLiteral(txtBracelet.Text.ToString(), 20);
            TMHRecDT[0].txtBraceletID = strSafeSqlLiteral(txtBraceletID.Text.ToString(), 20);
            TMHRecDT[0].txtDLNum = strSafeSqlLiteral(txtDLNum.Text.ToString(), 20);
            TMHRecDT[0].ddlDLState = ddlDLState.SelectedValue.ToString().Trim();

            string strDTValue = txtDLExpDT.Text.ToString().Trim();
            
            if (strDTValue.Length == 0) TMHRecDT[0].txtDLExpDT = Convert.ToDateTime("1900-01-01 00:00:00");
            else TMHRecDT[0].txtDLExpDT = Convert.ToDateTime(strDTValue); //txtDLExpDT.SelectedDate;

            TMHRecDT[0].ddlOrg = "";
            foreach (ListItem item in ddlOrg.Items)
            {
                if (item.Selected) TMHRecDT[0].ddlOrg = TMHRecDT[0].ddlOrg + "," + item.Value;
            }
            if (TMHRecDT[0].ddlOrg.ToString().Length > 2)
            {
                if (TMHRecDT[0].ddlOrg.ToString().Substring(0, 2) == ",,") TMHRecDT[0].ddlOrg = TMHRecDT[0].ddlOrg.ToString().Replace(",,", ",");
            }
            */

            TMHRecDT[0].txtEnrollDate = DateTime.Now;
            //txtEnrollDate.Text = TMHRecDT[0].txtEnrollDate.ToString();

            TMHRecDT[0].ddlEnrollingAgency = ddlEnrollingAgency.SelectedValue.ToString().Trim();

            string strUserID = "TMHSelf"; // User.Identity.Name.ToString().Trim();
            /*
            if (strUserID.Length > 3) // remove domain name
            {
                //string[] words = strUserID.Split('\\');
                //txtUserID.Text = words[1]
                //txtUserID.Text = strUserID;
            }
            */

            TMHRecDT[0].txtUserID = strUserID;

            //=========================================== Contact Tab Fields ================================
            TMHRecDT[0].ddlContactRelationship = ddlContactRelationship.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtContactFullName = strSafeSqlLiteral(txtContactFullName.Text.ToString(), 50);
            TMHRecDT[0].txtContactAddress = strSafeSqlLiteral(txtContactAddress.Text.ToString(), 100);
            TMHRecDT[0].txtContactCity = strSafeSqlLiteral(txtContactCity.Text.ToString(), 24);
            TMHRecDT[0].ddlContactState = ddlContactState.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtContactZip = strSafeSqlLiteral(txtContactZip.Text.ToString(), 10);
            TMHRecDT[0].txtContactHPhone = strSafeSqlLiteral(txtContactHPhone.Text.ToString(), 12);
            TMHRecDT[0].txtContactMPhone = strSafeSqlLiteral(txtContactMPhone.Text.ToString(), 12);
            TMHRecDT[0].txtContactOPhone = strSafeSqlLiteral(txtContactOPhone.Text.ToString(), 12);
            TMHRecDT[0].txtContactEMail = strSafeSqlLiteral(txtContactEMail.Text.ToString(), 50);
            //=========================================== Contact2 Fields ================================
            TMHRecDT[0].ddlContactRelationship2 = ddlContactRelationship2.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtContactFullName2 = strSafeSqlLiteral(txtContactFullName2.Text.ToString(), 50);
            TMHRecDT[0].txtContactAddress2 = strSafeSqlLiteral(txtContactAddress2.Text.ToString(), 100);
            TMHRecDT[0].txtContactCity2 = strSafeSqlLiteral(txtContactCity2.Text.ToString(), 24);
            TMHRecDT[0].ddlContactState2 = ddlContactState2.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtContactZip2 = strSafeSqlLiteral(txtContactZip2.Text.ToString(), 10);
            TMHRecDT[0].txtContactHPhone2 = strSafeSqlLiteral(txtContactHPhone2.Text.ToString(), 12);
            TMHRecDT[0].txtContactMPhone2 = strSafeSqlLiteral(txtContactMPhone2.Text.ToString(), 12);
            TMHRecDT[0].txtContactOPhone2 = strSafeSqlLiteral(txtContactOPhone2.Text.ToString(), 12);
            TMHRecDT[0].txtContactEMail2 = strSafeSqlLiteral(txtContactEMail2.Text.ToString(), 50);
            /*
            //=========================================== Contact3 Fields ================================
            TMHRecDT[0].ddlContactRelationship3 = ddlContactRelationship3.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtContactFullName3 = strSafeSqlLiteral(txtContactFullName3.Text.ToString(), 50);
            TMHRecDT[0].txtContactAddress3 = strSafeSqlLiteral(txtContactAddress3.Text.ToString(), 100);
            TMHRecDT[0].txtContactCity3 = strSafeSqlLiteral(txtContactCity3.Text.ToString(), 24);
            TMHRecDT[0].ddlContactState3 = ddlContactState3.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtContactZip3 = strSafeSqlLiteral(txtContactZip3.Text.ToString(), 10);
            TMHRecDT[0].txtContactHPhone3 = strSafeSqlLiteral(txtContactHPhone3.Text.ToString(), 12);
            TMHRecDT[0].txtContactMPhone3 = strSafeSqlLiteral(txtContactMPhone3.Text.ToString(), 12);
            TMHRecDT[0].txtContactOPhone3 = strSafeSqlLiteral(txtContactOPhone3.Text.ToString(), 12);
            TMHRecDT[0].txtContactEMail3 = strSafeSqlLiteral(txtContactEMail3.Text.ToString(), 50);
            */

            //=========================================== Vehicle Tab Fields ================================
            TMHRecDT[0].ddlVehType = ddlVehType.SelectedValue.ToString().Trim();
            TMHRecDT[0].ddlVehYear = ddlVehYear.SelectedValue.ToString().Trim();
            TMHRecDT[0].ddlVehMake = ddlVehMake.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtVehModel = strSafeSqlLiteral(txtVehModel.Text.ToString(), 20);
            TMHRecDT[0].ddlVehColor = ddlVehColor.SelectedValue.ToString().Trim();
            TMHRecDT[0].txtVehVIN = strSafeSqlLiteral(txtVehVIN.Text.ToString(), 40);
            TMHRecDT[0].txtVehLic = strSafeSqlLiteral(txtVehLic.Text.ToString(), 15);
            TMHRecDT[0].ddlVehLicState = ddlVehLicState.SelectedValue.ToString().Trim();
            //TMHRecDT[0].ddlVehLicYear = ddlVehLicYear.SelectedValue.ToString().Trim();

            //=========================================== Photo Tab Fields ================================
            TMHRecDT[0].ddlApproved = "N"; // ddlApproved.SelectedValue.ToString().Trim();

            TMHRecDT[0].AGE_IN_PHOTOStart = strSafeSqlLiteral(AGE_IN_PHOTOStart.Text.ToString(), 3);
            if (txtPhotoDate.Text.ToString().Trim().Length > 0) TMHRecDT[0].txtPhotoDate = Convert.ToDateTime(txtPhotoDate.Text);
            else TMHRecDT[0].txtPhotoDate = Convert.ToDateTime("1900-01-01 00:00:00");

            DataSet1TableAdapters.TMHFilesTableAdapter TMHFiles = new DataSet1TableAdapters.TMHFilesTableAdapter();

            byte[] decodedFromBase64 = { };
            if (base64.Value.ToString() != "")
            {
                int iLen = base64.Value.Length - 23;
                decodedFromBase64 = Convert.FromBase64String(base64.Value.ToString().Substring(23,iLen));
            }

            // if updating an existing photo
            if ((!bNewRecord) && (FileUpload.HasFile))
            {
                try
                {
                    if (bIsFileTypeOk(FileUpload.FileName.ToLower().Trim(), FileUpload.FileBytes.Length))
                    {
                        if (gPhotoID.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            Guid FileGuid2 = System.Guid.NewGuid();
                            if (base64.Value.ToString() != "") TMHFiles.Insert(FileGuid2, decodedFromBase64);
                            else TMHFiles.Insert(FileGuid2, FileUpload.FileBytes);
                            gPhotoID = FileGuid2;
                            HttpContext.Current.Session["PhotoID"] = FileGuid2;
                        }
                        else
                        {
                            if (base64.Value.ToString() != "")  TMHFiles.Update(gPhotoID, decodedFromBase64, gPhotoID);
                            else TMHFiles.Update(gPhotoID, FileUpload.FileBytes, gPhotoID);
                        }
                    }

                    else
                    {
                        msgVal.Attributes["class"] = "alert alert-danger";
                        lblMsg.Text = "Only jpg, jpeg, png, or gif files less than 2 Mega Bytes may be uploaded.";
                        return false;
                    }
                }

                catch
                {
                    msgVal.Attributes["class"] = "alert alert-danger";
                    lblMsg.Text = "Error TMHSelf-810, Unable to update/upload photo file " + FileUpload.FileName + " ,an Error Occurred.";
                    return false;
                }

                TMHRecDT[0].FileName = FileUpload.FileName;
                TMHRecDT[0].FileId = gPhotoID;
                lblPhoto.Text = FileUpload.FileName.ToString().Trim();
            }

            // if a new record
            if ((bNewRecord) && (FileUpload.HasFile))
            {
                try
                {
                    if (bIsFileTypeOk(FileUpload.FileName.ToLower().Trim(), FileUpload.FileBytes.Length))
                    {
                        Guid FileGuid2 = System.Guid.NewGuid();
                        if (base64.Value.ToString() != "") TMHFiles.Insert(FileGuid2, decodedFromBase64);
                        else TMHFiles.Insert(FileGuid2, FileUpload.FileBytes);
                        gPhotoID = FileGuid2;
                        HttpContext.Current.Session["PhotoID"] = FileGuid2;
                    }
                    else
                    {
                        msgVal.Attributes["class"] = "alert alert-danger";
                        lblMsg.Text = "Only jpg, jpeg, png, or gif files less than 1 Mega Bytes may be uploaded.";
                        return false;
                    }
                }

                catch
                {
                    msgVal.Attributes["class"] = "alert alert-danger";
                    lblMsg.Text = "Error TMHSelf-820, Unable to insert/upload photo file " + FileUpload.FileName + " , an Error Occurred.";
                    return false;
                }

                TMHRecDT[0].FileName = FileUpload.FileName;
                TMHRecDT[0].FileId = gPhotoID;
                lblPhoto.Text = FileUpload.FileName.ToString().Trim();
            }

            if (HttpContext.Current.Session["PhotoID"].ToString() == "00000000-0000-0000-0000-000000000000") lblImage.Text = "<img border=\"0\" src=\"Images/NoPicture.gif\">";
            else lblImage.Text = "<img border=\"0\" src=TMHPhoto.aspx?Data=" + HttpContext.Current.Session["PhotoID"] + ">";

            TMHRecDT[0].SubmittedDateTime = DateTime.Now;
            txtSubmittedDateTime.Text = TMHRecDT[0].SubmittedDateTime.ToString();

            //=========================================== Save the Record in internet SQL DB ==================================
            // save the record in the table
            taTMHRec.Update(TMHRecDT);

            DataSet1.TMHRecDataTable TMHRecDT_FromDB = taTMHRec.GetDataBy(gid);
            if (TMHRecDT_FromDB.Rows.Count > 0)
            {
                // save the Address Confirmation value in the TMHUserRecs table also
                taTMHUserRecs.Update(TMHRecDT2);

                //msgVal.Attributes["class"] = "alert alert-success";
                //lblMsg.Text = "Record saved Successfully."; // + " Rec: " + TMHRecDT[0].id.ToString();
            }
            else
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = "Error TMHSelf-830, The record was not saved, an Error Occurred.";
                return false;
            }

            // check if user selected to delete the record
            //if (bChkDelete()) return;

            // This method will send the record to the eMug server in sdsheriff.com via a web service
            // if at least all of the required fields are provided.
            bNewRecord = (bool) HttpContext.Current.Session["NewRecord"];

            lblMsg.Text = "Thank you, the Record is being saved and submitted to the Sheriff’s Department...";

            // submit the record to eMug
            strMsg = strMsg + "The Record is saved and is being submitted to the Sheriff’s Department...";

            if (TMHRecDT.Rows.Count < 1)
                {
                    msgVal.Attributes["class"] = "alert alert-danger";
                    lblMsg.Text = strMsg + "Error TMHSelf-900, Could not retrieve the record for submission to the Sheriff’s Department, an Error Occurred.";
                    return false;
                }

            string[] strFieldTags = new string[] {
                    "SAN_DIEGO_TMH_ADDRESS_CITY", //0
                    "SAN_DIEGO_TMH_ADDRESS_COUNTY",
                    "SAN_DIEGO_TMH_ADDRESS_NUMBER",
                    "SAN_DIEGO_TMH_ADDRESS_STATE",
                    "SAN_DIEGO_TMH_ADDRESS_STREET",
                    "SAN_DIEGO_TMH_ADDRESS_ZIP_CODE", //5
                    "SAN_DIEGO_TMH_AGE_IN_PHOTO",
                    "SAN_DIEGO_TMH_AGE",
                    "SAN_DIEGO_TMH_AGENCY_ORI",
                    "SAN_DIEGO_TMH_APPROACH_SUGGESTIONS",
                    "SAN_DIEGO_APPROVED", //10
                    "SAN_DIEGO_TMH_BEHAVIOR_TYPES",
                    "SAN_DIEGO_TMH_BRACELET_ID",
                    "SAN_DIEGO_TMH_BRACELET_NAME",
                    "SAN_DIEGO_TMH_COMM_METHOD",
                    "SAN_DIEGO_TMH_COMMONLY_WORN_ITEMS", //15
                    "SAN_DIEGO_TMH_DIAGNOSIS_DISABILITY",
                    "SAN_DIEGO_TMH_DRIVERS_LIC_NUM",
                    "SAN_DIEGO_TMH_DL_EXPIRATION_DATE",
                    "SAN_DIEGO_TMH_DRIVERS_LIC_STATE",
                    "SAN_DIEGO_TMH_DOB", //20
                    "SAN_DIEGO_TMH_ENROLLMENT_DATETIME",
                    "SAN_DIEGO_TMH_EYE_COLOR",
                    "SAN_DIEGO_TMH_FIRST_NAME",
                    "SAN_DIEGO_TMH_HAIR_COLOR",
                    "SAN_DIEGO_TMH_HEIGHT", //25
                    "SAN_DIEGO_TMH_HOME_TYPE",
                    "SAN_DIEGO_TMH_LAST_NAME",
                    "SAN_DIEGO_TMH_MED_PSYCH_ISSUES",
                    "SAN_DIEGO_TMH_MEDS_ENDANGER",
                    "SAN_DIEGO_TMH_MIDDLE_NAME", //30
                    "SAN_DIEGO_TMH_NAME_TO_CALL_ME",
                    "SAN_DIEGO_TMH_OPERATOR_ID",
                    "SAN_DIEGO_TMH_ORGANIZATIONS",
                    "SAN_DIEGO_TMH_ADDRESS_PHONE",
                    "SAN_DIEGO_TMH_PHOTO_DATE", //35
                    "SAN_DIEGO_TMH_RACE",
                    "SAN_DIEGO_TMH_SEX",
                    "SAN_DIEGO_TMH_SPECIAL_CONSIDERATIONS",
                    "SAN_DIEGO_TMH_SPOKEN_LANGUAGES",
                    "SAN_DIEGO_TMH_SUFFIX_NAME", //40
                    "SAN_DIEGO_TMH_WANDER_TENDANCY",
                    "SAN_DIEGO_TMH_WEIGHT",
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_COLOR", //43
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_LIC_NUM",
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_LIC_STATE", //45
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_LIC_YEAR",
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_MAKE_LIST",
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_MODEL",
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_TYPE",
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_VIN", //50
                    "SAN_DIEGO_TMH_VEHICLE_VEHICLE_YEAR", //51
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_CITY",
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_STATE",
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_ZIP",
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS", //55
                    "SAN_DIEGO_TMH_CONTACTS_EMAIL",
                    "SAN_DIEGO_TMH_CONTACTS_FULL_NAME",
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_HOME",
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_MOBILE",
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_OTHER", //60
                    "SAN_DIEGO_TMH_CONTACTS_RELATIONSHIP",
                    "SAN_DIEGO_TMH_CONTACTS_TYPE",
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_CITY2", //63
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_STATE2",
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_ZIP2", //65
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS2",
                    "SAN_DIEGO_TMH_CONTACTS_EMAIL2",
                    "SAN_DIEGO_TMH_CONTACTS_FULL_NAME2",
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_HOME2",
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_MOBILE2", //70
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_OTHER2",
                    "SAN_DIEGO_TMH_CONTACTS_RELATIONSHIP2",
                    "SAN_DIEGO_TMH_CONTACTS_TYPE2", //73
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_CITY3", //74
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_STATE3", //75
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS_ZIP3",
                    "SAN_DIEGO_TMH_CONTACTS_ADDRESS3",
                    "SAN_DIEGO_TMH_CONTACTS_EMAIL3",
                    "SAN_DIEGO_TMH_CONTACTS_FULL_NAME3",
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_HOME3", //80
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_MOBILE3",
                    "SAN_DIEGO_TMH_CONTACTS_PHONE_OTHER3",
                    "SAN_DIEGO_TMH_CONTACTS_RELATIONSHIP3",
                    "SAN_DIEGO_TMH_CONTACTS_TYPE3" //84
                };

                string[] strFieldVals = new string[85];
                for (int i = 0; i < 85; i++) { strFieldVals[i] = ""; }

                //=========================================== Person Tab Fields ==================================
                strFieldVals[0] = TMHRecDT[0].txtCity;
                strFieldVals[1] = TMHRecDT[0].ddlCounty;
                strFieldVals[2] = TMHRecDT[0].txtAddressNumber;
                strFieldVals[3] = TMHRecDT[0].ddlState;
                strFieldVals[4] = TMHRecDT[0].txtAddressStreet;
                strFieldVals[5] = TMHRecDT[0].txtZipCode;
                strFieldVals[6] = TMHRecDT[0].AGE_IN_PHOTOStart;
                strFieldVals[7] = TMHRecDT[0].txtAge;
                strFieldVals[8] = TMHRecDT[0].ddlEnrollingAgency;
                strFieldVals[9] = TMHRecDT[0].txtApproach;

                strFieldVals[10] = TMHRecDT[0].ddlApproved;

                strFieldVals[11] = TMHRecDT[0].txtBehaviors;
                strFieldVals[12] = TMHRecDT[0].txtBraceletID;
                strFieldVals[13] = TMHRecDT[0].txtBracelet;
                strFieldVals[14] = TMHRecDT[0].ddlCommunication;
                strFieldVals[15] = TMHRecDT[0].txtWornItems;
                strFieldVals[16] = TMHRecDT[0].lbxDiagnosis;
                strFieldVals[17] = TMHRecDT[0].txtDLNum;
                strFieldVals[18] = TMHRecDT[0].txtDLExpDT.ToString();
                strFieldVals[19] = TMHRecDT[0].ddlDLState;

                strFieldVals[20] = TMHRecDT[0].txtDOB.ToString().Trim();
                strFieldVals[21] = DateTime.Now.ToString(); // TMHRecDT[0].txtEnrollDate.ToString();
                strFieldVals[22] = TMHRecDT[0].ddlEye;
                strFieldVals[23] = TMHRecDT[0].txtFirstName;
                strFieldVals[24] = TMHRecDT[0].ddlHair;
                strFieldVals[25] = TMHRecDT[0].ddlHeight;
                strFieldVals[26] = TMHRecDT[0].ddlHomeType;
                strFieldVals[27] = TMHRecDT[0].txtLastName;
                strFieldVals[28] = TMHRecDT[0].txtMedical;
                strFieldVals[29] = TMHRecDT[0].ddlMedication;

                strFieldVals[30] = TMHRecDT[0].txtMiddleName;
                strFieldVals[31] = TMHRecDT[0].txtNameToCallMe;
                strFieldVals[32] = TMHRecDT[0].txtUserID;
                strFieldVals[33] = TMHRecDT[0].ddlOrg;
                strFieldVals[34] = TMHRecDT[0].txtHomePhone;
                strFieldVals[35] = TMHRecDT[0].txtPhotoDate.ToString();
                strFieldVals[36] = TMHRecDT[0].ddlRace;
                strFieldVals[37] = TMHRecDT[0].ddlSex;
                strFieldVals[38] = TMHRecDT[0].lbxSpecial;
                strFieldVals[39] = TMHRecDT[0].txtLanguages;

                strFieldVals[40] = TMHRecDT[0].SUFFIX_NAME;
                strFieldVals[41] = TMHRecDT[0].ddlWander;
                strFieldVals[42] = TMHRecDT[0].txtWeight;

                // if no vehicle info supplied, do not pass vehicle tags to eMug TMH to create a blank record
                if ((TMHRecDT[0].txtVehLic.Length < 1) && (TMHRecDT[0].txtVehModel.Length < 1)
                    && (TMHRecDT[0].txtVehVIN.Length < 1) && (TMHRecDT[0].ddlVehType.Length < 1)
                    && (TMHRecDT[0].ddlVehYear.Length < 1) && (TMHRecDT[0].ddlVehMake.Length < 1)
                    && (TMHRecDT[0].ddlVehColor.Length < 1) && (TMHRecDT[0].ddlVehLicState.Length < 1))
                {
                    for (int j = 43; j < 52; j++) { strFieldTags[j] = ""; }
                }
                else
                {
                    strFieldVals[43] = TMHRecDT[0].ddlVehColor;
                    strFieldVals[44] = TMHRecDT[0].txtVehLic;
                    strFieldVals[45] = TMHRecDT[0].ddlVehLicState;
                    strFieldVals[46] = ""; // TMHRecDT[0].ddlVehLicYear;
                    strFieldVals[47] = TMHRecDT[0].ddlVehMake;
                    strFieldVals[48] = TMHRecDT[0].txtVehModel;
                    strFieldVals[49] = TMHRecDT[0].ddlVehType;

                    strFieldVals[50] = TMHRecDT[0].txtVehVIN;
                    strFieldVals[51] = TMHRecDT[0].ddlVehYear;
                }

                strFieldVals[52] = TMHRecDT[0].txtContactCity;
                strFieldVals[53] = TMHRecDT[0].ddlContactState;
                strFieldVals[54] = TMHRecDT[0].txtContactZip;
                strFieldVals[55] = TMHRecDT[0].txtContactAddress;
                strFieldVals[56] = TMHRecDT[0].txtContactEMail;
                strFieldVals[57] = TMHRecDT[0].txtContactFullName;
                strFieldVals[58] = TMHRecDT[0].txtContactHPhone;
                strFieldVals[59] = TMHRecDT[0].txtContactMPhone;

                strFieldVals[60] = TMHRecDT[0].txtContactOPhone;
                strFieldVals[61] = TMHRecDT[0].ddlContactRelationship;
                strFieldVals[62] = "PRIMARY"; // SAN_DIEGO_TMH_CONTACTS_TYPE

                // if 2nd contact is not entered, do not pass 2nd contact tags to eMug TMH, otherwise blank contact records are created
                if ((TMHRecDT[0].txtContactFullName2.Length < 1) && (TMHRecDT[0].ddlContactRelationship2.Length < 1)
                    && (TMHRecDT[0].txtContactAddress2.Length < 1) && (TMHRecDT[0].txtContactEMail2.Length < 1))
                {
                    for (int j = 63; j < 74; j++) { strFieldTags[j] = ""; }
                }
                else
                {
                    strFieldVals[63] = TMHRecDT[0].txtContactCity2;
                    strFieldVals[64] = TMHRecDT[0].ddlContactState2;
                    strFieldVals[65] = TMHRecDT[0].txtContactZip2;
                    strFieldVals[66] = TMHRecDT[0].txtContactAddress2;
                    strFieldVals[67] = TMHRecDT[0].txtContactEMail2;
                    strFieldVals[68] = TMHRecDT[0].txtContactFullName2;
                    strFieldVals[69] = TMHRecDT[0].txtContactHPhone2;
                    strFieldVals[70] = TMHRecDT[0].txtContactMPhone2;

                    strFieldVals[71] = TMHRecDT[0].txtContactOPhone2;
                    strFieldVals[72] = TMHRecDT[0].ddlContactRelationship2;
                    strFieldVals[73] = "ALTERNATE"; // SAN_DIEGO_TMH_CONTACTS_TYPE2
                }

                // if 3rd contact is not entered, do not pass 3rd contact tags to eMug TMH, otherwise blank contact records are created
                if ((TMHRecDT[0].txtContactFullName3.Length < 1) && (TMHRecDT[0].ddlContactRelationship3.Length < 1)
                    && (TMHRecDT[0].txtContactAddress3.Length < 1) && (TMHRecDT[0].txtContactEMail3.Length < 1))
                {
                    for (int j = 74; j < 85; j++) { strFieldTags[j] = ""; }
                }
                else
                {
                    strFieldVals[74] = TMHRecDT[0].txtContactCity3;
                    strFieldVals[75] = TMHRecDT[0].ddlContactState3;
                    strFieldVals[76] = TMHRecDT[0].txtContactZip3;
                    strFieldVals[77] = TMHRecDT[0].txtContactAddress3;
                    strFieldVals[78] = TMHRecDT[0].txtContactEMail3;
                    strFieldVals[79] = TMHRecDT[0].txtContactFullName3;
                    strFieldVals[80] = TMHRecDT[0].txtContactHPhone3;
                    strFieldVals[81] = TMHRecDT[0].txtContactMPhone3;

                    strFieldVals[82] = TMHRecDT[0].txtContactOPhone3;
                    strFieldVals[83] = TMHRecDT[0].ddlContactRelationship3;
                    strFieldVals[84] = "ALTERNATE"; // SAN_DIEGO_TMH_CONTACTS_TYPE3
                }


                //=========================================== Photo Tab Fields ================================

                lblMsg.Text = strMsg;
                int iResult1 = 0;
                try
                {
                    // grab the photo from the DB table into a byte array
                    Guid gPhotoGuid = new Guid(TMHRecDT[0].FileId.ToString());
                    DataSet1TableAdapters.TMHFilesTableAdapter taTMHFiles = new DataSet1TableAdapters.TMHFilesTableAdapter();
                    DataSet1.TMHFilesDataTable dtTMHFiles = taTMHFiles.GetDataBy(gPhotoGuid);

                    if (dtTMHFiles.Rows.Count < 1)
                    {
                        msgVal.Attributes["class"] = "alert alert-danger";
                        lblMsg.Text = strMsg + "Error TMHSelf-910, The record was not successfully submitted to the Sheriff’s Department, an Error Occurred.";
                        return false;
                    }

                    // consume the web service provided by DataWorks at https://vemugiis003.sdlaw.us/takemehome/takemehome.asmx
                    ws_takemehome.TakeMeHome ws1 = new ws_takemehome.TakeMeHome();
                    string strReturnMsg = "";
                    // submit the record first, returns 0 - success, 1 - Fail
                    iResult1 = ws1.SaveRecord(gid.ToString(), gPhotoGuid.ToString(), strFieldTags, strFieldVals, ref strReturnMsg);
                    if (iResult1 == 1)
                    {
                        strMsg = strMsg + "<li>Error TMHSelf-920, The record was not submitted to the Sheriff’s Department, an Error Occurred.</li>";
                        strMsg = strMsg + "<li>The error message from the Sheriff’s Department: " + strReturnMsg + "</li>";

                        /*
                        for (int i = 0; i < 62; i++)
                        {
                            strMsg = strMsg + "<li>" + strFieldTags[i] + " = " + strFieldVals[i] + "</li>";
                        }
                        */
                    }

                    else
                    {
                        strMsg = strMsg + "<li>The Record is submitted to the Sheriff’s Department successfully.</li>";

                        // now submit the photo, returns 0 - success, 1 - Fail
                        int iResult2 = ws1.SaveImage(gPhotoGuid.ToString(), dtTMHFiles[0].FileImage, ref strReturnMsg);
                        if (iResult2 == 1)
                        {
                            strMsg = strMsg + "<li>Error TMHSelf-930, The photo was not submitted to the Sheriff’s Department, an Error Occurred.</li>";
                            strMsg = strMsg + "<li>The error message from the Sheriff’s Department: " + strReturnMsg + "</li>";
                        }

                        else strMsg = strMsg + "<li>The photo is submitted to the Sheriff’s Department successfully.</li>";
                    }
                }

                catch (Exception ex)
                {
                    msgVal.Attributes["class"] = "alert alert-danger";
                    strMsg = strMsg + "<li>Error TMHSelf-940, The record was not submitted to the Sheriff’s Department, an Error Occurred.</li>";
                    lblMsg.Text = strMsg + "<li>Excption Error: " + ex.ToString() + "</li>";
                }

            taTMHRec.Connection.Close();
            taTMHRec = null;

            taTMHUserRecs.Connection.Close();
            taTMHUserRecs = null;

            if (iResult1 == 1)
            {
                msgVal.Attributes["class"] = "alert alert-danger";
                lblMsg.Text = strMsg;
                return false;
            }

            msgVal.Attributes["class"] = "alert alert-success";
            lblMsg.Text = strMsg;
            return true;
        }

        //---------------------------- Method ---------------------------------------
        // check file size and file type
        protected bool bIsFileTypeOk(string strFileName, int iFileSize)
        {
            if (iFileSize > 2097152) return false; // 2 MB limit
            bool bFileOK = false;
            String FileExtension = Path.GetExtension(strFileName.ToLower());
            String[] allowedExtensions = { ".jpeg", ".jpg", ".gif", ".png" };

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (FileExtension == allowedExtensions[i])
                {
                    bFileOK = true;
                }
            }

            /*
            string[] strTypes = strFileName.Split('.');
            int iFileTypes = strTypes.Length;
            string strFileType = strTypes[iFileTypes - 1];

            if ((strFileType == "jpg") || (strFileType == "jpeg") || (strFileType == "gif")) return true;
            return false;
            */

            return bFileOK;
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
        /// <summary>This method enabels the type ahead lookup attribute of the list boxes.</summary>
        protected void vSetTypeAheadAttrib()
        {
            Type typ = GetType();
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(typ, js)) Page.ClientScript.RegisterClientScriptInclude(typ, js, js);

            //set the type ahead lookup attribute for the dropdown list boxes
            txtCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            SUFFIX_NAME.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlCounty.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlState.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlRace.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlSex.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlHeight.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlEye.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlHair.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");

            ddlHomeType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");

            ddlEnrollingAgency.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");

            ddlContactRelationship.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlContactState.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlContactRelationship2.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlContactState2.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");

            ddlVehType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlVehYear.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlVehMake.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlVehColor.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            ddlVehLicState.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
            //ddlVehLicYear.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);");
        }

        //---------------------------- Method ---------------------------------------
        /// <summary>This method populates the list boxes from the values stored in DB table TMH_Codes.</summary>
        protected void vPopulateAllLists()
        {
            bool bResults0 = true;
            bResults0 = bPopulate1List(lbxDiagnosis, "DIAGNOSIS");
            bResults0 = bPopulate1List(SUFFIX_NAME, "SUFFIX_NAME");
            bResults0 = bPopulate1List(txtCity, "CITY");
            bResults0 = bPopulate1List(ddlCounty, "COUNTY");
            bResults0 = bPopulate1List(ddlState, "STATE");
            bResults0 = bPopulate1List(ddlRace, "RACE");
            bResults0 = bPopulate1List(ddlSex, "SEX");
            bResults0 = bPopulate1List(ddlHeight, "HEIGHT");
            bResults0 = bPopulate1List(ddlEye, "EYE");
            bResults0 = bPopulate1List(ddlHair, "HAIR");

            bResults0 = bPopulate1List(ddlHomeType, "HOMETYPE");
            bResults0 = bPopulate1List(ddlWander, "WANDER");
            bResults0 = bPopulate1List(ddlCommunication, "COMMUNICATION");
            bResults0 = bPopulate1List(lbxSpecial, "SPECIAL");

            bResults0 = bPopulate1List(ddlEnrollingAgency, "LEAGENCY");

            bResults0 = bPopulate1List(ddlContactRelationship, "RELATIONSHIP");
            bResults0 = bPopulate1List(ddlContactState, "STATE");
            bResults0 = bPopulate1List(ddlContactRelationship2, "RELATIONSHIP");
            bResults0 = bPopulate1List(ddlContactState2, "STATE");

            bResults0 = bPopulate1List(ddlVehType, "VEHTYPE");
            bResults0 = bPopulate1List(ddlVehYear, "VEHYEAR");
            bResults0 = bPopulate1List(ddlVehMake, "VEHMAKE");
            bResults0 = bPopulate1List(ddlVehColor, "VEHCOLOR");
            bResults0 = bPopulate1List(ddlVehLicState, "STATE");
            //bResults0 = bPopulate1List(ddlVehLicYear, "VEHLICYEAR");
        }
        //---------------------------- Method ---------------------------------------
        /// <summary>This method runs when the page is being loaded to populate all of the drop down list boxes with values from the TMH_Codes table.</summary>
        /// <returns>true if success, false if fail</returns>
        protected bool bPopulate1List(ListControl lcList1, string strField)
        {
            try
            {
                DataSet1TableAdapters.TMH_CodesTableAdapter taTMH_Codes = new DataSet1TableAdapters.TMH_CodesTableAdapter();
                DataSet1.TMH_CodesDataTable dtTempDT = taTMH_Codes.GetDataByField(strField);

                int iCount1 = dtTempDT.Rows.Count;
                if (dtTempDT != null && iCount1 > 0)
                {
                    lcList1.DataSource = dtTempDT;
                    lcList1.DataTextField = "Description";
                    lcList1.DataValueField = "Code";
                    lcList1.DataBind();
                    //lcList1.Items.Insert(iCount1, "");
                    //lcList1.Items[iCount1].Selected = true;
                    taTMH_Codes.Connection.Close();
                    taTMH_Codes = null;
                    return true;
                }
                else
                {
                    taTMH_Codes.Connection.Close();
                    taTMH_Codes = null;
                    lblMsg.Text = "\r\nError TMH-100, Can't retrieve data for " + strField + " drop down list box, an Error Occured.";
                    return false;
                }
            }

            catch (Exception exc)
            {
                string strError = exc.Message.ToString();
                lblMsg.Text = "\r\nError TMH-100, Can't retrieve data for " + strField + " drop down list box, an Error Occured.";
                return false;
            }
        }

        //---------------------------- Method ---------------------------------------
        /// <summary>This method calculates a person's age in reference to another date</summary>
        /// <param name="birthDt">Birth Date, mmddyy format</param>
        /// <param name="endDt">Reference Date</param>
        /// <returns>Age as an integer</returns>
        protected int GetAge(string birthDt, DateTime endDt)
        {
            // get the total days in between the two dates and divid by 365.25 to get the age.
            // input birthDt is in MM/DD/YYYY format
            try
            {
                //int birthDtYYYY = int.Parse(birthDt.Substring(6, 4));
                //int birthDtMM = int.Parse(birthDt.Substring(0, 2));
                //int birthDtDD = int.Parse(birthDt.Substring(3, 2));

                string[] strAryDOB = birthDt.Split('/');
                int birthDtMM = int.Parse(strAryDOB[0]);
                int birthDtDD = int.Parse(strAryDOB[1]);
                int birthDtYYYY = int.Parse(strAryDOB[2]);

                DateTime birthDtD = new DateTime(birthDtYYYY, birthDtMM, birthDtDD);

                TimeSpan TSdiff = endDt - birthDtD;
                int TotalDays = (int)TSdiff.Days + 1;
                double dAge = (double)TotalDays / 365.25;
                int iAge = (int)dAge;
                if ((iAge > 0) && (iAge < 130)) return iAge;
                else return 0;
            }

            catch (Exception exc)
            {
                string strErr = exc.Message;
                return 0;
            }
        }

        //---------------------------- Method ---------------------------------------
        protected bool bChkRequiredFields()
        {
            // check for the required fields and more...
            bool bReqFields = true;
            strMsg = "<ul type=\"disc\">";

            if (ddlAddress.Text.ToString() == "")
            {
                ddlAddress.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Address Confirmation is required.</li>";
                bReqFields = false;
            }

            if (txtLastName.Text.ToString().Trim().Length < 2)
            {
                txtLastName.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's Last name is required.</li>";
                bReqFields = false;               
            }

            if (txtFirstName.Text.ToString().Length < 2)
            {
                txtFirstName.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's First name is required.</li>";
                bReqFields = false;
            }

            DateTime todayDt = new DateTime();
            todayDt = DateTime.Today;

            strDOB = txtDOB.Text.ToString().Trim();

            iPersonAge = 0;
            try
                {
                    DateTime dtDOB = Convert.ToDateTime(strDOB);

                    iPersonAge = GetAge(strDOB, todayDt);
                    if ((iPersonAge > 0) && (iPersonAge < 130))
                    {
                        txtAge.Text = iPersonAge.ToString();
                    }
                    else
                    {
                        txtDOB.CssClass = "red-border form-control required";
                        strMsg = strMsg + "<li>Person's Date of Birth is required and is not valid.</li>";
                        bReqFields = false;
                    }
                }
            catch
                {
                    txtDOB.CssClass = "red-border form-control required";
                    strMsg = strMsg + "<li>Person's Date of Birth is not valid.</li>";
                    bReqFields = false;
                }

            string strPDT = txtPhotoDate.Text.ToString().Trim();
            int iPhotoAge = 0;
            try
            {
                DateTime dtPDT = Convert.ToDateTime(strPDT);

                iPhotoAge = GetAge(strPDT, todayDt);
                if ((iPhotoAge >= 0) && (iPhotoAge < 130))
                {
                    //txtPhotoAge.Text = iPhotoAge.ToString();
                }
                else
                {
                    txtPhotoDate.CssClass = "red-border form-control required";
                    strMsg = strMsg + "<li>Photo Date is required and is not valid.</li>";
                    bReqFields = false;
                }
            }
            catch
            {
                txtPhotoDate.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Photo Date is not valid.</li>";
                bReqFields = false;
            }

            /*
            try
            {
                DateTime dtPhotoDate = Convert.ToDateTime(txtPhotoDate.Text);
            }
            catch
            {
                txtPhotoDate.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Photo Date is required and it is not valid.</li>";
                bReqFields = false;
            }
            */

            if (!bEditChecks()) bReqFields = false;

            string strDiagValue = "";
            foreach (ListItem item in lbxDiagnosis.Items)
            {
                if (item.Selected) strDiagValue = strDiagValue + item.Value + ",";
            }
            if (strDiagValue.Length < 2)
            {
                lbxDiagnosis.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Diagnosis/Disability is required.</li>";
                bReqFields = false;
            }

            if (txtNameToCallMe.Text.ToString().Trim().Length < 2)
            {
                txtNameToCallMe.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Name To Call Me is required.</li>";
                bReqFields = false;
            }

            if (txtHomePhone.Text.ToString().Trim().Length < 7)
            {
                txtHomePhone.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's Home Phone is required.</li>";
                bReqFields = false;
            }

            if (txtAddressNumber.Text.ToString().Trim().Length < 1)
            {
                txtAddressNumber.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's Address Number is required.</li>";
                bReqFields = false;
            }

            if (txtAddressStreet.Text.ToString().Trim().Length < 2)
            {
                txtAddressStreet.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's Address Street is required.</li>";
                bReqFields = false;
            }

            if (txtCity.Text.ToString().Trim().Length < 2)
            {
                txtCity.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's Address City is required.</li>";
                bReqFields = false;
            }
            if (ddlCounty.SelectedValue.ToString().Trim().Length < 2)
            {
                ddlCounty.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's Address County is required.</li>";
                bReqFields = false;
            }
            if (ddlState.SelectedValue.ToString().Trim().Length < 2)
            {
                ddlState.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's Address State is required.</li>";
                bReqFields = false;
            }

            if (txtZipCode.Text.ToString().Trim().Length < 5)
            {
                txtZipCode.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Person's Address Zip Code is required.</li>";
                bReqFields = false;
            }

            if (ddlEnrollingAgency.SelectedValue.ToString().Trim() == "")
            {
                ddlEnrollingAgency.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Law Enforcement Agency is required.</li>";
                bReqFields = false;
            }

            if (ddlContactRelationship.SelectedValue.ToString().Trim() == "")
            {
                ddlContactRelationship.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Contact Relationship is required.</li>";
                bReqFields = false;
            }

            if (txtContactFullName.Text.ToString().Trim() == "")
            {
                txtContactFullName.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Contact Full Name is required.</li>";
                bReqFields = false;
            }

            if (txtContactAddress.Text.ToString().Trim() == "")
            {
                txtContactAddress.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Contact Address is required.</li>";
                bReqFields = false;
            }
            if (txtContactCity.Text.ToString().Trim() == "")
            {
                txtContactCity.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Contact City is required.</li>";
                bReqFields = false;
            }
            if (ddlContactState.SelectedValue.ToString().Trim() == "")
            {
                ddlContactState.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Contact State is required.</li>";
                bReqFields = false;
            }
            if (txtContactZip.Text.ToString().Trim() == "")
            {
                txtContactZip.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Contact Zip is required.</li>";
                bReqFields = false;
            }

            if ((txtContactHPhone.Text.ToString().Trim() == "") && (txtContactMPhone.Text.ToString().Trim() == "") && (txtContactOPhone.Text.ToString().Trim() == ""))
            {
                txtContactHPhone.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>At least one Contact Phone number is required.</li>";
                bReqFields = false;
            }

            bNewRecord = (bool) HttpContext.Current.Session["NewRecord"];

            if (bNewRecord)
            {
                if (!FileUpload.HasFile)
                {
                    strMsg = strMsg + "<li>Photo is required.</li>";
                    bReqFields = false;
                }
            }

            /* *** fix this for edit/update photo if no photo provided ***
            else
            {
                if (lblPhoto.Text == "")
                {
                    strMsg = strMsg + "<li>Photo is required.</li>";
                    bReqFields = false;
                }
            }
            */

            if (AGE_IN_PHOTOStart.Text.ToString().Trim() == "")
            {
                AGE_IN_PHOTOStart.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Age in Photo is required.</li>";
                bReqFields = false;
            }

            strMsg += "</ul>";

            return bReqFields;
        }

        //---------------------------- Method ---------------------------------------
        protected bool bEditChecks()
        {
            // check for the field formats if data for the field is provided
            bool bFieldEdits = true;


            if ((txtLastName.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtLastName.Text.ToString().Trim(), @"^[a-zA-Z -_./s]{1,15}$")))
            {
                txtLastName.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Invalid last name format, 1-15 alpha characters expected and . - are ok.</li>";
                bFieldEdits = false;
                //throw new FormatException("Invalid last name format");
            }

            if ((txtFirstName.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtFirstName.Text.ToString().Trim(), @"^[a-zA-Z -_./s]{1,14}$")))
            {
                txtFirstName.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Invalid first name format, 1-14 alpha characters expected and . - are ok.</li>";
                bFieldEdits = false;
            }

            /*
            if ((txtDOB.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtDOB.Text.ToString().Trim(), @"([0-9]|1[012])[/]([0-9]|[12][0-9]|3[01])[/](19|20)\d\d")))
            {
                strMsg = strMsg + "\r\nInvalid Birth Date format, MM/DD/YYYY expected.";
                bFieldEdits = false;
            }            
            */

            if ((txtNameToCallMe.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtNameToCallMe.Text.ToString().Trim(), @"^[a-zA-Z -_./s]{1,50}$")))
            {
                txtNameToCallMe.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Invalid Name To Call Me format, 1-50 alpha characters expected and . - are ok.</li>";
                bFieldEdits = false;
            }

            if ((txtHomePhone.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtHomePhone.Text.ToString().Trim(), @"(\d{3})-(\d{3})-(\d{4})")))
            {
                txtHomePhone.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Invalid Home Phone format, 999-999-9999 expected.</li>";
                bFieldEdits = false;
            }

            if ((txtZipCode.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtZipCode.Text.ToString().Trim(), @"(^\d{5}$)|(^\d{5}-\d{4}$)")))
            {
                txtZipCode.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Invalid Zip Code format, 99999 or 99999-9999.</li>";
                bFieldEdits = false;
            }

            if ((txtContactFullName.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactFullName.Text.ToString().Trim(), @"^[a-zA-Z '-_./s]{1,50}$")))
            {
                txtContactFullName.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Invalid Contact Full Name format, 1-50 alpha characters expected and . - are ok.</li>";
                bFieldEdits = false;
            }

            if ((txtContactZip.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactZip.Text.ToString().Trim(), @"(^\d{5}$)|(^\d{5}-\d{4}$)")))
            {
                txtContactZip.CssClass = "red-border form-control required";
                strMsg = strMsg + "<li>Invalid Contact Zip Code format, 99999 or 99999-9999.</li>";
                bFieldEdits = false;
            }
            if ((txtContactHPhone.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactHPhone.Text.ToString().Trim(), @"(\d{3})-(\d{3})-(\d{4})")))
            {
                strMsg = strMsg + "<li>Invalid Contact Home Phone format, 999-999-9999 expected.</li>";
                bFieldEdits = false;
            }

            if ((txtContactMPhone.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactMPhone.Text.ToString().Trim(), @"(\d{3})-(\d{3})-(\d{4})")))
            {
                strMsg = strMsg + "<li>Invalid Contact Mobile Phone format, 999-999-9999 expected.</li>";
                bFieldEdits = false;
            }

            if ((txtContactOPhone.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactOPhone.Text.ToString().Trim(), @"(\d{3})-(\d{3})-(\d{4})")))
            {
                strMsg = strMsg + "<li>Invalid Contact Other Phone format, 999-999-9999 expected.</li>";
                bFieldEdits = false;
            }

            if ((txtContactEMail.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactEMail.Text.ToString().Trim(), @"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b")))
            {
                strMsg = strMsg + "<li>Invalid Contact Email Address.</li>";
                bFieldEdits = false;
            }

            //CONTACT2
            if ((txtContactFullName2.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactFullName2.Text.ToString().Trim(), @"^[a-zA-Z '-_./s]{1,50}$")))
            {
                strMsg = strMsg + "<li>Invalid Contact2 Full Name format, 1-50 alpha characters expected and . - are ok.</li>";
                bFieldEdits = false;
            }

            if ((txtContactZip2.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactZip2.Text.ToString().Trim(), @"(^\d{5}$)|(^\d{5}-\d{4}$)")))
            {
                strMsg = strMsg + "<li>Invalid Contact2 Zip Code format, 99999 or 99999-9999.</li>";
                bFieldEdits = false;
            }
            if ((txtContactHPhone2.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactHPhone2.Text.ToString().Trim(), @"(\d{3})-(\d{3})-(\d{4})")))
            {
                strMsg = strMsg + "<li>Invalid Contact2 Home Phone format, 999-999-9999 expected.</li>";
                bFieldEdits = false;
            }

            if ((txtContactMPhone2.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactMPhone2.Text.ToString().Trim(), @"(\d{3})-(\d{3})-(\d{4})")))
            {
                strMsg = strMsg + "<li>Invalid Contact2 Mobile Phone format, 999-999-9999 expected.</li>";
                bFieldEdits = false;
            }

            if ((txtContactOPhone2.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactOPhone2.Text.ToString().Trim(), @"(\d{3})-(\d{3})-(\d{4})")))
            {
                strMsg = strMsg + "<li>Invalid Contact2 Other Phone format, 999-999-9999 expected.</li>";
                bFieldEdits = false;
            }

            if ((txtContactEMail2.Text.ToString().Trim().Length > 0) && (!Regex.IsMatch(txtContactEMail2.Text.ToString().Trim(), @"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b")))
            {
                strMsg = strMsg + "<li>Invalid Contact2 Email Address.</li>";
                bFieldEdits = false;
            }

            if (AGE_IN_PHOTOStart.Text.ToString().Trim().Length > 0)
            {
                if (Regex.IsMatch(AGE_IN_PHOTOStart.Text.ToString().Trim(), @"^[1-9]+[0-9]*$"))
                {
                    int iAgeinPhoto = int.Parse(AGE_IN_PHOTOStart.Text.ToString().Trim());
                    if ((iAgeinPhoto > 120) || (iAgeinPhoto < 1))
                    {
                        AGE_IN_PHOTOStart.CssClass = "red-border form-control required";
                        strMsg = strMsg + "<li>Invalid Age in Photo format, 1 to 120 expected.</li>";
                        bFieldEdits = false;
                    }
                }
                else
                {
                    AGE_IN_PHOTOStart.CssClass = "red-border form-control required";
                    strMsg = strMsg + "<li>Invalid Age in Photo format, 1 to 120 expected.</li>";
                    bFieldEdits = false;
                }

            }

            return bFieldEdits;
        }

        //---------------------------- Method ---------------------------------------
        protected void ResetFields(ControlCollection pageControls)
        {
            HttpContext.Current.Session["SelectedID"] = "";
            HttpContext.Current.Session["NewRecord"] = "";

            foreach (Control contl in pageControls)
            {
                string strCntName = (contl.GetType()).Name;
                switch (strCntName)
                {
                    case "CheckBox":
                        CheckBox cbSource = (CheckBox)contl;
                        cbSource.Checked = false;
                        break;

                    case "CheckBoxList":
                        CheckBoxList cblSource = (CheckBoxList)contl;
                        cblSource.SelectedIndex = -1;
                        break;

                    case "DropDownList":
                        DropDownList ddlSource = (DropDownList)contl;
                        ddlSource.SelectedIndex = -1;
                        break;

                    case "ListBox":
                        ListBox lbSource = (ListBox)contl;
                        lbSource.SelectedIndex = -1;
                        break;

                    case "RadioButtonList":
                        RadioButtonList rblSource = (RadioButtonList)contl;
                        rblSource.SelectedIndex = -1;
                        break;

                    case "TextBox":
                        TextBox tbSource = (TextBox)contl;
                        tbSource.Text = "";
                        break;
                }
            }
        }

        //---------------------------- Method ---------------------------------------
        protected void ClearFields(string strMode)
        {
            // if strMode = "ADDNEW" from ADD a NEW Record path

            // Populate the fields in all pages with no values, clear all fields
            lblMsg.Text = "";
            HttpContext.Current.Session["SelectedID"] = "";
            HttpContext.Current.Session["NewRecord"] = "";
            HttpContext.Current.Session["PhotoID"] = "";

            // person fields           
            foreach (ListItem item in lbxDiagnosis.Items)
            {
                if (item.Selected) item.Selected = false;
            }

            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            SUFFIX_NAME.Text = "";
            txtNameToCallMe.Text = "";
            txtHomePhone.Text = "";
            txtAddressNumber.Text = "";
            txtAddressStreet.Text = "";
            txtCity.Text = "";
            ddlCounty.SelectedValue = "";
            ddlState.SelectedValue = "";
            txtZipCode.Text = "";

            // Physical Description fields
            txtDOB.Text = "";
            txtAge.Text = "";

            ddlRace.SelectedValue = "";
            ddlSex.SelectedValue = "";
            ddlHeight.SelectedValue = "";
            txtWeight.Text = "";
            ddlEye.SelectedValue = "";
            ddlHair.SelectedValue = "";

            // Special Fields
            ddlHomeType.SelectedValue = "";
            ddlWander.SelectedValue = "";
            foreach (ListItem item in ddlCommunication.Items)
            {
                if (item.Selected) item.Selected = false;
            }

            ddlMedication.SelectedValue = "";

            txtLanguages.Text = "";
            txtMedical.Text = "";
            txtWornItems.Text = "";
            txtApproach.Text = "";
            txtBehaviors.Text = "";

            foreach (ListItem item in lbxSpecial.Items)
            {
                if (item.Selected) item.Selected = false;
            }

            // Contact Fields
            ddlContactRelationship.SelectedValue = "";

            txtContactFullName.Text = "";
            txtContactAddress.Text = "";
            txtContactCity.Text = "";

            ddlContactState.SelectedValue = "";

            txtContactZip.Text = "";
            txtContactHPhone.Text = "";
            txtContactMPhone.Text = "";
            txtContactOPhone.Text = "";
            txtContactEMail.Text = "";

            // Contact2 Fields
            ddlContactRelationship2.SelectedValue = "";

            txtContactFullName2.Text = "";
            txtContactAddress2.Text = "";
            txtContactCity2.Text = "";

            ddlContactState2.SelectedValue = "";

            txtContactZip2.Text = "";
            txtContactHPhone2.Text = "";
            txtContactMPhone2.Text = "";
            txtContactOPhone2.Text = "";
            txtContactEMail2.Text = "";

            // Vehicle Fields
            ddlVehType.SelectedValue = "";

            ddlVehYear.SelectedValue = "";

            ddlVehMake.SelectedValue = "";

            txtVehModel.Text = "";
            ddlVehColor.SelectedValue = "";

            txtVehVIN.Text = "";
            txtVehLic.Text = "";
            ddlVehLicState.SelectedValue = "";
            ddlVehLicState.SelectedIndex = 0;

            //ddlVehLicYear.SelectedValue = "";

            // Photo Fields
            txtSubmittedDateTime.Text = "";
            lblPhoto.Text = "";
            AGE_IN_PHOTOStart.Text = "";
            txtPhotoDate.Text = "";
            lblImage.Text = "";
            //ddlApproved.SelectedIndex = 0;

            if (strMode == "ADDNEW")
            {
                ddlContactRelationship.SelectedIndex = 0;
                lbxSpecial.SelectedIndex = 0;
                ddlContactState.SelectedIndex = 0;
                ddlContactRelationship2.SelectedIndex = 0;
                ddlContactState2.SelectedIndex = 0;
                ddlVehType.SelectedIndex = 0;
                ddlVehYear.SelectedIndex = 0;
                ddlVehMake.SelectedIndex = 0;
                ddlVehColor.SelectedIndex = 0;
                //ddlVehLicYear.SelectedIndex = 0;

                lbxDiagnosis.SelectedIndex = 0;
                ddlCounty.SelectedIndex = 0;
                ddlState.SelectedIndex = 0;
                ddlRace.SelectedIndex = 0;
                ddlSex.SelectedIndex = 0;
                ddlHeight.SelectedIndex = 0;
                ddlEye.SelectedIndex = 0;
                ddlHair.SelectedIndex = 0;
                ddlCommunication.SelectedIndex = 0;
                ddlMedication.SelectedIndex = 0;
                txtContactEMail.Text = HttpContext.Current.Session["TMHUser"].ToString().Trim();
                lblImage.Text = "<img id=\"image\" class=\"img-responsive center-block\" alt=\"Responsive image\" src=\"Content/placeholder.jpg\">";
            }
        }
    }
}