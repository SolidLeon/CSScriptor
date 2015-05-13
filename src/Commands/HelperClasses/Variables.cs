using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    /// <summary>
    /// Stores all variables
    /// </summary>
    public class Variables : Nop
    {
        /// <summary>
        /// Script variables
        /// </summary>
        public List<string> variableKeys = new List<string>();
        public List<object> variables = new List<object>();
        //private Dictionary<String, int> gotoTargets = new Dictionary<string, int>();

        /// <summary>
        /// Neue script variable anlegen
        /// alte überschreiben falls bereits vorhanden :)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void addVariable(String key, object val)
        {
            this.removeVariable(key);
            this.variableKeys.Add(key);
            this.variables.Add(val);
       }

        /// <summary>
        /// UNSET vars
        /// </summary>
        /// <param name="key"></param>
        public void removeVariable(String key)
        {
            int idx = this.variableKeys.IndexOf(key.Trim());

            if (idx >= 0)
            {
                this.variables.RemoveAt(idx);
                this.variableKeys.RemoveAt(idx);
            }
        }

        public void clearVariables()
        {
            this.variableKeys.Clear();
            this.variables.Clear();
        }

        /// <summary>
        /// Variablen setzen
        /// </summary>
        /// <param name="parameters"></param>
        public void executeCommand_SET(List<string> parameters)
        {
            foreach (String para in parameters)
            {
                if (para.Trim() == "") continue;

                string[] pars = para.Split('='); //TODO allow = in value

                List<String> paras = new List<string>();
                paras.Add(pars[0]);
                paras.Add(pars[1]);

                executeCommand_DEF(paras);
            }
        }

        /// <summary>
        /// remove variables)
        /// </summary>
        /// <param name="parameters"></param>
        public void executeCommand_UNSET(List<string> parameters)
        {
            nop();
            foreach (String s in parameters) this.removeVariable(s);
        }

        /// <summary>
        /// like set but multiline
        /// </summary>
        /// <param name="parameters"></param>
        public void executeCommand_DEF(List<string> parameters)
        {
            //cut tail empty lines (multilines are handelt different so it makes sense)
            int emptyTailLines = 1;
            for (int a = parameters.Count - 1; a > 0; --a)
                if (parameters[a].Trim() == "") emptyTailLines++;


            List<string> trimmedParams = new List<string>();
            foreach (string par in parameters)
                trimmedParams.Add(par.Trim());

            //and add var :)
            this.addVariable(
                                    trimmedParams[0],
                                    trimmedParams.GetRange(1, parameters.Count - emptyTailLines)
                             );

        }//method
    }//class
}//namespace
