﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Utils
{
    /// <summary>
    /// Grabbed from https://gist.github.com/adrianseeley/4242677 . I take no credit for the following code.
    /// </summary>
    public static class Ease
        {
        public static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        /// <summary>
        /// Given two parallel arrays, lerp
        /// </summary>
        /// <param name="firstPos">first parallel array</param>
        /// <param name="secondPos">second parallel array</param>
        /// <param name="by">amount to go 0-1</param>
        /// <returns></returns>
        public static float[] Lerp(float[] firstPos, float[] secondPos, float by)
        {
            if (firstPos.Length != secondPos.Length) throw new ArgumentException("The two given arrays are not parallel");
            float[] result = new float[firstPos.Length];
            for (int i = 0; i < firstPos.Length; i++)
            {
                result[i] = firstPos[i] * (1 - by) + secondPos[i] * by;
            }
            return result;
        }
        /// <summary>
        /// Maps the names found at https://easings.net/en to the matching easing functions.
        /// Maps "easeLinear" to linear easing (x => x).
        /// </summary>
        public static Dictionary<string, Func<float, float>> byName = new Dictionary<string, Func<float, float>>
        {
            { "easeLinear", Linear },
            { "easeInQuad", Quadratic.In },
            { "easeOutQuad", Quadratic.Out },
            { "easeInOutQuad", Quadratic.InOut },
            { "easeInCubic", Cubic.In },
            { "easeOutCubic", Cubic.Out },
            { "easeInOutCubic", Cubic.InOut },
            { "easeInQuart", Quartic.In },
            { "easeOutQuart", Quartic.Out },
            { "easeInOutQuart", Quartic.InOut },
            { "easeInQuint", Quintic.In },
            { "easeOutQuint", Quintic.Out },
            { "easeInOutQuint", Quintic.InOut },
            { "easeInSine", Sinusoidal.In },
            { "easeOutSine", Sinusoidal.Out },
            { "easeInOutSine", Sinusoidal.InOut },
            { "easeInExpo", Exponential.In },
            { "easeOutExpo", Exponential.Out },
            { "easeInOutExpo", Exponential.InOut },
            { "easeInCirc", Circular.In },
            { "easeOutCirc", Circular.Out },
            { "easeInOutCirc", Circular.InOut },
            { "easeInBack", Back.In },
            { "easeOutBack", Back.Out },
            { "easeInOutBack", Back.InOut },
            { "easeInElastic", Elastic.In },
            { "easeOutElastic", Elastic.Out },
            { "easeInOutElastic", Elastic.InOut },
            { "easeInBounce", Bounce.In },
            { "easeOutBounce", Bounce.Out },
            { "easeInOutBounce", Bounce.InOut },
        };

        /// <summary>
        /// If an easing named <paramref name="name"/> exists, returns it.
        /// Otherwise, returns <see cref="Linear(float)"/>.
        /// <seealso cref="byName"/>
        /// </summary>
        /// <param name="name">The name of the desired easing.</param>
        /// <returns>The desired easing, or <see cref="Linear(float)"/> if that easing doesn't exist.</returns>
        public static Func<float, float> named(string name)
        {
            if (byName.TryGetValue(name, out var easing))
            {
                return easing;
            }
            return Linear;
        }

        public static float Linear(float k)
        {
            return k;
        }

        public class Quadratic
        {
            public static float In(float k)
            {
                return k * k;
            }

            public static float Out(float k)
            {
                return k * (2f - k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k;
                return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
            }
        };

        public class Cubic
        {
            public static float In(float k)
            {
                return k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k;
                return 0.5f * ((k -= 2f) * k * k + 2f);
            }
        };

        public class Quartic
        {
            public static float In(float k)
            {
                return k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f - ((k -= 1f) * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
                return -0.5f * ((k -= 2f) * k * k * k - 2f);
            }
        };

        public class Quintic
        {
            public static float In(float k)
            {
                return k * k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
                return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
            }
        };

        public class Sinusoidal
        {
            public static float In(float k)
            {
                return 1f - (float)Math.Cos(k * (float)Math.PI / 2f);
            }

            public static float Out(float k)
            {
                return (float)Math.Sin(k * (float)Math.PI / 2f);
            }

            public static float InOut(float k)
            {
                return 0.5f * (1f - (float)Math.Cos((float)Math.PI * k));
            }
        };

        public class Exponential
        {
            public static float In(float k)
            {
                return k == 0f ? 0f : (float)Math.Pow(1024f, k - 1f);
            }

            public static float Out(float k)
            {
                return k == 1f ? 1f : 1f - (float)Math.Pow(2f, -10f * k);
            }

            public static float InOut(float k)
            {
                if (k == 0f) return 0f;
                if (k == 1f) return 1f;
                if ((k *= 2f) < 1f) return 0.5f * (float)Math.Pow(1024f, k - 1f);
                return 0.5f * (-(float)Math.Pow(2f, -10f * (k - 1f)) + 2f);
            }
        };

        public class Circular
        {
            public static float In(float k)
            {
                return 1f - (float)Math.Sqrt(1f - k * k);
            }

            public static float Out(float k)
            {
                return (float)Math.Sqrt(1f - ((k -= 1f) * k));
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * ((float)Math.Sqrt(1f - k * k) - 1);
                return 0.5f * ((float)Math.Sqrt(1f - (k -= 2f) * k) + 1f);
            }
        };

        public class Elastic
        {
            public static float In(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return -(float)Math.Pow(2f, 10f * (k -= 1f)) * (float)Math.Sin((k - 0.1f) * (2f * (float)Math.PI) / 0.4f);
            }

            public static float Out(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return (float)Math.Pow(2f, -10f * k) * (float)Math.Sin((k - 0.1f) * (2f * (float)Math.PI) / 0.4f) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * (float)Math.Pow(2f, 10f * (k -= 1f)) * (float)Math.Sin((k - 0.1f) * (2f * (float)Math.PI) / 0.4f);
                return (float)Math.Pow(2f, -10f * (k -= 1f)) * (float)Math.Sin((k - 0.1f) * (2f * (float)Math.PI) / 0.4f) * 0.5f + 1f;
            }
        };

        public class Back
        {
            static float s = 1.70158f;
            static float s2 = 2.5949095f;

            public static float In(float k)
            {
                return k * k * ((s + 1f) * k - s);
            }

            public static float Out(float k)
            {
                return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
                return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
            }
        };

        public class Bounce
        {
            public static float In(float k)
            {
                return 1f - Out(1f - k);
            }

            public static float Out(float k)
            {
                if (k < (1f / 2.75f))
                {
                    return 7.5625f * k * k;
                }
                else if (k < (2f / 2.75f))
                {
                    return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
                }
                else if (k < (2.5f / 2.75f))
                {
                    return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
                }
                else
                {
                    return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
                }
            }

            public static float InOut(float k)
            {
                if (k < 0.5f) return In(k * 2f) * 0.5f;
                return Out(k * 2f - 1f) * 0.5f + 0.5f;
            }
        };

    }
    }

