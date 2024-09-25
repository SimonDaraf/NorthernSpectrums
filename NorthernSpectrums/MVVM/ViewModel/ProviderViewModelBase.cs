using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.MVVM.ViewModel
{
    /// <summary>
    /// <c>Class</c> The abstract base of a provider view model.
    /// </summary>
    public abstract class ProviderViewModelBase : Core.ViewModel
    {
        /// <summary>
        /// <c>Method</c> Sets the associated provider in the view model.
        /// </summary>
        /// <param name="provider">The provider to be used.</param>
        public abstract void SetProvider(IEffectsProvider provider);
    }
}
