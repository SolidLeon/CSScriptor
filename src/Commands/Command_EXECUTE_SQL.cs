using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    public sealed class Command_EXECUTE_SQL : CommandInterface
    {
        public void executeSQL(String connectionString, String sql)
        {
            using (SqlConnection myConn = new SqlConnection(connectionString))
            {
                SqlCommand myCommand = new SqlCommand(sql, myConn);
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
        }

        public void execute(List<String> parameters)
        {            
            String sql="";

            for(int a=1;a<parameters.Count;++a)
                sql+=("\r\n"+parameters[a]); // yes i knmow string builder :p

            executeSQL(parameters[0], sql);
        }
    }
}
