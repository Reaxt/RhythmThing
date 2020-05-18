using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.System_Stuff;
using RhythmThing.Components;

namespace RhythmThing.Objects.Menu.Options_Menu
{
    public class OptionsObject : GameObject
    {

        private Visual selector;
        private Visual quit;
        private RebindButton rebindButton;
        private OffsetButton offsetButton;
        //if this is true, we can select shit, if not, another thing is doing stuff we dont want to interfere with
        private bool selectorFocused = true;
        //0 = rebind, 1 = offset maybe, 2 = menu
        int selectedOption = 0;
        int maxOption = 2; //just in case I want to add more it wont be *as* bad
        private ConsoleColor quitFront = ConsoleColor.Black;
        private ConsoleColor quitBack = ConsoleColor.Red;
        public override void End()
        {
            //throw new NotImplementedException();
        }

        public override void Start(Game game)
        {
            //forwearning, lots of lazy code throughout all of the options tbh.
            this.components = new List<Component>();
            selector = new Visual();
            selector.active = true;

            selector.x = 3;
            selector.y = 45;
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

            //quit button. 
            quit = new Visual();
            quit.x = 5;
            quit.y = (45 - (maxOption * 5)); //will make it easier if I wanna add more buttons.
            quit.active = true;
            char[] quitText = "quit".ToCharArray();
            //tempted to add a button drawing thing into utils tbh
            for (int i = -1; i < 30; i++)
            {
                quit.localPositions.Add(new Coords(i, 1, ' ', quitFront, quitBack));
                quit.localPositions.Add(new Coords(i, 0, ' ', quitFront, quitBack));
                quit.localPositions.Add(new Coords(i, -1, ' ', quitFront, quitBack));
            }
            for (int i = 0; i < quitText.Length; i++)
            {
                quit.localPositions.Add(new Coords(i, 0, quitText[i], quitFront, quitBack));
            }
            components.Add(quit);

            //spawn buttons
            rebindButton = new RebindButton();
            offsetButton = new OffsetButton();
            game.addGameObject(rebindButton);
            game.addGameObject(offsetButton);

        }
        public void returnFocus()
        {
            selectorFocused = true;
        }
        public override void Update(double time, Game game)
        {
            if(selectorFocused)
            {
                //these two lines are responsible for selecting
                if(Input.downKey == Input.buttonState.press)
                {
                    if (selectedOption >= maxOption)
                    {
                        selectedOption = 0;
                        selector.y = 45;
                    } else
                    {
                        selectedOption++;
                        selector.y = selector.y - 5;
                    }

                } else  if(Input.upKey == Input.buttonState.press)
                {
                    if(selectedOption <= 0)
                    {
                        selectedOption = maxOption;
                        selector.y = (45 - (maxOption * 5));
                        
                    } else
                    {
                        selectedOption--;
                        selector.y = selector.y + 5;
                    }
                }
                if(Input.escKey == Input.buttonState.press)
                {
                    game.sceneManager.loadScene(0);
                }

                //handle enter/select
                if(Input.enterKey == Input.buttonState.press)
                {
                    //quit first, just in case I add more, this makes it easier
                    if(selectedOption == maxOption)
                    {
                        game.sceneManager.loadScene(0);//return to menu
                    }
                    //switch case for the rest of the options
                    switch (selectedOption)
                    {
                        case 0: //this is rebind.
                            this.selectorFocused = false;
                            rebindButton.rebind();
                            //bleeeeech ill finish this later
                            //probably have a listener inside of the actual like, input class, have that listen for keystrokes, if its one we want then set it. bleeeh I should rewrite so much stuff this project is turning into a mess ""
                            break;
                        default:
                            break;
                    }

                }
            }
        }
    }
}
