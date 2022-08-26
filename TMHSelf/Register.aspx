<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="TMHSelf.Register" %>


<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7"> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8"> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9"> <![endif]-->
<!--[if gt IE 8]><!--> <html class="no-js"> <!--<![endif]-->

    <head>
        
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <meta name="format-detection" content="telephone=no">
        <title>Take Me Home Self-Registration Portal</title>
        <link rel="shortcut icon" href="/tmhself/Content/favicon.ico" type="image/x-icon">
        <link rel="icon" href="/tmhself/Content/favicon.ico" type="image/x-icon">
        <link rel="apple-touch-icon" href="/tmhself/Content/AppIcon76x76.png">
        <link rel="apple-touch-icon" sizes="76x76" href="/tmhself/Content/AppIcon76x76.png">
        <link rel="apple-touch-icon" sizes="120x120" href="/tmhself/Content/AppIcon60x60@2x.png">
        <link rel="apple-touch-icon" sizes="152x152" href="/tmhself/Content/AppIcon76x76@2x.png">
        <meta name="msapplication-TileImage" content="/tmhself/Content/AppIcon72x72@2x.png">
        <meta name="msapplication-TileColor" content="#222222">

        <link href="Content/bootstrap.min.css" rel="stylesheet" />
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

        <a href="default.aspx" title="Take Me Home Main Page"></a>
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
                   <span><asp:Label ID="lblUser" runat="server"></asp:Label>  </span>
               </strong>   -
               <asp:Label ID="lblCurrDtTm" runat="server"></asp:Label>
            </h3>

            <p class="lead text-warning center-block">
                Please use this form to register or update a Take Me Home Person. If you need help, please read these <a  href="TMHDoc.pdf" target="_blank">Instructions</a>. 
                <span class="text-danger">*Indicates Required Fields</span>
            </p>
  
             
            <div id="msgVal" runat="server" class=" alert alert-info" role="alert">
                <asp:Label runat="server" ID="lblMsg"></asp:Label>
            </div>    

    

        <form id="form2" runat="server" class="form-horizontal">

            <asp:Panel ID="pnlRecordDeleted" runat="server" Visible="false">
                <asp:Button runat="server" ID="btnContinue" cssclass="btn btn-primary" 
                    Text="Click to Continue" OnClick="btnContinue_Click" />
            </asp:Panel>
      
            <asp:Panel ID="pnlMain" runat="server">

            <div class="form-group">
                <label for="btnSubmit1" class="col-md-2 control-label"></label>                          
                <div class="col-md-8">
                    <asp:Button runat="server" ID="btnCancel1" cssclass="btn btn-default btn-lg" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
                    <!-- <asp:Button runat="server" ID="btnSubmit1" cssclass="btn btn-primary btn-lg" Text="Submit to TMH" OnClick="btnSubmit_Click" ></asp:Button> -->
                   
                </div>  
                <div class="col-md-2"></div>                      
            </div>

                <h2>Take Me Home Person</h2>  
            <div class="form-group">
                <label for="txtLastName" class="col-md-2 control-label">
                    <span class="text-danger">*Last Name:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" TextMode="SingleLine" MaxLength="15" ID="txtLastName" cssclass="form-control required" onchange="cleanupInput(document.getElementById('txtLastName'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
            <div class="form-group">
                <label for="txtFirstName" class="col-md-2 control-label">
                    <span class="text-danger">*First Name:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" MaxLength="14" ID="txtFirstName" cssclass="form-control required" onchange="cleanupInput(document.getElementById('txtFirstName'))"></asp:TextBox> 
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
            <div class="form-group">
                <label for="txtMiddleName" class="col-md-2 control-label">
                    Middle Name:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" MaxLength="14" ID="txtMiddleName" cssclass="form-control" onchange="cleanupInput(document.getElementById('txtMiddleName'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
            <div class="form-group">
                <label for="SUFFIX_NAME" class="col-md-2 control-label">
                    Suffix:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="SUFFIX_NAME" runat="server" cssclass="form-control"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
            <div class="form-group">
                <label for="txtNameToCallMe" class="col-md-2 control-label">
                    <span class="text-danger">*Name To Call Me:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="txtNameToCallMe" MaxLength="50" cssclass="form-control required" onchange="cleanupInput(document.getElementById('txtNameToCallMe'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>       
            <div class="form-group">
                <label for="txtHomePhone" class="col-md-2 control-label">
                    <span class="text-danger">*Home Phone:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="txtHomePhone" MaxLength="12" cssclass="form-control required" ToolTip="999-999-9999"></asp:TextBox>                    
                </div>  
                <div class="col-md-2"> 
                    999-999-9999               
                </div>                      
            </div>
            <div class="form-group">
                <label for="lbxDiagnosis" class="col-md-2 control-label">
                    <span class="text-danger">*Diagnosis/Disability:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:ListBox ID="lbxDiagnosis" runat="server" cssclass="form-control required" SelectionMode="Multiple" Height="290"></asp:ListBox>
                </div>  
                <div class="col-md-2">
                    *Hold Control key to select multiple options.
                </div>                      
            </div>
                        
            <h2>Address
            </h2>                         
            <div class="form-group">
                <label for="txtAddressNumber" class="col-md-2 control-label">
                    <span class="text-danger">*Address Number:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="txtAddressNumber" MaxLength="10" cssclass="form-control required" onchange="cleanupInput(document.getElementById('txtAddressNumber'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                          
            <div class="form-group">
                <label for="txtAddressStreet" class="col-md-2 control-label">
                    <span class="text-danger">*Address Street:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="txtAddressStreet" MaxLength="30" cssclass="form-control required" onchange="cleanupInput(document.getElementById('txtAddressStreet'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
            <div class="form-group">
                <label for="txtCity" class="col-md-2 control-label">
                    <span class="text-danger">*City:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="txtCity" runat="server" cssclass="form-control required">
                        <asp:ListItem>San Diego</asp:ListItem>  
                    </asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>  
            <div class="form-group">
                <label for="ddlCounty" class="col-md-2 control-label">
                    <span class="text-danger">*County:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlCounty" runat="server" cssclass="form-control required">
                        <asp:ListItem>San Diego County</asp:ListItem>  
                    </asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div> 
            <div class="form-group">
                <label for="ddlState" class="col-md-2 control-label">
                    <span class="text-danger">*State:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlState" runat="server" cssclass="form-control required">
                        <asp:ListItem>CA</asp:ListItem>  
                    </asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                         
            <div class="form-group">
                <label for="txtZipCode" class="col-md-2 control-label">
                    <span class="text-danger">*Zip Code:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="txtZipCode" MaxLength="10" cssclass="form-control required" ToolTip="99999 or 99999-9999" onchange="cleanupInput(document.getElementById('txtZipCode'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">
                    99999 or 99999-9999
                </div>                      
            </div>
             <div class="form-group">
                <label for="" class="col-md-2 control-label">
                    <span class="text-danger"></span>
                </label>                          
                <div class="col-md-8"> 
                    <div id="map" class="form-control"></div> 
                        
                </div>  
                <div class="col-md-2">        
                </div>                      
            </div>
            <div class="form-group">
                <label for="" class="col-md-2 control-label">      
                    <span class="text-danger">*Address Confirmation: </span>              
                </label>                          
                <div class="col-md-8">  
                                       
                    <%--<asp:RadioButtonList id="rblAddress" cssclass="radio" RepeatDirection="Vertical"  RepeatLayout="Flow"  runat="server" >
                        <asp:ListItem value="confirmed">I <strong>confirm</strong> the address is correct.</asp:ListItem>
                        <asp:ListItem value="overridden">I want to <strong>override</strong> the address and acknowledge the warning.</asp:ListItem>
                    </asp:RadioButtonList>   --%>  

                    <asp:DropDownList ID="ddlAddress" runat="server" cssclass="form-control required">
                        <asp:ListItem value=""></asp:ListItem> 
                        <asp:ListItem value="confirmed">Address displayed on the map above is correct.</asp:ListItem> 
                        <asp:ListItem value="overridden">Override the address and acknowledge the warning.</asp:ListItem>
                    </asp:DropDownList>
                        
                    <p class=" lead text-danger">                        
                    </p>  
                    <p class="text-danger">
                        IMPORTANT! Please confirm that the address you entered displays in the right place on the map above. If it does not display properly please double check your address and re-enter the zip code to try again.                                                            
                    </p> 
                    <p class="text-danger">                        
                        WARNING! By selecting override you are acknowledging that information about the person you are registering may not be accessible in case of an emergency. 
                    </p>                                      
                </div> 
                <div class="col-md-2">                    
                </div>                      
            </div>
            <h2>Physical Description
            </h2> 
            <div class="form-group">
                <label for="txtDOB" class="col-md-2 control-label">
                    <span class="text-danger">*Date of Birth:</span>
                </label>                          
                <div class="col-md-8">
                    <%--<asp:TextBox runat="server" ID="txtDOB" OnTextChanged="txtDOB_Changed" AutoPostBack="true" MaxLength="10"  ToolTip="MM/DD/YYYY"></asp:TextBox>MM/DD/YYYY--%>
                    <asp:TextBox runat="server" ID="txtDOB" cssclass="form-control required" AutoPostBack="false" MaxLength="10" ToolTip="MM/DD/YYYY"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                    MM/DD/YYYY
                </div>                      
            </div>
             <!--                         
            <div class="form-group">
                <label for="txtAge" class="col-md-2 control-label">                                                             
                        Age:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="txtAge" MaxLength="3"  Enabled="false" cssclass="form-control"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
             -->
                                    
            <div class="form-group">
                <label for="ddlRace" class="col-md-2 control-label">
                    Race:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlRace" runat="server"  cssclass="form-control"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                         
            <div class="form-group">
                <label for="ddlSex" class="col-md-2 control-label">
                    Sex:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlSex" runat="server" cssclass="form-control"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                          
            <div class="form-group">
                <label for="ddlHeight" class="col-md-2 control-label">
                    Height:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlHeight" runat="server"  cssclass="form-control"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                         
            <div class="form-group">
                <label for="" class="col-md-2 control-label">
                    Weight:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="txtWeight" MaxLength="3" type="number" cssclass="form-control" onchange="cleanupInput(document.getElementById('txtWeight'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">  lbs               
                </div>                      
            </div>                
            <div class="form-group">
                <label for="ddlEye" class="col-md-2 control-label">
                    Eye Color:
                </label>                          
                <div class="col-md-8">
                        <asp:DropDownList ID="ddlEye" runat="server"  cssclass="form-control"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                       
            <div class="form-group">
                <label for="ddlHair" class="col-md-2 control-label">
                    Hair Color:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlHair" runat="server" cssclass="form-control"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>

            <h2>Special Information</h2>
            <div class="form-group">
                <label for="ddlHomeType" class="col-md-2 control-label">
                    Home Type:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlHomeType" runat="server" cssclass="form-control"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                           
            <div class="form-group">
                <label for="ddlWander" class="col-md-2 control-label">
                    Wander Tendency:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlWander" runat="server" cssclass="form-control"></asp:DropDownList>
                </div>  
                <div class="col-md-2"> 
                                                   
                </div>                      
            </div>                  
            <div class="form-group">
                <label for="ddlCommunication" class="col-md-2 control-label">
                    Communication Method:
                </label>                          
                <div class="col-md-8">
                    <asp:ListBox ID="ddlCommunication" runat="server" cssclass="form-control" SelectionMode="Multiple" Height="180"></asp:ListBox>
                </div>  
                <div class="col-md-2">
                    * Hold Control key to select multiple options.                   
                </div>                      
            </div>                          
            <div class="form-group">
                <label for="ddlMedication" class="col-md-2 control-label">
                    Medication Endangered:
                </label>                          
                <div class="col-md-8">
                        <asp:DropDownList ID="ddlMedication" runat="server" cssclass="form-control" >
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="NO">NO</asp:ListItem>
                        <asp:ListItem Value="YES">YES</asp:ListItem>
                    </asp:DropDownList>
                </div>  
                <div class="col-md-2">
                    Are there any medications that would endanger the participant's life if not taken on schedule?                   
                </div>                      
            </div>                           
            <div class="form-group">
                <label for="txtLanguages" class="col-md-2 control-label">
                    Spoken Languages:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtLanguages" runat="server" TextMode="MultiLine"  MaxLength="250" cssclass="form-control" onchange="cleanupInput(document.getElementById('txtLanguages'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                        
            <div class="form-group">
                <label for="txtMedical" class="col-md-2 control-label">
                    Medical/Psych Issues:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtMedical" runat="server" TextMode="MultiLine" MaxLength="250" cssclass="form-control" onchange="cleanupInput(document.getElementById('txtMedical'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                     
            <div class="form-group">
                <label for="txtWornItems" class="col-md-2 control-label">
                    Commonly Worn Items:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtWornItems" runat="server" TextMode="MultiLine" MaxLength="250" cssclass="form-control" onchange="cleanupInput(document.getElementById('txtWornItems'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                       
            <div class="form-group">
                <label for="txtApproach" class="col-md-2 control-label">
                    Approach Suggestions:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtApproach" runat="server" TextMode="MultiLine" MaxLength="250" cssclass="form-control" onchange="cleanupInput(document.getElementById('txtApproach'))" ></asp:TextBox>               
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                         
            <div class="form-group">
                <label for="txtBehaviors" class="col-md-2 control-label">
                    Noted Behaviors:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtBehaviors" runat="server" TextMode="MultiLine" MaxLength="250" cssclass="form-control" onchange="cleanupInput(document.getElementById('txtBehaviors'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2"></div>                    
            </div>                       
            <div class="form-group">
                <label for="lbxSpecial" class="col-md-2 control-label">
                    Special Considerations:
                </label>                          
                <div class="col-md-8">
                    <asp:ListBox ID="lbxSpecial" runat="server" SelectionMode="Multiple" cssclass="form-control" Height="280"></asp:ListBox>
                </div>
                <div class="col-md-2"> * Hold Control key to select multiple options.</div>   
                                      
            </div>

             <h2>Primary Contact</h2>   
            <div class="form-group">
                <label for="ddlContactRelationship" class="col-md-2 control-label">
                    <span class="text-danger">*Relationship:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlContactRelationship" cssclass="form-control required" runat="server"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                          
            <div class="form-group">
                <label for="txtContactFullName" class="col-md-2 control-label">
                    <span class="text-danger">*Full Name:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactFullName" runat="server" cssclass="form-control required" MaxLength="50"  ToolTip="FirstName LastName" onchange="cleanupInput(document.getElementById('txtContactFullName'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">FirstName LastName                   
                </div>                      
            </div>                           
            <div class="form-group">
                <label for="txtContactAddress" class="col-md-2 control-label">
                    <span class="text-danger">*Address:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactAddress" cssclass="form-control required" runat="server" MaxLength="50" onchange="cleanupInput(document.getElementById('txtContactAddress'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                       
            <div class="form-group">
                <label for="txtContactCity" class="col-md-2 control-label">
                    <span class="text-danger">*City:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactCity" cssclass="form-control required" runat="server" MaxLength="24" onchange="cleanupInput(document.getElementById('txtContactCity'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                      
            <div class="form-group">
                <label for="ddlContactState" class="col-md-2 control-label">
                    <span class="text-danger">*State:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlContactState" cssclass="form-control required" runat="server" ></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                    
            <div class="form-group">
                <label for="txtContactZip" class="col-md-2 control-label">
                    <span class="text-danger">*Zip:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactZip" cssclass="form-control required" runat="server" MaxLength="10" ToolTip="99999 or 99999-9999" onchange="cleanupInput(document.getElementById('txtContactZip'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">  99999 or 99999-9999               
                </div>                      
            </div>                       
            <div class="form-group">
                <label for="" class="col-md-2 control-label">                                    
                </label>                          
                <div class="col-md-8">
                    <h4><span class="text-danger">* At Least one of the three following contact phone numbers is required.</span></h4>
                </div>  
                <div class="col-md-2">                
                </div>                      
            </div>                       
            <div class="form-group">
                <label for="txtContactHPhone" class="col-md-2 control-label">
                    <span class="text-danger">*Home Phone:</span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactHPhone" cssclass="form-control required" runat="server" MaxLength="12" ToolTip="999-999-9999" ></asp:TextBox>
                </div>  
                <div class="col-md-2"> 999-999-9999                
                </div>                      
            </div>                            
            <div class="form-group">
                <label for="txtContactMPhone" class="col-md-2 control-label">
                    <span class="text-danger">*</span> Mobile Phone:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactMPhone" cssclass="form-control required" runat="server" MaxLength="12" ToolTip="999-999-9999" ></asp:TextBox>
                </div>  
                <div class="col-md-2">   999-999-9999               
                </div>                      
            </div>                      
            <div class="form-group">
                <label for="txtContactOPhone" class="col-md-2 control-label">
                    <span class="text-danger">*</span>Other Phone:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactOPhone" cssclass="form-control required" runat="server" MaxLength="12" ToolTip="999-999-9999" ></asp:TextBox>
                </div>  
                <div class="col-md-2"> 999-999-9999                 
                </div>                      
            </div>                           
            <div class="form-group">
                <label for="txtContactEMail" class="col-md-2 control-label">
                    Email:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactEMail" cssclass="form-control" runat="server" MaxLength="100" onchange="cleanupInput(document.getElementById('txtContactEMail'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
            <h2>Secondary Contact
            </h2> 
            <div class="form-group">
                <label for="ddlContactRelationship2" class="col-md-2 control-label">
                    Relationship:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlContactRelationship2" cssclass="form-control" runat="server"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                           
            <div class="form-group">
                <label for="txtContactFullName2" class="col-md-2 control-label">
                    <!-- <span class="text-danger">Full Name:</span> -->
                    Full Name:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactFullName2" cssclass="form-control" runat="server" MaxLength="50"  ToolTip="FirstName LastName" onchange="cleanupInput(document.getElementById('txtContactFullName2'))"></asp:TextBox>
                </div>  
                <div class="col-md-2"> FirstName LastName                 
                </div>                      
            </div>                        
            <div class="form-group">
                <label for="txtContactAddress2" class="col-md-2 control-label">
                    Address:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactAddress2" cssclass="form-control" runat="server" MaxLength="50" onchange="cleanupInput(document.getElementById('txtContactAddress2'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                            
            <div class="form-group">
                <label for="txtContactCity2" class="col-md-2 control-label">
                    City:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactCity2" cssclass="form-control" runat="server" MaxLength="24" onchange="cleanupInput(document.getElementById('txtContactCity2'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                            
            <div class="form-group">
                <label for="ddlContactState2" class="col-md-2 control-label">
                    State:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlContactState2" cssclass="form-control" runat="server" ></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                       
            <div class="form-group">
                <label for="txtContactZip2" class="col-md-2 control-label">
                    Zip:
                </label>                          
                <div class="col-md-8">
                        <asp:TextBox ID="txtContactZip2" cssclass="form-control" runat="server" MaxLength="10" ToolTip="99999 or 99999-9999" onchange="cleanupInput(document.getElementById('txtContactZip2'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">99999 or 99999-9999                  
                </div>                      
            </div>                            
            <div class="form-group">
                <label for="txtContactHPhone2" class="col-md-2 control-label">
                    Home Phone:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactHPhone2" cssclass="form-control" runat="server" MaxLength="12" ToolTip="999-999-9999" ></asp:TextBox>
                </div>  
                <div class="col-md-2"> 999-999-9999                
                </div>                      
            </div>                        
            <div class="form-group">
                <label for="txtContactMPhone2" class="col-md-2 control-label">
                    Mobile Phone:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactMPhone2" cssclass="form-control" runat="server" MaxLength="12" ToolTip="999-999-9999" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                         
            <div class="form-group">
                <label for="txtContactOPhone2" class="col-md-2 control-label">
                    Other Phone:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactOPhone2" cssclass="form-control" runat="server" MaxLength="12" ToolTip="999-999-9999" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                      
            <div class="form-group">
                <label for="txtContactEMail2" class="col-md-2 control-label">
                    Email:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtContactEMail2" cssclass="form-control" runat="server" MaxLength="100" onchange="cleanupInput(document.getElementById('txtContactEMail2'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>

            <h2>Photo</h2>                    
            <div class="form-group">
                        <label for="" class="col-md-2 control-label">
                            <span class="text-danger">*
                            <asp:Label ID="LabelFileUpload" runat="server" Text="Upload Photo:" ></asp:Label></span>
                        </label>                          
                        <div class="col-md-8">
                            <asp:FileUpload ID="FileUpload" runat="server" Visible="True" ToolTip="Click Browse to Upload a Photo" accept="image/*"/>
                            <asp:HiddenField  runat="server" ID="base64" ></asp:HiddenField>
                            <br />
                            Photo Guidelines: 
                            <ul>
                                <li>Photos should be Passport Quality</li>
                                <li>Front facing picture</li>
                                <li>Clearly visible eyes</li>
                                <li>Images Only (.jpg & .gif)</li>
                                <li>5MB or smaller file size recommended</li>
                            </ul>
                            <img src="IdealPhoto.jpg" alt="Ideal Photo" border="0" />
                            <br />Ideal Photo
                           <%-- <p class="text-danger">Important message about the file upload, 1 Mega Byte Limit.</p>--%>
                            <div class="row">
                                <div class="col-md-12" >
                                    <div class="row" id="crop-gui">
                                        <div class="col-md-12">                                                        
                                            <%--<div id="crop-msg-welcome" class="alert alert-info" role="alert">
                                                <strong>To begin click "Choose File" or "Browse"</strong> <span class="hidden-xs"></span>
                                            </div>--%>
                                            <div id="crop-msg-done" class="alert alert-success" role="alert" style="display:none;">
                                                <h3 class="text-info">Your Photo is ready!</h3>
                                            </div>
                                            <div id="crop-msg-active" class="alert alert-danger" role="alert" style="display:none;">
                                                <h3 class="text-danger">Click Crop if you have selected the area you wish to crop.</h3>
                                            </div>
                                            <div id="crop-msg-info" class="alert alert-info" role="alert" style="display:none;">
                                                <h3 class="text-info">If you want to crop your photo click below.</h3>
                                            </div>                                                        
                                        </div>
                                        <div class="col-md-6">
                                            <a id="" href="#crop-gui" class="startcrop btn btn-primary btn-block disabled">
                                                Click Here to Enable Cropping
                                            </a>
                                        </div>
                                        <div class="col-md-6">                                                       
                                            <a id="" href="#crop-gui" class="finishcrop btn btn-success btn-block disabled">
                                                Crop!
                                            </a>
                                        </div>                                                    
                                    </div>                                            
                                </div>   
                                <div class="col-md-12">
                                    <br />
                                    <asp:Label ID="lblImage"  runat="server"></asp:Label>
                                    <asp:TextBox ID="lblPhoto" Enabled="false"  runat="server" onchange="cleanupInput(document.getElementById('lblPhoto'))"></asp:TextBox> 
                                    <canvas style="display:none;" class="center-block" id="canvas"></canvas>   
                                    <p> <asp:Label ID="filesize"  runat="server"></asp:Label> </p>
                                </div>   
                            </div>
                        </div>  
                        <div class="col-md-2">                                                                         
                        </div>             
                    </div>
            <div class="form-group">
                <label for="" class="col-md-2 control-label">
                    <span class="text-danger">*
                    <asp:Label ID="Label10" runat="server"  Text="Age in Photo in Years:" ></asp:Label></span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" cssclass="form-control required" ID="AGE_IN_PHOTOStart" MaxLength="3" type="number"></asp:TextBox>
                </div>  
                <div class="col-md-2">   
                </div>                  
            </div>     
            <div class="form-group">
                <label for="" class="col-md-2 control-label">
                    <span class="text-danger">*
                    <asp:Label ID="Label11"  runat="server"  Text="Photo Date:"  ></asp:Label></span>
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" cssclass="form-control required" ID="txtPhotoDate" MaxLength="10" ToolTip="MM/DD/YYYY" ></asp:TextBox>
                </div>  
                <div class="col-md-2"> MM/DD/YYYY                
                </div>                      
            </div>
        
            <h2>                
                <span style="cursor:pointer;" id="optional3a" class="text-info">[+] Show  </span>
                <span style="cursor:pointer;" id="optional3b" class="text-info">[-] Hide  </span>
                Vehicle information 
                <small> <span>Optional</span>  </small>
            </h2>     
            <div id="optionalpanel3">                   
            <div class="form-group">
                <label for="ddlVehType" class="col-md-2 control-label">
                    Type:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlVehType" cssclass="form-control" runat="server"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
                                     
            <div class="form-group">
                <label for="ddlVehYear" class="col-md-2 control-label">
                    Year:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlVehYear" cssclass="form-control" runat="server"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                            
            <div class="form-group">
                <label for="ddlVehMake" class="col-md-2 control-label">
                    Make:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlVehMake" cssclass="form-control" runat="server"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
            <div class="form-group">
                <label for="txtVehModel" class="col-md-2 control-label">
                    Model:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtVehModel" cssclass="form-control" runat="server" MaxLength="20" onchange="cleanupInput(document.getElementById('txtVehModel'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
            <div class="form-group">
                <label for="ddlVehColor" class="col-md-2 control-label">
                    Color:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlVehColor" cssclass="form-control" runat="server"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                          
            <div class="form-group">
                <label for="txtVehVIN" class="col-md-2 control-label">
                    Vehicle VIN:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtVehVIN" cssclass="form-control" runat="server" MaxLength="40" onchange="cleanupInput(document.getElementById('txtVehVIN'))" ></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>                         
            <div class="form-group">
                <label for="txtVehLic" class="col-md-2 control-label">
                    Vehicle Lic. #:
                </label>                          
                <div class="col-md-8">
                    <asp:TextBox ID="txtVehLic" cssclass="form-control" runat="server" MaxLength="15" onchange="cleanupInput(document.getElementById('txtLanguages'))"></asp:TextBox>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>   
            <div class="form-group">
                <label for="ddlVehLicState" class="col-md-2 control-label">
                    Lic. State:
                </label>                          
                <div class="col-md-8">
                    <asp:DropDownList ID="ddlVehLicState" cssclass="form-control" runat="server"></asp:DropDownList>
                </div>  
                <div class="col-md-2">                 
                </div>                      
            </div>
 
            </div>

            <h2>Enrollment</h2>
            <div class="form-group">
                <label for="txtSubmittedDateTime" class="col-md-2 control-label">Enrolled:</label>                          
                <div class="col-md-8">
                    <asp:TextBox runat="server" cssclass="form-control" ID="txtSubmittedDateTime" MaxLength="30" Enabled="false"></asp:TextBox>
                </div>  
                <div class="col-md-2"></div>                      
            </div>

            <div class="form-group">
                <label for="ddlEnrollingAgency" class="col-md-2 control-label">
                    <span class="text-danger">*Closest Law Enforcement Agency:</span>
                </label>                          
                <div class="col-md-8">
                        <asp:DropDownList ID="ddlEnrollingAgency"  cssclass="form-control required" runat="server"></asp:DropDownList>
                </div>  
                <div class="col-md-2"></div>                      
            </div>
                

            <div class="form-group">
                <label for="btnSubmit" class="col-md-2 control-label"></label>                          
                <div class="col-md-8">
                    <asp:Button runat="server" ID="btnCancel" cssclass="btn btn-default btn-lg" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
                    <asp:Button runat="server" ID="btnSubmit" cssclass="btn btn-primary btn-lg" Text="Submit to TMH" OnClick="btnSubmit_Click" ></asp:Button>
                    <img id="loadergif" src="Content/loader.gif" />
                </div>  
                <div class="col-md-2"></div>                      
            </div>

             <div class="col-md-2"></div>                      
            <div class="col-md-2"></div>                      
        
            </asp:Panel>
          </form>
         
   </div>
          <script type="text/javascript">

              document.getElementById('btnSubmit').onclick =
                  function validate(form) {

                  var LastName = document.getElementById('txtLastName').value;
                  var FirstName = document.getElementById('txtFirstName').value;
                  var NameToCallMe = document.getElementById('txtNameToCallMe').value;
                  var HomePhone = document.getElementById('txtHomePhone').value;
                  var AddressNumber = document.getElementById('txtAddressNumber').value;
                  var AddressStreet = document.getElementById('txtAddressStreet').value;
                  var ZipCode = document.getElementById('txtZipCode').value;
                  var DOB = document.getElementById('txtDOB').value;
                  var ContactFullName = document.getElementById('txtContactFullName').value;
                  var ContactAddress = document.getElementById('txtContactAddress').value;
                  var ContactCity = document.getElementById('txtContactCity').value;
                  var ContactZip = document.getElementById('txtContactZip').value;
                  var ContactHPhone = document.getElementById('txtContactHPhone').value
                  var ContactMPhone = document.getElementById('txtContactMPhone').value;
                  var ContactOPhone = document.getElementById('txtContactOPhone').value;


                  var PhotoFile = document.getElementById('FileUpload').value;
                  var PhotoFileName = document.getElementById('lblPhoto').value;
                  var AGE_IN_PhotoStart = document.getElementById('AGE_IN_PHOTOStart').value;
                  var PhotoDate = document.getElementById('txtPhotoDate').value;

                  var errors = [];

                  if (!checkLength(LastName)) { errors.push("You must enter a Last Name."); }
                  if (!checkLength(FirstName)) { errors.push("You must enter a First Name."); }
                  if (!checkLength(NameToCallMe)) { errors.push("You must enter a Name to Call Me."); }
                  if (!checkLength(HomePhone)) { errors.push("You must enter a Home Phone."); }

                  if (!checkSelect(document.getElementById('lbxDiagnosis'))) {
                      errors.push("You must choose a Diagnosis/Disability.");
                  }

                  if (!checkLength(AddressNumber)) { errors.push("You must enter an Address Number."); }
                  if (!checkLength(AddressStreet)) { errors.push("You must enter an Address Street."); }

                  if (!checkSelect(document.getElementById('txtCity'))) {
                      errors.push("You must choose a City.");
                  }

                  if (!checkSelect(document.getElementById('ddlCounty'))) {
                      errors.push("You must choose a County.");
                  }
                  if (!checkSelect(document.getElementById('ddlState'))) {
                      errors.push("You must choose a State.");
                  }

                  if (!checkLength(ZipCode)) { errors.push("You must enter a Zip Code."); }
                  if (!checkSelect(document.getElementById('ddlAddress'))) {
                      errors.push("You must choose an Address Confirmation.");
                  }

                  if (!checkLength(DOB)) { errors.push("You must enter Date of Birth."); }

                  if (!checkSelect(document.getElementById('ddlContactRelationship'))) {
                      errors.push("You must choose a Primary Contact Relationship.");
                  }

                  if (!checkLength(ContactFullName)) { errors.push("You must enter Primary Contact Full Name."); }
                  if (!checkLength(ContactAddress)) { errors.push("You must enter Primary Contact Address."); }
                  if (!checkLength(ContactCity)) { errors.push("You must enter Primary Contact City."); }
                  if (!checkSelect(document.getElementById('ddlContactState'))) {
                      errors.push("You must choose a Primary Contact State.");
                  }
                  if (!checkLength(ContactZip)) { errors.push("You must enter Primary Contact Zip Code."); }

                  if (!(checkLength(ContactHPhone)) && !(checkLength(ContactMPhone)) && (!checkLength(ContactOPhone))) {
                      errors.push("You must enter at least one Primary Contact Phone Number.");
                  }

                  //if (!(checkLength(PhotoFile)) && !(checkLength(PhotoFileName))) { errors.push("You must Upload Photo."); }

                  if (!checkLength(AGE_IN_PhotoStart)) { errors.push("You must enter Age in Photo in Years."); }
                  if (!checkLength(PhotoDate)) { errors.push("You must enter a Photo Date."); }

                  if (!checkSelect(document.getElementById('ddlEnrollingAgency'))) {
                      errors.push("You must choose the Closetst Law Enforcement Agency.");
                  }

                  if (errors.length > 0) {
                      reportErrors(errors);
                      return false;
                  }

                  return true;
              };

              // https://www.webucator.com/tutorial/learn-javascript/javascript-form-validation.cfm
              function reportErrors(errors) {
                  var msg = "There were some problems...\n";
                  var numError;
                  for (var i = 0; i < errors.length; i++) {
                      numError = i + 1;
                      msg += "\n" + numError + ". " + errors[i];
                  }
                  alert(msg);
              };

              function cleanupInput(text) {
                  var x = text
                  x.value = x.value.replace(/[<>%=\(\)\{\}:?;!]/g, "");
              }
              function checkLength(text, min, max) {
                  min = min || 1;
                  max = max || 10000;

                  if (text.length < min || text.length > max) {
                      return false;
                  }
                  return true;
              };
              //usage:
              /*
              // inside validate function
                  var ContactOPhone = document.getElementById('txtContactOPhone').value;

                  if (!checkLength(ContactPhone)) {
                    errors.push("You must enter a Contact Phone Number.");
                    }
              */

              function checkSelect(select) {
                  return (select.selectedIndex > 0);
              };
              //usage:
              /*
              // inside the form body
              <select name="flavor">
                <option value="0" selected></option>
                <option value="choc">Chocolate</option>
                <option value="straw">Strawberry</option>
                <option value="van">Vanilla</option>
              </select>

              // inside your validate(form) function
              if ( !checkSelect(form.flavor) ) {
                errors[errors.length] = "You must choose a flavor.";
                }

              */

              function checkRadioArray(radioButtons) {
                  for (var i = 0; i < radioButtons.length; i++) {
                      if (radioButtons[i].checked) {
                          return true;
                      }
                  }
                  return false;
              };
              //usage:
              /*
              // inside the form body
                <strong>Cup or Cone?</strong>
                <input type="radio" name="container" value="cup"> Cup
                <input type="radio" name="container" value="plaincone"> Plain cone
                <input type="radio" name="container" value="sugarcone"> Sugar cone
                <input type="radio" name="container" value="wafflecone"> Waffle cone
                <br><br>

              // inside your validate(form) function
                  if ( !checkRadioArray(form.container) ) {
                    errors[errors.length] = "You must choose a cup or cone.";
                    }
              */

              function checkCheckBox(cb) {
                  return cb.checked;
              }

              //usage:
              /*
              // inside the form body
              <input type="checkbox" name="terms"> I understand that I'm really not going to get any ice cream.
                <br><br>

              // inside your validate(form) function
              if (!checkCheckBox(form.terms)) {
                  errors[errors.length] = "You must agree to the terms.";
              }
              */

              function checkTextArea(textArea, max) {
                  var numChars, chopped, message;
                  if (!checkLength(textArea.value, 0, max)) {
                      numChars = textArea.value.length;
                      chopped = textArea.value.substr(0, max);
                      message = 'You typed ' + numChars + ' characters.\n';
                      message += 'The limit is ' + max + '.';
                      message += 'Your entry will be shortened to:\n\n' + chopped;
                      alert(message);
                      textArea.value = chopped;
                  }
              }
              //usage:
              /*
              // inside the form body
              <p>
                <strong>Special Requests:</strong><br>
                <textarea name="requests" cols="40" rows="6" wrap="virtual" onblur="checkTextArea(this, 100);"></textarea>
              </p>
              */

          </script>
    
    <script src="Scripts/jquery-3.6.0.min.js"></script>
    <script src="Scripts/megapix-image.js"></script>
    <script src="Scripts/binaryajax.js"></script>
    <script src="Scripts/exif.js"></script>
  <%--  <link href="Content/jquery.Jcrop.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.Jcrop.min.js"></script>--%>
    <script src="Scripts/bootstrap.inputmask.min.js"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=true&key=AIzaSyCYZ3aQMXk0t7olKJtvXq3jTdPFetAc4HY"></script>

    <script>

        $('#btnSubmit').click(function () {
            $('#loadergif').show();
        });

        //SN Input masks        
        $('#txtDOB,#txtPhotoDate').inputmask({
            mask: '99/99/9999'
        });

        $('#txtHomePhone,#txtContactHPhone,#txtContactMPhone,#txtContactOPhone,#txtContactHPhone2,#txtContactMPhone2,#txtContactOPhone2').inputmask({
            mask: '999-999-9999'
        });
        
        //SN Show optional fields
        $('#optional1a').click(function () { $('#optionalpanel1,#optional1b').show(); $(this).hide(); });
        $('#optional2a').click(function () { $('#optionalpanel2,#optional2b').show(); $(this).hide(); });
        $('#optional3a').click(function () { $('#optionalpanel3,#optional3b').show(); $(this).hide(); });
        $('#optional1b').click(function () { $('#optional1a').show(); $('#optionalpanel1').hide(); $(this).hide(); });
        $('#optional2b').click(function () { $('#optional2a').show(); $('#optionalpanel2').hide(); $(this).hide(); });
        $('#optional3b').click(function () { $('#optional3a').show(); $('#optionalpanel3').hide(); $(this).hide(); });

        //SN test for html5 canvas
        if (Modernizr.canvas) {
            //SN picture upload
            $("#FileUpload").change(function () {
                if (window.File && window.FileList && window.FileReader) {
                    clearpicture();
                    loadphoto(this);
                    //$("#crop-gui").show();
                }
                else {
                    alert("Your browser does not support File API");
                }
            });

            //var jcrop_api;
            //var tempW;
            //var tempH;
            //var canvas = document.getElementById('canvas');
            ////SN start cropping
            //$('.startcrop').click(function () {
            //    $('#crop-msg-info').hide();
            //    $('#crop-msg-active').show();
            //    $('.finishcrop').show();

            //    $('#image').Jcrop({
            //        onSelect: setCanvasCoords,
            //        trueSize: [tempW, tempH],
            //        setSelect: [40, 40, tempW - 40, tempH - 40],
            //        bgColor: 'white'
            //    }, function () {
            //        jcrop_api = this;
            //        $(".jcrop-holder").not(":last").remove();
            //    }
            //    );
            //    $('.startcrop').addClass('disabled');
            //    $('.finishcrop').removeClass('disabled');
            //});
            ////SN Finish corpping
            //$('.finishcrop').click(function () {
            //    $('#crop-msg-done').show();
            //    $('#crop-msg-active').hide();
            //    $('canvas').show();

            //    if (jcrop_api != null) {
            //        jcrop_api.release();
            //        jcrop_api.destroy();
            //    }
            //    $('#image, #crop-gui').hide();
            //    $('.finishcrop').addClass('disabled');
            //});
            //SN Map canvas
            //function setCanvasCoords(c) {
            //    canvas.width = c.w;
            //    canvas.height = c.h;
            //    var context = canvas.getContext('2d');
            //    var image = new Image();
            //    image.src = document.getElementById('image').src;

            //    //image.onload = function() {
            //    context.drawImage(image, c.x, c.y, c.w, c.h, 0, 0, c.w, c.h);
            //    //}

            //    var dataURL = canvas.toDataURL("image/jpeg");
            //    $('#base64').val(dataURL);
            //};

            //SN resize and set base64 string for form submittal, show original picture, rotate and diminish quality if needed
            function loadphoto(file) {

                var fileName = file.value;
                var fileType = fileName.split(".")[1].toUpperCase()

                file = file.files[0];

                if (fileType == "PNG" || fileType == "JPG" || fileType == "JPEG" || fileType == "GIF") {
                    var reader = new FileReader();

                    reader.onloadend = function () {

                        var tempImg = new Image();
                        tempImg.src = reader.result;

                        //$('#image').attr('src', tempImg.src); //show original picture preview

                        var b64 = tempImg.src;
                        var bin = atob(b64.split(',')[1]);
                        var exif = EXIF.readFromBinaryFile(new BinaryFile(bin)); //extract exif data                    

                        tempImg.onload = function () {

                            var MAX_WIDTH = 1024;
                            var MAX_HEIGHT = 1024;

                            tempW = tempImg.width;
                            tempH = tempImg.height;

                            if (tempW > tempH) {
                                if (tempW > MAX_WIDTH) {
                                    tempH *= MAX_WIDTH / tempW;
                                    tempW = MAX_WIDTH;
                                }
                            } else {
                                if (tempH > MAX_HEIGHT) {
                                    tempW *= MAX_HEIGHT / tempH;
                                    tempH = MAX_HEIGHT;
                                }
                            }

                            var mpImg = new MegaPixImage(this);
                            var resImg = document.getElementById('image');

                            //SN orientation needs to be fixed for photos taken on Apple devices
                            if (exif.Orientation != null && exif.Orientation != 1) {
                                mpImg.render(canvas, { maxWidth: MAX_WIDTH, maxHeight: MAX_HEIGHT, orientation: exif.Orientation });
                                // swap height and width for cropping of 90o and 270o rotated pix
                                if (exif.Orientation == 6 || exif.Orientation == 8) {
                                    var tempHtoW = tempH;
                                    tempH = tempW;
                                    tempW = tempHtoW;
                                }
                            } else {
                                mpImg.render(canvas, { maxWidth: MAX_WIDTH, maxHeight: MAX_HEIGHT });
                            };

                            //SN resize happens here if its a large photo
                            if (file.size > 1024 * 1024) {
                                var dataURL = canvas.toDataURL("image/jpeg", 0.65);
                            } else {
                                var dataURL = canvas.toDataURL("image/jpeg");
                            }

                            $('#base64').val(dataURL);

                            $('canvas').hide();
                            //if (jcrop_api != null) {
                            //    jcrop_api.release();
                            //    jcrop_api.destroy();
                            //}

                            //$('#crop-msg-done, #crop-msg-welcome').hide();
                            //$('#crop-msg-info, .finishcrop').show();
                            //$('.startcrop').show().removeClass('disabled');

                            $('#image').attr('src', dataURL);
                            $('#image').css({ 'height': 'auto', 'width': 'auto' });

                        }

                    }
                    reader.readAsDataURL(file);

                    if (file) {
                        var fileSize = 0;
                        if (file.size > 1024 * 1024) {
                            fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB (Large File!)';
                        } else {
                            fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
                        }
                        $('#filesize').text('Size: ' + fileSize);
                        $('#lblPhoto').text(file.name);
                    }
                                        
                }
                else {
                    clearpicture();
                    alert("File extension " + fileType + " is invalid. Please use an image that has the file type JPG, PNG or GIF.");
                }
            }

            function clearpicture() {
                //stopJcrop();
                //jcrop_api = null;
                //if (jcrop_api != null) {
                //    jcrop_api.release();
                //    jcrop_api.destroy();
                //}
                $('canvas').hide();
                //$('#crop-msg-welcome').show();
                //$("#crop-gui").hide();

                var control = $("#file");
                control.replaceWith(control = control.clone(true)); //IE input type=file is read only, clone to clear it.        

                $('#base64').val("");
                $('#image').css({ 'height': 'auto', 'width': 'auto' });
                $('#image').attr('src', "Content/placeholder.png");

            }
        }

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

//SN - initialize map display
        var myOptions = {
            zoom: 14,
            center: new google.maps.LatLng(32.715, -117.158),
            mapTypeId: google.maps.MapTypeId.STREET,
            draggable: false,
            panControl: true
        };
        var map = new google.maps.Map(document.getElementById("map"), myOptions);

        var address = $('#txtAddressNumber').val() + " " +
                        $('#txtAddressStreet').val() + ", " +
                        $('#txtCity').val() + ", " +
                        //$('#ddlCounty').val() + " " +
                        $('#ddlState').val() + " " +
                        $('#txtZipCode').val();

        var zipexists = $("#txtZipCode").val();
        if (zipexists.length > 0) {
            gmapgeocode(address);
        }

        $("#txtZipCode").on( "blur", function () {
            var newaddress = $('#txtAddressNumber').val() + " " +
                        $('#txtAddressStreet').val() + ", " +
                        $('#txtCity').val() + ", " +
                        //$('#ddlCounty').val() + " " +
                        $('#ddlState').val() + " " +
                        $('#txtZipCode').val();
            if (newaddress.length > 4) {
                gmapgeocode(newaddress);
            }
        });

        function gmapgeocode(address) {
            geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    map.setCenter(results[0].geometry.location);
                    var infowindow = new google.maps.InfoWindow({
                        content: "Please Confirm this address! <br>" + address
                    });
                    var marker = new google.maps.Marker({
                        map: map,
                        position: results[0].geometry.location,
                        title: "TMH Address"
                    });
                    //SN - marker click event
                    infowindow.open(map, marker);
                    google.maps.event.addListener(marker, 'click', function () {
                        infowindow.open(map, marker);
                    });
                    $('#rblAddress').focus();
               } else {
                    $("#maperror").text("Map can't display the address: " + status);
                }

            });
        }


        // * iOS zooms on form element focus. This script prevents that behavior.
        // * <meta name="viewport" content="width=device-width,initial-scale=1">
        //      If you dynamically add a maximum-scale where no default exists,
        //      the value persists on the page even after removed from viewport.content.
        //      So if no maximum-scale is set, adds maximum-scale=10 on blur.
        //      If maximum-scale is set, reuses that original value.
        // * <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=2.0,maximum-scale=1.0">
        //      second maximum-scale declaration will take precedence.
        // * Will respect original maximum-scale, if set.
        // * Works with int or float scale values.
        function cancelZoom() {
            var d = document,
                viewport,
                content,
                maxScale = ',maximum-scale=',
                maxScaleRegex = /,*maximum\-scale\=\d*\.*\d*/;

            // this should be a focusable DOM Element
            if (!this.addEventListener || !d.querySelector) {
                return;
            }

            viewport = d.querySelector('meta[name="viewport"]');
            content = viewport.content;

            function changeViewport(event) {
                // http://nerd.vasilis.nl/prevent-ios-from-zooming-onfocus/
                viewport.content = content + (event.type == 'blur' ? (content.match(maxScaleRegex, '') ? '' : maxScale + 10) : maxScale + 1);
            }

            // We could use DOMFocusIn here, but it's deprecated.
            this.addEventListener('focus', changeViewport, true);
            this.addEventListener('blur', changeViewport, false);
        }

        // jQuery-plugin
        (function ($) {
            $.fn.cancelZoom = function () {
                return this.each(cancelZoom);
            };
            // Usage:
            $('input:text,select,textarea').cancelZoom();
        })(jQuery);
        
    </script> 
    
</body>
</html>

