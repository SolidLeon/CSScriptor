using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    /// <summary>
    /// Copy ALL files in directory a to directory b
    /// but only if a files are newer then b files
    /// 
    /// Does NOT copy sub directories
    /// </summary>
    public class Command_COPY_NEW : Logger, CommandInterface
    {
        private bool ignoreErrors = false;

        public Command_COPY_NEW(bool ignoreErrors)
        {
            this.ignoreErrors = ignoreErrors;
        }


        /// <summary>
        /// Copy file fi to f2 if fi is newer !
        /// </summary>
        /// <param name="fi">FROM</param>
        /// <param name="fi2">TO</param>
        public static void copyFileIfNewer(FileInfo fi, FileInfo fi2)
        {
            try
            {
                if (fi.LastWriteTime > fi2.LastWriteTime)
                {
                    Logger.write(LogLevel.INFO, "COPY : " + fi.FullName + " > " + fi2.FullName);
                    File.Copy(fi.FullName, fi2.FullName, true);
                }
                else
                    Logger.write(LogLevel.DEBUG, "no copy");
            }
            catch (Exception exi)
            {
                Logger.write(LogLevel.ERROR, "Copy failed : " + exi.Message);
                //throw;
                snop();
            }
        }
            
        public void execute(List<String> parameters)
        {
            try
            {
                    String[] extensions = parameters[0].Trim().Split(',');
                    DirectoryInfo diFrom = new DirectoryInfo(parameters[1]);
                    if (diFrom.Exists == false) throw new Exception("directory from does not exist");

                    for (int b = 2; b < parameters.Count; ++b)
                    {
                        if (parameters[b].Trim() == "")
                            continue;

                        DirectoryInfo diTo = new DirectoryInfo(parameters[b]);

                        if (diTo.Exists == false) throw new Exception("directory to does not exist"); //maybe ask for creation ?

                        foreach (FileInfo fi in diFrom.GetFiles())
                        {
                            try
                            {
                                if ((extensions.Contains(fi.Extension.ToLower()) == true) || (extensions[0] == "*"))
                                {
                                    String s1 = parameters[1].Trim();
                                    String s2 = parameters[b].Trim();
                                    String s3 = fi.FullName;
                                    String toFile = s3.Replace(s1, s2);
                                    FileInfo fi2 = new FileInfo(toFile);

                                    Logger.write(LogLevel.DEBUG, "" + fi.LastWriteTime + ">" + fi2.LastWriteTime);
                                    Logger.write(LogLevel.DEBUG, fi.FullName + " > " + fi2.FullName);                                

                                    copyFileIfNewer(fi, fi2);
                                }
                                else
                                {
                                    nop();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }

                //}
            }
            catch (Exception ex)
            {
                if (this.ignoreErrors == true)                    
                    nop(ex.Message);
                else
                    throw;
            }

        }
    }
}
