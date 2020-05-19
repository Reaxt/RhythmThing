using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System.Windows.Input;
namespace RhythmThing.Objects.Menu.Options_Menu
{
    public class RebindButton : GameObject
    {
        private Visual visual;
        private Visual rebindVisual;
        private ConsoleColor frontColor = ConsoleColor.Black;
        private ConsoleColor backColor = ConsoleColor.Yellow;
        private ConsoleColor visualFront = ConsoleColor.Black;
        private ConsoleColor visualBack = ConsoleColor.White;
        private int rebindState = 0; //collumn order. (0 = left, 1 = down, etc..)
        private bool boundConfirm = false;
        private bool activated = false;

        public override void End()
        {
            //throw new NotImplementedException();
        }

        public override void Start(Game game)
        {
            this.components = new List<Component>();
            visual = new Visual();
            visual.active = true;
            visual.x = 5;
            visual.y = 45;

            char[] rebindText = "Rebind Keys".ToCharArray(); ;
            for (int i = -1; i < 30; i++)
            {
                visual.localPositions.Add(new Coords(i, 1, ' ', frontColor, backColor));
                visual.localPositions.Add(new Coords(i, 0, ' ', frontColor, backColor));
                visual.localPositions.Add(new Coords(i, -1, ' ', frontColor, backColor));
            }
            for (int i = 0; i < rebindText.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, 0, rebindText[i], frontColor, backColor));
            }

            components.Add(visual);

            rebindVisual = new Visual();
            rebindVisual.active = false;
            rebindVisual.x = 20;
            rebindVisual.y = 30;
            components.Add(rebindVisual);

        }

        public void rebind()
        {
            activated = true;
            rebindVisual.active = true;
            char[] leftText = "Please press the key you wish to assign to LEFT".ToCharArray();

            for (int i = 0; i < leftText.Length; i++)
            {
                rebindVisual.localPositions.Add(new Coords(i, 0, leftText[i], visualFront, visualBack));

            }
        }
        public static void KeyboardEventHandle(object sender, KeyboardEventArgs args)
        {
            Console.WriteLine("g");
        }
        //vro how do I DO THIS I JUST WANT INPUT WINDOWS WHY
        //is this how delegates work?
        KeyboardEventHandler eventHandler = KeyboardEventHandle;

        public override void Update(double time, Game game)
        {
            //hrow new NotImplementedException();
            if(activated)
            {
                if(!boundConfirm)
                {
                    
                }
            }
        }
    }
}
