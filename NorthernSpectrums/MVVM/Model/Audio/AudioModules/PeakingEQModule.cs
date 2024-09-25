using NAudio.Dsp;
using NAudio.Wave;

namespace NorthernSpectrums.MVVM.Model.Audio.AudioModules
{
    public class PeakingEQModule : ISampleProvider
    {
        private BiQuadFilter filter;
        private WaveFormat waveFormat;
        private int frequency;
        private float gain;
        private float qFactor;

        /// <summary>
        /// <c>Property</c> The wave format to be used.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get => waveFormat;
            set
            {
                waveFormat = value;
                UpdateFilter();
            }
        }

        /// <summary>
        /// <c>Property</c> The target frequency to be modified.
        /// </summary>
        public int Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                UpdateFilter();
            }
        }

        /// <summary>
        /// <c>Property</c> The gain to be applied to target frequency.
        /// </summary>
        public float Gain
        {
            get => gain;
            set
            {
                gain = value;
                UpdateFilter();
            }
        }

        /// <summary>
        /// <c>Property</c> The Q factor to be used.
        /// </summary>
        public float QFactor
        {
            get => qFactor;
            set
            {
                qFactor = value;
                UpdateFilter();
            }
        }

        /// <summary>
        /// <c>Constructor</c>
        /// </summary>
        public PeakingEQModule(WaveFormat format, int frequency, float gain, float qFactor)
        {
            waveFormat = format;
            this.frequency = frequency;
            this.gain = gain;
            this.qFactor = qFactor;
            filter = BiQuadFilter.PeakingEQ(format.SampleRate, frequency, qFactor, gain);
        }

        /// <summary>
        /// <c>Method</c> Updates the filter after a change has been made.
        /// </summary>
        private void UpdateFilter()
        {
            filter.SetPeakingEq(waveFormat.SampleRate, frequency, qFactor, gain);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i] = filter.Transform(buffer[i]);
            }

            return count;
        }
    }
}
