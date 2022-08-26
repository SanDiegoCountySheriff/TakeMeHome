<%@ Page Title="Registration Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="TMHSelf.MyAccount" %>
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

            <h1>Update User!
                <small>
                    <span class="text-danger">*Indicates Required Fields</span>
                </small>
            </h1>
             
            <div id="msgVal" runat="server" class=" alert alert-info" role="alert">
                <asp:Label runat="server" ID="lblMsg"></asp:Label>
            </div>
             
            <p>
            <a id="urlChgPass" name="urlChgPass" runat="server" href="ChangePass.aspx" class="btn btn-lg btn-primary">Change Password</a>
            </p>
            
            <div class="form-group">
                <label for="useremail1" class="col-md-2 control-label">
                    <span class="text-danger">*Current E-Mail Address:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:Label runat="server" ID="useremail1" cssclass="form-control" TextMode="Email"></asp:Label>
                </div>  
                <div class="col-md-6">                                                 
                </div>                      
            </div>

             <div class="form-group">
                <label for="useremail" class="col-md-2 control-label">
                    <span class="text-danger">*New E-Mail Address:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="useremail" MaxLength="200" cssclass="form-control required" TextMode="Email"></asp:TextBox>
                </div>  
                <div class="col-md-6">  If you like to change it, Type your new E-Mail Address here                                               
                </div>                      
            </div>
             
            <div class="form-group">
                <label for="userpin1" class="col-md-2 control-label">
                    <span class="text-danger">*Current 4 Digit PIN Number:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:Label runat="server" ID="userpin1" cssclass="form-control" TextMode="Number"></asp:Label>
                </div>  
                <div class="col-md-6">Used when Forgot Password, can be the last 4 digits of your SSN, or last 4 digits of your phone number, or last 4 digits of your birth date YYMM.                                                 
                </div>                      
            </div>
   
              
            <div class="form-group">
                <label for="userpin" class="col-md-2 control-label">
                    <span class="text-danger">*New 4 Digit PIN Number:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userpin" MaxLength="4" cssclass="form-control required" TextMode="Number"></asp:TextBox>
                </div>  
                <div class="col-md-6">  If you like to change it, Type your new 4 Digit PIN Number here                    
                </div> 
            </div>                     
                          
            <div class="form-group">  
                <label for="" class="col-md-2 control-label">                                   
                </label>                                      
                <div class="col-md-4">                        
                    <asp:Button runat="server" ID="btnCancel" cssclass="btn btn-default" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
                    <asp:Button runat="server" ID="btnUpdate" cssclass="btn btn-primary" Text="Update" OnClick="btnUpdate_Click"></asp:Button>                                    
                </div>
                <div class="col-md-2">                                                 
                </div>                      
            </div>                      
         

    <script src="Scripts/jquery-3.6.0.min.js"></script>
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


