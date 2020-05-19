using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.System_Stuff;
using RhythmThing.Components;

namespace RhythmThing.Objects.Menu
{
    public class SongContainer : GameObject
    {
        public string chartName;
        public Visual visual;
        public Chart chart;
        private ConsoleColor backColor;
        private ConsoleColor frontColor;
        public int pos;
        public SongContainer(string chartName, int pos)
        {
            this.chartName = chartName;
            chart = new Chart(chartName);
            this.pos = pos;
            this.type = objType.visual;

            //sorry for moving this here "('w')
            switch (chart.chartInfo.difficulty)
            {
                case 0:
                    frontColor = ConsoleColor.Black;
                    backColor = ConsoleColor.Green;
                    break;
                case 1:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.DarkGreen;
                    break;
                case 2:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.Blue;
                    break;
                case 3:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.DarkBlue;
                    break;
                case 4:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.DarkRed;
                    break;
                case 5:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }
            components = new List<Component>();
            visual = new Visual();
            visual.x = 5;
            visual.y = 45;
            visual.active = false;

            visual.y = (visual.y + (pos * -5));

            chart = new Chart(chartName);
            char[] chartChar;
            chartChar = chart.chartInfo.songName.ToCharArray();

            for (int i = -1; i < 30; i++)
            {
                visual.localPositions.Add(new Coords(i, 1, ' ', frontColor, backColor));
                visual.localPositions.Add(new Coords(i, 0, ' ', frontColor, backColor));
                visual.localPositions.Add(new Coords(i, -1, ' ', frontColor, backColor));
            }
            for (int i = 0; i < chartChar.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, 0, chartChar[i], frontColor, backColor));
            }
            visual.localPositions.Add(new Coords(-1, 0, ' ', frontColor, backColor));
        }
        public override void End()
        {
            //throw new NotImplementedException();
        }

        public override void Start(Game game)
        {


            components.Add(visual);
        }

        public override void Update(double time, Game game)
        {
            //throw new NotImplementedException();
        }
    }
}
