using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.Model.Audio.RackProviders.EQRack;
using NorthernSpectrums.MVVM.Model.Audio.RackProviders.ReverbRack;
using NorthernSpectrums.MVVM.ViewModel;
using NorthernSpectrums.MVVM.ViewModel.Racks;
using System.ComponentModel;

namespace NorthernSpectrums.MVVM.Model.Audio.RackProviders
{
    /// <summary>
    /// <c>Class</c> Allows the usage of EffectsPackage and provides a factory for rack providers.
    /// Original implementation by Rohan West. <see href="https://stackoverflow.com/a/6037906"/>
    /// </summary>
    public class RackPackageExtension
    {
        private readonly Dictionary<RackPackage, Func<IEffectsProvider>> rackStore = new Dictionary<RackPackage, Func<IEffectsProvider>>();
        private readonly Dictionary<RackPackage, Func<ProviderViewModelBase>> viewModelStore = new Dictionary<RackPackage, Func<ProviderViewModelBase>>();

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of each available rack package.
        /// </summary>
        public RackPackageExtension()
        {
            RackPackage[] packages = Enum.GetValues(typeof(RackPackage)).Cast<RackPackage>().ToArray();

            foreach (RackPackage package in packages)
            {
                switch(package)
                {
                    case RackPackage.None:
                        RegisterViewModel<EmptyRackViewModel>(package);
                        break;
                    case RackPackage.ReverbRack:
                        RegisterRackProvider<ReverbRackProvider>(package);
                        RegisterViewModel<ReverbRackViewModel>(package);
                        break;
                    case RackPackage.EqRack:
                        RegisterRackProvider<EqRackProvider>(package);
                        RegisterViewModel<EqRackViewModel>(package);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// <c>Method</c> Register a rack package and associate it to specified effect provider.
        /// </summary>
        /// <typeparam name="T">The specified effect provider type.</typeparam>
        /// <param name="package">The rack package to associate it with.</param>
        private void RegisterRackProvider<T>(RackPackage package) where T : IEffectsProvider, new()
        {
            RegisterRackProvider(package, () => new T());
        }

        /// <summary>
        /// <c>Method</c> Register a rack package and associate with specified view model.
        /// </summary>
        /// <typeparam name="T">The specified view model type.</typeparam>
        /// <param name="package">The rack package to associate with it.</param>
        private void RegisterViewModel<T>(RackPackage package) where T : ProviderViewModelBase, new ()
        {
            RegisterViewModel(package, () => new T());
        }

        /// <summary>
        /// <c>Method</c> Registers a rack package with a factory delegate to the dictionary.
        /// </summary>
        /// <param name="package">The rack package.</param>
        /// <param name="factory">The factory delegate.</param>
        private void RegisterRackProvider(RackPackage package, Func<IEffectsProvider> factory)
        {
            rackStore.Add(package, factory);
        }

        /// <summary>
        /// <c>Method</c> Register a view model with a factory delegate to the dictionary.
        /// </summary>
        /// <param name="package">The rack package.</param>
        /// <param name="factory">The factory delegate.</param>
        private void RegisterViewModel(RackPackage package, Func<ProviderViewModelBase> factory)
        {
            viewModelStore.Add(package, factory);
        }

        /// <summary>
        /// <c>Method</c> Creates an IEffectsProvider instance based of specified rack package.
        /// </summary>
        /// <param name="package">The rack package.</param>
        /// <returns>An IEffectsProvider instance from specified rack package.</returns>
        /// <exception cref="UnregisteredRackPackageException">In the case where a rack package is not properly registered.</exception>
        public IEffectsProvider CreateInstance(RackPackage package)
        {
            Func<IEffectsProvider> factory;
            return rackStore.TryGetValue(package, out factory!) ? factory.Invoke() : throw new UnregisteredRackPackageException();
        }

        /// <summary>
        /// <c>Method</c> Creates a ViewModel instance based of specified rack package.
        /// </summary>
        /// <param name="package">The rack package.</param>
        /// <param name="provider">The associated provider.</param>
        /// <exception cref="UnregisteredViewModelException">In the case where a viewmodel is not properly registered</exception>
        /// <returns>A ViewModel instance from specified rack package.</returns>
        public Core.ViewModel CreateViewModel(RackPackage package, IEffectsProvider? provider)
        {
            Func<ProviderViewModelBase> factory;

            if (provider == null)
            {
                return viewModelStore.TryGetValue(package, out factory!) ? factory.Invoke() : throw new UnregisteredViewModelException();
            }

            ProviderViewModelBase vmBase = viewModelStore.TryGetValue(package, out factory!) ? factory.Invoke() : throw new UnregisteredViewModelException();
            vmBase.SetProvider(provider);
            return vmBase;
        }
    }

    /// <summary>
    /// <c>Enum</c> Defines each available rack.
    /// </summary>
    public enum RackPackage
    {
        None,
        [Description("NS Reverberator")]
        ReverbRack,
        [Description("NS Equalizer")]
        EqRack
    }

    /// <summary>
    /// <c>Exception</c> An attempt to access an unregistered rack provider.
    /// </summary>
    public class UnregisteredRackPackageException : Exception
    {
        public UnregisteredRackPackageException() { }
        public UnregisteredRackPackageException(string message) : base(message) { }
        public UnregisteredRackPackageException(string message, Exception inner) : base(message, inner) { }
    }
}
