using Microsoft.Extensions.DependencyInjection;
using NorthernSpectrums.Core;
using NorthernSpectrums.MVVM.Model.Audio.AudioComponents;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.View.Windows;
using NorthernSpectrums.MVVM.ViewModel;
using NorthernSpectrums.MVVM.ViewModel.ApplicationWindow;
using NorthernSpectrums.Services.AmpHeadService;
using NorthernSpectrums.Services.AudioProcessService;
using NorthernSpectrums.Services.ConfigService;
using NorthernSpectrums.Services.DeviceService;
using NorthernSpectrums.Services.NavigationService;
using NorthernSpectrums.Services.Presets;
using NorthernSpectrums.Services.RackService;
using NorthernSpectrums.Services.ThemeService;
using System.Windows;

namespace NorthernSpectrums
{
    /// <summary>
    /// Interaction logic for the main application.
    /// </summary>
    public sealed partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// <c>Constructor</c> Registers all necessary services, view models and factories to the service provider.
        /// </summary>
        public App()
        {
            // Declare DataContext bindings and Services.
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            });

            // We add a transient to avoid an exception where a singleton window is opened.
            services.AddTransient(provider => new SettingsWindow
            {
                DataContext = provider.GetRequiredService<SettingsViewModel>()
            });

            // Base view models, required to keep their state.
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<AudioSettingsViewModel>();
            services.AddSingleton<GeneralSettingsViewModel>();
            services.AddSingleton<PedalViewModel>();
            services.AddSingleton<AmpViewModel>();
            services.AddSingleton<RackViewModel>();
            services.AddSingleton<EndControlViewModel>();

            // We add our services here so we can utilize dependency injection with view models that utilize these services.
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddSingleton<IAmpHeadService, AmpHeadService>();
            services.AddSingleton<IRackService, RackService>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<IAudioProcessService, AudioProcessService>();
            services.AddSingleton<EffectsPackageExtension>();
            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<PresetService>();
            services.AddSingleton<PresetSaverService>();
            services.AddSingleton<LevelReader>();
            services.AddTransient<INavigationService, NavigationService>();

            // Delegates so that we can provide correct view model and application window with the correct data context.
            services.AddSingleton<Func<Type, ViewModel>>(provider => viewModelType => (ViewModel)provider.GetRequiredService(viewModelType));
            services.AddSingleton<Func<Type, ApplicationWindow>>(provider => applicationWindowType => (ApplicationWindow)provider.GetRequiredService(applicationWindowType));
            services.AddSingleton<Func<EffectsPackage, IEffectsProvider>>(provider => (specifiedEffectsPackage) => provider.GetRequiredService<EffectsPackageExtension>().CreateInstance(specifiedEffectsPackage));
            services.AddSingleton<Func<EffectsPackage, IEffectsProvider?, ViewModel>>(provider => (specifiedEffectsPackage, specifiedEffectsProvider) => provider.GetRequiredService<EffectsPackageExtension>().CreateViewModel(specifiedEffectsPackage, specifiedEffectsProvider));

            serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            serviceProvider.GetRequiredService<MainWindow>();

            ConfigurationService configService = serviceProvider.GetRequiredService<ConfigurationService>();
            configService?.LoadOnStartup();

            MainWindow.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ConfigurationService configService = serviceProvider.GetRequiredService<ConfigurationService>();
            configService?.SaveOnExit();

            base.OnExit(e);
        }
    }
}
