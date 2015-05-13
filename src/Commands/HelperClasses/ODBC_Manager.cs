using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    public class ODBC_Manager : Logger
    {
        [DllImport("ODBCCP32.dll")]
        public static extern bool SQLConfigDataSource(IntPtr parent, int request, string driver, string attributes);

        [DllImport("ODBCCP32.dll")]
        public static extern int SQLGetPrivateProfileString(string lpszSection, string lpszEntry, string lpszDefault, string @RetBuffer, int cbRetBuffer, string lpszFilename);

        private const short ODBC_ADD_DSN = 1;
        private const short ODBC_CONFIG_DSN = 2;
        private const short ODBC_REMOVE_DSN = 3;
        private const short ODBC_ADD_SYS_DSN = 4;
        private const short ODBC_CONFIG_SYS_DSN = 5;
        private const short ODBC_REMOVE_SYS_DSN = 6;
        private const int vbAPINull = 0;

        public void CreateDSN(string server //like (local)
            , string strDSNName)
        {        
            try
            {            
                string strDriver = "SQL Server Native Client 10.0";

                string str = "SERVER="+server+"\0DSN=" + strDSNName + "\0DESCRIPTION=NewDSN\0DATABASE=" + strDSNName + "\0TRUSTED_CONNECTION=YES";
                if (SQLConfigDataSource((IntPtr)0, 4, strDriver, str)==true) 
                    nop(); 
                else 
                    throw new Exception("dsn could not be generated : "+strDSNName);
            }
            catch (Exception ex)
            {
                nop(ex);
                throw;
            }
        }

        public int CheckForDSN(string strDSNName)
        {
            int iData;
            string strRetBuff = "";
            iData = SQLGetPrivateProfileString("ODBC Data Sources", strDSNName, "", strRetBuff, 200, "odbc.ini");
            return iData;
        }


        /// <summary>
        /// from :
        /// http://www.codeproject.com/Tips/350601/Create-SQL-DSN-in-Csharp
        /// </summary>
        /// <param name="name"></param>
        public void addDSNviaRegistry(String server, String name)
        {
            string ODBC_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";     
            string driverName = "SQL Server Native Client 10.0";
            string dsnName = name;
            string database = name;
            string description = "This DSN was created from code!";
            //string server = "(local)";
            bool trustedConnection = false;

            // Lookup driver path from driver name         
            string driverPath = "C:\\WINDOWS\\System32\\sqlncli10.dll";//sqlsrv32.dll"; 
                     
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(ODBC_PATH + "ODBC Data Sources");         
            if (datasourcesKey == null)             
                throw new Exception("ODBC Registry key does not exist"); 
                    
            datasourcesKey.SetValue(dsnName, driverName);          
            // Create new key in odbc.ini with dsn name and add values        
            var dsnKey = Registry.LocalMachine.CreateSubKey(ODBC_PATH + dsnName);        
            if (dsnKey == null) 
                throw new Exception("ODBC Registry key for DSN was not created"); 
   
           
            dsnKey.SetValue("Database", database);         
            dsnKey.SetValue("Description", description);         
            dsnKey.SetValue("Driver", driverPath);         
            dsnKey.SetValue("LastUser", "sa");         
            dsnKey.SetValue("Server", server);         
            dsnKey.SetValue("Database", database);
            dsnKey.SetValue("username", "uc4");
            dsnKey.SetValue("password", "uc4");
            dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");
            //Please note, here I am using the hardcoded values. you can pass these values through the user 
            //input also or as per your requirement. Note the following lines in the code:
        }
    }
}
