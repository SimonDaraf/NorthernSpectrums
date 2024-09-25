using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.MVVM.ViewModel.Racks
{
    /// <summary>
    /// <c>Class</c> Represents an empty rack. Used when the option None is selected.
    /// </summary>
    public class EmptyRackViewModel : ProviderViewModelBase
    {
        public override void SetProvider(IEffectsProvider provider)
        {
            throw new NotImplementedException(); // This is not needed for this placeholder.
        }
    }
}
