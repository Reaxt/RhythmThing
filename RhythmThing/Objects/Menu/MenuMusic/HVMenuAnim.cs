using CSCore;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RhythmThing.Objects.Menu.MenuMusic
{

    public class HVMenuAnim : SongLoadAnim
    {
        private float timePoint1 = 0.2f;
        private float timePoint2 = 0.5f;
        private float timePoint3 = 0.9f;
        private float timePoint4 = 0.9f;
        private float timePoint5 = 2.25f;
        private string audioPath = Path.Combine(Program.ContentPath, "MenuMusic", "HVMusic", "Select.wav");
        private AudioTrack audio;
        private string resourcePath = Path.Combine(Program.ContentPath, "MenuMusic", "HVMusic");
        private Visual leftVis1;
        private Visual leftVis2;
        private Visual bestVisTop;
        private Visual bestVisBottom;
        private Visual bestVisLetter;
        private Visual bestVisPercent;
        private Visual receptors;
        private Visual[] visuals; 
        private Chart chart;
        private float percent;
        public override void End()
        {

        }
        public HVMenuAnim()
        {
            chart = new Chart(Game.MainInstance.ChartToLoad);
        }
        public override void Start(Game game)
        {
            if (Program.hotload)
            {
                PlayerSettings.Instance.HotloadSum(chart.hash);
            }
            string grade = PlayerSettings.Instance.chartScores[chart.hash].letter;
            percent = PlayerSettings.Instance.chartScores[chart.hash].percent;
            GameObjectType = objType.visual;
            leftVis1 = new Visual();
            leftVis1.z = -1;
            leftVis1.overrideColor = true;
            leftVis1.overrideback = ConsoleColor.Red;
            leftVis1.overridefront = ConsoleColor.Red;
            leftVis1.LoadBMP(Path.Combine(resourcePath, "SideBG.bmp"), new int[] { -28, -15 });
            leftVis2 = new Visual();
            leftVis2.z = 1;
            leftVis2.LoadBMP(Path.Combine(resourcePath, $"Side1{chart.chartInfo.difficulty}.bmp"), new int[] { 0, 0 });
            leftVis2.Active = true;
            leftVis1.Active = false;
            leftVis2.Animate(new int[] { -70, -70 }, new int[] { 0, 0 }, "easeOutExpo", 0.5f, true);

            bestVisTop = new Visual();
            bestVisTop.z = 2;
            bestVisTop.Active = false;
            bestVisTop.LoadBMP(Path.Combine(resourcePath, "BEST.bmp"), new int[] { 0, 0 });

            bestVisBottom = new Visual();
            bestVisBottom.z = 2;
            bestVisBottom.Active = false;
            bestVisBottom.LoadBMP(Path.Combine(resourcePath, "BESTBOTTOM2.bmp"), new int[] { -20, -3 });

            bestVisLetter = new Visual();
            bestVisLetter.z = 5;
            bestVisLetter.Active = false;
            bestVisLetter.LoadBMP(Path.Combine(resourcePath, $"BEST{grade}.bmp"), new int[] { 0, -3 });

            bestVisPercent = new Visual();
            bestVisPercent.z = 4;
            bestVisPercent.Active = false;
            bestVisPercent.LoadBMP(Path.Combine(resourcePath, "BESTBOTTOM2.bmp"), new int[] { -20, -3 });
            bestVisPercent.overrideColor = true;
            if (percent > 60)
            {
                bestVisPercent.overridefront = ConsoleColor.Green;
                bestVisPercent.overrideback = ConsoleColor.Green;

            }
            else
            {
                bestVisPercent.overridefront = ConsoleColor.Red;
                bestVisPercent.overrideback = ConsoleColor.Red;

            }

            receptors = new Visual();
            receptors.z = 0;
            receptors.x = 23;
            receptors.y = 36;
            receptors.Active = false;
            receptors.LoadBMP(Path.Combine(Program.ContentPath, "Sprites", "LeftReceiver.bmp"), new int[] { 0,0});
            receptors.LoadBMP(Path.Combine(Program.ContentPath, "Sprites", "DownReceiver.bmp"), new int[] { 14, 0 });
            receptors.LoadBMP(Path.Combine(Program.ContentPath, "Sprites", "UpReceiver.bmp"), new int[] { 28, 1 });
            receptors.LoadBMP(Path.Combine(Program.ContentPath, "Sprites", "RightReceiver.bmp"), new int[] { 43, 0 });

            visuals = new Visual[] { leftVis1, leftVis2, bestVisTop, bestVisBottom, bestVisLetter, bestVisPercent };
            Components.Add(leftVis1);
            Components.Add(leftVis2);
            Components.Add(bestVisTop);
            Components.Add(bestVisBottom);
            Components.Add(bestVisLetter);
            Components.Add(bestVisPercent);
            Components.Add(receptors);
            audio = game.AudioManagerInstance.addTrack(audioPath);

        }

        public override void Update(double time, Game game)
        {
            double currentPoint = audio.sampleSource.GetPosition().TotalSeconds;
            if (audio.sampleSource.Length <= audio.sampleSource.Position)
            {
                game.SceneManagerInstance.LoadScene(1);
            }

            //timepoints
            if (currentPoint >= timePoint1)
            {
                leftVis1.Active = true;
                leftVis1.Animate(new int[] { 70, 70 }, new int[] { 0, 0 }, "easeOutExpo", 0.75f, true);

                //leftVis1.active = false;
                //leftVis2.active = false;
                timePoint1 = 999;
            } 
            if (currentPoint >= timePoint2)
            {
                bestVisTop.Active = true;
                bestVisTop.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", 0.5f, true);
                timePoint2 = 999;
            }
            if(currentPoint >= timePoint3)
            {
                bestVisLetter.Active = true;
                bestVisLetter.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", 0.7f, true);

                bestVisBottom.Active = true;
                bestVisBottom.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", 0.5f, true);
                timePoint3 = 999;
            }
            if(currentPoint >= timePoint4)
            {
                bestVisPercent.Active = true;
                float percentPos = (float)Math.Ceiling(-150 + (150 * (percent / 100)));
                bestVisPercent.Animate(new int[] { -100, 0 }, new int[] { (int)percentPos, 0 }, "easeOutBack", 1f, true);
                timePoint4 = 999;

            }
            if(currentPoint >= timePoint5)
            {
                timePoint5 = 999;
                float percentPos = (float)Math.Ceiling(-150 + (150 * (percent / 100)));
                receptors.Active = true;
                leftVis1.Animate(new int[] { 0, 0 }, new int[] { 70, 70 }, "easeInExpo", 1f, true);
                bestVisTop.Animate(new int[] { -0, 0 }, new int[] { -100, 0 }, "easeInExpo", 1f, true);
                bestVisPercent.Animate(new int[] { (int)percentPos, 0 }, new int[] { -100, 0 }, "easeInExpo", 1f, true);
                bestVisBottom.Animate(new int[] { -0, 0 }, new int[] { -100, 0 }, "easeInExpo", 1f, true);
                bestVisLetter.Animate(new int[] { -0, 0 }, new int[] { -100, 0 }, "easeInExpo", 1f, true);
                bestVisTop.Animate(new int[] { -0, 0 }, new int[] { -100, 0 }, "easeInExpo", 1f, true);
                leftVis1.Animate(new int[] { 0, 0 }, new int[] { 70, 70 }, "easeInExpo", 1f, true);
                leftVis2.Animate(new int[] { 0, 0 }, new int[] { -70, -70 }, "easeInExpo", 1f, true);

            }

        }
    }
}
