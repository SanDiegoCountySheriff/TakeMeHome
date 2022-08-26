<%@ Page Title="Registration Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="TMHSelf.index" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


        <style> 
            @viewport {width:device-width;}
            /* SN Password styles*/
            .progress {margin-bottom: 0 !important;}
            .password-verdict { color:#000; }
            /* SN End*/
        </style>


        <h1 class="text-danger" style="display:none;">Warning: JavaScript is turned off! This Webform does not work properly without JavaScript.</h1>
        <!--[if lt IE 8]>    
        <h1 class="text-info">You are using an <strong>outdated</strong> browser or running your browser in an outdated mode. Please <a href="http://browsehappy.com/?locale=en/">upgrade your browser</a> to improve your experience on this site.</h1>
        <![endif]-->      
        
         <a href="default.aspx" title="Take Me Home Main Page">
            <img class="img-responsive center-block" src="Content/new-tmh-logo.png" alt="Take Me Home Registration Portal" /></a>

            <h1>New User Sign-up.
                <small>
                    <span class="text-danger">*Indicates Required Fields</span>
                </small>
            </h1>
             
             
                 
            
             <div id="msgVal" runat="server" class=" alert alert-info" role="alert">
                <asp:Label runat="server" ID="lblMsg"></asp:Label>
            </div>               
            
             <asp:Panel ID="pnlAddUser" runat="server" Visible="true">

             <div class="form-group">
                <label for="useremail" class="col-md-2 control-label">
                    <span class="text-danger">*E-Mail Address:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="useremail" MaxLength="200" cssclass="form-control required" TextMode="Email"></asp:TextBox>
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>   

            <div class="form-group password-container">
                <label for="userpassword" class="col-md-2 control-label">
                    <span class="text-danger">*Password</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userpassword" MaxLength="100" cssclass="form-control required" TextMode="Password"></asp:TextBox>
                </div>
                <div class="col-md-6">   
                    Please refer to password complexity requirements below.                                                          
                </div> 
            </div>
            <div class="form-group  password-container">
                <label for="userpasswordc" class="col-md-2 control-label">
                    <span class="text-danger">*Confirm Password</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userpasswordC" MaxLength="100" cssclass="form-control required" TextMode="Password"></asp:TextBox>
                </div>
                <div class="col-md-6">     
                    Please refer to password complexity requirements below.                                                             
                </div> 
            </div>                    
            <div class="form-group">
                <label for="userssn" class="col-md-2 control-label">
                    <span class="text-danger">*4 Digit PIN Number:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userpin" MaxLength="4" cssclass="form-control required" TextMode="Number"></asp:TextBox>
                </div>  
                <div class="col-md-6">                 
                    Used for password reset. The PIN can be any 4 digits, for example the last 4 digits of your SSN, last 4 digits of your phone number, or last 4 digits of your birth date in the YYMM format.                     
                </div> 
            </div>                     
                          
            <div class="form-group">  
                <label for="" class="col-md-2 control-label">                                   
                </label>                                      
                <div class="col-md-4">                        
                    <asp:Button runat="server" ID="btnCancel" cssclass="btn btn-default" Text="Cancel" OnClick="btnAddUserCancel_Click"></asp:Button>  
                    <asp:Button runat="server" ID="btnAddUser" cssclass="btn btn-primary" Text="Add Me" OnClick="btnAddUser_Click"></asp:Button>                                    
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>                      
         
              

            <div class="form-group">
                <label for="" class="col-md-2 control-label">                                   
                </label>                          
                <div class="col-md-6">
                    <p><a href="TMHLogin.aspx">Back to Take Me Home Login!</a></p>
                    
                </div>  
                <div class="col-md-6">  
                                                                 
                </div>                      
            </div>

            <div>
                


			<div class="text-info" >              
              <p>The passwords you choose must:</p> 				
				<ul>
					<li>be at least 8 characters long.</li>
                    <li>contain both upper and lower case characters.</li>
					<li>contain at least one numeric character.</li>
					<li>not repeat a previous password used on the account.</li>
					<li>not contain your account or full name.</li>
                </ul>              
            </div>  
                            
			
             </div>

            </asp:Panel>

            <asp:Panel ID="pnlindexPage" runat="server" Visible="false">
                <div class="form-group">
                    <label for="" class="col-md-2 control-label">                                   
                </label>                          
                <div class="col-md-4">
                    <p><a href="default.aspx">Click Here To Continue</a></p>
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>


             </asp:Panel>



    <script src="Scripts/pwstrength.js"></script>
    <script>
            jQuery(document).ready(function () {
                 "use strict";
                 var options = {};
                 options.ui = {
                    showVerdictsInsideProgressBar: true               
                 };
                 $('#userpassword').pwstrength(options);
           
                 var optionsc = {};
                 optionsc.ui = {
                     showVerdictsInsideProgressBar: true,                    
                 };
                 $('#userpasswordC').pwstrength(optionsc);

                 //SN quick validate form fields
                 $(".required").blur(function () {
                     var fieldlength = $(this).val().length;
                     if (fieldlength === 0) {
                         //alert("Oops, can't skip this field.");
                         $(this).addClass("red-border");
                     } else {
                         $(this).removeClass("red-border");
                     }

                 });

             });

    </script>
           
</asp:Content>
