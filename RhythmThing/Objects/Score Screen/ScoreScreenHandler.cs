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
        private Visual visual;
        private Visual letterGrade;
        private Visual backDrop;
        private Visual clearGuage;
        private Visual gradeCover;
        private float percent;
        private float guagePercent = -1;
        private float timePassed = 0;
        private float guageTick = 0.04f;
        private float coverWait = 0.05f;
        private float coverTick = 0.025f;
        private bool coverSlide = false;
        private Random random;
        private bool _highScore = false;
        private float _lastScore = 0;

        string grade;
        public override void End()
        {

        }

        public override void Start(Game game)
        {
            //Save the score.

            components = new List<Component>();
            percent = (float)game.notesHit / (float)game.totalNotes;
            //just write it out for now
            visual = new Visual();
            letterGrade = new Visual();
            backDrop = new Visual();
            clearGuage = new Visual();
            gradeCover = new Visual();
            gradeCover.z = 2;
            clearGuage.y = 25;
            letterGrade.z = 1;
            letterGrade.x = 60;
            visual.y = 49;
            random = new Random();
            percent = (float)Math.Floor(percent * 100);
            char[] songName = ("Song: " + (game.songName)).ToCharArray();
            char[] percentText = ("Percent: " + percent + "%").ToCharArray();
            string grade = "NA";


            //This esentially draws the letter sprites. Its a bit of a mess, but it works fine.
            //TODO: now that I can do BMP stuff, make these into cframes!
            if (percent == 100)
            {
                grade = "SSS";
                //first s
                letterGrade.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "SSS.bmp"), new int[] { 0, 0 });


            }
            else if (percent >= 90)
            {
                letterGrade.x = 65;
                grade = "SS";
                letterGrade.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "SS.bmp"), new int[] { 0, 0 });


            }
            else if (percent >= 87)
            {
                letterGrade.x = 70;
                grade = "S";
                letterGrade.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "S.bmp"), new int[] { 2, 0 });

            }
            else if (percent >= 80)
            {
                letterGrade.x = 69;
                grade = "A";
                letterGrade.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "A.bmp"), new int[] { 0, 0 });


            }
            else if (percent >= 73)
            {
                letterGrade.x = 69;
                grade = "B";
                letterGrade.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "B.bmp"), new int[] { 0, 0 });


            }
            else if (percent >= 67)
            {
                letterGrade.x = 65;
                grade = "C";
                letterGrade.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "C.bmp"), new int[] { 4, 0 });


            }
            else if (percent >= 60)
            {
                letterGrade.x = 65;
                grade = "D";
                letterGrade.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "D.bmp"), new int[] { 5, 0 });

            }
            else
            {
                letterGrade.x = 67;
                grade = "F";
                letterGrade.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "F.bmp"), new int[] { 5, 0 });

            }
            if(grade == "NA")
            {
                throw new Exception("NO VALID SCORE");

            }
            _lastScore = PlayerSettings.Instance.chartScores[game.songHash].percent;
            if (percent > PlayerSettings.Instance.chartScores[game.songHash].percent)
            {
                PlayerSettings.Instance.SaveScore(game.songHash, grade, percent);

            }

            for (int i = 0; i < 101; i++)
            {
                visual.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                visual.localPositions.Add(new Coords(i, -1, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                visual.localPositions.Add(new Coords(i, -2, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
            }
            for (int i = 0; i < songName.Length; i++)
            {
                visual.localPositions.Add(new Coords((100 / 2) - (songName.Length / 2) + i, -1, songName[i], ConsoleColor.White, ConsoleColor.DarkGray));
            }
            for (int i = 0; i < percentText.Length; i++)
            {
                visual.localPositions.Add(new Coords(i + 13, -30, percentText[i], ConsoleColor.White, ConsoleColor.DarkGray));

            }
            char[] enter = "Press enter to return to the main menu".ToCharArray();
            for (int i = 0; i < enter.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, -40, enter[i], ConsoleColor.White, ConsoleColor.DarkGray));

            }

            //draw backdrop and grade cover
            for (int x = 60; x < 100; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    backDrop.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                    backDrop.localPositions.Add(new Coords(x - 60, y, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));

                    gradeCover.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                    gradeCover.localPositions.Add(new Coords(x - 60, y, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                }
            }
            backDrop.localPositions.Add(new Coords(50, 5, ' ', ConsoleColor.Blue, ConsoleColor.Blue));
            //draw "clear line"
            for (int x = 40; x < 60; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    backDrop.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Red, ConsoleColor.Red));

                }
            }
            for (int i = 25; i < 100; i++)
            {
                backDrop.localPositions.Add(new Coords(59, i, ' ', ConsoleColor.Red, ConsoleColor.Red));
            }
            char[] GradeText = "Grade".ToCharArray();
            for (int i = 0; i < GradeText.Length; i++)
            {
                backDrop.localPositions.Add(new Coords(i + 75, 24, GradeText[i], ConsoleColor.White, ConsoleColor.Gray));
            }
            //draw clear guage

            for (int i = 0; i < percent; i++)
            {
                //clearGuage.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Green, ConsoleColor.Green));
            }

            visual.active = true;
            letterGrade.active = true;
            backDrop.active = true;
            clearGuage.active = true;
            gradeCover.active = true;
            components.Add(backDrop);
            components.Add(visual);
            components.Add(letterGrade);
            components.Add(clearGuage);
            components.Add(gradeCover);
            clearGuage.randBreak = true;
        }



        public override void Update(double time, Game game)
        {

            //draw guage
            //timePassed = 20;
            if (timePassed >= guageTick)
            {
                if (guagePercent <= percent)
                {
                    //clearGuage.randAmount = (int)percent-(int)guagePercent;
                    if (guagePercent < 60)
                    {

                        for (int y = 0; y < 22; y++)
                        {
                            //AddPixelAnim((int)guagePercent - 1, y, 5, ConsoleColor.Red, "easeLinear", 0.0125f);
                            clearGuage.localPositions.Add(new Coords((int)guagePercent - 1, y, ' ', ConsoleColor.Red, ConsoleColor.Red));

                        }
                        game.audioManager.playForget("syssd_score.mp3");
                        guagePercent++;
                    }
                    else if (guagePercent == 60)
                    {
                        clearGuage.localPositions.Clear();
                        for (int x = 0; x < 60; x++)
                        {
                            for (int y = 0; y < 22; y++)
                            {
                                clearGuage.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Green, ConsoleColor.Green));
                            }
                        }
                        for (int x = 40; x < 60; x++)
                        {
                            for (int y = 0; y < 25; y++)
                            {
                                backDrop.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Green, ConsoleColor.Green));

                            }
                        }
                        game.audioManager.playForget("syssd_res_2.mp3");
                        guagePercent++;
                    }
                    else
                    {
                        for (int y = 0; y < 22; y++)
                        {
                            clearGuage.localPositions.Add(new Coords((int)guagePercent - 1, y, ' ', ConsoleColor.Green, ConsoleColor.Green));
                        }
                        game.audioManager.playForget("syssd_score.mp3");
                        guagePercent++;
                    }
                    timePassed = 0;
                }
                else
                {

                }
            }
            if (guagePercent >= percent)
            {
                if (coverSlide)
                {
                    if (timePassed >= coverTick)
                    {
                        gradeCover.y--;
                        timePassed = 0;
                    }
                }
                else if (timePassed >= coverWait)
                {
                    timePassed = 0;
                    coverSlide = true;
                }
            }
            timePassed = timePassed + (float)time;
            if (game.input.ButtonStates[Input.ButtonKind.Confirm] == Input.ButtonState.Press || game.input.ButtonStates[Input.ButtonKind.Cancel] == Input.ButtonState.Press)
            {
                game.sceneManager.loadScene(0);
            }
        }
    }
}
