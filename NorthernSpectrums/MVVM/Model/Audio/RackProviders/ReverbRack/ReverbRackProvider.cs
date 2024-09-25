using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.AudioModules;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.MVVM.Model.Audio.RackProviders.ReverbRack
{
    /// <summary>
    /// <c>Class</c> Represents a reverb rack.
    /// </summary>
    public class ReverbRackProvider : ISampleProvider, IEffectsProvider, IReverbRackProvider
    {
        private ISampleProvider? sourceProvider;
        private WaveFormat waveFormat;

        // The amount of diffusion steps.
        private int stepCount = 4;

        private float level = 0.5f;
        private float decay = 5f;
        private int delayMs = 147;

        private readonly int channels = 8;

        // Audio modules.
        private readonly DiffusionModule[] diffusionSteps;
        private readonly MultiChannelFeedbackModule multiChannelFeedbackModule;

        public float Level
        {
            get => level;
            set
            {
                level = value;
                Reconfigure();
            }
        }
        public float Decay
        {
            get => decay;
            set
            {
                decay = value;
                Reconfigure();
            }
        }
        public int DelayMs
        {
            get => delayMs;
            set
            {
                delayMs = value;
                multiChannelFeedbackModule.DelayMs = value;
                Reconfigure();
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
            set
            {
                waveFormat = value;
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of a reverb rack.
        /// </summary>
        public ReverbRackProvider()
        {
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

            int diffuseHalfTime = (int)(delayMs * 0.5f);
            diffusionSteps = new DiffusionModule[channels];

            for (int i = 0; i < channels; i++)
            {
                diffusionSteps[i] = new DiffusionModule(8, diffuseHalfTime, 0.5f, level, waveFormat);
            }

            multiChannelFeedbackModule = new MultiChannelFeedbackModule(8, level, delayMs, decay, waveFormat);
        }

        /// <summary>
        /// <c>Method</c> Reconfigures every diffusion module.
        /// </summary>
        private void Reconfigure()
        {
            for (int i = 0; i < channels; i++)
            {
                diffusionSteps[i].Level = level;
                diffusionSteps[i].DelayMs = (int)(delayMs * 0.5f);
            }

            multiChannelFeedbackModule.Rt60Decay = decay;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            sourceProvider?.Read(buffer, offset, count);

            for (int i = 0; i < count; i++)
            {
                float[] multiChannelSamples = new float[channels];
                Array.Fill(multiChannelSamples, buffer[i]);

                // Diffuse the samples x times, x is the amount of diffusion steps.
                for (int x = 0; x < stepCount; x++)
                {
                    multiChannelSamples = diffusionSteps[x].Read(multiChannelSamples);
                }

                float[] feedbackSamples = multiChannelFeedbackModule.Read(multiChannelSamples);

                // Mix to mono.
                float mixedSamples = 0;
                float scaling = 1f / (channels / 2);
                for (int x = 0; x < channels / 2; x++)
                {
                    mixedSamples += (feedbackSamples[x] + feedbackSamples[1 + (channels / 2)]) * scaling;
                }

                // Adjust level.
                buffer[i] = (buffer[i] * (1 - level)) + (mixedSamples * level);
            }

            return count;
        }
    }
}
