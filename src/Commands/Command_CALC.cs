using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    /// <summary>
    /// Einfachtse mathematische funktionen
    /// </summary>
    public class Command_CALC : CommandInterface
    {
        Variables variabless = null;

        public Command_CALC(ref Variables vars)
        {
            this.variabless = vars;
        }


        public void execute(List<String> parameters)
        {
            String what = parameters[0].Trim();
            String resultVar = parameters[1].Trim();

            //startwert
            BigInteger result = new BigInteger(0);
            BigInteger.TryParse(parameters[2], out result);

            //restlichen werte auf start wert/last result anwenden
            for (int a = 3; a < parameters.Count; a++)
            {
                try
                {
                    if (parameters[a].Trim() == "")
                        continue;

                    BigInteger bi = new BigInteger();
                    BigInteger.TryParse(parameters[a],out bi);

                    switch (what)
                    {
                        case "+":
                            result += bi;
                            break;
                        case "-":
                            result -= bi;
                            break;
                        case "*":
                            result *= bi;
                            break;
                        case "/":
                            result /= bi;
                            break;
                    }
                    
                }
                catch (Exception ex)
                {
                    throw;
                }

                List<String> res = new List<string>();
                res.Add(result.ToString());
                this.variabless.addVariable(resultVar, res );
            }

        }
    }
}
