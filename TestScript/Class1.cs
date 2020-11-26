using System;
using RhythmThing.Objects;
using RhythmThing.System_Stuff;
namespace TestScript
{
    public class TestScript : SongScript
    {
        private int rot = 0;
        private float timepassed = 0;
        private float timetopass = 0.5f;
        private bool togthing = false;
        public string Name => "neatoo";

        public string Description => "this sure is a cool testing thing";

        public void endScript(Chart chart, Game game)
        {

        }


        public void mainScript(Chart chart, Game game, double time)
        {
            timepassed += (float)time;
            if (timepassed > timetopass)
            {
                timepassed = 0;
                rot++;
                for (int i = 0; i < chart.receivers.Length; i++)
                {
                    if (togthing)
                    {
                        chart.receivers[i].visual.Animate(new int[] { chart.receivers[i].visual.x, chart.receivers[i].visual.y }, new int[] { chart.receivers[i].visual.x + 5, chart.receivers[i].visual.y }, "easeOutExpo", timetopass, true);
                    } else
                    {
                        chart.receivers[i].visual.Animate(new int[] { chart.receivers[i].visual.x, chart.receivers[i].visual.y }, new int[] { chart.receivers[i].visual.x - 5, chart.receivers[i].visual.y }, "easeOutExpo", timetopass, true);

                    }
                    chart.receivers[i].visual.rotation = rot;
                    chart.receivers[i].visual.useMatrix = true;
                    chart.receivers[i].pressedVisual.rotation = rot;
                    chart.receivers[i].pressedVisual.useMatrix = true;
                    for (int x = 0; x < chart.receivers[i].arrows.Count; x++)
                    {
                        chart.receivers[i].arrows[x].visual.writeText(0, 0, "HEHE :D", ConsoleColor.Black, chart.receivers[i].arrows[x].noteColor);
                        if (togthing)
                        {
                            chart.receivers[i].arrows[x].visual.overrideColor = true;
                            chart.receivers[i].arrows[x].visual.overrideback = ConsoleColor.White;
                            chart.receivers[i].arrows[x].visual.overridefront = ConsoleColor.White;

                        }
                        else
                        {
                            chart.receivers[i].arrows[x].visual.overrideColor = false;

                        }
                    }
                    
                }
                togthing = !togthing;
            }
        }


        public void runScript(Chart chart, Game game)
        {
            
        }
    }
}
