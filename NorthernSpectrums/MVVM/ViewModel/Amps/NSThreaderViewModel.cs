using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.AmpProviders.NSThreader;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using System.Reflection;

namespace NorthernSpectrums.MVVM.ViewModel.Amps
{
    /// <summary>
    /// <c>Class</c> The view model for the NS Threader provider.
    /// </summary>
    public class NSThreaderViewModel : ProviderViewModelBase, IPreservable
    {
        private INSThreaderProvider? provider;

        private float gainKnobRotation;
        private float bassKnobRotation;
        private float middleKnobRotation;
        private float trebleKnobRotation;
        private float masterKnobRotation;

        // Expose properties to view.
        public float GainKnobRotation
        {
            get => gainKnobRotation;
            set
            {
                gainKnobRotation = value;
                if (provider != null)
                {
                    provider.Gain = CalculateGainValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float BassKnobRotation
        {
            get => bassKnobRotation;
            set
            {
                bassKnobRotation = value;
                if (provider != null)
                {
                    provider.BassGain = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float MiddleKnobRotation
        {
            get => middleKnobRotation;
            set
            {
                middleKnobRotation = value;
                if (provider != null)
                {
                    provider.MiddleGain = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float TrebleKnobRotation
        {
            get => trebleKnobRotation;
            set
            {
                trebleKnobRotation = value;
                if (provider != null)
                {
                    provider.TrebleGain = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float MasterKnobRotation
        {
            get => masterKnobRotation;
            set
            {
                masterKnobRotation = value;
                if (provider != null)
                {
                    provider.MasterGain = CalculateMasterValue(value);
                }
                OnPropertyChanged();
            }
        }

        public void Load(Dictionary<string, object> data)
        {
            foreach(KeyValuePair<string, object> kvp in data)
            {
                PropertyInfo? property = typeof(NSThreaderViewModel).GetProperty(kvp.Key);

                // If property was found.
                property?.SetValue(this, kvp.Value);
            }
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>
            {
                { "GainKnobRotation", gainKnobRotation },
                { "BassKnobRotation", bassKnobRotation },
                { "MiddleKnobRotation", middleKnobRotation },
                { "TrebleKnobRotation", trebleKnobRotation },
                { "MasterKnobRotation", masterKnobRotation }
            };
        }

        public override void SetProvider(IEffectsProvider provider)
        {
            this.provider = (INSThreaderProvider)provider;
        }

        /// <summary>
        /// <c>Method</c> Calculates the db gain from the given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The Db gain value.</returns>
        private float CalculateEQDbValue(float angle)
        {
            return angle * 0.09f; // Around [-12, 12]
        }

        /// <summary>
        /// <c>Method</c> Calculates the gain value from the given angle. 
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The gain value.</returns>
        private float CalculateGainValue(float angle)
        {
            return ((angle + 140) / 35) + 2; // [2, 10]
        }

        /// <summary>
        /// <c>Method</c> Calculates the master calue from the given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The master value.</returns>
        private float CalculateMasterValue(float angle)
        {
            return (angle + 140) / 280 * 2; // [0, 2]
        }
    }
}
