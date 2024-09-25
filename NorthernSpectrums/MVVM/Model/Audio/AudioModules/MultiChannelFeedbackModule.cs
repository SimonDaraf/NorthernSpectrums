using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.AudioComponents;
using NorthernSpectrums.MVVM.Model.Matrices;

namespace NorthernSpectrums.MVVM.Model.Audio.AudioModules
{
    /// <summary>
    /// <c>Class</c> Represents a multi channel feedback.
    /// </summary>
    public class MultiChannelFeedbackModule : ISampleProvider
    {
        private int channels;
        private int delayMs;
        private float rt60Decay;
        private readonly Fifo[] feedbackStorage;

        /// <summary>
        /// <c>Property</c> The dry/wet level of the signal.
        /// </summary>
        public float Level { get; set; }

        /// <summary>
        /// <c>Property</c> The delay in ms.
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
        /// <c>Property</c> The Rt60 decay.
        /// </summary>
        public float Rt60Decay
        {
            get => rt60Decay;
            set
            {
                rt60Decay = SetRT60(value);
            }
        }

        public WaveFormat WaveFormat { get; set; }

        /// <summary>
        /// <c>Constructor</c> Constructs a new instance of the multi channel feedback module.
        /// </summary>
        /// <param name="channels">Number of channels.</param>
        /// <param name="delayMs">The delay in ms.</param>
        /// <param name="decay">The amount of decay for each iteration.</param>
        /// <param name="format">The wave format.</param>
        public MultiChannelFeedbackModule(int channels, float level, int delayMs, float rt60Decay, WaveFormat format)
        {
            this.channels = channels;
            Level = level;
            this.delayMs = delayMs;
            Rt60Decay = rt60Decay;
            WaveFormat = format;

            feedbackStorage = new Fifo[channels];

            for (int i = 0; i < feedbackStorage.Length; i++)
            {
                feedbackStorage[i] = new Fifo();
            }

            Reconfigure();
        }

        /// <summary>
        /// <c>Method</c> Calculates the decay based on rt60. That is the time it should take for the signal to decay by 60db in ms.
        /// </summary>
        /// <param name="value">The rt60 decay.</param>
        /// <returns></returns>
        private float SetRT60(float value)
        {
            float loopInMs = delayMs * 1.5f; // time around the feedback loop.

            float loopsPerRt60 = value / (loopInMs * 0.001f); // Times per RT60 period.
            
            float dbPerCycle = -60 / loopsPerRt60; // Amount of db to reduce by per loop.

            return MathF.Pow(10, dbPerCycle * 0.05f);
        }

        /// <summary>
        /// <c>Method</c> Reconfigures the fifo storage.
        /// </summary>
        private void Reconfigure()
        {
            // Get base amount of samples to be delayed.
            int delayedSamples = (int)(delayMs * 0.001 * WaveFormat.SampleRate);

            // Use different delays for each channel.
            for (int i = 0; i < channels; i++)
            {
                float ratio = i * 1f / channels;
                int size = (int)MathF.Pow(2, ratio) * delayedSamples;

                feedbackStorage[i].Reconfigure(size + 1, size);
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                float[] delayedSamples = new float[channels];

                // Delay samples for each channel.
                for (int z = 0; z < channels; z++)
                {
                    delayedSamples[z] = feedbackStorage[z].Pop();

                    float sampleToBeDelayed = buffer[i] + (delayedSamples[z] * rt60Decay);

                    feedbackStorage[z].Push(sampleToBeDelayed);
                }

                // Mix with a householder matrix.
                HouseholderMatrix.InPlace(delayedSamples);

                // Mix to mono.
                float feedbackSignal = 0;
                float scaling = 1f / channels;

                for (int x = 0; x < channels; x++)
                {
                    feedbackSignal += delayedSamples[x] * scaling;
                }

                buffer[i] = (buffer[i] * (1 - Level)) + (feedbackSignal * Level);
            }

            return count;
        }

        /// <summary>
        /// <c>Method</c> Instead of directly modifying the buffer signal.
        /// This returns a new array of only feedback samples.
        /// </summary>
        /// <param name="buffer">The buffer data, length needs to be equal to number of channels.</param>
        /// <returns>An array of feedback samples.</returns>
        public float[] Read(float[] buffer)
        {
            if (buffer.Length != channels)
            {
                return []; // In case the channels doesn't match.
            }

            float[] feedbackSamples = new float[channels];

            for (int i = 0; i < channels; i++)
            {
                // Pop and push samples into fifo storage.
                feedbackSamples[i] = feedbackStorage[i].Pop();
            }

            // Mix with a householder matrix.
            HouseholderMatrix.InPlace(feedbackSamples);

            // Push into fifo storage.
            for (int i = 0; i < channels; i++)
            {
                float sampleToBeDelayed = buffer[i] + (feedbackSamples[i] * rt60Decay);
                feedbackStorage[i].Push(sampleToBeDelayed);
            }

            return feedbackSamples;
        }
    }
}
