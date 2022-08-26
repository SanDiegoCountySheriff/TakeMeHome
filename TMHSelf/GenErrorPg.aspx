<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenErrorPg.aspx.cs" Inherits="TMHSelf.GenErrorPg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   <div>
        <h2>An Error Has Occurred</h2> 
        <hr />
        <p>
            An unexpected error occurred on our website. 
            <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="https://www.sdsheriff.gov/">Return to the home page</asp:HyperLink>
        </p>
    </div>

</asp:Content>
