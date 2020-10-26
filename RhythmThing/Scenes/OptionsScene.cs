using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using RhythmThing.Objects.Menu.Options_Menu;

namespace RhythmThing.Scenes
{
    class OptionsScene : Scene
    {
        public OptionsScene()
        {
            this.name = "Options Screen";
            this.index = 4;
        }

        public override void Start()
        {
            initialObjs = new List<GameObject>();
            //This is a tempoary placeholder while I make the actual menu. . . (Is there really much to even put in there? Offset I guess. Might remove) 
            initialObjs.Add(new OptionsObject());

        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
