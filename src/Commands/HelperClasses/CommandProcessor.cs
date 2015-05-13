using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{

    public class CommandProcessor : Nop
    {
        Statistik statistik=new Statistik();
        Jumper _jumper = null;
        Jumper jumper { get { if (this._jumper == null) this._jumper = new Jumper(ref this.scriptLines, this.commandDepth); return _jumper; } }

        int commandDepth = 0;                               //auf welcher einrückungstiefe befinden sich die commandos (im moment, may change)        
        List<String> scriptLines = new List<string>();

        Variables variables = new Variables();
        Functions functions = new Functions();

        /// <summary>
        /// Tab Replacement
        /// </summary>
        private int tabSize = 4;
        private String _tabReplace=null;
        private String tabReplace
        {
            get
            {
                if (_tabReplace == null)
                {
                    _tabReplace = "";
                    for (int a = 0; a < this.tabSize; a++) 
                        _tabReplace += " ";
                }

                return _tabReplace;
            }
        }

        /// <summary>
        /// SKip mode
        /// 
        /// from SKIP to SKIP END
        /// </summary>
        private bool skipping = false;


        /// <summary>
        /// Einzel commando mit parametern ausführen
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns>true if success false if error or exit</returns>
        private bool executeCommand(String command, ref List<String> parameters, bool filecheck, ref int lineIndex)
        {
            if (command == "") return true;  //RETURN

            CommandInterface ci = null;          

            Logger.write(LogLevel.DEBUG, "----------------------------------------------------------------");
            Logger.write(LogLevel.DEBUG, lineIndex+" COMMAND : " + command);
            foreach (String para in parameters)
                Logger.write(LogLevel.DEBUG, "    " + para);

            String cmd = command.Trim().ToUpper();

            if ((this.skipping == true) && (cmd != "SKIP END")) return true; //RETURN
            //this.addGotoTargets(cmd, filecheck,ref parameters,lineIndex);

            if (this.jumper.executeJumps(cmd, ref parameters, filecheck, ref lineIndex) == false)
            {
                try
                {
                    switch (cmd)
                    {
                        case "CALC": ci = new Command_CALC(ref this.variables); ci.execute(parameters); break;
                        case "COPY": ci = new Command_COPY(); if (filecheck == false) ci.execute(parameters); break;
                        case "COPY NEW": ci = new Command_COPY_NEW(false); if (filecheck == false) ci.execute(parameters); break;
                        case "COPY NEW IGNORE ERRORS": ci = new Command_COPY_NEW(true); if (filecheck == false) ci.execute(parameters); break;
                        case "CREATE DB SQL": ci = new Command_CREATE_SQL_DB(); if (filecheck == false) ci.execute(parameters); break;
                        case "CREATE ODBC SYSTEM DSN": ci = new Command_CREATE_ODBC_SYSTEM_DSN(); if (filecheck == false) ci.execute(parameters); break;
                        case "CREATE SQL DB": ci = new Command_CREATE_SQL_DB(); if (filecheck == false) ci.execute(parameters); break; //doublicate
                        case "DEF": this.variables.executeCommand_DEF(parameters); break;
                        case "DIRCOPY": ci = new Command_DIRCOPY(Command_DIRCOPY_CopyMode.FAIL_IF_SOMETHING_EXISTS); if (filecheck == false) ci.execute(parameters); break;
                        case "DIRCOPY ONLY NEW": ci = new Command_DIRCOPY(Command_DIRCOPY_CopyMode.OVERWRITE_IF_NEWER); if (filecheck == false) ci.execute(parameters); break;
                        case "DIRCOPY OVERWRITE": ci = new Command_DIRCOPY(Command_DIRCOPY_CopyMode.OVERWRITE_ALWAYS); if (filecheck == false) ci.execute(parameters); break;
                        case "DOCU": nop(); break;
                        case "ECHO": ci = new Command_ECHO(); if (filecheck == false) ci.execute(parameters); break;
                        case "ECHO PAUSED": ci = new Command_ECHO_PAUSED(); if (filecheck == false) ci.execute(parameters); break;
                        case "EXEC": ci = new Command_EXEC(); if (filecheck == false) ci.execute(parameters); break;
                        case "EXECUTE SQL": ci = new Command_EXECUTE_SQL(); if (filecheck == false) ci.execute(parameters); break;
                        case "EXIT": if (filecheck == false) throw new Comand_EXIT_Exception(); break;
                        case "FUNC": this.functions.createFunction(ref parameters); break;
                        case "LABEL": nop("label load is be done seperate"); break; //neues goto traget eintragen //TODO make doublicates save
                        case "MKDIR": ci = new Command_MKDIR(); if (filecheck == false) ci.execute(parameters); break;
                        case "NOP": nop(); break;
                        case "PARAM": nop("rest von function call der nicht sauber removed wurde"); break;
                        case "PARAMS": nop("rest von function call der nicht sauber removed wurde"); break;
                        case "PAUSE": if (filecheck == false) Console.ReadLine(); break;
                        case "REM": nop(); break;
                        case "REMOVE FILE ATTRIBUTE READ ONLY": ci = new Command_REMOVE_FILE_ATTRIBUTE_READ_ONLY(); if (filecheck == false) ci.execute(parameters); break;
                        case "REPLACE IN FILE": ci = new Command_REPLACE_IN_FILE(); if (filecheck == false) ci.execute(parameters); break;
                        case "REPLACE IN FILES": ci = new Command_REPLACE_IN_FILE(); if (filecheck == false) ci.execute(parameters); break; //doublicate
                        case "SET": this.variables.executeCommand_SET(parameters); break;
                        case "SETA": this.variables.executeCommand_DEF(parameters); break;
                        case "SKIP": this.skipping = true; ; break; //skipps all lines until SKIP END
                        case "SKIP END": this.skipping = false; ; break; //skipps all lines until SKIP END                   
                        case "TODO": nop(); break; //maybe echo magenta ?
                        case "UNSET": this.variables.executeCommand_UNSET(parameters); break;
                        case "WRITE TEXT FILE": ci = new Command_WRITE_TEXT_FILE(); if (filecheck == false) ci.execute(parameters); break;
                        default:
                            if (this.functions.callFunction(command, parameters,filecheck) == true)
                                nop("alles ok");
                            else
                                throw new Exception("unknown command")
                            ; break;
                    }
                }
                catch (Comand_EXIT_Exception exc)
                {
                    Logger.write(LogLevel.SPECIAL, exc.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    if (filecheck == false)
                        Logger.write(LogLevel.ERROR, ex.Message);
                    else
                        throw;
                }
            }

            parameters.Clear();
            return true;
        }


        /// <summary>
        /// Variablen auflösen, etc.
        /// 
        /// KEIN trim !
        /// </summary>
        /// <param name="line"></param>
        private String prepareLine(String command, String line, ref int cursor, ref int endPos)
        {
            line = line.Replace("\t", tabReplace);

            for (int i = 0; i < this.variables.variableKeys.Count; ++i)
            {
                try
                {
                    List<String> varLines = (List<String>)this.variables.variables[i];
                    if (varLines.Count == 1)
                    {
                        if (command.Trim() == "UNSET")
                            nop();
                        else
                            line = line.Replace(this.variables.variableKeys[i], varLines[0]);
                    }
                    else
                    {
                        if (command.Trim() == "DEF")
                            nop(); //do not re copy replacement in same def again (perhaps def is in loop)
                        else
                            if (this.variables.variableKeys[i] == line.Trim())
                            {
                                this.scriptLines.RemoveAt(cursor);
                                --endPos;

                                int add = 0;
                                foreach (String sline in varLines)
                                {
                                    this.scriptLines.Insert(cursor + add, sline);
                                    ++add;
                                    endPos++;
                                }
                                nop();
                            }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }//for
                  
            return line;
        }
        

        /// <summary>
        /// Parse lines 
        /// splits lines to commands and parameterds
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="filecheck"></param>
        public void executeLines(List<String> linesIn, bool filecheck)
        {
            this.scriptLines = linesIn;

            int depth = 0;                                      //welche tiefe hat die aktuelle zeile
            int lastDepth = 0;                                  //welche tiefe hatte die letzte zeile
            List<String> parameters = new List<string>();       //sammlunga ller zeilen seit dem letzten commando       
            String command="";                                  //letztes commando (die parameter kommen ja nachher)

            int endPos = this.scriptLines.Count;

            for (int cursor = 0; cursor < endPos; ++cursor)
            {
                String line = this.prepareLine(command, this.scriptLines[cursor], ref cursor, ref endPos); //also handles multiline params
                depth = DepthCalculator.countTiefe(line);    
                  
                if (depth > commandDepth)
                    parameters.Add(line);
                else 
                    if (depth == commandDepth)                
                        if (line.Trim() == "")  //can not be a new command (maybe empty line)
                            parameters.Add(line); //assume as parameter                    
                        else
                        {
                            int tempcursor = cursor;
                            if (this.executeCommand(command, ref parameters, filecheck, ref cursor) == false) 
                                break; //EXIT / ERROR
                            else
                                if (tempcursor != cursor) //cursor changed, go on with new cursor ;
                                {
                                    command = "";
                                    continue;
                                }

                            command = line; //save new one
                        }
                
                lastDepth = depth;
            }
        }

        /// <summary>
        /// Falls ein script eingerückt ist checken wir hier ab 
        /// welcher tiefe überhaupt das erste kommando zum finden ist.
        /// </summary>
        /// <param name="lines"></param>
        private void calculateCommandoDepth(List<String> lines)
        {
            for (int a = 0; a < 100; a++)
            {
                foreach (string line in lines)
                {
                    try
                    {
                        if (line.Trim() == "") 
                            continue;
                        else
                            if (line.Length > a)
                            {
                                String l = line.ToUpper();
                                if (l[a] >= 'A' && l[a] <= 'Z')
                                {
                                    this.commandDepth = a; //1st blood ^^
                                    return;
                                }
                            }
                            else
                                continue;
                    }
                    catch (Exception ex)
                    {
                        nop(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Lines in neuem worker process verarbeiten
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="filecheck"></param>
        public static void executeLinesWithNewProcess(string[] linesIn, bool filecheck)
        {

            List<String> lines = new List<string>();
            lines.AddRange(linesIn);

            CommandProcessor cp = new CommandProcessor();
            cp.calculateCommandoDepth(lines);
            cp.executeLines(lines, filecheck);   //check for script errors and preload labels with true , execute script with false
           
        }

      

        /// <summary>
        /// RUN THIS
        /// </summary>
        /// <param name="filename"></param>
        public static void runBatch(String filename)
        {
            String[] lines = File.ReadAllLines(filename);
            //int a = 0;
            executeLinesWithNewProcess(lines,true);           //error check (optional)
            executeLinesWithNewProcess(lines, false);         //real run          
        }

    }//class
}//namespace
