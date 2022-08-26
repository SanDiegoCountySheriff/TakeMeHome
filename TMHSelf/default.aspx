<%@ Page Title="Registration Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="TMHSelf._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <style>

        .panel {
           margin-top:10px;
           margin-bottom:10px;
            padding-left:20px;
             padding-bottom:20px;
            border: solid 1px #c4c2c2;
            border-radius: 4px;
        }

    </style>


        <h1 class="text-danger" style="display:none;">Warning: JavaScript is turned off! This Webform does not work properly without JavaScript.</h1>
        <!--[if lt IE 8]>
        <h1 class="text-info">You are using an <strong>outdated</strong> browser or running your browser in an outdated mode. Please <a href="http://browsehappy.com/?locale=en/">upgrade your browser</a> to improve your experience on this site.</h1>
        <![endif]-->
    
        <div class="row">
                <h3 class="pull-right">
                   <a href="MyAccount.aspx" >My Account</a> | 
                   <a href="signout.aspx" >Sign Out</a>                              
                </h3>
            </div>
             <img class="img-responsive center-block" src="Content/new-tmh-logo.png" alt="Take Me Home Registration Portal" />
           <h3>
               Welcome, you are signed in as: 
               <strong>
                   <span><asp:Label ID="lblUser" runat="server"></asp:Label>   </span>
               </strong>   -
               <asp:Label ID="lblCurrDtTm" runat="server"></asp:Label>
            </h3>

       
        <p class="lead text-warning center-block">
            You may enter up to four persons into the TMH Self-Registry System. We encourage you to sign back in and update information about your registrations as frequently as neccessary. 
            If you need help, please read these <a  href="TMHDoc.pdf" target="_blank">Instructions</a>.
        </p>
        <p class="lead text-danger">
            <asp:Label runat="server" ID="lblMsg"></asp:Label>
        </p>
        <asp:Panel ID="pnlRecList" runat="server">
        <p class="lead text-danger">
            You are allowed to register a total of <asp:Label ID="lblLimit" runat="server"></asp:Label> persons.<br />
            You currently have <strong><asp:Label ID="lblRegCount" runat="server"></asp:Label></strong>registered. To edit details or delete, please see below.
        </p>
        </asp:Panel>
                
        <p>
            <a id="urlRegister" name="urlRegister" runat="server" href="Register.aspx?Operation=Add" class="btn btn-lg btn-primary">Register a New Person</a>
        </p>

        <div class="row">      
            <asp:Label runat="server" ID="lblRecords"></asp:Label>
        </div>

</asp:Content>


