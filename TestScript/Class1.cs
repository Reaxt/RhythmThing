using System;
using RhythmThing.Objects;
using RhythmThing.System_Stuff;
using TestScript.Shaders;
using RhythmThing.Components;

namespace TestScript
{
    public class TestScript : SongScript
    {
        private int rot = 0;
        private float timepassed = 0;
        private float timetopass = 0.5f;
        public SlaveManager[] colWindows;
        public static int colX = 15;
        public static int colY = 50;
        private bool[] activates = new bool[2];
        public string Name => "neatoo";

        public string Description => "this sure is a cool testing thing";

        public void endScript(Chart chart, Game game)
        {
            Input.focusInput = true;
        }


        public void mainScript(Chart chart, Game game, double time)
        {
            if (chart.beat >= 60 && !activates[0])
            {
                activates[0] = true;
                game.addGameObject(new Visual_Gameobject_stuff.FirstNotes(chart));
            }
            if(chart.beat >= 132.25 && !activates[1])
            {
                activates[1] = true;
                game.addGameObject(new Visual_Gameobject_stuff.CollumnWindows(colWindows, chart));

            }

        }


        public void runScript(Chart chart, Game game)
        {
            Input.focusInput = false;
            colWindows = new SlaveManager[4];

            
            for (int i = 0; i < 4; i++)
            {
                int startX = 600;
                colWindows[i] = new SlaveManager(i.ToString(), colX, colY);
                colWindows[i].MoveWindow(startX + (i * (colX*(-23))), -500);
                
                //colWindows[i].MoveWindow(1000, 50);
            }
            colWindows[0].SetWindowTitle("<-");
            colWindows[1].SetWindowTitle("/\\");
            colWindows[2].SetWindowTitle("\\/");
            colWindows[3].SetWindowTitle("->");

            for (int i = 0; i < 4; i++)
            {
                chart.chartEventHandler.moveCollumn(i, 0, -60);
            }
            game.addGameObject(new Visual_Gameobject_stuff.FirstArrows(chart));
            //test shit
        }
    }
}
