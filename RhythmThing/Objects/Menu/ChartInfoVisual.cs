using RhythmThing.System_Stuff;
using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Objects.Menu
{
    class ChartInfoVisual : GameObject
    {
        private Visual visual;
        private Chart.JsonChart chartInfo;
        private ConsoleColor foreground = ConsoleColor.Black;
        private ConsoleColor background = ConsoleColor.White;
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

        }
        public void UpdateChart(Chart.JsonChart jsonChart)
        {
            this.chartInfo = jsonChart;
            Draw();
        }
        private void Draw()
        {
            visual.localPositions.Clear();

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
        public override void Update(double time, Game game)
        {

        }
    }
}
