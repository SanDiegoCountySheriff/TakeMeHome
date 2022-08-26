<%@ Page Title="Registration Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPass.aspx.cs" Inherits="TMHSelf.ForgotPass" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



        <h1 class="text-danger" style="display:none;">Warning: JavaScript is turned off! This Webform does not work properly without JavaScript.</h1>
        <!--[if lt IE 8]>    
        <h1 class="text-info">You are using an <strong>outdated</strong> browser or running your browser in an outdated mode. Please <a href="http://browsehappy.com/?locale=en/">upgrade your browser</a> to improve your experience on this site.</h1>
        <![endif]-->      
         <a href="default.aspx" title="Take Me Home Main Page">
        <img class="img-responsive center-block" src="Content/new-tmh-logo.png" alt="Take Me Home Registration Portal" />                           
        </a>

            <h1>Password Reset.</h1>        
                   <div id="msgVal" runat="server" class=" alert alert-info" role="alert">
                <asp:Label runat="server" ID="lblMsg"></asp:Label>
            </div>                  
            <div class="form-group">
                <label for="useremail" class="col-md-2 control-label">
                    <span class="text-danger">*E-Mail Address:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="useremail" MaxLength="200" cssclass="form-control" TextMode="Email"></asp:TextBox>
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>                         
            <div class="form-group">
                <label for="userPIN" class="col-md-2 control-label">
                    <span class="text-danger">*4 Digit PIN</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userPIN" MaxLength="4" cssclass="form-control" TextMode="Number"></asp:TextBox>
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>                       
                          
            <div class="form-group">
                <label for="btnSendPass" class="col-md-2 control-label">                                   
                </label>                          
                <div class="col-md-4">
                        <asp:Button runat="server" ID="btnCancel" cssclass="btn btn-default" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>  
                        <asp:Button runat="server" ID="btnSendPass" cssclass="btn btn-primary" Text="eMail Me a Password!" OnClick="btnSendPass_Click"></asp:Button>                                      
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>               
            <div class="form-group">
                <label for="" class="col-md-2 control-label">                                   
                </label>                          
                <div class="col-md-4">
                    <p><a href="TMHLogin.aspx">Back to TMH Login!</a></p>
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>

             
    <script src="Scripts/jquery-3.6.0.min.js"></script>
        
    <script>
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
    </script>
</asp:Content>

