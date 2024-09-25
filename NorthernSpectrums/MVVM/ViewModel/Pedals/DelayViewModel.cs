using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Delay;
using System.Configuration;
using System.Reflection;

namespace NorthernSpectrums.MVVM.ViewModel.Pedals
{
    /// <summary>
    /// <c>Class</c> The view model for the delay provider.
    /// </summary>
    public class DelayViewModel : ProviderViewModelBase, IPreservable
    {
        private IDelayProvider? delayProvider;
        private float levelKnobRotation;
        private float feebackKnobRotation;
        private string[] noteFractions;
        private int fractionIndex;
        private int bpm = 120;

        // Property view bindings.
        public float LevelKnobRotation
        {
            get => levelKnobRotation;
            set
            {
                levelKnobRotation = value;
                CalculateLevelFromAngle(value);
                OnPropertyChanged();
            }
        }
        public float FeedbackKnobRotation
        {
            get => feebackKnobRotation;
            set
            {
                feebackKnobRotation = value;
                CalculateFeedbackFromAngle(value);
                OnPropertyChanged();
            }
        }
        public string[] NoteFractions
        {
            get => noteFractions;
            set
            {
                noteFractions = value;
                OnPropertyChanged();
            }
        }
        public int FractionIndex
        {
            get => fractionIndex;
            set
            {
                fractionIndex = value;
                SetDelayFraction(fractionIndex);
                OnPropertyChanged();
            }
        }
        public int Bpm
        {
            get => bpm;
            set
            {
                bpm = value;
                SetDelayBpm(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the delay pedal view model.
        /// </summary>
        public DelayViewModel()
        {
            noteFractions = ConfigurationManager.AppSettings.AllKeys!;
            FractionIndex = 3;
        }

        public override void SetProvider(IEffectsProvider provider)
        {
            delayProvider = (IDelayProvider) provider;
        }

        /// <summary>
        /// <c>Method</c> Calculates the value to be used from given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        private void CalculateLevelFromAngle(float angle)
        {
            if (delayProvider != null)
            {
                delayProvider.Level = (angle + 140) / 280; // Set level to between [0, 1];
            }
        }

        /// <summary>
        /// <c>Method</c> Calculates the value to be used from given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        private void CalculateFeedbackFromAngle(float angle)
        {
            if (delayProvider != null)
            {
                delayProvider.Feedback = 0.5f + (0.4f * angle / 140); // Set feedback to between [0.1, 0.9].
            }
        }

        /// <summary>
        /// <c>Method</c> Sets the delay bpm.
        /// </summary>
        /// <param name="bpm">The bpm.</param>
        private void SetDelayBpm(int bpm)
        {
            if (delayProvider != null)
            {
                delayProvider.Bpm = bpm;
            }
        }

        /// <summary>
        /// <c>Method</c> Sets the delay fraction.
        /// </summary>
        /// <param name="fraction">The fraction.</param>
        private void SetDelayFraction(int fractionIndex)
        {
            if (delayProvider != null)
            {
                try
                {
                    float noteFraction = float.Parse(ConfigurationManager.AppSettings.Get(index: fractionIndex)!); // Get fraction value from app config.
                    delayProvider.NoteFraction = noteFraction;
                }
                catch (Exception)
                {
                    delayProvider.NoteFraction = 1; // Standard quarter note.
                }
            }
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>
            {
                { "LevelKnobRotation", levelKnobRotation },
                { "FeedbackKnobRotation", feebackKnobRotation },
                { "FractionIndex", fractionIndex },
                { "Bpm", bpm }
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            foreach (KeyValuePair<string, object> kvp in data)
            {
                PropertyInfo? property = typeof(DelayViewModel).GetProperty(kvp.Key);

                // If property was found.
                property?.SetValue(this, kvp.Value);
            }
        }
    }
}
