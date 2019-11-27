# Take Me Home (Self Registry)

Take Me Home (TMH) Self-Registry is the public facing web app for individuals to use and register a person using San Diego Sheriff's
secure public facing web site. 

TMH program has basically three modules tied together.

1.	Public facing web app is hosted on San Diego Sheriff's public internet web/app server so that the public can sign up and register themselves or family members.
This module also contains a SQL database to store the data entered by the public so that they can come back and add, remove, or update their records.
[San Diego Sheriff  TMH self-registry](https://apps.sdsheriff.net/tmhself/)

2.	Our Extranet Take Me Home app is developed by Data Works Plus, our vendor and it is installed in our Extranet for law enforcement users only.
The data entered by the public in the public facing TMH app is transmitted to our TMH database in Extranet using an interface between these two modules.
Law Enforcement users, and crime analysts can view, update, enter and approve records using this module in Extranet.
This is the main repository for all TMH records In our system.
There is also connectivity to our Extranet established through our wide area network system if you are using a device that has connectivity to Extranet while you are out in the field not connected directly to Sheriff network.

3.	We also developed a web app for our social agencies to use if people decide to go to a social agency and register through them. This module is hosted in our public facing web servers so that social agencies can access it. Social agencies do not have access to Extranet. Again, any data entered by our
Social agencies is also transmitted to our main Extranet repository using an interface we developed for this.

## Requirements

* Windows server 2012 R2 or higher with Microsoft IIS Web Server version 8 or higher
* Microsoft SQL server database version 2012 or higher on a separate server
* Visual Studio version 2013 or higher for the TMH-Self registry web app written with C# .NET on a development windows PC

## Installation Instructions for the TMH Self-Registry App:

Please review the SQL scripts first and make appropriate modifications for your platform before running them. Files numbered are the scripts for the database objects.

Modify the 1st sql script file for the location of the SQL db and log files on your SQL server.

After creating the database on your server, run the 2nd sql script file to create all SQL tables in the newly TMH created database.

Use the 3rd file (3 TMHCodesData.txt) which is a text data file to be imported into a table created above. Use SQL import to get this data imported.

**Update Web.config** - Make sure you update the app settings in Web.config with your agency specific information.

The connection string needs to be modified for each site installing the TMH software in the .NET solution's web.config file. Also please check the web.config file and adjust for session state tag. We use SQL database for our session states. You may want to change that to whatever method your site is using for storing session states.
``` xml
  <appSettings>
    <add key="MaxRecords" value="300"/>
    
    <!-- <add key="DBSQLServer" value="Data Source=inettestsql; Initial Catalog=InetApps;integrated security=TRUE;persist security info=False;Trusted_Connection=Yes"/> -->
    <add key="DBSQLServer" value="Data Source=principalServer;failover partner=mirorServer; Initial Catalog=TakeMeHome; integrated security=TRUE;persist security info=False;Trusted_Connection=Yes" />
  </appSettings>

  <connectionStrings>
    <add name="LocalSqlServer" 
          connectionString="Data Source=principalServer;failover partner=mirorServer; Initial Catalog=ASPNETDB; integrated security=TRUE;persist security info=False;Trusted_Connection=Yes" 
          providerName="System.Data.SqlClient"/>
    <add name="TakeMeHomeConnectionString" 
          connectionString="Data Source=principalServer;failover partner=mirorServer;Initial Catalog=TakeMeHome;Integrated Security=True" 
          providerName="System.Data.SqlClient"/>

  </connectionStrings>
  
  <sessionState mode="SQLServer" allowCustomSqlDatabase="true" 
			  sqlConnectionString="Integrated Security=SSPI;data source=principalServer; failover partner=mirorServer;initial catalog=ASPState;" cookieless="false" timeout="180" />

  
  <setting name="SMTP_Server" serializeAs="String">
	<value>IP address</value>
  </setting>
  <setting name="TMHUrl" serializeAs="String">
	<value>https://apps.mysite.net/TMHSelf/TMHLogin.aspx</value>
  </setting>
```

**Note:** TMH code has a web interface call to our DataWorks Plus Mugshot system where we store all of our TMH records. The TMH web app has its own database to store the data in SQL server, You may want to comment out the section that Sends the data to the Mugshot system. It is a method call into the web service that needs to be commented out.