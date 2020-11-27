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
            
        }


        public void runScript(Chart chart, Game game)
        {
            for (int i = 0; i < 4; i++)
            {
                chart.chartEventHandler.moveCollumn(i, 0, -60);
                game.addGameObject(new Visual_Gameobject_stuff.FirstArrows(chart));
            }
        }
    }
}
