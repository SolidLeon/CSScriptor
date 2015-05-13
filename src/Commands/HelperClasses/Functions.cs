using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    /// <summary>
    /// This one handles functions (FUNC)
    /// 
    /// </summary>
    public class Functions : Nop
    {
        public Dictionary<String, string[]> functions = new Dictionary<string, string[]>();


        public void createFunction(ref List<String> parameters)
        {
            String functionName = parameters[0].Trim();

            string[] dummy;

            if (this.functions.TryGetValue(functionName, out dummy) == false)
            {

                List<string> funParams = new List<string>();
                for (int a = 1; a < parameters.Count; a++)
                    funParams.Add(parameters[a]);

                this.functions.Add(functionName, funParams.ToArray());
            }
            else
                nop("already addet");
        }

        public bool callFunction(string command, List<String> parameters, bool filecheck)
        {
            string[] lines = null;
            //look if function exists
            if (this.functions.TryGetValue(command.Trim(), out lines) == false)
                return false;

            List<string> linesWithParams = new List<string>();

            linesWithParams.Add(lines[0].ToLower().Replace("param", "DEF"));
            //this.labelCorrection++;
            linesWithParams.Add(lines[1]);

            int tiefe = DepthCalculator.countTiefe(lines[1]);
            foreach (String para in parameters)
            {
                string spar = para.Trim();
                linesWithParams.Add(spar.PadLeft(spar.Length + tiefe, ' '));
            }

            linesWithParams.AddRange(lines);

            CommandProcessor.executeLinesWithNewProcess(linesWithParams.ToArray(), filecheck);

            return true;
        }
    }
}
