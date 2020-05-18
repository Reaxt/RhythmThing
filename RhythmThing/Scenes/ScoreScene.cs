using RhythmThing.Components;
using RhythmThing.System_Stuff;
using RhythmThing.Objects.ScoreScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Scenes
{
    class ScoreScene : Scene
    {
        public ScoreScene()
        {
            this.name = "ScoreScene";
            this.index = 2;
        }
        public override void Start()
        {
            initialObjs = new List<GameObject>();
            initialObjs.Add(new ScoreScreenHandler());
            
        }

        public override void Update()
        {

        }
    }
}