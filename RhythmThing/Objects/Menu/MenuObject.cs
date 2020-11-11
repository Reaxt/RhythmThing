using RhythmThing.System_Stuff;
using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RhythmThing.Objects.Menu.MenuMusic;
using RhythmThing.Utils;

namespace RhythmThing.Objects.Menu
{
    class MenuObject : GameObject
    {
        private enum MenuSection
        {
            songSelect,
            optSelect
        }
        private enum optSelections
        {
            options,
            quit
        }

        const int songMenuX = 8;
        const int optionMenuX = 58;

        private float animTime = 0.05f;
        private string animEasing = "easeLinear";
        private int[] selectorOn = new int []{ 8, 25 };
        private int[] selectorOff = new int[] { 8 - 41, 25 };
        List<SongContainer> songs;
        private ContainerHandler containerHandler;
        private MenuMusicHandler menuMusic;
        private Visual selector;
        private Visual optButton;
        private Visual buttonSelector;
        //option button colors
        private ConsoleColor optFront = ConsoleColor.Black;
        private ConsoleColor optBack = ConsoleColor.Yellow;
        private Visual quitButton;
        //quit button colors
        private ConsoleColor quitFront = ConsoleColor.Black;
        private ConsoleColor quitBack = ConsoleColor.Red;
        private ChartInfoVisual chartInfoVisual;
        public static int selected = 0;
        private int count = -1;
        private int drawAmount = 8;
        private int currentHighestSelect = 7;//must be same as drawAmount-1
        private int currentLowestSelect = 0;
        private MenuSection menuSection = MenuSection.songSelect;
        private optSelections optionSelected = optSelections.options;
        private string assetPath = Path.Combine(Program.contentPath, "MenuMusic", "MainMenu");
        private Visual ringVisual;

        private int selectorAnim = 0;
        private double timePassed = 0;
        const float selectorAnimTick = 0.025f;
        const float fastSelectorAnimTick = 0f;
        private float timePerSelectorAnim = selectorAnimTick;
        private float timeForSpeedup = 0.5f;
        private float speedupTimePassed = 0;
        private bool speedup = false;
        public override void End()
        {
            //throw new NotImplementedException();
        }

        public override void Start(Game game)
        {
            
            components = new List<Component>();
            menuMusic = new MenuMusicHandler();
            string[] chartNames = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "!Content/!Songs"));
            
            songs = new List<SongContainer>();
            for (int i = 0; i < chartNames.Length; i++)
            {
                SongContainer tempcon = new SongContainer(chartNames[i], i);
                game.addGameObject(tempcon);
                songs.Add(tempcon);
                count++;
            }
            songs.Sort((a, b) =>
            {
                return (a.chart.chartInfo.difficulty > b.chart.chartInfo.difficulty) ? 1 : -1;
            });

            for (int i = 0; i < songs.Count; i++)
            {
                songs[i].pos = i;
                chartNames[i] = songs[i].chartName;
            }

            selector = new Visual();
            selector.active = true;

            selector.x = 3+5;
            selector.y = 25;

            selector.localPositions.Add(new Coords(0, 0, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(0, 1, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(0, -1, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(32, 0, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(32, 1, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(32, -1, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            for (int i = 0; i < 33; i++)
            {
                selector.localPositions.Add(new Coords(i, 2, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
                selector.localPositions.Add(new Coords(i, -2, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            }
            components.Add(selector);
            chartInfoVisual = new ChartInfoVisual(songs[selected].chart);
            game.addGameObject(chartInfoVisual);


            //draw the option visual
            optButton = new Visual();
            optButton.active = true;
            char[] optText = "Options".ToCharArray();
            optButton.x = 50;
            optButton.y = 3;
            optButton.LoadBMP(Path.Combine(assetPath, "button.bmp"));
            optButton.writeText(7, 3, "Options", quitFront, quitBack);
            optButton.overrideColor = true;
            optButton.overridefront = optBack;
            optButton.overrideback = optFront;
            components.Add(optButton);

            //draw the quit visual
            quitButton = new Visual();
            quitButton.active = true;
            quitButton.x = 75;
            quitButton.y = 3;

            quitButton.LoadBMP(Path.Combine(assetPath, "button.bmp"));
            quitButton.writeText(9, 3, "Quit", quitFront, quitBack);
            quitButton.overrideColor = true;
            quitButton.overridefront = quitBack;
            quitButton.overrideback = quitFront;
            //selector
            buttonSelector = new Visual();
            buttonSelector.active = true;
            buttonSelector.x = 75;
            buttonSelector.y = 3-10;
            buttonSelector.z = 10;
            buttonSelector.LoadBMP(Path.Combine(assetPath, "buttonSelector.bmp"));
            components.Add(buttonSelector);
            //draw the rings!
            ringVisual = new Visual();
            ringVisual.LoadBMP(Path.Combine(assetPath, "WheelGrey.bmp"), new int[] { 0, -1 });
            ringVisual.LoadBMP(Path.Combine(assetPath, "WheelBlack.bmp"), new int[] {0,-1 });
            ringVisual.z = -1;
            ringVisual.y = -1;
            ringVisual.active = true;
            components.Add(ringVisual);
            components.Add(quitButton);

            containerHandler = new ContainerHandler(songs);
            game.addGameObject(containerHandler);

            ringVisual.Animate(new int[] { -10, 0 }, new int[] { 0, 0 }, "easeOutQuad", 0.5f);

            game.addGameObject(menuMusic);
            menuMusic.StartMainMusic();
        }

        //should only ever be called from a safe index. otherwise I fucked up elsewhere lo
        private void selectorAnimation(int i)
        {
            if (i < 32)
            {
                selector.localPositions.Add(new Coords(32-i, -2, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
                selector.localPositions.Add(new Coords(i, 2, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            } else
            {
                selector.localPositions.Add(new Coords(32, 2-(i-32), ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
                selector.localPositions.Add(new Coords(0, (-2+(i-32)), ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));

            }

        }

        public override void Update(double time, Game game)
        {
            //selector animation
            if (timePassed>timePerSelectorAnim)
            {
                selectorAnim++;
                selector.localPositions.Clear();
                for (int i = 0; i < 10; i++)
                {
                    selectorAnimation((selectorAnim + i) % 36);
                }
                if (selectorAnim > 36)
                {
                    selectorAnim = 0;
                }
                timePassed = 0;
            }
            timePassed += time;

            if (speedup)
            {
                if (speedupTimePassed > timeForSpeedup)
                {
                    timePerSelectorAnim = selectorAnimTick;
                    speedupTimePassed = 0;
                    speedup = false;
                } else
                {
                    float easeAmount = Ease.Exponential.In(speedupTimePassed / timeForSpeedup);
                    timePerSelectorAnim = Ease.Lerp(fastSelectorAnimTick, selectorAnimTick, easeAmount);
                }
                speedupTimePassed += (float)time;
            }


            if (menuSection == MenuSection.songSelect)
            {
                if (game.input.ButtonStates[Input.ButtonKind.Down] == Input.ButtonState.Press)
                {
                    if (selected < count)
                    {
                        selected++;


                    }
                    else
                    {
                        selected = 0;
                    }
                    containerHandler.AnimUp();
                    chartInfoVisual.UpdateChart(songs[selected].chart);
                    menuMusic.SelectNoise(songs[selected]);
                    speedup = true;
                    speedupTimePassed = 0;

                }
                if (game.input.ButtonStates[Input.ButtonKind.Up] == Input.ButtonState.Press)
                {
                    if (selected > 0)
                    {
                        selected--;
                        
                    }
                    else
                    {

                        selected = count;
                    }

                    containerHandler.AnimDown();
                    chartInfoVisual.UpdateChart(songs[selected].chart);
                    menuMusic.SelectNoise(songs[selected]);
                    speedup = true;
                    speedupTimePassed = 0;
                }

                //a little out animation!
                

                if (game.input.ButtonStates[Input.ButtonKind.Confirm] == Input.ButtonState.Press)
                {
                    game.ChartToLoad = songs[selected].chartName;
                    game.sceneManager.loadScene(6);
                }
                if (game.input.ButtonStates[Input.ButtonKind.Left] == Input.ButtonState.Press || game.input.ButtonStates[Input.ButtonKind.Right] == Input.ButtonState.Press)
                {
                    menuSection = MenuSection.optSelect;
                    hideSelector();
                    optionSelected = optSelections.options;
                    selectOpt();
                }
            } else if (menuSection == MenuSection.optSelect)
            {
                //else if is so juuuust in case, switching cant be at the same time as trying to press one of these
                if (game.input.ButtonStates[Input.ButtonKind.Left] == Input.ButtonState.Press )
                {
                    if(optionSelected == optSelections.quit)
                    {
                        optionSelected = optSelections.options;
                        selectOpt();

                    } else if (optionSelected == optSelections.options)
                    {
                        menuSection = MenuSection.songSelect;
                        hideOptSelector();
                        showSelector();
                    }
                    

                    //this line can check for either right now as there is only two options. I dont see any case where there would be more.
                } else if (game.input.ButtonStates[Input.ButtonKind.Right] == Input.ButtonState.Press)
                {
                    if (optionSelected == optSelections.options)
                    {
                        selectQuit();
                        optionSelected = optSelections.quit;
                    }
                    else if(optionSelected == optSelections.quit)
                    {
                        hideOptSelector();
                        showSelector();
                        menuSection = MenuSection.songSelect;
                    }
                } else if(game.input.ButtonStates[Input.ButtonKind.Confirm] == Input.ButtonState.Press)
                {
                    //follow through with the selectiooooon
                    if(optionSelected == optSelections.quit)
                    {
                        //THIS SHOULD BE THE ONLY PLACE IN THE CODE WHERE THIS LINE IS EVER WRITTEN. IF ITS ANYWHERE ELSE BAD THINGS COULD HAPPEN
                        Game.gameLoopLives = false;
                        game.running = false;
                    } else if(optionSelected == optSelections.options)
                    {
                        game.sceneManager.loadScene(4);
                    }
                }
                

            }

        }

        private void hideSelector()
        {
            selector.Animate(selectorOn, selectorOff, "easeOutSine", 0.25f);
        }
        private void showSelector()
        {
            selector.Animate(selectorOff, selectorOn, "easeOutSine", 0.25f);

        }
        private void hideOptSelector()
        {
            buttonSelector.Animate(new int[] { buttonSelector.x, buttonSelector.y }, new int[] { buttonSelector.x, buttonSelector.y - 10 }, "easeOutSine", 0.25f);
        }
        private void selectQuit()
        {
            buttonSelector.Animate(new int[] { buttonSelector.x, buttonSelector.y }, new int[] { quitButton.x, quitButton.y }, "easeOutSine", 0.125f);

        }
        private void selectOpt()
        {
            buttonSelector.Animate(new int[] { buttonSelector.x, buttonSelector.y }, new int[] { optButton.x, optButton.y }, "easeOutSine", 0.125f);

        }
    }
}
