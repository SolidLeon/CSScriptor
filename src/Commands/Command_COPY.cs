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
    /// Copys SINGLE files from parameter 0 to all other locations (parameter 1 to prameter n)
    /// </summary>
    public class Command_COPY : CommandInterface
    {
        public void execute(List<String> parameters)
        {
            for (int a = 1; a < parameters.Count; a++)
            {
                try
                {
                    if (parameters[a].Trim() == "")
                        continue;

                    Logger.write(LogLevel.INFO,parameters[0] + " > " + parameters[a]);
                    File.Copy(parameters[0], parameters[a],true);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
    }
}
