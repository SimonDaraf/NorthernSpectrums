using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.Model.Audio.RackProviders.ReverbRack;
using NorthernSpectrums.MVVM.ViewModel.Amps;
using System.Reflection;

namespace NorthernSpectrums.MVVM.ViewModel.Racks
{
    /// <summary>
    /// <c>Class</c> Handles the bindings between the view and reverb rack provider.
    /// </summary>
    public class ReverbRackViewModel : ProviderViewModelBase, IPreservable
    {
        private IReverbRackProvider? reverbRackProvider;

        private float levelKnobRotation;
        private float decayKnobRotation;
        private float timeKnobRotation;

        // Property view bindings.
        public float LevelKnobRotation
        {
            get => levelKnobRotation;
            set
            {
                levelKnobRotation = value;
                if (reverbRackProvider != null)
                {
                    reverbRackProvider.Level = CalculateLevelFromAngle(value);
                }
                OnPropertyChanged();
            }
        }
        public float DecayKnobRotation
        {
            get => decayKnobRotation;
            set
            {
                decayKnobRotation = value;
                if (reverbRackProvider != null)
                {
                    reverbRackProvider.Decay = CalculateDecayFromAngle(value);
                }
                OnPropertyChanged();
            }
        }
        public float TimeKnobRotation
        {
            get => timeKnobRotation;
            set
            {
                timeKnobRotation = value;
                if (reverbRackProvider != null)
                {
                    reverbRackProvider.DelayMs = CalculateTimeFromAngle(value);
                }
                OnPropertyChanged();
            }
        }

        public override void SetProvider(IEffectsProvider provider)
        {
            reverbRackProvider = (IReverbRackProvider)provider;
        }

        /// <summary>
        /// <c>Method</c> Calculates the level value from given angle.
        /// </summary>
        /// <param name="angle">The given angle.</param>
        /// <returns>The level value.</returns>
        private float CalculateLevelFromAngle(float angle)
        {
            return (angle + 140) / 280; // Level to between [0, 1].
        }

        /// <summary>
        /// <c>Method</c> Calculates the decay value from given angle.
        /// </summary>
        /// <param name="angle">The given angle.</param>
        /// <returns>The decay value.</returns>
        private float CalculateDecayFromAngle(float angle)
        {
            return ((140 + angle) / 280f) * 10f; // Between [0, 10];
        }

        /// <summary>
        /// <c>Method</c> Calculates the time value from given angle.
        /// </summary>
        /// <param name="angle">The given angle.</param>
        /// <returns>The time value.</returns>
        private int CalculateTimeFromAngle(float angle)
        {
            return (int)(angle * 1.05f) + 189; // Around [42, 336].
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>
            {
                { "LevelKnobRotation", levelKnobRotation },
                { "DecayKnobRotation", decayKnobRotation },
                { "TimeKnobRotation", timeKnobRotation }
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            foreach (KeyValuePair<string, object> kvp in data)
            {
                PropertyInfo? property = typeof(ReverbRackViewModel).GetProperty(kvp.Key);

                // If property was found.
                property?.SetValue(this, kvp.Value);
            }
        }
    }
}
