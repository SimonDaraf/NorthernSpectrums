using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.MVVM.ViewModel.Pedals
{
    /// <summary>
    /// <c>Class</c> Represents an empty pedal selection.
    /// </summary>
    public class EmptyPedalViewModel : ProviderViewModelBase
    {
        public override void SetProvider(IEffectsProvider provider) { }
    }
}
