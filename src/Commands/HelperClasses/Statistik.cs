using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    /// <summary>
    /// Übersicht über den Programm Ablauf.
    /// Bzw. den Erfolg der Abarbeitung 
    /// </summary>
    public class Statistik
    {
        public long commandsExecuted = 0;
        public long commandsWithErrors = 0;
        public long commandsWithWarning = 0;
    }

}
