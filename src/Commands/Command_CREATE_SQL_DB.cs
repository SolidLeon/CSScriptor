using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    /// <summary>
    /// Erzeugt eine SQL Server Datenbank mit gegebenem namen
    /// </summary>
    public class Command_CREATE_SQL_DB : CommandInterface
    {
        public String dbFilePath = @"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"; //TODO configurable

        public void createDB(String dbname, String username, String password, String server)
        {
            String str;
            SqlConnection myConn = new SqlConnection("Server="+server+";User ID = "+username+"; Pwd = "+password+";database=master");

            str = "CREATE DATABASE MyDatabase ON PRIMARY " +
                "(NAME = MyDatabase_Data, " +
                "FILENAME = 'C:\\MyDatabaseData.mdf', " +
                "SIZE = 12MB, MAXSIZE = 110MB, FILEGROWTH = 10%) " +
                "LOG ON (NAME = MyDatabase_Log, " +
                "FILENAME = 'C:\\MyDatabaseLog.ldf', " +
                "SIZE = 11MB, " +
                "MAXSIZE = 51MB, " +
                "FILEGROWTH = 10%)";

            str = str.Replace("MyDatabase", dbname.Trim());
            str = str.Replace("C:\\", dbFilePath);

            rirbatch.Commands.HelperClasses.Logger.write(HelperClasses.LogLevel.INFO, "Create db with : " + str);

            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();                
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public void execute(List<String> parameters)
        {
            //foreach (String para in parameters)
            for (int a = 3; a < parameters.Count; a++)
            {
                String user = parameters[0];    //
                String pass = parameters[1];    //
                String server = parameters[2];  //localhost

                String para = parameters[a];
                if (para.Trim() == "")
                    continue;
                else
                    createDB(para,user,pass,server);
            }

        }
    }
}
