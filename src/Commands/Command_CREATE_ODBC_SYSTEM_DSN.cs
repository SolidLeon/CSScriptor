using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    /// <summary>
    /// Creates a 32bit!!!!! system odb dsn 
    /// dsn entry will be useless for installations on 64 bit system
    /// </summary>
    public  class Command_CREATE_ODBC_SYSTEM_DSN : Logger, CommandInterface
    {
        public void execute(List<String> parameters)
        {
            ODBC_Manager om = new ODBC_Manager();
            for(int a=1;a<parameters.Count;a++)
            {
                String para = parameters[a];
                if (para.Trim() == "") 
                    continue;

                //TODO remove one
                om.CreateDSN(parameters[0].Trim(),para.Trim()+"cd");
                om.addDSNviaRegistry(parameters[0].Trim(),para.Trim()); 
            }//for

        }//execute
    }//class
}//namespace
