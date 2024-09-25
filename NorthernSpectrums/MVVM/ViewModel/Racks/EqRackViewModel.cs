using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.Model.Audio.RackProviders.EQRack;
using NorthernSpectrums.MVVM.ViewModel.Amps;
using System.Reflection;

namespace NorthernSpectrums.MVVM.ViewModel.Racks
{
    /// <summary>
    /// <c>Class</c> The view model for the eq rack provider.
    /// </summary>
    public class EqRackViewModel : ProviderViewModelBase, IPreservable
    {
        private IEqRackProvider? rackProvider;

        private float bandOneValue = 0.5f;
        private float bandTwoValue = 0.5f;
        private float bandThreeValue = 0.5f;
        private float bandFourValue = 0.5f;
        private float bandFiveValue = 0.5f;
        private float bandSixValue = 0.5f;
        private float bandSevenValue = 0.5f;
        private float bandEightValue = 0.5f;

        // Property bindings.
        public float BandOneValue
        {
            get => bandOneValue;
            set
            {
                bandOneValue = value;
                if (rackProvider != null)
                {
                    rackProvider.BandOneDb = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float BandTwoValue
        {
            get => bandTwoValue;
            set
            {
                bandTwoValue = value;
                if (rackProvider != null)
                {
                    rackProvider.BandTwoDb = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float BandThreeValue
        {
            get => bandThreeValue;
            set
            {
                bandThreeValue = value;
                if (rackProvider != null)
                {
                    rackProvider.BandThreeDb = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float BandFourValue
        {
            get => bandFourValue;
            set
            {
                bandFourValue = value;
                if (rackProvider != null)
                {
                    rackProvider.BandFourDb = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float BandFiveValue
        {
            get => bandFiveValue;
            set
            {
                bandFiveValue = value;
                if (rackProvider != null)
                {
                    rackProvider.BandFiveDb = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float BandSixValue
        {
            get => bandSixValue;
            set
            {
                bandSixValue = value;
                if (rackProvider != null)
                {
                    rackProvider.BandSixDb = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float BandSevenValue
        {
            get => bandSevenValue;
            set
            {
                bandSevenValue = value;
                if (rackProvider != null)
                {
                    rackProvider.BandSevenDb = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }
        public float BandEightValue
        {
            get => bandEightValue;
            set
            {
                bandEightValue = value;
                if (rackProvider != null)
                {
                    rackProvider.BandEightDb = CalculateEQDbValue(value);
                }
                OnPropertyChanged();
            }
        }

        public void Load(Dictionary<string, object> data)
        {
            foreach (KeyValuePair<string, object> kvp in data)
            {
                PropertyInfo? property = typeof(EqRackViewModel).GetProperty(kvp.Key);

                // If property was found.
                property?.SetValue(this, kvp.Value);
            }
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>
            {
                { "BandOneValue", bandOneValue },
                { "BandTwoValue", bandTwoValue },
                { "BandThreeValue", bandThreeValue },
                { "BandFourValue", bandFourValue },
                { "BandFiveValue", bandFiveValue },
                { "BandSixValue", bandSixValue },
                { "BandSevenValue", bandSevenValue },
                { "BandEightValue", bandEightValue },
            };
        }

        public override void SetProvider(IEffectsProvider provider)
        {
            rackProvider = (IEqRackProvider)provider;
        }

        /// <summary>
        /// <c>Method</c> Calculates the db gain from the given value.
        /// </summary>
        /// <param name="value">The slider value.</param>
        /// <returns>The Db gain value.</returns>
        private float CalculateEQDbValue(float value)
        {
            return (value * 20) - 10; // [-10, 10]
        }
    }
}
