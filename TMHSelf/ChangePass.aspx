<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="TMHSelf.ChangePass" %>

<!DOCTYPE html>

<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7"> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8"> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9"> <![endif]-->
<!--[if gt IE 8]><!--> <html class="no-js"> <!--<![endif]-->
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <meta name="format-detection" content="telephone=no">
        <title>Take Me Home Registration Portal</title>
        <link rel="shortcut icon" href="/tmhself/Content/favicon.ico" type="image/x-icon">
        <link rel="icon" href="/tmhself/Content/favicon.ico" type="image/x-icon">
        <link rel="apple-touch-icon" href="/tmhself/Content/AppIcon76x76.png">
        <link rel="apple-touch-icon" sizes="76x76" href="/tmhself/Content/AppIcon76x76.png">
        <link rel="apple-touch-icon" sizes="120x120" href="/tmhself/Content/AppIcon60x60@2x.png">
        <link rel="apple-touch-icon" sizes="152x152" href="/tmhself/Content/AppIcon76x76@2x.png">
        <meta name="msapplication-TileImage" content="/tmhself/Content/AppIcon72x72@2x.png">
        <meta name="msapplication-TileColor" content="#222222">

        <link href="/tmhself/Content/bootstrap.min.css" rel="stylesheet" />
        <style> 
            @viewport {width:device-width;}
              /* SN Password styles*/
            .progress {margin-bottom: 0 !important;}
            .password-verdict { color:#000; }
            /* SN End*/
            .red-border { border-color:#c9302c;  }
            .no-js h1 { display:block !important;}
            .no-js .form-horizontal, #crop-gui {display:none;}
            #map {height:300px;}
            #optionalpanel1, #optionalpanel2, #optionalpanel3, #optional1b, #optional2b, #optional3b {display:none;}
        </style>
           <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
            <!--[if lt IE 9]>
              <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
              <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
            <![endif]-->
        <script src="Scripts/modernizr.custom.83114.min.js"></script>
</head>

<body id="PageBody" runat="server">
    <div class="container body-content">
        <h1 class="text-danger" style="display:none;">Warning: JavaScript is turned off! This Webform does not work properly without JavaScript.</h1>
        <!--[if lt IE 8]>    
        <h1 class="text-info">You are using an <strong>outdated</strong> browser or running your browser in an outdated mode. Please <a href="http://browsehappy.com/?locale=en/">upgrade your browser</a> to improve your experience on this site.</h1>
        <![endif]-->      
         <a href="default.aspx" title="Take Me Home Main Page">
        <img class="img-responsive center-block" src="Content/new-tmh-logo.png" alt="Take Me Home Registration Portal" /></a>

         <form id="form1" runat="server" class="form-horizontal">               
            <h1>TMH Self-Registry, Change Password!</h1>
            
            
              <div id="msgVal" runat="server" class=" alert alert-info" role="alert">
                <asp:Label runat="server" ID="lblMsg"></asp:Label>
            </div>               
            
             
            <asp:Panel ID="pnlChangePass" runat="server" Visible="true">
                                        
            <div class="form-group">
                <label for="useremail" class="col-md-2 control-label">
                    <span class="text-danger">*E-Mail Address:</span>
                </label>                          
                <div class="col-md-4">
                    <asp:Label runat="server" ID="useremail" cssclass="form-control"></asp:Label>
                </div>  
                <div class="col-md-2">                                                 
                </div>                      
            </div>
                                        
            <div class="form-group">
                <label for="userpasswordOld" class="col-md-2 control-label">
                    <span class="text-danger">*Old Password</span>
                </label>                          
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userpasswordOld" MaxLength="100" cssclass="form-control" TextMode="Password"></asp:TextBox>
                </div>
            </div>

            <div class="form-group password-container">
                <label for="userpasswordNew" class="col-md-2 control-label">
                    <span class="text-danger">*New Password</span>
                </label>   
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userpasswordNew" MaxLength="100" cssclass="form-control" TextMode="Password"></asp:TextBox>
                </div>   
                <div class="col-md-6">   Type your new password here                                                
                </div>                     
            </div>

             
            <div class="form-group password-container">
                <label for="userpasswordNewC" class="col-md-2 control-label">
                    <span class="text-danger">*New Password Confirm</span>
                </label>   
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="userpasswordNewC" MaxLength="100" cssclass="form-control" TextMode="Password"></asp:TextBox>
                </div>   
                <div class="col-md-6">   Re-Type your new password again here                                                
                </div>                     
            </div>

            <div class="form-group">
                <label for="btnChangeUserPass" class="col-md-2 control-label">                                   
                </label>                          
                <div class="col-md-4">
                    <asp:Button runat="server" ID="btnCancel" cssclass="btn btn-default" Text="Cancel" OnClick="btnChangePassCancel_Click"></asp:Button>
                    <asp:Button runat="server" ID="btnChangeUserPass" cssclass="btn btn-primary" Text="Change Password" OnClick="btnChangePass_Click" />                    
                </div>  
                <div class="col-md-6">                                                 
                </div>                      
            </div>              
                   
                    

            <div class="text-info" >              
              <p>The password you choose must:</p> 				
				<ul>
					<li>be at least 8 characters long.</li>
                    <li>contain both upper and lower case characters.</li>
					<li>contain at least one numeric character.</li>
					<li>not repeat a previous password used on the account.</li>
					<li>not contain your account or full name.</li>
                </ul>              
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
        </form>
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
            $('#userpasswordNew').pwstrength(options);

            var optionsc = {};
            optionsc.ui = {
                showVerdictsInsideProgressBar: true,
            };
            $('#userpasswordNewC').pwstrength(optionsc);

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

</body>
</html>
