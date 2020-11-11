using RhythmThing.System_Stuff;
using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Utils;
using System.IO;

namespace RhythmThing.Objects.Menu
{
    class ChartInfoVisual : GameObject
    {
        private Visual infoVisual;
        private Visual scoreVisual;
        private Visual barVisual;
        private Visual letterVisual;
        private Chart chart;
        private Chart.JsonChart chartInfo;
        private ConsoleColor foreground = ConsoleColor.Black;
        private ConsoleColor background = ConsoleColor.White;

        private bool Animating = false;
        private float timePassed = 0;
        private float timeToPass = 0.3f;
        private int xSteps = 40;
        private int ySteps = 0;
        private int[] xyPoint1 = new int[] { 0, 0 };
        private int[] xyPoint2 = new int[] { 0, 0 };
        private Random random;
        private string resourcePath = Path.Combine(Program.contentPath, "MenuMusic", "MainMenu");
        public ChartInfoVisual(Chart chart)
        {
            this.chart = chart;
            this.chartInfo = chart.chartInfo;
        }
        public override void End()
        {

        }

        public override void Start(Game game)
        {
            components = new List<Component>();
            infoVisual = new Visual();
            infoVisual.active = true;
            //visual.x = 60;
            //visual.y = 45;
            infoVisual.x += 8;
            components.Add(infoVisual);

            scoreVisual = new Visual();
            scoreVisual.active = true;


            scoreVisual.x += 8;
            scoreVisual.y -= 1;
            components.Add(scoreVisual);

            barVisual = new Visual();
            barVisual.active = true;


            barVisual.x += 8;
            barVisual.y -= 1;
            barVisual.z = 1;
            barVisual.overrideColor = true;
            barVisual.overrideback = ConsoleColor.Green;
            barVisual.overridefront = ConsoleColor.Green;
            components.Add(barVisual);

            letterVisual = new Visual();
            letterVisual.active = true;


            letterVisual.x += 8;
            letterVisual.y -= 1;
            letterVisual.z = 2;
            components.Add(letterVisual);

            Draw();
            AnimateIn();
            random = new Random();

        }
        public void UpdateChart(Chart chart)
        {
            this.chart = chart;
            this.chartInfo = chart.chartInfo;
            Draw();
            AnimateIn();

        }
        private void Draw()
        {
            infoVisual.localPositions.Clear();
            scoreVisual.localPositions.Clear();
            barVisual.localPositions.Clear();
            letterVisual.localPositions.Clear();
            infoVisual.LoadBMP(Path.Combine(Program.contentPath, "MenuMusic", "MainMenu", "InfoBar.bmp"), new int[] {20,-1 });

            //visual.x = 60;
            //visual.y = 45;
            


            //visual.localPositions.Add(new Coords(0, 0, ' ', ConsoleColor.DarkBlue, ConsoleColor.DarkBlue));
            string SongName = ("Song Name: " + chartInfo.songName);
            string AuthorName = ("Song Author: " + chartInfo.songAuthor);
            string ChartAuthor = ("Chart Author: " + chartInfo.chartAuthor);
            string bpm = ("BPM: " + chartInfo.bpm.ToString());
            string diff = ("Difficulty: " + chartInfo.difficulty.ToString());
            float percent = PlayerSettings.Instance.chartScores[chart.hash].percent;
            string grade = PlayerSettings.Instance.chartScores[chart.hash].letter;

            if (percent >= 60)
            {
                barVisual.overrideback = ConsoleColor.Green;
                barVisual.overridefront = ConsoleColor.Green;
            } else
            {
                barVisual.overrideback = ConsoleColor.Red;
                barVisual.overridefront = ConsoleColor.Red;
            }
            int letx = -8;
            if(grade == "SSS")
            {
                letx = 16;
            }
            else if (grade == "SS")
            {
                letx = 4;
            }
            
            scoreVisual.LoadBMP(Path.Combine(resourcePath, "ScoreBar.bmp"));
            barVisual.LoadBMP(Path.Combine(resourcePath, "ScoreBar.bmp"), new int[] { 100 - (int)((percent / 100) * 100), 0 });
            letterVisual.LoadBMP(Path.Combine(resourcePath, $"grade{grade}.bmp"), new int[] { letx, 12 });


            letterVisual.writeText(72, 26, percent.ToString()+"%", foreground, background);
            infoVisual.writeText(40, 47, SongName, foreground, background);
            infoVisual.writeText(45, 43, AuthorName, foreground, background);
            infoVisual.writeText(50, 39, ChartAuthor, foreground, background);
            infoVisual.writeText(54, 35, bpm, foreground, background);
            infoVisual.writeText(58, 31, diff, foreground, background);

        }
        private void AnimateIn()
        {
            /*
            Animating = true;

            timePassed = 0;
            */
            //visual.ClearAnims();
            xyPoint1 = new int[] { infoVisual.x+60, infoVisual.y };
            xyPoint2 = new int[] { infoVisual.x, infoVisual.y };
            infoVisual.Animate(xyPoint1, xyPoint2, "easeOutBack", timeToPass, false);

            xyPoint1 = new int[] { scoreVisual.x + 60, scoreVisual.y };
            xyPoint2 = new int[] { scoreVisual.x, scoreVisual.y };
            scoreVisual.Animate(xyPoint1, xyPoint2, "easeOutBack", timeToPass + 0.125f, false);
            barVisual.Animate(xyPoint1, xyPoint2, "easeOutBack", timeToPass + 0.15f, false);
            letterVisual.Animate(xyPoint1, xyPoint2, "easeOutBack", timeToPass + 0.175f, false);

            //Game.mainInstance.audioManager.playForget("wheelMove.ogg");
        }
        public override void Update(double time, Game game)
        {
            if(infoVisual.x != 60)
            {
                Logger.DebugLog("a");
            }
            if (Animating)
            {
                infoVisual.x = (int)Math.Ceiling((float)xyPoint1[0] + (((float)xyPoint2[0] - (float)xyPoint1[0]) * Ease.Cubic.Out(timePassed / timeToPass)));
                infoVisual.y = (int)Math.Ceiling((float)xyPoint1[1] + (((float)xyPoint2[1] - (float)xyPoint1[1]) * Ease.Cubic.Out(timePassed / timeToPass)));

                if (timePassed >= timeToPass)
                {
                    infoVisual.x = xyPoint2[0];
                    infoVisual.y = xyPoint2[1];
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
