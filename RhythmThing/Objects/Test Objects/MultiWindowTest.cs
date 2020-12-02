using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
namespace RhythmThing.Objects.Test_Objects
{
    
    class MultiWindowTest : GameObject
    {
        SlaveManager manager;
        SlaveManager manager2;
        SlaveManager manager3;
        private Visual visualTest;
        public override void End()
        {

        }

        public override void Start(Game game)
        {
            manager = new SlaveManager("A", 50, 50);
            manager2 = new SlaveManager("B", 30, 30);
            manager3 = new SlaveManager("C", 20, 50);
            
            Input.focusInput = false;
            visualTest = new Visual();
            visualTest.LoadBMP("RightReceiver.bmp");
            manager.visuals.Add(visualTest);
            manager2.visuals.Add(visualTest);
            manager3.visuals.Add(visualTest);
        }

        public override void Update(double time, Game game)
        {
            if(game.input.ButtonStates[Input.ButtonKind.Right] == Input.ButtonState.Press)
            {
                manager.MoveWindowEase(-100, -100, 100, 100, 5, "easeInOutExpo");
                

                visualTest.x++;
            }
            if (game.input.ButtonStates[Input.ButtonKind.Left] == Input.ButtonState.Press)
            {
                manager.MoveWindow(100, 0);
                manager2.MoveWindow(100, 0);

                visualTest.x--;
            }
            if (game.input.ButtonStates[Input.ButtonKind.Up] == Input.ButtonState.Press)
            {
                manager.MoveWindow(0, 100);
                manager2.MoveWindow(0, 100);

                visualTest.y++;
            }
            if (game.input.ButtonStates[Input.ButtonKind.Down] == Input.ButtonState.Press)
            {
                manager.MoveWindow(100, 100);
                manager2.MoveWindow(100, 100);

                visualTest.y--;
            }
            if (manager.alive)
            {
                manager.UpdateVisualsAsync();

            }
            manager2.UpdateVisualsAsync();
            manager3.UpdateVisualsAsync();
            if(game.input.ButtonStates[Input.ButtonKind.Cancel] == Input.ButtonState.Press)
            {
                manager.CloseWindow();
            }
        }
    }
}
