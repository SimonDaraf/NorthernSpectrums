using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Volume
{
    /// <summary>
    /// <c>Class</c> The VolumeProvider class allows volume modification in a signal chain.
    /// </summary>
    public class VolumeProvider : ISampleProvider, IEffectsProvider, IVolumeProvider
    {
        private readonly WaveFormat waveFormat;
        private float volume = 1;
        private ISampleProvider? sourceProvider;

        public float Volume
        {
            get => volume;
            set
            {
                volume = Math.Min(1, Math.Max(0, value)); // Clamp value to between 0 and 1.
            }
        }

        public WaveFormat WaveFormat
        {
            get => waveFormat;
        }
        public ISampleProvider? SourceProvider
        {
            set
            {
                sourceProvider = value;
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the VolumeProvider class.
        /// </summary>
        public VolumeProvider()
        {
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the VolumeProvider class. With a provided source.
        /// </summary>
        /// <param name="source">The source to read from.</param>
        public VolumeProvider(ISampleProvider source)
        {
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            sourceProvider = source;
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the VolumeProvider class. With a provided source and wave format.
        /// </summary>
        /// <param name="source">The source to read from.</param>
        /// <param name="format">The wave format. Should be a IEEE 32 bit wave format.</param>
        public VolumeProvider(ISampleProvider source, WaveFormat format)
        {
            waveFormat = format;
            sourceProvider = source;
        }

        // This is an example originally written by the NAudio creator Mark Heath.
        public int Read(float[] buffer, int offset, int count)
        {
            // Only read if source isn't null.
            sourceProvider?.Read(buffer, offset, count);
            if (volume != 1f)
            {
                for (int i = 0; i < count; i++)
                {
                    buffer[offset + i] *= volume;
                }
            }

            return count;
        }
    }
}
