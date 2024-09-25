using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.AudioComponents;

namespace NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Delay
{
    /// <summary>
    /// <c>Class</c> Represents a delay pedal provider.
    /// </summary>
    public class DelayProvider : ISampleProvider, IEffectsProvider, IDelayProvider
    {
        private int bpm = 120;
        private float noteFraction = 1;
        private float level = 0.50f;
        private float feedback = 0.50f;
        private int delaySamples;

        private readonly Fifo fifo;
        private ISampleProvider? source;
        private WaveFormat format;

        public WaveFormat WaveFormat
        {
            get => format;
            set
            {
                format = value;
            }
        }

        public ISampleProvider? SourceProvider
        {
            get => source;
            set
            {
                source = value;
            }
        }

        public int Bpm
        {
            get => bpm;
            set
            {
                bpm = value;
                Configure();
            }
        }
        public float NoteFraction
        {
            get => noteFraction;
            set
            {
                noteFraction = value;
                Configure();
            }
        }
        public float Level
        {
            get => level;
            set
            {
                level = value;
            }
        }
        public float Feedback
        {
            get => feedback;
            set
            {
                feedback = value;
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the DelayProvider.
        /// </summary>
        public DelayProvider()
        {
            format = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            fifo = new Fifo();
            Configure();
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the DelayProvider.
        /// </summary>
        /// <param name="waveFormat">The wave format.</param>
        public DelayProvider(WaveFormat waveFormat)
        {
            format = waveFormat;
            fifo = new Fifo();
            Configure();
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the DelayProvider.
        /// </summary>
        /// <param name="waveFormat">The wave format.</param>
        /// <param name="sourceProvider">The input source to read from.</param>
        public DelayProvider(WaveFormat waveFormat, ISampleProvider sourceProvider)
        {
            format = waveFormat;
            source = sourceProvider;
            fifo = new Fifo();
            Configure();
        }

        /// <summary>
        /// <c>Method</c> Configures the delay.
        /// </summary>
        private void Configure()
        {
            float delayMs = 60000 / (bpm / noteFraction); // One minute in ms, divided by bpm and note fraction (1 => quarter, 0.5 => eight etc).
            delaySamples = (int)(delayMs * 0.001 * WaveFormat.SampleRate); // The amount of samples to store.

            fifo.Reconfigure(delaySamples + 1, delaySamples); // Reconfigure fifo data storage.
        }

        public int Read(float[] buffer, int offset, int count)
        {
            source?.Read(buffer, offset, count); // Read from source if available.

            for (int i = 0; i < count; i++)
            {
                // Get delayed sample.
                float delayedSample = fifo.Pop();

                // Level should only affect audio played. So we store samples to be delayed separately from what will be modified in the buffer.
                float samplesToBeDelayed = buffer[i] + (delayedSample * feedback);

                // Add delayed sample after feedback and level adjustment.
                buffer[i] = (buffer[i] * (1 - level)) + (delayedSample * feedback * level);

                // Push modified buffer into fifo.
                fifo.Push(samplesToBeDelayed);
            }

            return count;
        }
    }
}
