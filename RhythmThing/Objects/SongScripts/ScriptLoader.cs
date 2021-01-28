using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.Objects.SongScripts
{
    public class ScriptLoader : GameObject
    {

        private SongScript script;
        private Chart chart;
        private bool songStarted = false;
        public ScriptLoader(SongScript script, Chart chart)
        {
            this.script = script;
            this.chart = chart;

        }
        public override void End()
        {
            
        }
        public void songEnd()
        {
            script.EndScript(chart, Game.MainInstance);
        }
        public void songStart()
        {
            script.RunScript(chart, Game.MainInstance);
            songStarted = true;

        }
        public override void Start(Game game)
        {

        }

        public override void Update(double time, Game game)
        {
            if (songStarted)
            {
                script.MainScript(chart, game, time);
            }
        }
    }
}
