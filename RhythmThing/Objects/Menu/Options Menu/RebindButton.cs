using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System.Windows.Input;
using RhythmThing.Utils;

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
        private Input.ButtonKind keyToRebind;
        private bool boundConfirm = false;
        private OptionsObject optionsObject;
        private bool activated = false;
        private bool liftGood = false;
        private Key[] allKeys;

        public RebindButton(OptionsObject optionsObject)
        {
            this.optionsObject = optionsObject;
        }

        public override void End()
        {
            //throw new NotImplementedException();
        }
        
        public override void Start(Game game)
        {
            //populate key array
            allKeys = Enum.GetValues(typeof(Key)).Cast<Key>().ToArray<Key>();
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
            rebindVisual.writeText(0, 0, "Please press the key you wish to assign to LEFT", visualFront, visualBack);
            keyToRebind = Input.ButtonKind.Left;
            Input.Instance.RebindKey(Input.ButtonKind.Left);
        }
        private void rebindNext()
        {
            rebindVisual.localPositions.Clear();
            switch (keyToRebind)
            {
                case Input.ButtonKind.Left:
                    rebindVisual.writeText(0, 0, "Please press the key you wish to assign to DOWN", visualFront, visualBack);
                    keyToRebind = Input.ButtonKind.Down;
                    Input.Instance.RebindKey(Input.ButtonKind.Down);

                    break;
                case Input.ButtonKind.Down:
                    rebindVisual.writeText(0, 0, "Please press the key you wish to assign to UP", visualFront, visualBack);
                    keyToRebind = Input.ButtonKind.Up;
                    Input.Instance.RebindKey(Input.ButtonKind.Up);
                    break;
                case Input.ButtonKind.Up:
                    rebindVisual.writeText(0, 0, "Please press the key you wish to assign to RIGHT", visualFront, visualBack);
                    keyToRebind = Input.ButtonKind.Right;
                    Input.Instance.RebindKey(Input.ButtonKind.Right);
                    break;
                case Input.ButtonKind.Right:
                    rebindVisual.writeText(0, 0, "Please press the key you wish to assign to CONFIRM", visualFront, visualBack);
                    keyToRebind = Input.ButtonKind.Confirm;
                    Input.Instance.RebindKey(Input.ButtonKind.Confirm);
                    break;
                case Input.ButtonKind.Confirm:
                    rebindVisual.writeText(0, 0, "Please press the key you wish to assign to CANCEL", visualFront, visualBack);
                    keyToRebind = Input.ButtonKind.Cancel;
                    Input.Instance.RebindKey(Input.ButtonKind.Cancel);
                    break;
                case Input.ButtonKind.Cancel:
                    rebindVisual.localPositions.Clear();
                    rebindVisual.active = false;
                    activated = false;
                    optionsObject.returnFocus();
                    Input.Instance.SaveCurrentBindings();
                    break;
                default:
                    break;
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
                if (!Input.Instance.RebindStatus)
                {
                    rebindNext();
                }
            }
        }
    }
}
