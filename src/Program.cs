using rirbatch.Commands;
using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch
{    
   

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                foreach (String arg in args)
                {
                    if (arg == "--help")
                    {
                        String[] lines = File.ReadAllLines(".\\help.txt");
                        foreach (String line in lines)
                            Logger.write(LogLevel.INFO, line);
                    }
                    else
                    {
                        CommandProcessor.runBatch(arg);
                        break; //execute 1st file, maybe all in future
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

                
        }
    }
}
