using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    public class Command_WRITE_TEXT_FILE : Logger, CommandInterface
    {
        public void execute(List<String> parameters)
        {
                Encoding enc = Encoding.Default;//.GetEncoding(1252);
                String s1 = parameters[0];

                if (File.Exists(s1) == true) throw new Exception("file does already exist");

                for (int i = 1; i < parameters.Count; ++i)
                {
                    try
                    {
                        File.AppendAllText(s1, parameters[i] + "\r\n", enc); //TODO make newline defineable
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }      
        }
    }
}
