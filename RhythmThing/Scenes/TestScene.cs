using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using RhythmThing.Objects.Test_Objects;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.Scenes
{
    class TestScene : Scene
    {
        public TestScene()
        {
            this.name = "Testing";
            this.index = 5;
        }
        public override void Start()
        {

            initialObjs = new List<GameObject>();
            initialObjs.Add(new MultiWindowTest());

        }

        public override void Update()
        {

        }
    }
}
