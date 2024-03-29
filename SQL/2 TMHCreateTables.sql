USE [TakeMeHome]
GO
/****** Object:  Table [dbo].[TMH_Codes]    Script Date: 8/14/2018 12:09:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TMH_Codes](
	[Field] [char](30) NOT NULL,
	[Code] [char](20) NOT NULL,
	[Description] [char](80) NULL,
 CONSTRAINT [PK_TMH_Codes] PRIMARY KEY CLUSTERED 
(
	[Field] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TMHFiles]    Script Date: 8/14/2018 12:09:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMHFiles](
	[id] [uniqueidentifier] NOT NULL,
	[FileImage] [image] NOT NULL,
 CONSTRAINT [PK_TMHFiles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TMHRec]    Script Date: 8/14/2018 12:09:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMHRec](
	[id] [uniqueidentifier] NOT NULL,
	[lbxDiagnosis] [nvarchar](max) NULL,
	[FileId] [uniqueidentifier] NULL,
	[FileName] [nvarchar](max) NULL,
	[AGE_IN_PHOTOStart] [nvarchar](3) NULL,
	[txtPhotoDate] [datetime] NULL,
	[txtLastName] [nvarchar](20) NULL,
	[txtFirstName] [nvarchar](20) NULL,
	[txtMiddleName] [nvarchar](20) NULL,
	[SUFFIX_NAME] [nvarchar](4) NULL,
	[txtNameToCallMe] [nvarchar](50) NULL,
	[txtHomePhone] [nvarchar](20) NULL,
	[txtAddressNumber] [nvarchar](10) NULL,
	[txtAddressStreet] [nvarchar](30) NULL,
	[txtCity] [nvarchar](30) NULL,
	[ddlCounty] [nvarchar](30) NULL,
	[ddlState] [nvarchar](2) NULL,
	[txtZipCode] [nvarchar](10) NULL,
	[ddlRace] [nvarchar](5) NULL,
	[ddlSex] [nvarchar](1) NULL,
	[txtDOB] [datetime] NULL,
	[txtAge] [nvarchar](4) NULL,
	[ddlHeight] [nvarchar](5) NULL,
	[txtWeight] [nvarchar](4) NULL,
	[ddlEye] [nvarchar](5) NULL,
	[ddlHair] [nvarchar](5) NULL,
	[ddlHomeType] [nvarchar](30) NULL,
	[ddlWander] [nvarchar](5) NULL,
	[ddlCommunication] [nvarchar](max) NULL,
	[ddlMedication] [nvarchar](5) NULL,
	[txtLanguages] [nvarchar](max) NULL,
	[txtMedical] [nvarchar](max) NULL,
	[txtWornItems] [nvarchar](max) NULL,
	[txtApproach] [nvarchar](max) NULL,
	[txtBehaviors] [nvarchar](max) NULL,
	[lbxSpecial] [nvarchar](max) NULL,
	[ddlApproved] [nvarchar](5) NULL,
	[txtBracelet] [nvarchar](20) NULL,
	[txtBraceletID] [nvarchar](20) NULL,
	[ddlOrg] [nvarchar](max) NULL,
	[txtDLNum] [nvarchar](20) NULL,
	[txtDLExpDT] [datetime] NULL,
	[ddlDLState] [nvarchar](2) NULL,
	[ddlContactRelationship] [nvarchar](20) NULL,
	[txtContactFullName] [nvarchar](50) NULL,
	[txtContactAddress] [nvarchar](50) NULL,
	[txtContactCity] [nvarchar](30) NULL,
	[ddlContactState] [nvarchar](2) NULL,
	[txtContactZip] [nvarchar](10) NULL,
	[txtContactHPhone] [nvarchar](20) NULL,
	[txtContactMPhone] [nvarchar](20) NULL,
	[txtContactOPhone] [nvarchar](20) NULL,
	[txtContactEMail] [nvarchar](100) NULL,
	[ddlContactRelationship2] [nvarchar](20) NULL,
	[txtContactFullName2] [nvarchar](50) NULL,
	[txtContactAddress2] [nvarchar](50) NULL,
	[txtContactCity2] [nvarchar](30) NULL,
	[ddlContactState2] [nvarchar](2) NULL,
	[txtContactZip2] [nvarchar](10) NULL,
	[txtContactHPhone2] [nvarchar](20) NULL,
	[txtContactMPhone2] [nvarchar](20) NULL,
	[txtContactOPhone2] [nvarchar](20) NULL,
	[txtContactEMail2] [nvarchar](100) NULL,
	[ddlContactRelationship3] [nvarchar](20) NULL,
	[txtContactFullName3] [nvarchar](50) NULL,
	[txtContactAddress3] [nvarchar](50) NULL,
	[txtContactCity3] [nvarchar](30) NULL,
	[ddlContactState3] [nvarchar](2) NULL,
	[txtContactZip3] [nvarchar](10) NULL,
	[txtContactHPhone3] [nvarchar](20) NULL,
	[txtContactMPhone3] [nvarchar](20) NULL,
	[txtContactOPhone3] [nvarchar](20) NULL,
	[txtContactEMail3] [nvarchar](100) NULL,
	[ddlVehType] [nvarchar](5) NULL,
	[ddlVehYear] [nvarchar](4) NULL,
	[ddlVehMake] [nvarchar](5) NULL,
	[txtVehModel] [nvarchar](20) NULL,
	[ddlVehColor] [nvarchar](5) NULL,
	[txtVehVIN] [nvarchar](40) NULL,
	[txtVehLic] [nvarchar](15) NULL,
	[ddlVehLicState] [nvarchar](2) NULL,
	[ddlVehLicYear] [nvarchar](4) NULL,
	[txtEnrollDate] [datetime] NULL,
	[ddlEnrollingAgency] [nvarchar](10) NULL,
	[txtUserID] [nvarchar](50) NULL,
	[SubmittedDateTime] [datetime] NULL,
 CONSTRAINT [PK_TMHRec_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TMHUserExceptions]    Script Date: 8/14/2018 12:09:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMHUserExceptions](
	[id] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[CreatedWhen] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TMHUserRecs]    Script Date: 8/14/2018 12:09:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMHUserRecs](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Recid] [uniqueidentifier] NOT NULL,
	[ddlAddress] [nchar](10) NULL,
 CONSTRAINT [PK_TMHUserRecs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TMHUsers]    Script Date: 8/14/2018 12:09:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TMHUsers](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[PassWord] [nvarchar](128) NOT NULL,
	[CreatedWhen] [datetime] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
	[Status] [varchar](1) NULL,
	[PIN] [varchar](4) NOT NULL,
	[IP] [varchar](20) NULL,
 CONSTRAINT [PK_TMHUsers] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
