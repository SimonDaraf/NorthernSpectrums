using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Delay;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Distortion;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Volume;
using NorthernSpectrums.MVVM.ViewModel;
using NorthernSpectrums.MVVM.ViewModel.Pedals;
using System.ComponentModel;

namespace NorthernSpectrums.MVVM.Model.Audio.EffectsProviders
{
    /// <summary>
    /// <c>Class</c> Allows usage of EffectsPackage and provides a factory for effect providers.
    /// Original implementation by Rohan West. <see href="https://stackoverflow.com/a/6037906"/>
    /// </summary>
    public class EffectsPackageExtension
    {
        private readonly Dictionary<EffectsPackage, Func<IEffectsProvider>> effectStore = new Dictionary<EffectsPackage, Func<IEffectsProvider>>();
        private readonly Dictionary<EffectsPackage, Func<ProviderViewModelBase>> viewModelStore = new Dictionary<EffectsPackage, Func<ProviderViewModelBase>>();

        /// <summary>
        /// <c>Constructor</c> Registers each available effect provider.
        /// </summary>
        public EffectsPackageExtension()
        {
            EffectsPackage[] packages = Enum.GetValues(typeof(EffectsPackage)).Cast<EffectsPackage>().ToArray();

            foreach (EffectsPackage package in packages)
            {
                switch (package)
                {
                    case EffectsPackage.Volume:
                        RegisterEffectProvider<VolumeProvider>(package);
                        RegisterViewModel<VolumeViewModel>(package);
                        break;
                    case EffectsPackage.Delay:
                        RegisterEffectProvider<DelayProvider>(package);
                        RegisterViewModel<DelayViewModel>(package);
                        break;
                    case EffectsPackage.Distortion:
                        RegisterEffectProvider<DistortionProvider>(package);
                        RegisterViewModel<DistortionViewModel>(package);
                        break;
                    default:
                        RegisterViewModel<EmptyPedalViewModel>(package);
                        break;
                }
            }
        }

        /// <summary>
        /// <c>Method</c> Register a effects package and associate it to specified effect provider.
        /// </summary>
        /// <typeparam name="T">The specified effect provider type.</typeparam>
        /// <param name="package">The effects package to associate with it.</param>
        private void RegisterEffectProvider<T>(EffectsPackage package) where T : IEffectsProvider, new()
        {
            RegisterEffectProvider(package, () => new T());
        }

        /// <summary>
        /// <c>Method</c> Register a effects package and associate it to specified view model.
        /// </summary>
        /// <typeparam name="T">The specified view model type.</typeparam>
        /// <param name="package">The effects package to associate with it.</param>
        private void RegisterViewModel<T>(EffectsPackage package) where T : ProviderViewModelBase, new()
        {
            RegisterViewModel(package, () => new T());
        }

        /// <summary>
        /// <c>Method</c> Registers an effects package with a factory delegate to the dictionary.
        /// </summary>
        /// <param name="package">The effects package.</param>
        /// <param name="factory">The factory delegate.</param>
        private void RegisterEffectProvider(EffectsPackage package, Func<IEffectsProvider> factory)
        {
            effectStore.Add(package, factory);
        }

        /// <summary>
        /// <c>Method</c> Registers an view model with a factory delegate to the dictionary.
        /// </summary>
        /// <param name="package">The effects package.</param>
        /// <param name="factory">The factory delegate.</param>
        private void RegisterViewModel(EffectsPackage package, Func<ProviderViewModelBase> factory)
        {
            viewModelStore.Add(package, factory);
        }

        /// <summary>
        /// <c>Method</c> Creates a IEffectsProvider instance based of specified effects package.
        /// </summary>
        /// <param name="package">The effects package.</param>
        /// <exception cref="UnregisteredEffectsPackageException">In the case where a effects package is not properly registered.</exception>
        /// <returns>A IEffectsProvider instance from specified effects package.</returns>
        public IEffectsProvider CreateInstance(EffectsPackage package)
        {
            Func<IEffectsProvider> factory;
            return effectStore.TryGetValue(package, out factory!) ? factory.Invoke() : throw new UnregisteredEffectsPackageException();
        }

        /// <summary>
        /// <c>Method</c> Creates a ViewModel instance based of specified effects package.
        /// </summary>
        /// <param name="package">The effects package.</param>
        /// <param name="provider">The associated provider.</param>
        /// <exception cref="UnregisteredViewModelException">In the case where a viewmodel is not properly registered</exception>
        /// <returns>A ViewModel instance from specified effects package.</returns>
        public Core.ViewModel CreateViewModel(EffectsPackage package, IEffectsProvider? provider)
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
    /// <c>Enum</c> Defines available pedals.
    /// </summary>
    public enum EffectsPackage
    {
        None,
        Volume,
        Delay,
        Distortion
    }

    /// <summary>
    /// <c>Exception</c> An attempt to acces an unregistered provider.
    /// </summary>
    public class UnregisteredEffectsPackageException : Exception
    {
        public UnregisteredEffectsPackageException() { }
        public UnregisteredEffectsPackageException(string message) : base(message) { }
        public UnregisteredEffectsPackageException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// <c>Exception</c> An attempt to access an unregistered view model.
    /// </summary>
    public class UnregisteredViewModelException : Exception
    {
        public UnregisteredViewModelException() { }
        public UnregisteredViewModelException(string message) : base(message) { }
        public UnregisteredViewModelException(string message, Exception inner) : base(message, inner) { }
    }
}
