using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    public class Command_MKDIR:CommandInterface
    {
        public void execute(List<String> parameters)
        {
            foreach (String para in parameters)
             if (para.Trim() == "") 
                    continue;
             else
                    Directory.CreateDirectory(para);
            

        }
    }
}
