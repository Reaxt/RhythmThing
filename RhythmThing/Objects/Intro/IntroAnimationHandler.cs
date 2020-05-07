using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using RhythmThing.Components;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;

namespace RhythmThing.Objects.Intro
{
    public class IntroAnimationHandler : GameObject
    {
        AudioTrack introTrack;
        Visual consoleLines;
        Random random;
        private float songTime = 0;
        private float timeSince = 0;
        string line1 = "Loading rad tunes";
        private float time1 = 0;
        string line2 = "Loading fancy effects";
        private float time2 = 0.5f;
        string line3 = "Loading fair gameplay";
        private float time3 = 1;
        int y = 45;
        string line4 = "Initializing real loading lines";
        private float time4 = 1.50f;
        private float time5 = 1.75f;
        string[] restofthelines =  new string[] { "This ones real", "Totally doing stuff I promise", "This aint flair!!", "Spooling the spools", "Why did I even include this", "Loading loading messages", "Loading the loading messages for the loading messages", "Loading something actually useful" };
        private float loadingStep = 0.05f;
        private float endTime = 4f;
        public override void End()
        {
        }

        public override void Start(Game game)
        {
            random = new Random();
            //do I want a visual?
            //aaaaah fuck it
            consoleLines = new Visual();
            this.components = new List<Component>();
            introTrack = game.audioManager.addTrack("intro.mp3");

            for (int i = 0; i < line1.Length; i++)
            {
                consoleLines.localPositions.Add(new Coords(i, 49, line1[i], ConsoleColor.Green, ConsoleColor.Black));
            }
            consoleLines.active = true;

            components.Add(consoleLines);
        }

        public override void Update(double time, Game game)
        {
            songTime = (float)introTrack.sampleSource.GetPosition().TotalMilliseconds / 1000;
            if(time1 <= songTime)
            {
                for (int i = 0; i < line1.Length; i++)
                {
                    consoleLines.localPositions.Add(new Coords(i, 49, line1[i], ConsoleColor.Green, ConsoleColor.Black));
                }
            }
            if (time2 <= songTime)
            {
                for (int i = 0; i < line2.Length; i++)
                {
                    consoleLines.localPositions.Add(new Coords(i, 48, line2[i], ConsoleColor.Green, ConsoleColor.Black));
                }
            }
            if (time3 <= songTime)
            {
                for (int i = 0; i < line3.Length; i++)
                {
                    consoleLines.localPositions.Add(new Coords(i, 47, line3[i], ConsoleColor.Green, ConsoleColor.Black));
                }
            }
            if(time4 <=songTime)
            {
                for (int i = 0; i < line4.Length; i++)
                {
                    consoleLines.localPositions.Add(new Coords(i, 46, line4[i], ConsoleColor.Green, ConsoleColor.Black));
                }
            }
            if(time5 <= songTime)
            {
                if(timeSince >= loadingStep)
                {

                    int index = random.Next(0, restofthelines.Length-1);
                    for (int i = 0; i < restofthelines[index].Length; i++)
                    {
                        consoleLines.localPositions.Add(new Coords(i, y, restofthelines[index][i], ConsoleColor.Green, ConsoleColor.Black));

                    }
                    y--;
                    timeSince = 0;
                }
                timeSince = timeSince + (float)time;
            }
            if(endTime <= songTime)
            {
                consoleLines.active = false;
            }
            if(introTrack.sampleSource.GetLength().TotalMilliseconds <= introTrack.sampleSource.GetPosition().TotalMilliseconds)
            {
                game.sceneManager.loadScene(0);
            }
        }
    }
}
