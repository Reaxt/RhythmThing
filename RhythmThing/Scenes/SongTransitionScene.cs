using RhythmThing.Components;
using RhythmThing.Objects.Menu.MenuMusic;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.Scenes
{
    class SongTransitionScene : Scene
    {
        public SongTransitionScene()
        {
            this.name = "Intro Animation";
            this.index = 6;
        }
        public override void Start()
        {


            initialObjs = new List<GameObject>();
            initialObjs.Add(MenuMusicHandler.GetMusicAnim());

        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
