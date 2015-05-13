using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    /// <summary>
    /// stellt ein simples echo zur verfügung
    /// so wie in bat dateien
    /// </summary>
    public class Command_ECHO : CommandInterface
    {
        public void execute(List<String> parameters)
        {
            foreach (String para in parameters)
                Console.WriteLine(para);

        }
    }
}
