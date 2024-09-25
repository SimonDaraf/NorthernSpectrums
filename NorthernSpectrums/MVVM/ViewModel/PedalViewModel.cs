using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.ViewModel.Pedals;
using NorthernSpectrums.Services.AudioProcessService;
using System.Reflection;
using System.Windows;

namespace NorthernSpectrums.MVVM.ViewModel
{
    /// <summary>
    /// <c>Class</c> The pedal view model.
    /// </summary>
    public class PedalViewModel : Core.ViewModel, IPreservable
    {
        private readonly Func<EffectsPackage, IEffectsProvider> packageRetriever;
        private readonly Func<EffectsPackage, IEffectsProvider?, Core.ViewModel> vmRetriever;
        private readonly IAudioProcessService audioProcessService;
        private readonly EffectsPackage[] packageSource;

        private EffectsPackage spotOneSelection;
        private EffectsPackage spotTwoSelection;
        private EffectsPackage spotThreeSelection;
        private Core.ViewModel spotOneModel;
        private Core.ViewModel spotTwoModel;
        private Core.ViewModel spotThreeModel;

        private IEffectsProvider? spotOneProvider;
        private IEffectsProvider? spotTwoProvider;
        private IEffectsProvider? spotThreeProvider;

        // Property view bindings.
        public EffectsPackage[] PackageSource
        {
            get => packageSource;
        }
        public EffectsPackage SpotOneSelection
        {
            get => spotOneSelection;
            set
            {
                spotOneSelection = SetPedalFromPackageExtension(value, ref spotOneProvider);
                SpotOneModel = GetViewModelFromPackageExtension(value, ref spotOneProvider);
                OnPropertyChanged();
            }
        }
        public EffectsPackage SpotTwoSelection
        {
            get => spotTwoSelection;
            set
            {
                spotTwoSelection = SetPedalFromPackageExtension(value, ref spotTwoProvider);
                SpotTwoModel = GetViewModelFromPackageExtension(value, ref spotTwoProvider);
                OnPropertyChanged();
            }
        }
        public EffectsPackage SpotThreeSelection
        {
            get => spotThreeSelection;
            set
            {
                spotThreeSelection = SetPedalFromPackageExtension(value, ref spotThreeProvider);
                SpotThreeModel = GetViewModelFromPackageExtension(value, ref spotThreeProvider);
                OnPropertyChanged();
            }
        }
        public Core.ViewModel SpotOneModel
        {
            get => spotOneModel;
            set
            {
                spotOneModel = value;
                OnPropertyChanged();
            }
        }
        public Core.ViewModel SpotTwoModel
        {
            get => spotTwoModel;
            set
            {
                spotTwoModel = value;
                OnPropertyChanged();
            }
        }
        public Core.ViewModel SpotThreeModel
        {
            get => spotThreeModel;
            set
            {
                spotThreeModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the PedalViewModel.
        /// </summary>
        /// <param name="audioProcessService">The audio process service which handles reading pedal buffers.</param>
        /// <param name="packageRetriever">A func delegate to access registered effect providers.</param>
        public PedalViewModel(IAudioProcessService audioProcessService, Func<EffectsPackage, IEffectsProvider> packageRetriever, Func<EffectsPackage, IEffectsProvider?, Core.ViewModel> vmRetriever)
        {
            this.audioProcessService = audioProcessService;
            this.packageRetriever = packageRetriever;
            this.vmRetriever = vmRetriever;
            spotOneSelection = EffectsPackage.None;
            spotTwoSelection = EffectsPackage.None;
            spotThreeSelection = EffectsPackage.None;

            // Defaults.
            spotOneModel = new EmptyPedalViewModel();
            spotTwoModel = new EmptyPedalViewModel();
            spotThreeModel = new EmptyPedalViewModel();

            packageSource = Enum.GetValues(typeof(EffectsPackage)).Cast<EffectsPackage>().ToArray();
        }

        /// <summary>
        /// <c>Method</c> Retrieves registered effects provider from package extension.
        /// </summary>
        /// <param name="package">EffectsPackage as a key.</param>
        /// <returns>Validated effects package, else None.</returns>
        private EffectsPackage SetPedalFromPackageExtension(EffectsPackage package, ref IEffectsProvider? providerRef)
        {
            // Avoid error message if user tries to select none.
            if (package != EffectsPackage.None)
            {
                try
                {
                    IEffectsProvider provider = packageRetriever.Invoke(package);
                    
                    // If one is selected, remove it.
                    if (providerRef != null)
                    {
                        audioProcessService.RemoveEffectProvider(provider: providerRef);
                    }

                    audioProcessService.AddEffectProvider(provider);
                    providerRef = provider;
                    return package;
                }
                catch (UnregisteredEffectsPackageException)
                {
                    MessageBox.Show("Unavailable to retrieve selected pedal.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
            // Remove associated effect provider.
            if (providerRef != null)
            {
                try
                {
                    audioProcessService.RemoveEffectProvider(providerRef);
                    providerRef = null;
                }
                catch (InvalidEffectsProviderReferenceException)
                {
                    // Indication that pedal did not exist in signal chain.
                }
            }
            return EffectsPackage.None;
        }

        /// <summary>
        /// <c>Method</c> Retrieves registered view model from package extension.
        /// </summary>
        /// <param name="package">EffectsPackage as a key.</param>
        /// <returns>The retrieved view model if found, else empty standard.</returns>
        private Core.ViewModel GetViewModelFromPackageExtension(EffectsPackage package, ref IEffectsProvider? provider)
        {
            try
            {
                Core.ViewModel vm = vmRetriever.Invoke(package, provider);
                return vm;
            }
            catch (UnregisteredViewModelException)
            {
                MessageBox.Show("Unavailable to retrieve associated pedal view.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new EmptyPedalViewModel(); // Set to empty.
            }
        }

        public Dictionary<string, object> Save()
        {
            Dictionary<string, object> toSave = new Dictionary<string, object>();

            Dictionary<EffectsPackage, Dictionary<string, object>> pedalOne = new Dictionary<EffectsPackage, Dictionary<string, object>>();
            Dictionary<EffectsPackage, Dictionary<string, object>> pedalTwo = new Dictionary<EffectsPackage, Dictionary<string, object>>();
            Dictionary<EffectsPackage, Dictionary<string, object>> pedalThree = new Dictionary<EffectsPackage, Dictionary<string, object>>();

            IPreservable? spotOne = SpotOneModel as IPreservable;
            IPreservable? spotTwo = SpotTwoModel as IPreservable;
            IPreservable? spotThree = SpotThreeModel as IPreservable;

            // If null, we have an empty view model which does not implement IPreservable.
            pedalOne.Add(SpotOneSelection, spotOne?.Save() ?? new Dictionary<string, object> { { "Empty", "Empty" } });
            pedalTwo.Add(SpotTwoSelection, spotTwo?.Save() ?? new Dictionary<string, object> { { "Empty", "Empty" } });
            pedalThree.Add(SpotThreeSelection, spotThree?.Save() ?? new Dictionary<string, object> { { "Empty", "Empty" } });

            toSave.Add("SpotOneSelection:SpotOneModel", pedalOne);
            toSave.Add("SpotTwoSelection:SpotTwoModel", pedalTwo);
            toSave.Add("SpotThreeSelection:SpotThreeModel", pedalThree);

            return toSave;
        }

        public void Load(Dictionary<string, object> data)
        {
            // Parse dictionary data.
            Dictionary<string, Dictionary<EffectsPackage, Dictionary<string, object>>> pedals = data
                    .Where(kvp => kvp.Value is Dictionary<EffectsPackage, Dictionary<string, object>>)
                    .ToDictionary(kvp => kvp.Key, kvp => (Dictionary<EffectsPackage, Dictionary<string, object>>)kvp.Value);

            foreach (KeyValuePair<string, Dictionary<EffectsPackage, Dictionary<string, object>>> a in pedals)
            {
                string[] propertyNames = a.Key.Split(':');
                foreach(KeyValuePair<EffectsPackage, Dictionary<string, object>> b in a.Value)
                {
                    PropertyInfo? selectionProperty = typeof(PedalViewModel).GetProperty(propertyNames[0]);

                    // If Property was found.
                    selectionProperty?.SetValue(this, b.Key);

                    PropertyInfo? vmProperty = typeof(PedalViewModel).GetProperty(propertyNames[1]);

                    // If vm implements Preservable interface, load settings.
                    if (vmProperty?.GetValue(this) is IPreservable preservable)
                    {
                        preservable.Load(b.Value);
                    }
                }
            }
        }
    }
}
