<%@ Page Title="Registration Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TMHLogin.aspx.cs" Inherits="TMHSelf.TMHLogin" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


<!-- Global site tag (gtag.js) - Google Analytics -->
<script async="async" src="https://www.googletagmanager.com/gtag/js?id=UA-136214989-1"></script>
<script>
    window.dataLayer = window.dataLayer || [];
    function gtag() { dataLayer.push(arguments); }
    gtag('js', new Date());

    gtag('config', 'UA-136214989-1');
</script>

        <h1 class="text-danger" style="display:none;">Warning: JavaScript is turned off! This Webform does not work properly without JavaScript.</h1>
        <!--[if lt IE 8]>    
        <h1 class="text-info">You are using an <strong>outdated</strong> browser or running your browser in an outdated mode. Please <a href="http://browsehappy.com/?locale=en/">upgrade your browser</a> to improve your experience on this site.</h1>
        <![endif]-->      
        <img class="img-responsive center-block" src="Content/new-tmh-logo.png" alt="Take Me Home Registration Portal" />                           
        <p class="lead">
            The San Diego County Sheriff's Department has developed this registration portal for use by San Diego County residents 
             to collect information (data, picture, and contact information) about individuals with special needs 
            (For example, people with autism, Alzheimer's patients etc.). San Diego County residents should use this web page to 
            register a person with special needs. In cases where the special needs person is contacted by law enforcement, 
            the system assists in providing accurate identification and emergency contact information to ensure their safe
            return home.  
        </p>
        <p class="lead text-warning center-block">
            Before you begin, please read these <a style="text-decoration:underline;" href="TMHDoc.pdf" target="_blank">Instructions and Photo guidelines</a>. 
        </p>
      

            <h1>Sign in to get started!
            <small><span class="text-danger">*Indicates Required Fields</span></small>

            </h1> 
            <div id="msgVal" runat="server" class=" alert alert-info" role="alert">
                <asp:Label runat="server" ID="lblMsg"></asp:Label>
            </div>                      
            <div class="form-group">
                <label for="useremail" class="col-md-2 control-label">
                    <span class="text-danger">*E-Mail Address:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" id="useremail" MaxLength="200" text="" cssclass="form-control" TextMode="Email"></asp:TextBox>
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>                         
            <div class="form-group">
                <label for="userpassword" class="col-md-2 control-label">
                    <span class="text-danger">*Password</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userpassword" MaxLength="100" cssclass="form-control" TextMode="Password"></asp:TextBox>
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>                       
            <%--<div class="form-group">
                <label for="userssn" class="col-md-2 control-label">
                    Last 4 SSN:
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userssn" cssclass="form-control" TextMode="Number"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div> 
                    --%>             
            <div class="form-group">
                <label for="userremember" class="col-md-2 control-label">                                   
                </label>                          
                <div class="col-md-4">
                        <asp:Checkbox runat="server" ID="userremember" cssclass="" ></asp:Checkbox>
                        Remember me!
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>
                         
            <div class="form-group">
                <label for="btnLogin" class="col-md-2 control-label">                                   
                </label>                          
                <div class="col-md-4">
                        <asp:Button runat="server" ID="btnLogin" cssclass="btn btn-primary" Text="Log Me In!" OnClick="btnLogin_Click"></asp:Button>                                      
                </div>  
                <div class="col-md-4"> </div>                      
            </div>               
            <div class="form-group">
                <label for="" class="col-md-2 control-label">                                   
                </label>                          
                <div class="col-md-4">
                    <p>Don't have an account? <a href="AddUser.aspx">Sign Me Up!</a></p>
                    <p>Forgot your password? <a href="ForgotPass.aspx">Forgot Password</a></p>   
                </div>  
                <div class="col-md-2"></div>                      
            </div>

            
        
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

