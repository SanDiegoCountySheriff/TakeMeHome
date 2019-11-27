using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SQLCalssLib
/// </summary>
public class SQLCalssLib
{
    SqlConnection conn = new SqlConnection();
    SqlDataAdapter adapter = new SqlDataAdapter();
    SqlCommand Command = new SqlCommand();
    DataSet ds = new DataSet();

    public SQLCalssLib()
    {
        string strDBSQLServer = System.Configuration.ConfigurationManager.AppSettings["DBSQLServer"];

        string csConnString = strDBSQLServer;

        conn.ConnectionString = csConnString;
    }   

    public DataSet mds_ExecuteQuery(string psCmdText, string psTableName)
        {
            try
            {
                adapter.SelectCommand = new SqlCommand(psCmdText, conn);
                adapter.SelectCommand.CommandTimeout = 300;					// 5 mintues
                adapter.Fill(ds, psTableName);
                return ds;
            }
            catch
            {
                //return exception error handling
                return ds;
            }
        }

        public string GetDatabaseName()
        {
            //return conn.Database.ToString();
            return adapter.SelectCommand.ToString();
        }

        public int mv_InsertOrUpdate(string psIn)
        {
            Command.CommandText = psIn;
            return (Command.ExecuteNonQuery());
        }

        public void mv_OpenConnection2(string strRemoteAddress)
        {
            string strDBSQLServer = System.Configuration.ConfigurationManager.AppSettings["DBSQLServer"];
            string csConnString = strDBSQLServer + "; Application Name=" + strRemoteAddress;

            conn.ConnectionString = csConnString;
            Command.Connection = conn;
            try
            {
                conn.Open();
            }
            catch
            {
            }
        }

        public void mv_CloseConnection()
        {
            Command.Connection.Close();
        }
    }
    // end SQlDataAccessorClass 
	
