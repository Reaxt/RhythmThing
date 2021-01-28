using RhythmThing.Components;
using RhythmThing.Objects;
using RhythmThing.Objects.Menu;
using RhythmThing.Objects.Test_Objects;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Scenes
{
    class SongScene : Scene
    {
        public SongScene()
        {
            this.name = "SongScene";
            this.index = 1;
        }
        public override void Start()
        {
            initialObjs = new List<GameObject>();

            initialObjs.Add(new Chart(Game.MainInstance.ChartToLoad));
        }

        public override void Update()
        {

        }
    }
}
