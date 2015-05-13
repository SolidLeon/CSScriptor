using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{
    public class Command_REMOVE_FILE_ATTRIBUTE_READ_ONLY : Logger, CommandInterface
    {
        public void removeFlagFromFile(FileInfo fi)
        {
            Logger.write(LogLevel.INFO, "Removing Readonly Flag (if set) for : " + fi.FullName);
            fi.IsReadOnly = false;
            fi.Refresh();
        }

        public void removeFlag(DirectoryInfo di)
        {
            foreach (FileInfo fi in di.GetFiles())            
                this.removeFlagFromFile(fi);            

            foreach (DirectoryInfo diSub in di.GetDirectories())
                removeFlag(diSub);
        }


        public void execute(List<String> parameters)
        {
            foreach (String para in parameters)
            {
                if (para.Trim() == "")
                    continue;

                if(Directory.Exists(para))
                {
                    DirectoryInfo di=new DirectoryInfo(para);
                    removeFlag(di);
                }
                else
                {
                    FileInfo fi = new FileInfo(para);
                    this.removeFlagFromFile(fi);
                }
                
                
            }

        }
    }
}
