<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TMHLogin.aspx.cs" Inherits="TMHSelf.TMHLogin" %>

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
        <img class="img-responsive center-block" src="Content/takemehome.jpg" alt="Take Me Home Registration Portal" />                           
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
      

         <form id="form1" runat="server" class="form-horizontal">               
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

            
             </form>
        </div>

    <script src="Scripts/jquery-1.10.2.min.js"></script>
        
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

</body>
</html>
