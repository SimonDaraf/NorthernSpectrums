using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.AudioModules;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.MVVM.Model.Audio.AmpProviders.NSThreader
{
    /// <summary>
    /// <c>Class</c> Represents the NS Threader amplifier.
    /// </summary>
    public class NSThreaderProvider : ISampleProvider, IEffectsProvider, INSThreaderProvider
    {
        private ISampleProvider? sourceProvider;
        private WaveFormat waveFormat;

        // Audio modules.
        private readonly AmplificationModule amplificationModule;
        private readonly AmplificationModule masterAmplification;
        private readonly PhaseInvertionModule phaseInvertionModule = new PhaseInvertionModule();
        private readonly ColdClipperModule negativeColdClipper = new ColdClipperModule(-0.03f);
        private readonly ColdClipperModule positiveColdClipper = new ColdClipperModule(0.45f);
        private readonly DiffusionModule diffusionModule1;
        private readonly DiffusionModule diffusionModule2;
        private readonly DiffusionModule diffusionModule3;
        private readonly DiffusionModule diffusionModule4;
        private readonly PeakingEQModule bassModule;
        private readonly PeakingEQModule middleModule;
        private readonly PeakingEQModule trebleModule;

        private float gain = 6;
        private float bassGain = 0;
        private float middleGain = 0;
        private float trebleGain = 0;
        private float masterGain = 1;

        public ISampleProvider? SourceProvider
        {
            get => sourceProvider;
            set => sourceProvider = value;
        }

        public WaveFormat WaveFormat
        {
            get => waveFormat;
            set => waveFormat = value;
        }

        public float Gain
        {
            get => gain;
            set
            {
                gain = value;
                amplificationModule.Gain = value;
            }
        }
        public float BassGain
        {
            get => bassGain;
            set
            {
                bassGain = value;
                bassModule.Gain = value;
            }
        }
        public float MiddleGain
        {
            get => middleGain;
            set
            {
                middleGain = value;
                middleModule.Gain = value;
            }
        }
        public float TrebleGain
        {
            get => trebleGain;
            set
            {
                trebleGain = value;
                trebleModule.Gain = value;
            }
        }
        public float MasterGain
        {
            get => masterGain;
            set
            {
                masterGain = value;
                masterAmplification.Gain = value;
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the NSThreaderProvider.
        /// </summary>
        public NSThreaderProvider()
        {
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

            amplificationModule = new AmplificationModule(gain);
            masterAmplification = new AmplificationModule(masterGain);

            bassModule = new PeakingEQModule(waveFormat, 100, bassGain, 1.4f);
            middleModule = new PeakingEQModule(waveFormat, 1500, middleGain, 1.4f);
            trebleModule = new PeakingEQModule(waveFormat, 4000, trebleGain, 1.4f);

            diffusionModule1 = new DiffusionModule(8, 48, 0.2f, 0.2f, waveFormat);
            diffusionModule2 = new DiffusionModule(8, 72, 0.2f, 0.2f, waveFormat);
            diffusionModule3 = new DiffusionModule(8, 96, 0.3f, 0.3f, waveFormat);
            diffusionModule4 = new DiffusionModule(8, 120, 0.4f, 0.3f, waveFormat);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            sourceProvider?.Read(buffer, offset, count); // If available first read from source.

            // Amplify signal.
            amplificationModule.Read(buffer, offset, count);

            //// Invert signal.
            phaseInvertionModule.Read(buffer, offset, count);

            diffusionModule1.Read(buffer, offset, count);
            diffusionModule2.Read(buffer, offset, count);
            diffusionModule3.Read(buffer, offset, count);
            diffusionModule4.Read(buffer, offset, count);

            //// Read from EQ.
            bassModule.Read(buffer, offset, count);
            middleModule.Read(buffer, offset, count);
            trebleModule.Read(buffer, offset, count);

            //// Perform an asymmetrical clipping.
            negativeColdClipper.Read(buffer, offset, count);
            positiveColdClipper.Read(buffer, offset, count);

            masterAmplification.Read(buffer, offset, count);

            return count;
        }
    }
}
