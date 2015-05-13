using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    public class Nop 
    { 
        public object nop(params object[] parameters) { return null; }
        public static object snop(params object[] parameters) { return null; }
    }

}
