using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.AmpProviders;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.Model.Audio.RackProviders;
using NorthernSpectrums.MVVM.ViewModel.Racks;
using NorthernSpectrums.Services.RackService;
using System.Reflection;

namespace NorthernSpectrums.MVVM.ViewModel
{
    /// <summary>
    /// <c>Class</c> Handles the rack bindings with the view.
    /// </summary>
    public class RackViewModel : Core.ViewModel, IPreservable
    {
        private readonly RackPackageExtension rackPackageExtension;
        private readonly IRackService rackService;
        private Core.ViewModel spotOneRackVm;
        private Core.ViewModel spotTwoRackVm;
        private RackPackage[] rackSource;
        private RackPackage spotOneRack;
        private RackPackage spotTwoRack;

        // Property bindings.
        public Core.ViewModel SpotOneRackVm
        {
            get => spotOneRackVm;
            set
            {
                spotOneRackVm = value;
                OnPropertyChanged();
            }
        }
        public Core.ViewModel SpotTwoRackVm
        {
            get => spotTwoRackVm;
            set
            {
                spotTwoRackVm = value;
                OnPropertyChanged();
            }
        }
        public RackPackage[] RackSource
        {
            get => rackSource;
            set
            {
                rackSource = value;
                OnPropertyChanged();
            }
        }
        public RackPackage SpotOneRack
        {
            get => spotOneRack;
            set
            {
                spotOneRack = value;
                OnPropertyChanged();
                SpotOneRackVm = SetViewModel(value, 0);
            }
        }
        public RackPackage SpotTwoRack
        {
            get => spotTwoRack;
            set
            {
                spotTwoRack = value;
                OnPropertyChanged();
                SpotTwoRackVm = SetViewModel(value, 1);
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs a new instance of the rack view model.
        /// </summary>
        public RackViewModel(IRackService rackService)
        {
            rackPackageExtension = new RackPackageExtension();
            this.rackService = rackService;

            spotOneRackVm = rackPackageExtension.CreateViewModel(RackPackage.None, null);
            spotTwoRackVm = rackPackageExtension.CreateViewModel(RackPackage.None, null);

            rackSource = Enum.GetValues(typeof(RackPackage)).Cast<RackPackage>().ToArray();
        }

        /// <summary>
        /// <c>Method</c> Sets the passed view model reference to the selected rack package.
        /// </summary>
        /// <param name="vmRef">The view model reference to be replaced.</param>
        /// <param name="package">The selected rack package.</param>
        private Core.ViewModel SetViewModel(RackPackage package, int position)
        {
            if (package == RackPackage.None)
            {
                // If None is selected we don't need the provider.
                rackService.AddToRack(null, position);
                return rackPackageExtension.CreateViewModel(package, null);
            }

            IEffectsProvider provider = rackPackageExtension.CreateInstance(package);
            rackService.AddToRack(provider, position);
            Core.ViewModel vm = rackPackageExtension.CreateViewModel(package, provider);

            // Set the reference to the new view model.
            return vm;
        }

        public Dictionary<string, object> Save()
        {
            Dictionary<string, object> toSave = new Dictionary<string, object>();

            Dictionary<RackPackage, Dictionary<string, object>> rackOne = new Dictionary<RackPackage, Dictionary<string, object>>();
            Dictionary<RackPackage, Dictionary<string, object>> rackTwo = new Dictionary<RackPackage, Dictionary<string, object>>();

            IPreservable? spotOne = spotOneRackVm as IPreservable;
            IPreservable? spotTwo = spotTwoRackVm as IPreservable;

            // If spotOne or two is null, that means that we have an empty rack vm which does not implement the IPreservable interface.
            rackOne.Add(spotOneRack, spotOne?.Save() ?? new Dictionary<string, object>{ { "Empty", "Empty" } });
            rackTwo.Add(spotTwoRack, spotTwo?.Save() ?? new Dictionary<string, object> { { "Empty", "Empty" } });

            toSave.Add("SpotOneRack:SpotOneRackVm", rackOne);
            toSave.Add("SpotTwoRack:SpotTwoRackVm", rackTwo);

            return toSave;
        }

        public void Load(Dictionary<string, object> data)
        {
            // Parse dictionary data.
            Dictionary<string, Dictionary<RackPackage, Dictionary<string, object>>> racks = data
                    .Where(kvp => kvp.Value is Dictionary<RackPackage, Dictionary<string, object>>)
                    .ToDictionary(kvp => kvp.Key, kvp => (Dictionary<RackPackage, Dictionary<string, object>>)kvp.Value);

            foreach (KeyValuePair<string, Dictionary<RackPackage, Dictionary<string, object>>> a in racks)
            {
                string[] propertyNames = a.Key.Split(':');
                foreach (KeyValuePair<RackPackage, Dictionary<string, object>> b in a.Value)
                {
                    PropertyInfo? selectionProperty = typeof(RackViewModel).GetProperty(propertyNames[0]);

                    // If Property was found.
                    selectionProperty?.SetValue(this, b.Key);

                    PropertyInfo? vmProperty = typeof(RackViewModel).GetProperty(propertyNames[1]);

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
