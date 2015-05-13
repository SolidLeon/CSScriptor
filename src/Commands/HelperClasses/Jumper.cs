using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    /// <summary>
    /// Handles the jump to / label logic
    /// </summary>
   public  class Jumper : Nop
    {
       //int countTiefe;
       List<String> scriptLines;
       int commandDepth;
       public Jumper(//int p_countTiefe, 
           ref List<String>  p_scriptLines, int p_commandDepth)
       {
          // this.countTiefe = p_countTiefe;
           this.scriptLines = p_scriptLines;
           this.commandDepth = p_commandDepth;
       }

        public int getLabelPos(String name)
        {
            for (int a = 0; a < this.scriptLines.Count; ++a)
            {
                if (DepthCalculator.countTiefe(this.scriptLines[a]) == this.commandDepth)
                    if (this.scriptLines[a].Trim() == "LABEL")
                        if (this.scriptLines[a + 1].Trim() == name)
                            return a - 1;
            }

            return -1;
        }

        /// <summary>
        /// auswerten von = < und > zb in jump to if anweisungen
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool checkBool(String str)
        {
            if (str.Trim() == "") return true;

            String[] vergleich = null;

            try
            {
                vergleich = str.Trim().Split(new string[] { "!=", "<>" }, StringSplitOptions.RemoveEmptyEntries);
                if (vergleich.Length > 1)
                {
                    //UNGLEICH
                    if (vergleich[0] != vergleich[1]) return true;
                }
                else
                {
                    vergleich = str.Trim().Split(new char[] { '=' });
                    if (vergleich.Length > 1)
                    {
                        //GLEICH
                        if (vergleich[0] == vergleich[1]) return true;
                    }
                    else
                    {
                        vergleich = str.Trim().Split(new char[] { '<' });
                        if (vergleich.Length > 1)
                        {
                            //KLEINER
                            if (Convert.ToInt32(vergleich[0]) < Convert.ToInt32(vergleich[1])) return true;
                        }
                        else
                        {
                            //GROESSER
                            vergleich = str.Trim().Split(new char[] { '>' });
                            if (vergleich.Length > 1)
                            {
                                if (Convert.ToInt32(vergleich[0]) > Convert.ToInt32(vergleich[1])) return true;
                            }
                        }//kleiner else
                    }//gleich else
                }//ungleich else
            }
            catch (Exception ex)
            {
                nop(ex);
                throw;
            }

            return false;
        }

        /// <summary>
        /// Jum to if contains auflösung
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool checkContains(String str)
        {
            if (str.Trim() == "") return true;

            String[] vergleich = null;

            try
            {
                vergleich = str.Trim().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (vergleich.Length > 1)
                {
                    //Vergleich
                    if (vergleich[0].Contains(vergleich[1]) == true)
                        return true;
                }

            }
            catch (Exception ex)
            {
                nop(ex);
                throw;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <param name="filecheck"></param>
        /// <param name="lineIndex"></param>
        /// <returns>true if jumped</returns>
        public bool executeJumps(String cmd, ref List<String> parameters, bool filecheck, ref int lineIndex)
        {
            switch (cmd)
            {
                case "JUMP TO": if (filecheck == false)
                        lineIndex = this.getLabelPos(parameters[0].Trim());
                    return true;
                case "JUMP TO IF":
                    bool bok = true;
                    for (int a = 1; a < parameters.Count; a++)
                        if (checkBool(parameters[a]) == true) nop(); else bok = false; //one wrong -> ignore command

                    if (bok == true)
                        if (filecheck == false)
                            lineIndex = this.getLabelPos(parameters[0].Trim());
                    return true;
                case "JUMP TO IF (OR)":
                    bool bok2 = false;
                    for (int a = 1; a < parameters.Count; a++)
                        if (parameters[a].Trim() == "") continue;
                        else
                            if (checkBool(parameters[a]) == true) bok2 = true; //one ok -> do it

                    if (bok2 == true)
                        if (filecheck == false)
                            lineIndex = this.getLabelPos(parameters[0].Trim());
                    return true;
                case "JUMP TO IF (CONTAINS)":
                    bool bok3 = false;
                    for (int a = 1; a < parameters.Count; a++)
                        if (parameters[a].Trim() == "") continue;
                        else
                            if (checkContains(parameters[a]) == true) bok3 = true; //one ok -> do it

                    if (bok3 == true)
                        if (filecheck == false)
                            lineIndex = this.getLabelPos(parameters[0].Trim());
                    return true;

            }

            return false;
        }
    }
}
