using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        GOOD,
        SPECIAL
    }

    public class Logger  : Nop
    {
        public static void write(LogLevel ll, String text)
        {
            ConsoleColor cc = Console.ForegroundColor;

            switch (ll)  //Log Level NOT eleven XD
            {
                default:
                case LogLevel.DEBUG: Console.ForegroundColor = ConsoleColor.Gray; break;
                case LogLevel.INFO: Console.ForegroundColor = ConsoleColor.White; break;
                case LogLevel.WARNING: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogLevel.ERROR: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogLevel.GOOD: Console.ForegroundColor = ConsoleColor.Green; break;
                case LogLevel.SPECIAL: Console.ForegroundColor = ConsoleColor.Magenta; break;
            }

            Console.WriteLine(text);
            Console.ForegroundColor = cc;
        }
    }
}
