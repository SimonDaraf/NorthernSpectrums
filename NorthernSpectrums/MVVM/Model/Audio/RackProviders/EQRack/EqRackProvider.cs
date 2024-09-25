using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.AudioModules;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.MVVM.Model.Audio.RackProviders.EQRack
{
    /// <summary>
    /// <c>Class</c> Provides an interactable 8-band EQ.
    /// </summary>
    public class EqRackProvider : ISampleProvider, IEffectsProvider, IEqRackProvider
    {
        // Audio Modules
        private readonly PeakingEQModule BandOne;
        private readonly PeakingEQModule BandTwo;
        private readonly PeakingEQModule BandThree;
        private readonly PeakingEQModule BandFour;
        private readonly PeakingEQModule BandFive;
        private readonly PeakingEQModule BandSix;
        private readonly PeakingEQModule BandSeven;
        private readonly PeakingEQModule BandEight;

        public ISampleProvider? SourceProvider { get; set; }
        public WaveFormat WaveFormat { get; set; }

        public float BandOneDb
        {
            get => BandOne.Gain;
            set => BandOne.Gain = value;
        }
        public float BandTwoDb
        {
            get => BandTwo.Gain;
            set => BandTwo.Gain = value;
        }
        public float BandThreeDb
        {
            get => BandThree.Gain;
            set => BandThree.Gain = value;
        }
        public float BandFourDb
        {
            get => BandFour.Gain;
            set => BandFour.Gain = value;
        }
        public float BandFiveDb
        {
            get => BandFive.Gain;
            set => BandFive.Gain = value;
        }
        public float BandSixDb
        {
            get => BandSix.Gain;
            set => BandSix.Gain = value;
        }
        public float BandSevenDb
        {
            get => BandSeven.Gain;
            set => BandSeven.Gain = value;
        }
        public float BandEightDb
        {
            get => BandEight.Gain;
            set => BandEight.Gain = value;
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the 8-Band EQ Provider.
        /// </summary>
        public EqRackProvider()
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

            BandOne = new PeakingEQModule(WaveFormat, 125, 0, 1.4f);
            BandTwo = new PeakingEQModule(WaveFormat, 250, 0, 1.4f);
            BandThree = new PeakingEQModule(WaveFormat, 500, 0, 1.4f);
            BandFour = new PeakingEQModule(WaveFormat, 1000, 0, 1.4f);
            BandFive = new PeakingEQModule(WaveFormat, 2000, 0, 1.4f);
            BandSix = new PeakingEQModule(WaveFormat, 4000, 0, 1.4f);
            BandSeven = new PeakingEQModule(WaveFormat, 8000, 0, 1.4f);
            BandEight = new PeakingEQModule(WaveFormat, 16000, 0, 1.4f);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            SourceProvider?.Read(buffer, offset, count);

            BandOne.Read(buffer, offset, count);
            BandTwo.Read(buffer, offset, count);
            BandThree.Read(buffer, offset, count);
            BandFour.Read(buffer, offset, count);
            BandFive.Read(buffer, offset, count);
            BandSix.Read(buffer, offset, count);
            BandSeven.Read(buffer, offset, count);
            BandEight.Read(buffer, offset, count);

            return count;
        }
    }
}
