using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace RhythmThing.Objects.Test_Objects
{
    class RotationTest : GameObject
    {
        private Visual visual;
        int index = 0;
        List<Bitmap> frames;
        Bitmap[] frameArray;
        string[] files;
        int[] startPoint = { 0, 0 };
        bool tog = false;
        private Visual debugText;
        private float rotation = 0;
        public override void End()
        {

        }


        public override void Start(Game game)
        {
            frames = new List<Bitmap>();
            this.components = new List<Component>();
            visual = new Visual();
            visual.active = true;
            visual.x = 50;
            visual.y = 25;
            visual.LoadBMP(Path.Combine(Program.contentPath, "Sprites", "RightReceiver.bmp"), new int[] { -5, -6 });
            components.Add(visual);
            debugText = new Visual();
            debugText.active = true;
            debugText.writeText(0, 0, "TEST STRING", ConsoleColor.White, ConsoleColor.Black);
            components.Add(debugText);
        }

        public override void Update(double time, Game game)
        {
            if(Input.Instance.ButtonStates[Input.ButtonKind.Confirm] == Input.ButtonState.Held)
            {
                visual.matrix.Rotate(1);
                rotation++;

            }
            if(Input.Instance.ButtonStates[Input.ButtonKind.Up] == Input.ButtonState.Press)
            {
                visual.useMatrix = !visual.useMatrix;
            }
            if(Input.Instance.ButtonStates[Input.ButtonKind.Right] == Input.ButtonState.Press)
            {
                visual.x++;
            }
            if (Input.Instance.ButtonStates[Input.ButtonKind.Left] == Input.ButtonState.Press)
            {
                visual.x--;
            }

            debugText.localPositions.Clear();
            debugText.writeText(0, 0, $"Deg: {rotation} || Matrix: {visual.useMatrix}", ConsoleColor.White, ConsoleColor.Black);
        }
    }
}
