using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    /// <summary>
    /// Echo mit pause nach jeder zeile
    /// 
    /// </summary>
    public class Command_ECHO_PAUSED : CommandInterface
    {
        public void execute(List<String> parameters)
        {
            foreach (String para in parameters)
            {
                Console.WriteLine(para);               
                new Nop().nop(Console.ReadLine());
            }

        }
    }
}
