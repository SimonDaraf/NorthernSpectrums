using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.Services.AudioProcessService
{
    /// <summary>
    /// <c>Interface</c> Interface used for the audio process service.
    /// </summary>
    public interface IAudioProcessService
    {
        /// <summary>
        /// <c>Method</c> Adds an effects provider to the singal chain.
        /// </summary>
        /// <param name="provider">The effects provider to be added.</param>
        public void AddEffectProvider(IEffectsProvider provider);

        /// <summary>
        /// <c>Method</c> Removes the specified effects provider from the signal chain.
        /// </summary>
        /// <param name="provider">The specified provider reference to be removed.</param>
        /// <exception cref="InvalidEffectsProviderReferenceException">When provided a invalid reference.</exception>
        public void RemoveEffectProvider(IEffectsProvider provider);
    }
}
