using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.Objects.Menu.MenuMusic
{
    public class MenuMusicHandler : GameObject
    {
        
        public static MenuMusic menuMusic;
        const float timeToPreview = 1;
        private float _timeSinceSelect = 0;
        private bool _previewPlayed = true;
        public override void End()
        {

        }
        public static GameObject GetMusicAnim()
        {
            //this will do things better later
            return new HVMenuAnim();
        }
        public MenuMusicHandler()
        {
            this.type = objType.nonvisual;
            //manual for now
            menuMusic = new HVMusic();

            Game.mainInstance.addGameObject(menuMusic);
        }
        public override void Start(Game game)
        {

        }
        public void StartMainMusic()
        {
            menuMusic.StartMenuMusic();
        }
        public void SelectNoise(SongContainer container)
        {
            menuMusic.SongSelected(container);
            _previewPlayed = false;
            _timeSinceSelect = 0;
        }

        public override void Update(double time, Game game)
        {
            if (!_previewPlayed)
            {
                if(_timeSinceSelect >= timeToPreview)
                {
                    menuMusic.PreviewSelected();
                    _previewPlayed = true;
                } else
                {
                    _timeSinceSelect += (float)time;
                }
            }
        }
    }
}
