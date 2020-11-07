using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.Objects.Menu.MenuMusic
{
    public abstract class MenuMusic : GameObject
    {
        public static MenuMusic selected;
        public abstract void StartMenuMusic();
        public abstract void SongSelected(SongContainer container);
        public abstract void PreviewSelected();
    }
}
