using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    /// <summary>
    /// Minimum set an funktionen die eine command_xxxx klasse supporten muss
    /// </summary>
    public interface CommandInterface
    {
        void execute(List<String> parameters);

        //public void generateCode(List<String> parameters);
    }

}
