using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Volume;
using NorthernSpectrums.MVVM.ViewModel.Amps;
using System.Reflection;

namespace NorthernSpectrums.MVVM.ViewModel.Pedals
{
    /// <summary>
    /// <c>Class</c> The volume pedal view model.
    /// </summary>
    public class VolumeViewModel : ProviderViewModelBase, IPreservable
    {
        // Private fields.
        private IVolumeProvider? volumeProvider;
        private float volumeKnobRotation = 0;

        // Property view bindings.
        public float VolumeKnobRotation
        {
            get => volumeKnobRotation;
            set
            {
                volumeKnobRotation = value;
                calculateVolumeFromAngle(value);
                OnPropertyChanged();
            }
        }

        public void Load(Dictionary<string, object> data)
        {
            foreach (KeyValuePair<string, object> kvp in data)
            {
                PropertyInfo? property = typeof(VolumeViewModel).GetProperty(kvp.Key);

                // If property was found.
                property?.SetValue(this, kvp.Value);
            }
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>
            {
                { "VolumeKnobRotation", volumeKnobRotation }
            };
        }

        public override void SetProvider(IEffectsProvider provider)
        {
            volumeProvider = (IVolumeProvider)provider;
            volumeProvider.Volume = 0.5f;
        }

        /// <summary>
        /// <c>Method</c> Calculates the colume based on given angle and range [-140,140].
        /// </summary>
        /// <param name="angle">The given angle.</param>
        private void calculateVolumeFromAngle(float angle)
        {
            if (volumeProvider != null)
            {
                volumeProvider.Volume = (angle + 140) / 280; // Offsetting the angle by 140 to bring -140 to 0, and then dividing by the size of the range.
            }
        }
    }
}
