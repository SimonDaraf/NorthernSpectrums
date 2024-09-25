using NorthernSpectrums.Core;
using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.AmpProviders;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.Services.AmpHeadService;

namespace NorthernSpectrums.MVVM.ViewModel
{
    /// <summary>
    /// <c>Class</c> The view model for the amp view.
    /// </summary>
    public class AmpViewModel : Core.ViewModel, IPreservable
    {
        private readonly IAmpHeadService ampHeadService;
        private readonly AmpPackageExtension ampPackageExtension = new AmpPackageExtension();
        private AmpPackage[] ampPackageSource;
        private int selectedAmpPackage;
        private Core.ViewModel currentAmpViewModel;

        // Property view bindings.
        public Core.ViewModel CurrentAmpViewModel
        {
            get => currentAmpViewModel;
            set
            {
                currentAmpViewModel = value;
                OnPropertyChanged();
            }
        }
        public string[] AmpPackageSource
        {
            get => ampPackageSource.Select(package => EnumExtensionMethod.GetDescription(package)).ToArray();
        }
        public int SelectedAmpPackage
        {
            get => selectedAmpPackage;
            set
            {
                SelectedAmp(value);
                selectedAmpPackage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the amp view model.
        /// Sets the selected amp in the amp head service.
        /// </summary>
        /// <param name="ampHeadService">The amp head service.</param>
        public AmpViewModel(IAmpHeadService ampHeadService)
        {
            this.ampHeadService = ampHeadService;

            ampPackageSource = Enum.GetValues(typeof(AmpPackage)).Cast<AmpPackage>().ToArray();

            // Set default.
            // To avoid constructor complaining about null value I also have to put this here... Ugly.
            IEffectsProvider provider = ampPackageExtension.CreateInstance(ampPackageSource[0]);
            currentAmpViewModel = ampPackageExtension.CreateViewModel(ampPackageSource[0], provider);
            ampHeadService.AmpHeadSource = provider;
        }

        /// <summary>
        /// <c>Method</c> Selects a amp based on selected amp package.
        /// </summary>
        /// <param name="index"></param>
        private void SelectedAmp(int index)
        {
            IEffectsProvider provider = ampPackageExtension.CreateInstance(ampPackageSource[index]);
            CurrentAmpViewModel = ampPackageExtension.CreateViewModel(ampPackageSource[index], provider);
            ampHeadService.AmpHeadSource = provider;
        }

        public Dictionary<string, object> Save()
        {
            Dictionary<string, object> toSave = new Dictionary<string, object>();

            Dictionary<AmpPackage, Dictionary<string, object>> spotOne = new Dictionary<AmpPackage, Dictionary<string, object>>();

            // Try to cast to the IPreservable interface.
            IPreservable? amp = currentAmpViewModel as IPreservable ?? throw new Exception("Failed to access save logic for selected amp.");

            spotOne.Add(((AmpPackage)selectedAmpPackage), amp.Save());

            toSave.Add("SpotOne", spotOne);

            return toSave;
        }

        public void Load(Dictionary<string, object> data)
        {
            Dictionary<string, Dictionary<AmpPackage, Dictionary<string, object>>> amps = data
                    .Where(kvp => kvp.Value is Dictionary<AmpPackage, Dictionary<string, object>>)
                    .ToDictionary(kvp => kvp.Key, kvp => (Dictionary<AmpPackage, Dictionary<string, object>>)kvp.Value);

            // Interpret passed data.
            foreach(KeyValuePair<string, Dictionary<AmpPackage, Dictionary<string, object>>> a in amps)
            {
                foreach(KeyValuePair<AmpPackage, Dictionary<string, object>> b in a.Value)
                {
                    SelectedAmpPackage = (int)b.Key; // Set selected amp to int value of enum.

                    if (currentAmpViewModel is IPreservable preservable)
                    {
                        preservable.Load(b.Value); // Pass settings to load.
                    }
                }
            }
        }
    }
}
