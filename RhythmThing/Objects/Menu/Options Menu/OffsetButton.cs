using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
namespace RhythmThing.Objects.Menu.Options_Menu
{
    class OffsetButton : GameObject
    {
        private Visual visual;
        private ConsoleColor frontColor = ConsoleColor.Black;
        private ConsoleColor backColor = ConsoleColor.Yellow;
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
            visual.y = 40;
            char[] offsetText = "Set Offset".ToCharArray(); ;
            for (int i = -1; i < 30; i++)
            {
                visual.localPositions.Add(new Coords(i, 1, ' ', frontColor, backColor));
                visual.localPositions.Add(new Coords(i, 0, ' ', frontColor, backColor));
                visual.localPositions.Add(new Coords(i, -1, ' ', frontColor, backColor));
            }
            for (int i = 0; i < offsetText.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, 0, offsetText[i], frontColor, backColor));
            }

            components.Add(visual);
        }

        public override void Update(double time, Game game)
        {
            //hrow new NotImplementedException();
        }
    }
}
