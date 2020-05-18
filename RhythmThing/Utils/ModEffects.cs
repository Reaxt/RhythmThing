using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Objects;
namespace RhythmThing.Utils
{
      static class ModEffects
    {

        //various values for mods so they can be changed easily
        //tan?
        private static float tanFreq = 2.0f;

        //Cordie
        private static float cordieSub = 3.5f;
        //bumpy
        private static float bumpyFreq = 2.0f;
        //afterimage
        private static float afterimageFreq = 40;
        private static float afterimageExpo = 75;
        //beat
        private static float beatModAccelTime = 0.2f;
        private static float beatModTotalTime = 0.5f;
        //if over beatBPMCap, it will be treated as if we are at this value 
        private static float beatBPMCap = 150f;
        //amplitude and freq for the beat sin
        private static float beatAmplitude = 10f;
        private static float beatFrequency = 2f;
        /// <summary>
        /// Create a new mods dictionary for a receiver.
        /// </summary>
        public static Dictionary<string, float> getNewModsDictionary()
        {
            Dictionary<string, float> mods = new Dictionary<string, float>();
            mods.Add("bumpy", 0);
            mods.Add("wave", 0);
            mods.Add("beat", 0);
            mods.Add("tan", 0);
            mods.Add("dAVE", 0);
            mods.Add("cordie", 0);
            mods.Add("afterimageX", 0);
            mods.Add("afterimageY", 0);
            return mods;
        }
        //this class is used to calculate all the mod effects on receivers and arrows.
        
            /// <summary>
            /// Calculate the X mod offset
            /// </summary>
            /// <param name="mods">The current mods dictionary</param>
            /// <param name="percent">The arrow's percent value</param>
            /// <returns></returns>
        public static int CalculateArrowX(Dictionary<string, float> mods, float percent)
        {
            int modOffset = 0;
            //simpler mods 
            modOffset += (int)(mods["bumpy"] * Math.Sin(percent * bumpyFreq * Math.PI * 1));
            //Dave is scary. A visual of daves function can be seen here: https://awau.moe/82R54Vv.png . I do not understand why dave exists. Dave scares me. There are no constants for dave because I dont fucking understand it. It is capitalised weirdly to discourage people using it in charts.
            //I will not remove dave.
            modOffset += (int)(mods["dAVE"] * Math.Sin((Math.Pow((-2), (int)(percent * 10)))));
            //Cordie is less scary. Still pretty illegal https://awau.moe/31wUcsL.png
            modOffset += (int)(mods["cordie"] * Math.Pow(Math.Sin(percent * Math.Tan((Math.Tanh(percent-cordieSub) + 0.5) * 100)), 3));
            //afterimage kinda makes a glitchy like effect. Originally tried to make a mountain, but led to this instead https://awau.moe/4FqJgsr.png
            modOffset += (int)(mods["afterimageX"] * Math.Pow(Math.Sin(percent * afterimageFreq), afterimageExpo));


            //(smh Nytlaz your comments make my comments look bad >:c) -Reaxt
            //beat! Mod code ported from OpenITG
            float beatModAccelTime = 0.2f;
            float beatModTotalTime = 0.5f;
            // Slow it down if the song is too fast, otherwise things look wrong
            float beatModDivRate = Math.Max(1.0f, (float)(Math.Truncate(Chart.instance.chartInfo.bpm / beatBPMCap)));

            // Speed up the time the beat occurs over, otherwise it starts to overlap
            beatModAccelTime /= beatModDivRate;
            beatModTotalTime /= beatModDivRate;

            float beatModBeat = Chart.instance.beat;
            beatModBeat /= beatModDivRate;

            // False if the beat is even, true if beat is odd
            bool beatModEvenBeat = ((int)(beatModBeat) % 2) != 0;

            // Only start if we're slightly past the start of the beat
            if (beatModBeat >= 0)
            {
                // Get the fractional component of the beat, and take absolute value
                beatModBeat -= (int)Math.Truncate(beatModBeat);
                beatModBeat += 1;
                beatModBeat -= (int)Math.Truncate(beatModBeat);

                // Check to make sure we haven't finished the mod calculation for this beat yet
                if (beatModBeat < beatModTotalTime)
                {
                    float beatModAmount;

                    // If we haven't finished the startup acceleration, do that scaling first
                    if (beatModBeat < beatModAccelTime)
                    {
                        // Scale the amount to the time we accelerate outwards
                        beatModAmount = beatModBeat / beatModAccelTime;
                        beatModAmount *= beatModAmount;
                    }
                    else
                    {
                        // Scale the amount to the time we accelerate backwards
                        beatModAmount = ((beatModBeat - beatModAccelTime) * (0.0f - 1.0f) / (beatModTotalTime - beatModAccelTime)) + 1.0f;
                        // Invert and square beatmodamount
                        beatModAmount = 1 - (1 - beatModAmount) * (1 - beatModAmount);
                    }

                    if (beatModEvenBeat)
                    {
                        // Go the other way on even beats
                        beatModAmount *= -1;
                    }

                    // Use the amount to scale a fast sin wave, so things beat 
                    // back and forth differently depending on the kind of note.
                    float beatModShift = beatAmplitude * beatModAmount * (float)Math.Sin(percent * beatFrequency * Math.PI + Math.PI / 2.0f);

                    // We're done!
                    modOffset += (int)(mods["beat"] * beatModShift);
                }
            }

            //return the value
            return modOffset;
        }
        /// <summary>
        /// Calculate Y mod offset
        /// </summary>
        /// <param name="mods">The mods dictionary</param>
        /// <param name="percent">The arrows percent</param>
        /// <returns></returns>
        public static int CalculateArrowY(Dictionary<string, float> mods, float percent)
        {
            int modOffset = 0;
            //calculate wave!
            modOffset += (int)(mods["wave"] * 3 * Math.Cos(percent * 2 * Math.PI * 2));
            modOffset += (int)(mods["tan"] * Math.Tan(percent * tanFreq * Math.PI * 1));
            modOffset += (int)(mods["afterimageY"] * Math.Pow(Math.Sin(percent * 40), 75));

            return modOffset;
        }

        /// <summary>
        /// Calculate the X Receiver mod offset
        /// </summary>
        /// <param name="mods">The mods dictionary</param>
        /// <param name="percent">The arrows percent</param>
        /// <returns></returns>
        public static int CalculateReceiverX(Dictionary<string, float> mods)
        {
            int modOffset = 0;
            //beat! Mod code ported from OpenITG

            // Slow it down if the song is too fast, otherwise things look wrong
            float beatModDivRate = Math.Max(1.0f, (float)(Math.Truncate(Chart.instance.chartInfo.bpm / beatBPMCap)));

            // Speed up the time the beat occurs over, otherwise it starts to overlap
            beatModAccelTime /= beatModDivRate;
            beatModTotalTime /= beatModDivRate;

            float beatModBeat = Chart.instance.beat;
            beatModBeat /= beatModDivRate;

            // False if the beat is even, true if beat is odd
            bool beatModEvenBeat = ((int)(beatModBeat) % 2) != 0;

            // Only start if we're slightly past the start of the beat
            if (beatModBeat >= 0)
            {
                // Get the fractional component of the beat, and take absolute value
                beatModBeat -= (int)Math.Truncate(beatModBeat);
                beatModBeat += 1;
                beatModBeat -= (int)Math.Truncate(beatModBeat);

                // Check to make sure we haven't finished the mod calculation for this beat yet
                if (beatModBeat < beatModTotalTime)
                {
                    float beatModAmount;

                    // If we haven't finished the startup acceleration, do that scaling first
                    if (beatModBeat < beatModAccelTime)
                    {
                        // Scale the amount to the time we accelerate outwards
                        beatModAmount = beatModBeat / beatModAccelTime;
                        beatModAmount *= beatModAmount;
                    }
                    else
                    {
                        // Scale the amount to the time we accelerate backwards
                        beatModAmount = ((beatModBeat - beatModAccelTime) * (0.0f - 1.0f) / (beatModTotalTime - beatModAccelTime)) + 1.0f;
                        // Invert and square beatmodamount
                        beatModAmount = 1 - (1 - beatModAmount) * (1 - beatModAmount);
                    }

                    if (beatModEvenBeat)
                    {
                        // Go the other way on even beats
                        beatModAmount *= -1;
                    }

                    // Use the amount to scale a fast sin wave, so things beat 
                    // back and forth differently depending on the kind of note.
                    float beatModShift = beatAmplitude * beatModAmount;

                    // We're done!
                    modOffset += (int)(mods["beat"] * beatModShift);
                }
            }

            return modOffset;
        }
        /// <summary>
        /// Calculate the receiver Y offset
        /// </summary>
        /// <param name="mods">The mods dictionary</param>
        /// <returns></returns>
        public static int CalculateReceiverY(Dictionary<string, float> mods)
        {
            int modOffset = 0;
            //nothing here yet :c

            return modOffset;
        }

    }
}
