using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Distortion;
using NorthernSpectrums.MVVM.ViewModel.Amps;
using System.Reflection;

namespace NorthernSpectrums.MVVM.ViewModel.Pedals
{
    /// <summary>
    /// <c>Class</c> The volume pedal view model.
    /// </summary>
    public class DistortionViewModel : ProviderViewModelBase, IPreservable
    {
        // Private fields.
        private IDistortionProvider? distortionProvider;
        private float levelKnobRotation = 0;
        private float gainKnobRotation = 0;

        // Property view bindings.
        public float LevelKnobRotation
        {
            get => levelKnobRotation;
            set
            {
                levelKnobRotation = value;
                CalculateLevelValueFromangle(value);
                OnPropertyChanged();
            }
        }
        public float GainKnobRotation
        {
            get => gainKnobRotation;
            set
            {
                gainKnobRotation = value;
                CalculateGainValueFromAngle(value);
                OnPropertyChanged();
            }
        }

        public override void SetProvider(IEffectsProvider provider)
        {
            distortionProvider = (IDistortionProvider)provider;
        }

        /// <summary>
        /// <c>Method</c> Calculates the level value from the given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        private void CalculateLevelValueFromangle(float angle)
        {
            if (distortionProvider != null)
            {
                distortionProvider.Level = ((-1 / 560 * angle) + (1 - (7 / 28))) / 10; // set value between [0.1, 0.05].
            }
        }

        /// <summary>
        /// <c>Method</c> Calculates the gain value from the given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        private void CalculateGainValueFromAngle(float angle)
        {
            if (distortionProvider != null)
            {
                distortionProvider.Gain = (((angle + 140) / 280 * 4) + 1); // Convert angle to a float from 0 to 1 and increase its range to [1, 5].
            }
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>
            {
                { "LevelKnobRotation", levelKnobRotation },
                { "GainKnobRotation", gainKnobRotation }
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            foreach (KeyValuePair<string, object> kvp in data)
            {
                PropertyInfo? property = typeof(DistortionViewModel).GetProperty(kvp.Key);

                // If property was found.
                property?.SetValue(this, kvp.Value);
            }
        }
    }
}
