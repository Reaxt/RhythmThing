using RhythmThing.System_Stuff;
using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Utils;

namespace RhythmThing.Objects.Menu
{
    class ChartInfoVisual : GameObject
    {
        private Visual visual;
        private Chart.JsonChart chartInfo;
        private ConsoleColor foreground = ConsoleColor.Black;
        private ConsoleColor background = ConsoleColor.White;

        private bool Animating = false;
        private float timePassed = 0;
        private float timeToPass = 0.125f;
        private int xSteps = 40;
        private int ySteps = 0;
        private int[] xyPoint1 = new int[] { 0, 0 };
        private int[] xyPoint2 = new int[] { 0, 0 };
        private Random random;

        public ChartInfoVisual(Chart.JsonChart chartInfo)
        {
            this.chartInfo = chartInfo;
        }
        public override void End()
        {

        }

        public override void Start(Game game)
        {
            components = new List<Component>();
            visual = new Visual();
            visual.active = true;
            visual.x = 60;
            visual.y = 45;
            components.Add(visual);
            Draw();
            random = new Random();

        }
        public void UpdateChart(Chart.JsonChart jsonChart)
        {
            this.chartInfo = jsonChart;
            Draw();
            AnimateIn();

        }
        private void Draw()
        {
            visual.localPositions.Clear();

            //visual.x = 60;
            //visual.y = 45;

            for (int x = 0; x < 30; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    visual.localPositions.Add(new Coords(x, -y, ' ', foreground, background));
                }
            }

            visual.localPositions.Add(new Coords(0, 0, ' ', ConsoleColor.DarkBlue, ConsoleColor.DarkBlue));
            char[] SongName = ("Song Name: " + chartInfo.songName).ToCharArray();
            char[] AuthorName = ("Song Author: " + chartInfo.songAuthor).ToCharArray();
            char[] ChartAuthor = ("Chart Author: " + chartInfo.chartAuthor).ToCharArray();
            char[] bpm = ("BPM: " + chartInfo.bpm.ToString()).ToCharArray();
            char[] diff = ("Difficulty: " + chartInfo.difficulty.ToString()).ToCharArray();

            for (int i = 0; i < SongName.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, 0, SongName[i], foreground, background));
            }
            for (int i = 0; i < AuthorName.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, -3, AuthorName[i], foreground, background));
            }
            for (int i = 0; i < ChartAuthor.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, -6, ChartAuthor[i], foreground, background));
            }
            for (int i = 0; i < bpm.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, -9, bpm[i], foreground, background));
            }
            for (int i = 0; i < diff.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, -12, diff[i], foreground, background));
            }
        }
        private void AnimateIn()
        {
            /*
            Animating = true;

            timePassed = 0;
            */
            //visual.ClearAnims();
            xyPoint1 = new int[] { visual.x+40, visual.y };
            xyPoint2 = new int[] { visual.x, visual.y };
            visual.Animate(xyPoint1, xyPoint2, "easeOutCubic", 0.125f, false);

            //Game.mainInstance.audioManager.playForget("wheelMove.ogg");
        }
        public override void Update(double time, Game game)
        {
            if(visual.x != 60)
            {
                Logger.DebugLog("a");
            }
            if (Animating)
            {
                visual.x = (int)Math.Ceiling((float)xyPoint1[0] + (((float)xyPoint2[0] - (float)xyPoint1[0]) * Ease.Cubic.Out(timePassed / timeToPass)));
                visual.y = (int)Math.Ceiling((float)xyPoint1[1] + (((float)xyPoint2[1] - (float)xyPoint1[1]) * Ease.Cubic.Out(timePassed / timeToPass)));

                if (timePassed >= timeToPass)
                {
                    visual.x = xyPoint2[0];
                    visual.y = xyPoint2[1];
                    timePassed = 0;
                    Animating = false;
                }
                else
                {
                    timePassed += (float)time;
                }
            }
        }
    }
}
