using CSCore;
using CSCore.Codecs;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using RhythmThing.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RhythmThing.Objects.ScoreScreen
{

    
    public class ScoreScreenHandler : GameObject
    {
        /*
         * <60 = F
         * 60-66 = D
         * 67-72 = C
         * 73-79 = B
         * 80-86 = A
         * 87-90 = S
         * 90-99 = SS
         * 100 = SSS
         */
        private Visual songTitle;
        private Visual gradeBar;
        private Visual gradeletter;
        private Visual percentBar;
        private Visual gaugeOutline;
        private Visual gauge;
        private Visual lastBest;
        private Visual yousuck;
        private Visual[] numVisuals;
        private float percent;
        private float timePassed = 0;
        private float _lastScore;
        private string _assetPath = Path.Combine(Program.contentPath, "MenuMusic", "ScoreScreen");
        private string grade;
        private float slideInTime = 0.5f;
        private float time1 = 0f;
        private float time2 = 0.25f/2;
        private float time3 = 0.5f/2;
        private float time4 = 0.75f/2;
        private float startDoing = 1.25f;
        private float timeForGauge = 5f;
        private int lastTick = 0;
        private int numOff = -9;
        private int lastPercent = 0;
        private float savePercent = 0;
        private string lastGrade = "F";
        private bool gaugeFinished = false;
        private AudioTrack bgmMusic;
        private string bgmName = "event.wav";
        private ISampleSource scoreDing;
        private bool over60 = false;
        AudioTrack cheer;
        public override void End()
        {
            Game.mainInstance.audioManager.removeTrack(bgmMusic);
            if(cheer != null)
            {
                Game.mainInstance.audioManager.removeTrack(cheer);
            }
        }

        private static string getGrade(float percent)
        {
            string grade = "NA";
            if (percent == 100)
            {
                grade = "SSS";


            }
            else if (percent >= 90)
            {
                grade = "SS";


            }
            else if (percent >= 87)
            {
                grade = "S";

            }
            else if (percent >= 80)
            {
                grade = "A";


            }
            else if (percent >= 73)
            {
                grade = "B";


            }
            else if (percent >= 67)
            {
                grade = "C";


            }
            else if (percent >= 60)
            {
                grade = "D";

            }
            else
            {
                grade = "F";

            }
            if (grade == "NA")
            {
                throw new Exception("NO VALID SCORE");

            }
            return grade;
        }
        public override void Start(Game game)
        {
            //a fix to a bug
            game.display.windowManager.CenterWindow();

            components = new List<Component>();
            savePercent = (((float)game.notesHit / (float)game.totalNotes)*100);
            percent = (float)Math.Floor(savePercent);
            grade = "NA";
            grade = getGrade(savePercent);


            
            _lastScore = PlayerSettings.Instance.chartScores[game.songHash].percent;
            if (savePercent > PlayerSettings.Instance.chartScores[game.songHash].percent)
            {
                PlayerSettings.Instance.SaveScore(game.songHash, grade, percent);

            }


            //visual stuff!
            
            //songTitle
            songTitle = new Visual();
            songTitle.LoadBMP(Path.Combine(_assetPath, "songTitle.bmp"));
            songTitle.active = true;
            songTitle.writeText(50-(game.songName.Length/2), 47, game.songName, ConsoleColor.Black, ConsoleColor.White);
            components.Add(songTitle);
            //grade bar
            gradeBar = new Visual();
            gradeBar.LoadBMP(Path.Combine(_assetPath, "gradeBar.bmp"));
            components.Add(gradeBar);
            //grade letter (may merge)
            gradeletter = new Visual();
            gradeletter.z = 1;
            gradeletter.LoadBMP(Path.Combine(_assetPath, $"gradeF.bmp"));
            components.Add(gradeletter);
            //percent bar
            percentBar = new Visual();
            percentBar.LoadBMP(Path.Combine(_assetPath, "percentBar.bmp"));
            components.Add(percentBar);
            //guage (god help me)
            gaugeOutline = new Visual();
            gaugeOutline.z = 1;
            gaugeOutline.LoadBMP(Path.Combine(_assetPath, "gaugeOutline.bmp"));
            components.Add(gaugeOutline);
            
            gauge = new Visual();
            gauge.active = false;
            gauge.LoadBMP(Path.Combine(_assetPath, "gauge.bmp"));
            components.Add(gauge);
            gauge.overrideColor = true;
            gauge.overridefront = ConsoleColor.Red;
            //numbers
            numVisuals = new Visual[3];
            for (int i = 0; i < numVisuals.Length; i++)
            {
                numVisuals[i] = new Visual();
                numVisuals[i].z = 1;
                numVisuals[i].x = numOff * i;
                numVisuals[i].active = true;//fornow
                numVisuals[i].LoadBMP(Path.Combine(_assetPath, "0.bmp"));
                components.Add(numVisuals[i]);
            }
            //last best
            lastBest = new Visual();
            lastBest.active = false;
            lastBest.z = -1;
            lastBest.LoadBMP(Path.Combine(_assetPath, "gauge.bmp"), new int[] {-100+(int)Math.Ceiling(_lastScore), 0 });
            components.Add(lastBest);
            lastBest.overrideColor = true;
            lastBest.overridefront = ConsoleColor.Gray;
            //easter egg for doing really badly
            yousuck = new Visual();
            yousuck.active = false;
            yousuck.z = 10;
            yousuck.LoadBMP(Path.Combine(_assetPath, "yousuck.bmp"));
            components.Add(yousuck);
            scoreDing = CodecFactory.Instance.GetCodec(Path.Combine(_assetPath, "exp.wav")).ChangeSampleRate(AudioManager.sampleRate).ToStereo().ToSampleSource();
            bgmMusic = game.audioManager.addTrack(Path.Combine(_assetPath, bgmName));

        }



        public override void Update(double time, Game game)
        {
            #region Animations
            timePassed = timePassed + (float)time;
            if(timePassed >= time1)
            {
                songTitle.active = true;
                time1 = float.MaxValue;
                songTitle.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", slideInTime);

            }
            if (timePassed >= time2)
            {
                gaugeOutline.active = true;
                lastBest.active = true;
                time2 = float.MaxValue;
                gaugeOutline.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", slideInTime);
                lastBest.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", slideInTime);
            }
            if (timePassed >= time3)
            {
                percentBar.active = true;
                time3 = float.MaxValue;
                percentBar.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", slideInTime);
                for (int i = 0; i < numVisuals.Length; i++)
                {
                    numVisuals[i].Animate(new int[] { -100, 0 }, new int[] { numVisuals[i].x, 0 }, "easeOutExpo", slideInTime + (float)(0.5 * i));
                }
                
            }
            if(timePassed >= time4)
            {
                gradeBar.active = true;
                time4 = float.MaxValue;
                gradeBar.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", slideInTime);
                gradeletter.active = true;
                gradeletter.Animate(new int[] { -100, 0 }, new int[] { 0, 0 }, "easeOutExpo", slideInTime+0.5f);

            }
            if(timePassed >= startDoing&& !gaugeFinished)
            {

                int percentVal = (int)Math.Ceiling(percent * Ease.Sinusoidal.InOut(((timePassed - startDoing) / timeForGauge)));
                if ((timePassed - startDoing) >= timeForGauge || percent == 0)
                {
                    gaugeFinished = true;
                    percentVal = (int)percent;
                    if (percent == 0 && !game.exitViaEsc)
                    {
                        yousuck.active = true;
                        yousuck.Animate(new int[] { 0, 100 }, new int[] { 0, 0 }, "easeOutBounce", 1f);
                        cheer = game.audioManager.addTrack(Path.Combine(_assetPath, "failsound.wav"));
                        cheer.volumeSource.Volume = 1.0f;
                    }
                }
                if(percentVal > lastPercent)
                {
                    scoreDing.Position = 0;
                    game.audioManager.playForget(scoreDing);
                    for (int i = 0; i < numVisuals.Length; i++)
                    {
                        numVisuals[i].localPositions.Clear();
                    }
                    int third = (percentVal == 100) ? 1 : 0;
                    int second = (percentVal / 10) % 10;
                    int first = (percentVal) % 10;
                    numVisuals[0].LoadBMP(Path.Combine(_assetPath, $"{first}.bmp"));
                    numVisuals[1].LoadBMP(Path.Combine(_assetPath, $"{second}.bmp"));
                    numVisuals[2].LoadBMP(Path.Combine(_assetPath, $"{third}.bmp"));
                    gauge.active = true;
                    gauge.x = (-100) + percentVal;

                    if (percentVal > 60 && !over60)
                    {
                        //ya PASSED bud
                        gauge.overridefront = ConsoleColor.Green;
                        game.audioManager.playForget(Path.Combine(_assetPath, "res.wav"));
                        over60 = true;

                        //idk play a cheer
                    }
                    if (getGrade(percentVal) != lastGrade)
                    {
                        lastTick++;
                        lastGrade = getGrade(percentVal);
                        int amount = 40;
                        int sExtra = 0;
                        if(lastGrade != "D")
                        {
                            game.audioManager.playForget(Path.Combine(_assetPath,"touch.wav"));

                        }
                        if(lastGrade == "SSS")
                        {
                            sExtra = 20;
                            cheer = game.audioManager.addTrack(Path.Combine(_assetPath, "cheer.wav"));
                        }

                        gradeletter.LoadBMP(Path.Combine(_assetPath, $"grade{lastGrade}.bmp"), new int[] { (lastTick * amount)+sExtra, 0 });
                        gradeletter.Animate(new int[] { gradeletter.x, gradeletter.y }, new int[] { gradeletter.x - ( amount+sExtra), gradeletter.y }, "easeOutQuad", 0.155f, true);
                    }
                    lastPercent = percentVal;
                }


            }
            #endregion

            #region BGM
            if (bgmMusic.sampleSource.GetPosition().TotalMilliseconds >= bgmMusic.sampleSource.GetLength().TotalMilliseconds - 50)
            {
                bgmMusic.sampleSource.Position = 0;
            }

            #endregion

            if (game.input.ButtonStates[Input.ButtonKind.Confirm] == Input.ButtonState.Press || game.input.ButtonStates[Input.ButtonKind.Cancel] == Input.ButtonState.Press)
            {
                if (!gaugeFinished)
                {
                    timePassed = 2000f;
                } else
                {
                    game.sceneManager.loadScene(0);

                }
            }
        }
    }
}
