using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Objects.Menu;
namespace RhythmThing.Scenes
{
    class MenuScene : Scene
    {
        public MenuScene()
        {
            this.name = "Menu";
            this.index = 0;

        }
        public override void Start()
        {
            
            initialObjs = new List<GameObject>();
            initialObjs.Add(new MenuObject());

        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
