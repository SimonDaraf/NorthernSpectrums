using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.AudioComponents;
using NorthernSpectrums.MVVM.Model.Matrices;

namespace NorthernSpectrums.MVVM.Model.Audio.AudioModules
{
    /// <summary>
    /// <c>Class</c> Represents a multi-channel diffuser.
    /// </summary>
    public class DiffusionModule : ISampleProvider
    {
        private int channels;
        private int delayMs;
        private float decay;
        private float level;
        private float[] diffusionSamples;
        private bool[] flipPolarity;
        private Fifo[] diffusionStorage;
        private readonly Random rnd;

        /// <summary>
        /// <c>Property</c> The amount of channels to be used in diffusion step.
        /// </summary>
        public int Channels
        {
            get => channels;
            set
            {
                channels = value;
                Reconfigure();
            }
        }

        /// <summary>
        /// <c>Property</c> The amount of delay in ms to be used.
        /// </summary>
        public int DelayMs
        {
            get => delayMs;
            set
            {
                delayMs = value;
                Reconfigure();
            }
        }

        /// <summary>
        /// <c>Property</c> The amount of decay the diffused signal has.
        /// </summary>
        public float Decay
        {
            get => decay;
            set
            {
                decay = value;
            }
        }

        /// <summary>
        /// <c>Property</c> 
        /// </summary>
        public float Level
        {
            get => level;
            set
            {
                level = value;
            }
        }

        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the DiffusionModule class.
        /// Utilizes a multi-channel diffuser, before mixing the channels down to mono again.
        /// </summary>
        /// <param name="channels">Amount of channels.</param>
        /// <param name="delayMs">Desired delay in ms.</param>
        /// <param name="format">The wave format to be used.</param>
        public DiffusionModule(int channels, int delayMs, float decay, float level, WaveFormat format)
        {
            this.channels = channels;
            this.delayMs = delayMs;
            this.decay = decay;
            this.level = level;
            WaveFormat = format;
            rnd = new Random();

            // This has to be done twice to avoid null warnings...
            diffusionSamples = new float[channels];
            diffusionStorage = new Fifo[channels];
            flipPolarity = new bool[channels];

            for (int i = 0; i < diffusionStorage.Length; i++)
            {
                diffusionStorage[i] = new Fifo();
            }

            Reconfigure();
        }

        /// <summary>
        /// <c>Method</c> Reconfigures the diffusion module.
        /// </summary>
        private void Reconfigure()
        {
            int size = CalculateFifoSize(delayMs);

            for (int i = 0; i < channels; i++)
            {
                // Calculate the delay in segments up to specified delay.
                // This should make sure that the delay isn't the same for every channel.
                int rangeLow = (int)(size * ((i + 1f) / channels));
                int rangeHigh = (int)((size + 1) * ((i + 1f) / channels));

                int rndSize = rnd.Next(rangeLow, rangeHigh);

                // Reconfigure our fifo storage.
                diffusionStorage[i].Reconfigure(rndSize + 1, rndSize);

                flipPolarity[i] = rnd.Next(0,20) % 2 == 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Returns the calculated size of the fifo storage based on specified delay in ms.
        /// </summary>
        /// <returns>The size.</returns>
        private int CalculateFifoSize(int delayMs)
        {
            return (int)(delayMs * 0.001 * WaveFormat.SampleRate);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Array.Fill(diffusionSamples, buffer[i]); // Multiply sample over N-channels.

                // Delay Step
                for (int z = 0; z < channels; z++)
                {
                    diffusionSamples[z] = diffusionStorage[z].Pop(); // First extract curent sample stored.

                    float sampleToBeDelayed = buffer[i] + (diffusionSamples[z] * decay);

                    diffusionStorage[z].Push(sampleToBeDelayed);
                }

                // Apply an Orthogonal matrix (Hadamard) to preserve input energy, also scatters samples over every channel.
                HadamardMatrix.InPlace(diffusionSamples);

                // Mix down to mono.
                float diffusedSignal = 0;
                float scaling = 1f / channels;

                for (int x = 0; x < channels; x++)
                {
                    if (flipPolarity[x])
                    {
                        diffusionSamples[x] *= -1;
                    }

                    // Scaling is applied to not overamplify after mixing.
                    diffusedSignal += diffusionSamples[x] * scaling;
                }

                buffer[i] = (buffer[i] * (1 - level)) + (diffusedSignal * level);
            }

            return count;
        }

        /// <summary>
        /// <c>Method</c> Instead of directly modifying the buffer signal.
        /// This returns a new array of only diffused samples.
        /// </summary>
        /// <param name="buffer">The buffer data, length needs to be equal to number of channels.</param>
        /// <returns>An array of diffused samples.</returns>
        public float[] Read(float[] buffer)
        {
            if (buffer.Length != channels)
            {
                return []; // In case the channels doesn't match.
            }

            float[] diffusedSamples = new float[buffer.Length];

            for (int i = 0; i < channels; i++)
            {
                // Pop and push samples into fifo storage.
                diffusionStorage[i].Push(buffer[i]);
                diffusedSamples[i] = diffusionStorage[i].Pop();
            }

            // Apply an Orthogonal matrix (Hadamard) to preserve input energy, also scatters samples over every channel.
            HadamardMatrix.InPlace(diffusedSamples);

            // Flip polarity
            for (int x = 0; x < channels; x++)
            {
                if (flipPolarity[x])
                {
                    diffusionSamples[x] *= -1;
                }
            }

            return diffusedSamples;
        }
    }
}
