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
        const int latency = 1;
        Mixer mixer;
        public List<AudioTrack> tracks;
        WasapiOut soundOut;
        public AudioManager()
        {
            mixer = new Mixer(2, sampleRate) { FillWithZeros = true, DivideResult = false };
            tracks = new List<AudioTrack>();
            soundOut = new WasapiOut() { Latency = latency };

            soundOut.Initialize(mixer.ToWaveSource());
            soundOut.Play();
        }

        public AudioTrack addTrack(string path)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "!Content", path);
            VolumeSource tempvol;
            //this class is no longer available to me :( no pitch down on fail.
            //PitchShifter shifer;
            ISampleSource temp = CodecFactory.Instance.GetCodec(dir).ChangeSampleRate(sampleRate).ToStereo().ToSampleSource().AppendSource(x => new VolumeSource(x), out tempvol);
            AudioTrack track = new AudioTrack(path, temp, tempvol);


            mixer.AddSource(track.sampleSource);
            tracks.Add(track);
            return track;

        }
        public void addTrack(AudioTrack track)
        {
            mixer.AddSource(track.sampleSource);
            tracks.Add(track);
        }

        public void removeTrack(AudioTrack track)
        {
            if(track != null)
            {
                mixer.RemoveSource(track.sampleSource);
                tracks.Remove(track);

            }

        }

        public void removeTrack(string name)
        {
            try
            {
                AudioTrack temp = tracks.Find(x => x.name == name);
                mixer.RemoveSource(temp.sampleSource);
                tracks.Remove(temp);

            }
            catch
            {

                Console.WriteLine($"cant remove {name}");
            }
        }

        public void playForget(string path, float vol)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "!Content", path);
            VolumeSource tempvol;
            ISampleSource temp = CodecFactory.Instance.GetCodec(dir).ChangeSampleRate(sampleRate).ToStereo().ToSampleSource().AppendSource(x => new VolumeSource(x), out tempvol);
            tempvol.Volume = vol;
            mixer.AddSource(temp);
        }

        public void playForget(string path, float vol, float pitch)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "!Content", path);
            VolumeSource tempvol;

            //PitchShifter temppitch;
            ISampleSource temp = CodecFactory.Instance.GetCodec(dir).ChangeSampleRate(sampleRate).ToStereo().ToSampleSource().AppendSource(x => new VolumeSource(x), out tempvol);
            tempvol.Volume = vol;
            //temppitch.PitchShiftFactor = pitch;
            mixer.AddSource(temp);


        }

        public void playForget(string path)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "!Content", path);

            ISampleSource temp = CodecFactory.Instance.GetCodec(dir).ChangeSampleRate(sampleRate).ToStereo().ToSampleSource();

            mixer.AddSource(temp);
        }

        public void playForget(ISampleSource source)
        {

            mixer.AddSource(source);
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

