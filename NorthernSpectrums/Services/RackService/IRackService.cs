using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.Services.RackService
{
    /// <summary>
    /// <c>Interface</c> Exposes interactability with the rack service.
    /// </summary>
    public interface IRackService
    {
        /// <summary>
        /// <c>Property</c> The source provider to read from.
        /// </summary>
        public ISampleProvider? SourceProvider { set; get; }

        /// <summary>
        /// <c>Method</c> Adds a rack to the signal chain.
        /// </summary>
        /// <param name="newProvider">The new rack provider.</param>
        /// <param name="position">The position to be inserted at.</param>
        public void AddToRack(IEffectsProvider? newProvider, int position);
    }
}
