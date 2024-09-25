using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.Services.DeviceService;

namespace NorthernSpectrums.Services.RackService
{
    /// <summary>
    /// <c>Class</c> Handles racks integration the the overall signal chain.
    /// </summary>
    public class RackService : IRackService
    {
        private IEffectsProvider?[] rackProviders;
        private ISampleProvider? sourceProvider;
        private readonly IDeviceService deviceService;

        public ISampleProvider? SourceProvider
        {
            get => sourceProvider;
            set
            {
                sourceProvider = value;
                Configure();
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the RackService.
        /// </summary>
        /// <param name="deviceService">The device service used for one way communication when a source provider is present.</param>
        public RackService(IDeviceService deviceService)
        {
            this.deviceService = deviceService;
            rackProviders = new IEffectsProvider[2]; // Accepted number of racks currently.
        }

        public void AddToRack(IEffectsProvider? newProvider, int position)
        {
            try
            {
                rackProviders[position] = newProvider;
                Configure();
            }
            catch (IndexOutOfRangeException)
            {
                return; // If an attemp to access index out of bounds was made, we don't want to act any further.
            }
        }

        /// <summary>
        /// <c>Method</c> Configures the signal chain based on available providers.
        /// </summary>
        private void Configure()
        {
            // First check if all elements are null, in that case the rack chain is empty and we use the source provider if available.
            if (rackProviders.All(provider => provider == null))
            {
                // This could be null, but it's up to our device manager too check whether it can be read from. 
                deviceService.OutputSource = sourceProvider;
                return;
            }

            ISampleProvider? prevProvider = sourceProvider;

            // Configure chain.
            for (int i = 0; i < rackProviders.Length; i++)
            {
                if (rackProviders[i] != null)
                {
                    // Complaining about possible null after we check...
                    rackProviders[i]!.SourceProvider = prevProvider ?? sourceProvider;

                    prevProvider = (ISampleProvider)rackProviders[i]!;
                }
            }

            // Get last rack in chain to send to device manager.
            for (int i = rackProviders.Length - 1; i >= 0; i--)
            {
                if (rackProviders[i] != null)
                {
                    deviceService.OutputSource = (ISampleProvider)rackProviders[i]!;
                    break;
                }
            }
        }
    }
}
