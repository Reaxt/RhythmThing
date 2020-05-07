using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using RhythmThing.Objects.Intro;
namespace RhythmThing.Scenes
{
    class IntroScene : Scene
    {
        public IntroScene()
        {
            this.name = "Intro Animation";
            this.index = 3;
        }
        public override void Start()
        {
 

            initialObjs = new List<GameObject>();
            initialObjs.Add(new IntroAnimationHandler());

        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
