using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    /// <summary>
    /// Interupts programm
    /// </summary>
    public sealed class Comand_EXIT_Exception : Exception
    {
        private string p;

        public Comand_EXIT_Exception(string p) : base("Script aborted:"+p)
        {           
            this.p = p;
        }

        public Comand_EXIT_Exception() : base("Script aborted")
        {
   
        }
   
    }
}
