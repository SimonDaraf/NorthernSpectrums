using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthernSpectrums.MVVM.Model.Audio.AudioModules
{
    /// <summary>
    /// <c>Class</c> Performs an asymmetrical clipping on the signal.
    /// </summary>
    public class ColdClipperModule : IAudioModule
    {
        /// <summary>
        /// <c>Property</c> The threshold at which after the signal will get clipped.
        /// Negative values will clip the valleys and positive the peaks.
        /// </summary>
        public float Threshold { get; set; }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the cold clipper module.
        /// </summary>
        /// <param name="threshold"></param>
        public ColdClipperModule(float threshold)
        {
            Threshold = threshold;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (Threshold < 0)
                {
                    buffer[i] = Math.Max(buffer[i], Threshold);
                }
                else
                {
                    buffer[i] = MathF.Min(buffer[i], Threshold);
                }
            }

            return count;
        }
    }
}
