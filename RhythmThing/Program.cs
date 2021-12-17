using System;
using RhythmThing.System_Stuff;
using RhythmThing.Objects;
using System.Text;
using System.Globalization;
using System.Threading;
using RhythmThing.Utils;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace RhythmThing
{

    /*
     * the big ol TODO list
     * ENGINE:
     * Main loop DONE
     * obj and component system DONE
     * Visual class DONE, might need some work
     * input test DONE
     * audio controller DONE
     * no scene element this time I dont think, maybe, dunno. BLECH (just kidding. DONE )
     * 
     * GAME:
     * make the rest of the fucking game X
     *
     */
    class Program
    {
        //STATICS
        public static int ScreenX = 100;
        public static int ScreenY = 50;
        public static string ContentPath;
        public static MD5 mD5 = MD5.Create();
        public static bool hotload = false;
        public static string hotloadPath;

        //this feels wrong but it works!
        [STAThread]
         
        static void Main(string[] args)
        {
            CultureInfo nonInvariantCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = nonInvariantCulture;
            if (args.Length == 3)
            {
                SlaveWindow slave = new SlaveWindow(args);
               
            } else
            {
            
            ContentPath = Path.Combine(PlayerSettings.GetExeDir(), "!Content");
            //backup last log
            Logger.NewLog();

            Logger.DebugLog("we're starting!");
            //needed for some locale I guess


            //Console.ReadLine();
            //ConsoleHelper.SetConsoleFont();

            //Line needed for some pcs
            Console.WriteLine("proof");
            Console.OutputEncoding = Encoding.Unicode;
            //load player settings
            PlayerSettings.Instance.ReadSettings();
            if(args.Length == 1)
            {
                    hotload = true;
                    hotloadPath = args[0];
            }
            Game main = new Game(ScreenX, ScreenY);
                //testOUTPUT test = new testOUTPUT();
                // test.Setup();
                //put in test object
                //  main.addGameObject(new LeftArrow());
            }
        }
    }
}
