<%@ Page Title="- Session Sign Out" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signout.aspx.cs" Inherits="TMHSelf.signout" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script>
			document.execCommand("ClearAuthenticationCache")
			//window.close();
	</script>
    
</asp:Content>

