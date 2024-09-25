using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.Services.DeviceService;
using NorthernSpectrums.Services.RackService;

namespace NorthernSpectrums.Services.AmpHeadService
{
    /// <summary>
    /// <c>Class</c> Handles the amp configuration with the current signal chain.
    /// </summary>
    public class AmpHeadService : IAmpHeadService
    {
        private ISampleProvider? effectSource;
        private IEffectsProvider? ampSource;
        private readonly IRackService rackService;

        public ISampleProvider? EffectSource
        {
            get => effectSource;
            set
            {
                effectSource = value;
                Reconfigure();
            }
        }
        public IEffectsProvider? AmpHeadSource
        {
            get => ampSource;
            set
            {
                ampSource = value;
                Reconfigure();
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the AmpHeadService.
        /// </summary>
        /// <param name="deviceService">The Device Service, used to set the correct output source.</param>
        public AmpHeadService(IRackService rackService)
        {
            this.rackService = rackService;
        }

        /// <summary>
        /// <c>Method</c> Reconfigures the amps signal references.
        /// </summary>
        private void Reconfigure()
        {
            // Set the output source the device should read from, in case of null it won't read.
            rackService.SourceProvider = ampSource != null ? (ISampleProvider)ampSource : null;

            if (ampSource != null)
            {
                // Set the source the amp should read from, in case of null it won't read.
                ampSource.SourceProvider = effectSource ?? null;
            }
        }
    }
}
