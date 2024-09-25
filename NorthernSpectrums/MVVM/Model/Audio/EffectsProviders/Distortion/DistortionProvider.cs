using NAudio.Wave;

namespace NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Distortion
{
    /// <summary>
    /// <c>Class</c> Provides a signal clipping distorting the signal.
    /// </summary>
    class DistortionProvider : ISampleProvider, IEffectsProvider, IDistortionProvider
    {
        private float level = 0.075f;
        private float gain = 2f;
        private ISampleProvider? sourceProvider;
        private WaveFormat waveFormat;

        public float Level
        {
            get => level;
            set
            {
                level = value;
            }
        }
        public float Gain
        {
            get => gain;
            set
            {
                gain = value;
            }
        }
        public ISampleProvider? SourceProvider
        {
            set
            {
                sourceProvider = value;
            }
        }

        public WaveFormat WaveFormat
        {
            get => waveFormat;
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the DistortionProvider class.
        /// </summary>
        public DistortionProvider()
        {
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the DistortionProvider class. With a provided source.
        /// </summary>
        public DistortionProvider(ISampleProvider source)
        {
            sourceProvider = source;
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the DistortionProvider class. With a provided source and WaveFormat.
        /// </summary>
        public DistortionProvider(ISampleProvider source, WaveFormat format)
        {
            sourceProvider = source;
            waveFormat = format;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            sourceProvider?.Read(buffer, offset, count);

            for (int i = 0; i < count; i++)
            {
                float sample = buffer[i]; // Get current sample.
                buffer[i] = MathF.Max(MathF.Min(gain * sample, level), -level); // Multiply current sample by the gain value and clamp it to set clip level.
            }

            return count;
        }
    }
}
