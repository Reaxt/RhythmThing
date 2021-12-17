using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Utils
{
    public static class Logger
    {
        private static string logFile = "loge.txt";
        private static string logPath = Path.Combine(PlayerSettings.GetExeDir(), "!Content", logFile);
        public static void DebugLog(string log)
        {
#if DEBUG
            //broken..
            //File.AppendAllText(logPath, (log + "\n"));
#endif
        }
        public static void NewLog()
        {
#if DEBUG
            if(File.Exists(logPath+".last"))
            {
                File.Delete(logPath + ".last");
            }
            if(File.Exists(logPath))
            {
                File.Move(logPath, logPath + ".last");
            }
#endif
        }
    }
}
