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
        private int _animOff = 5;
        private string _animEase = "easeOutExpo";
        private float _animDuration = 0.05f;
        private string _scrollAnimEase = "easeLinear";
        private float _scrollAnimDur = 0.1f;
        private static ConsoleColor _normalBack = ConsoleColor.Gray;
        private static ConsoleColor _normalFront = ConsoleColor.Black;
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
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.Gray;
                    break;
                case 1:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.Blue;
                    break;
                case 2:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.Green;
                    break;
                case 3:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.Yellow;
                    break;
                case 4:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.Red;
                    break;
                case 5:
                    frontColor = ConsoleColor.White;
                    backColor = ConsoleColor.Magenta;
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
                visual.localPositions.Add(new Coords(i, 1, ' ', _normalFront, _normalBack));
                visual.localPositions.Add(new Coords(i, 0, ' ', _normalFront, _normalBack));
                visual.localPositions.Add(new Coords(i, -1, ' ', _normalFront, _normalBack));
            }
            for (int i = 0; i < chartChar.Length; i++)
            {
                visual.localPositions.Add(new Coords(i+1, 0, chartChar[i], _normalFront, _normalBack));
            }
            visual.localPositions.Add(new Coords(-1, 1, ' ', _normalFront, backColor));
            visual.localPositions.Add(new Coords(-1, 0, ' ', _normalFront, backColor));
            visual.localPositions.Add(new Coords(-1, -1, ' ', _normalFront, backColor));
        }
        public override void End()
        {
            //throw new NotImplementedException();
        }

        public void OutAnim()
        {
            int[] point1 = new int[] { visual.x, visual.y };
            int[] point2 = new int[] { visual.x+5, visual.y };
            visual.Animate(point1, point2, _animEase, _animDuration);
        }
        public void InAnim()
        {
            int[] point1 = new int[] { visual.x, visual.y };
            int[] point2 = new int[] { visual.x - 5, visual.y };
            visual.Animate(point1, point2, _animEase, _animDuration);
        }
        public void AnimTo(int y)
        {
            int[] point1 = new int[] { visual.x, visual.y };
            int[] point2 = new int[] { visual.x, y };
            visual.Animate(point1, point2, _scrollAnimEase, _scrollAnimDur);
        }
        public override void Start(Game game)
        {


            components.Add(visual);
            int[] point1 = { visual.x-(100+(10*pos)), visual.y };
            int[] point2 = { visual.x, visual.y };
            visual.Animate(point1, point2, "easeOutExpo", 0.25f+(0.0125f*pos));
        }

        public override void Update(double time, Game game)
        {
            //throw new NotImplementedException();
        }
    }
}
