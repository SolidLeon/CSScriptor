using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    public class Command_EXEC : Logger, CommandInterface
    {
        public void execute(List<String> parameters)
        {
            String pars="";

            if(parameters.Count>2)
                for (int a=2;a<parameters.Count;a++)
                {
                    if (parameters[a].Trim() == "")
                        continue;

                    pars+=" ";
                    pars+=parameters[a].Trim();               
                }

            //C:\uc4\900Alow_licensetest\Utility\bin>java -Xmx512m -cp C:\uc4\900Alow_licensetest\Utility\bin;C:\uc4\900Alow_licensetest\UserInterface\bin\UC4LookAndFeel.jar -jar C:\uc4\900Alow_licensetest\Utility\bin\ucybdbld.jar -B -XC:\mix\34616.txt
            //System.Diagnostics.Process.Start(parameters[0],pars);

            System.Diagnostics.Process clientProcess = new Process();
            clientProcess.StartInfo.FileName = parameters[0].Trim();
            clientProcess.StartInfo.Arguments = pars;
            //clientProcess.StartInfo.UseShellExecute = false;
            clientProcess.StartInfo.WorkingDirectory = parameters[1].Trim();
            clientProcess.Start();
            clientProcess.WaitForExit();
            int code = clientProcess.ExitCode;

        }
    }
}
