﻿using RhythmThing.System_Stuff;
using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Objects.Menu.Options_Menu
{
    //TODO: make this exist.
    public class OptionsPlaceholder : GameObject
    {
        private Visual visual;
        public override void End()
        {
            
        }

        public override void Start(Game game)
        {
            Components = new List<Component>();
            visual = new Visual();
            visual.Active = true;
            //temp thing
            char[] tempMessage = "Sorry! No options implemented yet! :(".ToCharArray();
            char[] enterMessage = "Please press enter to return to the menu".ToCharArray();
            for (int i = 0; i < tempMessage.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, 30, tempMessage[i], ConsoleColor.White, ConsoleColor.Black));
            }
            for (int i = 0; i < enterMessage.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, 25, enterMessage[i], ConsoleColor.White, ConsoleColor.Black));

            }

            Components.Add(visual);
        }

        public override void Update(double time, Game game)
        {
            if(game.InputInstance.ButtonStates[Input.ButtonKind.Confirm] == Input.ButtonState.Press)
            {
                game.SceneManagerInstance.LoadScene(0);
            }
        }
    }
}
