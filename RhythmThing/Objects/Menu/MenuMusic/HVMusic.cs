using CSCore;
using CSCore.Codecs;
using CSCore.Streams;
using RhythmThing.System_Stuff;
using RhythmThing.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace RhythmThing.Objects.Menu.MenuMusic
{
    class HVMusic : MenuMusic
    {
        string pathToMusic = Path.Combine(Program.contentPath, "MenuMusic", "HVMusic", "bgmPhase2.wav");
        AudioTrack mainMusic;
        ISampleSource ding;
        float timeToPreview = 1f;
        VolumeSource previewVol;
        float previewDur;
        bool previewLoaded = false;
        bool doPreview = false;
        float timePassed = 0;
        ISampleSource previewSampleSource;
        AudioTrack previewAudio;
        private SongContainer songContainer;
        private TimeSpanConverter spanConverter;
        string pathToSelectDing = Path.Combine(Program.contentPath, "MenuMusic", "HVMusic", "ding.wav");
        //ref may be relevant?
        private HVVideo video;
        public override void End()
        {
            Game.mainInstance.audioManager.removeTrack(mainMusic);
            Game.mainInstance.audioManager.removeTrack(previewAudio);

        }
        public HVMusic()
        {

        }
        public HVMusic(bool anim)
        {

        }
        public override void PreviewSelected()
        {
            timePassed = 0;
            doPreview = true;
            string previewPath = Path.Combine(songContainer.chart.chartPath, songContainer.chart.chartInfo.songPath);
            previewSampleSource = CodecFactory.Instance.GetCodec(previewPath).ChangeSampleRate(AudioManager.sampleRate).ToStereo().ToSampleSource().AppendSource(x => new VolumeSource(x), out previewVol);
            previewAudio = new AudioTrack("preview", previewSampleSource, previewVol);
            previewSampleSource.SetPosition(TimeSpan.FromSeconds(songContainer.chart.chartInfo.preview));
            previewVol.Volume = 0;


            Game.mainInstance.audioManager.addTrack(previewAudio);
        }

        public override void SongSelected(SongContainer container)
        {
            doPreview = false;

            songContainer = container;
            //just play the sound rn
            mainMusic.volumeSource.Volume = 1;
            previewDur = container.chart.chartInfo.previewLength;
            ding.Position = 0;
            Game.mainInstance.audioManager.playForget(ding);
            Game.mainInstance.audioManager.removeTrack(previewAudio);
        }


        public override void Start(Game game)
        {
            ding = CodecFactory.Instance.GetCodec(pathToSelectDing).ChangeSampleRate(AudioManager.sampleRate).ToStereo().ToSampleSource();
            spanConverter = new TimeSpanConverter();
            //ImageUtils.BMPToBinary(Path.Combine(Program.contentPath, "MenuMusic", "HVMusic", "bitmaps"), Path.Combine(Program.contentPath, "MenuMusic", "vidR.cvid"));

            video = new HVVideo();
            game.addGameObject(video);
        }

        public override void StartMenuMusic()
        {
            mainMusic = Game.mainInstance.audioManager.addTrack(pathToMusic);
        }

        public override void Update(double time, Game game)
        {
            if(mainMusic.sampleSource.GetPosition().TotalMilliseconds >= mainMusic.sampleSource.GetLength().TotalMilliseconds-50)
            {
                mainMusic.sampleSource.Position = 0;
            }
            if (doPreview)
            {
                if(timePassed <= timeToPreview)
                {
                    mainMusic.volumeSource.Volume = Ease.Lerp(1, 0, timePassed / timeToPreview);
                    previewVol.Volume = Ease.Lerp(0, 1, timePassed / timeToPreview);

                } else if(timePassed-timeToPreview >= previewDur && timePassed-timeToPreview <timeToPreview+previewDur)
                {
                    mainMusic.volumeSource.Volume = Ease.Lerp(0, 1, timePassed-(timeToPreview+previewDur) / timeToPreview);
                    previewVol.Volume = Ease.Lerp(1, 0, timePassed- (timeToPreview + previewDur) / timeToPreview);
                }

                timePassed += (float)time;

            }
        }
    }
}
