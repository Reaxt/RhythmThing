using System;
using RhythmThing.System_Stuff;
using RhythmThing.Objects;
using System.Text;
using System.Globalization;
using System.Threading;
using RhythmThing.Utils;
using System.IO;
using System.Security.Cryptography;

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
        public static string contentPath;
        public static MD5 mD5 = MD5.Create();
        //this feels wrong but it works!
        [STAThread]
         
        static void Main(string[] args)
        {
            contentPath = Path.Combine(Directory.GetCurrentDirectory(), "!Content");
            //backup last log
            Logger.NewLog();

            Logger.DebugLog("we're starting!");
            //needed for some locale I guess
            CultureInfo nonInvariantCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = nonInvariantCulture;

            //Console.ReadLine();
            //ConsoleHelper.SetConsoleFont();

            //Line needed for some pcs
            Console.WriteLine("proof");
            Console.OutputEncoding = Encoding.Unicode;
            //load player settings
            PlayerSettings.Instance.ReadSettings();

            Game main = new Game(ScreenX, ScreenY);
            //testOUTPUT test = new testOUTPUT();
           // test.Setup();
            //put in test object
          //  main.addGameObject(new LeftArrow());
        }
    }
}
