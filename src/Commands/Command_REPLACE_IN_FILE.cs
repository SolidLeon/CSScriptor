using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    /// <summary>
    /// Ersetzt zeichen kette p1 gegen zeichenkette p2 in allen files p3 bis pN
    /// </summary>
    public class Command_REPLACE_IN_FILE : Logger, CommandInterface
    {
        public void execute(List<String> parameters)
        {
            Encoding enc = Encoding.Default; //.GetEncoding(1252);

            foreach (String para in parameters)
            {
                Logger.write(LogLevel.DEBUG,para);

                String s1 = parameters[0].Trim();
                String s2 = parameters[1].Trim();

                for (int i = 2; i < parameters.Count; ++i)
                {
                    try
                    {
                        if (parameters[i].Trim() == "")
                            continue;

                        String sFile = File.ReadAllText(parameters[i]);//,enc);

                        sFile = sFile.Replace(s1, s2);

                        File.WriteAllText(parameters[i], sFile,enc);
                        nop();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                
            }

        }
    }
}
