using NorthernSpectrums.MVVM.Model.Audio.AmpProviders.NSThreader;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.ViewModel;
using NorthernSpectrums.MVVM.ViewModel.Amps;
using System.ComponentModel;

namespace NorthernSpectrums.MVVM.Model.Audio.AmpProviders
{
    /// <summary>
    /// <c>Class</c> Allows usage of EffectsPackage and provides a factory for amp providers.
    /// Original implementation by Rohan West. <see href="https://stackoverflow.com/a/6037906"/>
    /// </summary>
    public class AmpPackageExtension
    {
        private readonly Dictionary<AmpPackage, Func<IEffectsProvider>> ampStore = new Dictionary<AmpPackage, Func<IEffectsProvider>>();
        private readonly Dictionary<AmpPackage, Func<ProviderViewModelBase>> viewModelStore = new Dictionary<AmpPackage, Func<ProviderViewModelBase>>();
        private readonly Dictionary<AmpPackage, Type> viewModelTypeStore = new Dictionary<AmpPackage, Type>();

        /// <summary>
        /// <c>Constructor</c> Registers each available amp provider.
        /// </summary>
        public AmpPackageExtension()
        {
            AmpPackage[] packages = Enum.GetValues(typeof(AmpPackage)).Cast<AmpPackage>().ToArray();

            foreach (AmpPackage package in packages)
            {
                switch (package)
                {
                    case AmpPackage.NSThreader:
                        RegisterAmpProvider<NSThreaderProvider>(package);
                        RegisterViewModel<NSThreaderViewModel>(package);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// <c>Method</c> Register a amp package and associate it to specified effect provider.
        /// </summary>
        /// <typeparam name="T">The specified effect provider type.</typeparam>
        /// <param name="package">The amp package to associate with it.</param>
        private void RegisterAmpProvider<T>(AmpPackage package) where T : IEffectsProvider, new()
        {
            RegisterAmpProvider(package, () => new T());
        }

        /// <summary>
        /// <c>Method</c> Register a amp package and associate it to specified view model.
        /// </summary>
        /// <typeparam name="T">The specified view model type.</typeparam>
        /// <param name="package">The amp package to associate with it.</param>
        private void RegisterViewModel<T>(AmpPackage package) where T : ProviderViewModelBase, new()
        {
            RegisterViewModel(package, () => new T());
        }

        /// <summary>
        /// <c>Method</c> Registers an amp package with a factory delegate to the dictionary.
        /// </summary>
        /// <param name="package">The amp package.</param>
        /// <param name="factory">The factory delegate.</param>
        private void RegisterAmpProvider(AmpPackage package, Func<IEffectsProvider> factory)
        {
            ampStore.Add(package, factory);
        }

        /// <summary>
        /// <c>Method</c> Registers a view model with a factory delegate to the dictionary.
        /// </summary>
        /// <param name="package">The amp package.</param>
        /// <param name="factory">The factory delegate.</param>
        private void RegisterViewModel(AmpPackage package, Func<ProviderViewModelBase> factory)
        {
            viewModelStore.Add(package, factory);
        }

        /// <summary>
        /// <c>Method</c> Creates an IEffectsProvider instance based of specified amp package.
        /// </summary>
        /// <param name="package">The amp package.</param>
        /// <exception cref="UnregisteredEffectsPackageException">In the case where a amp package is not properly registered.</exception>
        /// <returns>A IEffectsProvider instance from specified amp package.</returns>
        public IEffectsProvider CreateInstance(AmpPackage package)
        {
            Func<IEffectsProvider> factory;
            return ampStore.TryGetValue(package, out factory!) ? factory.Invoke() : throw new UnregisteredAmpPackageException();
        }

        /// <summary>
        /// <c>Method</c> Creates a ViewModel instance based of specified amp package.
        /// </summary>
        /// <param name="package">The amp package.</param>
        /// <param name="provider">The associated provider.</param>
        /// <exception cref="UnregisteredViewModelException">In the case where a viewmodel is not properly registered</exception>
        /// <returns>A ViewModel instance from specified amp package.</returns>
        public Core.ViewModel CreateViewModel(AmpPackage package, IEffectsProvider? provider)
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
    /// <c>Enum</c> Defines available amplifiers.
    /// </summary>
    public enum AmpPackage
    {
        [Description("NS Threader")]
        NSThreader
        //[Description("NS Soother")]
        //NSSoother
    }

    /// <summary>
    /// <c>Exception</c> An attempt to access an unregistered amplifier.
    /// </summary>
    public class UnregisteredAmpPackageException : Exception
    {
        public UnregisteredAmpPackageException() { }
        public UnregisteredAmpPackageException(string message) : base(message) { }
        public UnregisteredAmpPackageException(string message, Exception inner) : base(message, inner) { }
    }
}

