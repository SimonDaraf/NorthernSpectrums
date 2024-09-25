using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.Services.AmpHeadService;
using NorthernSpectrums.Services.DeviceService;

namespace NorthernSpectrums.Services.AudioProcessService
{
    /// <summary>
    /// <c>Class</c> The Audio Process Service responsible for handling the singal chain.
    /// </summary>
    public class AudioProcessService : IAudioProcessService
    {
        private readonly IAmpHeadService ampHeadService;
        private readonly List<IEffectsProvider> effectsProviders;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the Audio Process Service.
        /// </summary>
        /// <param name="deviceService">The device service, used to pass input and output sources.</param>
        public AudioProcessService(IAmpHeadService ampHeadService)
        {
            this.ampHeadService = ampHeadService;
            effectsProviders = new List<IEffectsProvider>();
        }

        public void AddEffectProvider(IEffectsProvider provider)
        {
            effectsProviders.Add(provider);
            ReconfigureSignalChain();
        }

        public void RemoveEffectProvider(IEffectsProvider provider)
        {
            if (!effectsProviders.Remove(provider))
            {
                throw new InvalidEffectsProviderReferenceException("Couldn't remove effect provider.");
            }
            ReconfigureSignalChain();
        }

        /// <summary>
        /// <c>Method</c> Reconfigures the signal chain after a change has been made.
        /// </summary>
        private void ReconfigureSignalChain()
        {
            SetSignalChainStart();

            // Reconfigures signal chain and connects the current element with the previous one.
            for (int i = 1; i < effectsProviders.Count; i++)
            {
                effectsProviders.ElementAt(i).SourceProvider = (ISampleProvider)effectsProviders.ElementAt(i - 1);
            }

            // Convert the last effects provider in signal chain and set it as the output source.
            ampHeadService.EffectSource = effectsProviders.Count > 0 ? (ISampleProvider)effectsProviders.Last() : null;
        }

        /// <summary>
        /// <c>Method</c> Sets the input source in the first effects provider.
        /// </summary>
        private void SetSignalChainStart()
        {
            if (effectsProviders.Count > 0)
            {
                effectsProviders.First().SourceProvider = null;
            }
        }
    }
}
