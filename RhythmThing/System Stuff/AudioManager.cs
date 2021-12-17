using System;
using System.Collections.Generic;
using System.Text;
using CSCore;
using CSCore.Codecs;
using CSCore.DSP;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;
using System.IO;
namespace RhythmThing.System_Stuff
{

    /*
     * This class is of my own design. I made it for another game project of mine.
     * Its purpose is to allow easy handling of audio without needing to worry about it again throughout development outside of file names.
     * It supports tracks, a class to access audio streams after they have already started and use them with various CSCore funcs and properties
     * It also supports "firing" of audio, meaning that it plays a file once and completely forgets about it outside of its own scope.
     */
    public class AudioManager
    {
        public const int sampleRate = 44100;
        const int LATENCY = 1;
        private Mixer _mixer;
        public List<AudioTrack> Tracks;
        WasapiOut soundOut;
        public AudioManager()
        {
            _mixer = new Mixer(2, sampleRate) { FillWithZeros = true, DivideResult = false };
            Tracks = new List<AudioTrack>();
            soundOut = new WasapiOut() { Latency = LATENCY };
            soundOut.Initialize(_mixer.ToWaveSource());
            soundOut.Volume = 0.45f;
            soundOut.Play();
        }

        public AudioTrack addTrack(string path)
        {
            string dir = Path.Combine(PlayerSettings.GetExeDir(), "!Content", path);
            VolumeSource tempvol;
            //this class is no longer available to me :( no pitch down on fail.
            //PitchShifter shifer;
            ISampleSource temp = CodecFactory.Instance.GetCodec(dir).ChangeSampleRate(sampleRate).ToStereo().ToSampleSource().AppendSource(x => new VolumeSource(x), out tempvol);
            AudioTrack track = new AudioTrack(path, temp, tempvol);


            _mixer.AddSource(track.sampleSource);
            Tracks.Add(track);
            return track;

        }
        public void addTrack(AudioTrack track)
        {
            _mixer.AddSource(track.sampleSource);
            Tracks.Add(track);
        }

        public void removeTrack(AudioTrack track)
        {
            if(track != null)
            {
                _mixer.RemoveSource(track.sampleSource);
                Tracks.Remove(track);

            }

        }

        public void removeTrack(string name)
        {
            try
            {
                AudioTrack temp = Tracks.Find(x => x.name == name);
                _mixer.RemoveSource(temp.sampleSource);
                Tracks.Remove(temp);

            }
            catch
            {

                Console.WriteLine($"cant remove {name}");
            }
        }

        public void playForget(string path, float vol)
        {
            string dir = Path.Combine(PlayerSettings.GetExeDir(), "!Content", path);
            VolumeSource tempvol;
            ISampleSource temp = CodecFactory.Instance.GetCodec(dir).ChangeSampleRate(sampleRate).ToStereo().ToSampleSource().AppendSource(x => new VolumeSource(x), out tempvol);
            tempvol.Volume = vol;
            _mixer.AddSource(temp);
        }

        public void playForget(string path, float vol, float pitch)
        {
            string dir = Path.Combine(PlayerSettings.GetExeDir(), "!Content", path);
            VolumeSource tempvol;

            //PitchShifter temppitch;
            ISampleSource temp = CodecFactory.Instance.GetCodec(dir).ChangeSampleRate(sampleRate).ToStereo().ToSampleSource().AppendSource(x => new VolumeSource(x), out tempvol);
            tempvol.Volume = vol;
            //temppitch.PitchShiftFactor = pitch;
            _mixer.AddSource(temp);


        }

        public void playForget(string path)
        {
            string dir = Path.Combine(PlayerSettings.GetExeDir(), "!Content", path);

            ISampleSource temp = CodecFactory.Instance.GetCodec(dir).ChangeSampleRate(sampleRate).ToStereo().ToSampleSource();

            _mixer.AddSource(temp);
        }

        public void playForget(ISampleSource source)
        {

            _mixer.AddSource(source);
        }

    }


    public class AudioTrack
    {
        public AudioTrack(string name, ISampleSource sample, VolumeSource volume)
        {
            this.name = name;
            this.sampleSource = sample;
            this.volumeSource = volume;
            //this.shifter = shifter;
        }
        public string name;
        public ISampleSource sampleSource;
        public VolumeSource volumeSource;
        //public PitchShifter shifter;

    }
}

