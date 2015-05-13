using rirbatch.Commands.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands
{

    public enum Command_DIRCOPY_CopyMode
    {
        FAIL_IF_SOMETHING_EXISTS,
        OVERWRITE_IF_NEWER,
        OVERWRITE_ALWAYS
    }

    /// <summary>
    /// Directory kopieren.
    /// optional mit allen sub directories
    /// 
    /// BRAUCHT copy new klasse !
    /// </summary>
    public class Command_DIRCOPY : Logger, CommandInterface
    {
        private Command_DIRCOPY_CopyMode copyMode;

        public Command_DIRCOPY(Command_DIRCOPY_CopyMode cm)
        {
            this.copyMode = cm;
        }

        /// <summary>
        /// check if the source directory exists
        /// </summary>
        /// <param name="dir"></param>
        private void checkSourceDirectory(DirectoryInfo dir)
        {
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + dir.FullName);
            }
        }

        /// <summary>
        /// create target directory if it does not exist
        /// </summary>
        /// <param name="destDirName"></param>
        private void createTargetDirectoryIfMissing(string destDirName)
        {
            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Logger.write(LogLevel.INFO, "Creating : " + destDirName);
                Directory.CreateDirectory(destDirName);
            }
        }     

        /// <summary>
        /// copys all files in a directory
        /// target first THEN source
        /// </summary>
        /// <param name="destDirName">target</param>
        /// <param name="dir">source</param>
        private void getDirectoryFilesAndCopyThem(string destDirName, DirectoryInfo dir)
        {
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                Logger.write(LogLevel.INFO, file.FullName + " > " + destDirName);
                string temppath = Path.Combine(destDirName, file.Name);

                FileInfo f2 = new FileInfo(temppath);

                if (this.copyMode == Command_DIRCOPY_CopyMode.FAIL_IF_SOMETHING_EXISTS)
                    file.CopyTo(temppath, false);
                else if (this.copyMode == Command_DIRCOPY_CopyMode.OVERWRITE_ALWAYS)
                    file.CopyTo(temppath, true);
                else
                    Command_COPY_NEW.copyFileIfNewer(file, f2);
            }
        }


        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {
                // Get the subdirectories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                DirectoryInfo[] dirs = dir.GetDirectories();

                this.checkSourceDirectory(dir);                     //check if source exists
                this.createTargetDirectoryIfMissing(destDirName);   //create target if missing

                this.getDirectoryFilesAndCopyThem(destDirName, dir);  //copy directory files

                // If copying subdirectories, copy them and their contents to new location. 
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }
            }
            catch (Exception ex)
            {
                nop(ex);
                throw;
            }
        }


        public void execute(List<String> parameters)
        {
            for (int a = 1; a < parameters.Count; a++)
                if (parameters[a].Trim() == "")
                    continue;
                else
                    this.DirectoryCopy(parameters[0], parameters[a],true);

        }
    }
}
