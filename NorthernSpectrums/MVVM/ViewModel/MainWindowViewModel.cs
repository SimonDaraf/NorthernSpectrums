using NorthernSpectrums.Core;
using NorthernSpectrums.MVVM.View.Windows;
using NorthernSpectrums.Services.NavigationService;
using NorthernSpectrums.Services.Presets;
using System.Text.RegularExpressions;

namespace NorthernSpectrums.MVVM.ViewModel
{
    /// <summary>
    /// <c>Class</c> The main window view model.
    /// </summary>
    public class MainWindowViewModel : Core.ViewModel
    {
        private readonly Func<Type, ApplicationWindow.ApplicationWindow> windowFactory;
        private readonly INavigationService navigationService;
        private readonly PresetSaverService presetSaverService;
        private string currentPresetPath = "";
        private Core.ViewModel endControlVm;

        // Property view bindings.
        public INavigationService NavigationService
        {
            get => navigationService;
        }
        public string CurrentPreset
        {
            get
            {
                string presetPath = currentPresetPath;
                Regex regex = new Regex(@"([^\\]+)(?=\.nspreset$)"); // Grab everything inbetween "\" and ".nspreset"
                return regex.Match(presetPath).Groups[1].Value; // Return the filtered group.
            }
            private set
            {
                currentPresetPath = value;
                OnPropertyChanged();
            }
        }
        public Core.ViewModel EndControlVm
        {
            get => endControlVm;
            set
            {
                endControlVm = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand SettingsWindowCommand { get; private set; }
        public RelayCommand SavePresetAsCommand { get; private set; }
        public RelayCommand SavePresetCommand { get; private set; }
        public RelayCommand LoadPresetCommand { get; private set; }
        public RelayCommand NavigateToAmp { get; private set; }
        public RelayCommand NavigateToPedals { get; private set; }
        public RelayCommand NavigateToRack { get; private set; }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the main window view model.
        /// </summary>
        /// <param name="navigationService">The navigation service. Used to navigate and get the current view model in navigation.</param>
        /// <param name="presetSaverService">The preset saver service. Used to access the preset saver methods.</param>
        /// <param name="windowFactory">A window factory. Used to access windows registered in the service provider.</param>
        public MainWindowViewModel(Func<Type, ApplicationWindow.ApplicationWindow> windowFactory, 
            INavigationService navigationService, 
            PresetSaverService presetSaverService,
            EndControlViewModel endControlVm)
        {
            this.windowFactory = windowFactory;
            this.navigationService = navigationService;
            this.presetSaverService = presetSaverService;
            this.endControlVm = endControlVm;

            // Create new RelayCommand bound to DataContext property, allowing us to invoke it from a view.
            SettingsWindowCommand = new RelayCommand(execute: o => OpenSettingsWindow(), canExecute: o => true);
            SavePresetCommand = new RelayCommand(execute: o => SavePreset(), canExecute: o => true);
            SavePresetAsCommand = new RelayCommand(execute: o => SavePresetAs(), canExecute: o => true);
            LoadPresetCommand = new RelayCommand(execute: o => LoadPreset(), canExecute: o => true);
            NavigateToAmp = new RelayCommand(execute: o => NavigateToAmpView(), canExecute: o => true);
            NavigateToPedals = new RelayCommand(execute: o => NavigateToPedalView(), canExecute: o => true);
            NavigateToRack = new RelayCommand(execute: o => NavigateToRackView(), canExecute: o => true);

            NavigationService.NavigatoTo<AmpViewModel>();
        }

        /// <summary>
        /// <c>Method</c> Opens the settings window.
        /// </summary>
        private void OpenSettingsWindow()
        {
            // Invoke windowFactory to get specified window instance.
            ApplicationWindow.ApplicationWindow settingsWindow = windowFactory.Invoke(typeof(SettingsWindow));
            settingsWindow.Show();
        }

        /// <summary>
        /// <c>Method</c> Saves the current preset.
        /// </summary>
        private void SavePreset()
        {
            CurrentPreset = presetSaverService.SavePreset(currentPresetPath);
        }

        /// <summary>
        /// <c>Method</c> Saves a new preset.
        /// </summary>
        private void SavePresetAs()
        {
            CurrentPreset = presetSaverService.SavePresetAs();
        }

        /// <summary>
        /// <c>Method</c> Loads a preset.
        /// </summary>
        private void LoadPreset()
        {
            CurrentPreset = presetSaverService.LoadPreset();
        }

        /// <summary>
        /// <c>Method</c> Navigates to the amp view.
        /// </summary>
        private void NavigateToAmpView()
        {
            NavigationService.NavigatoTo<AmpViewModel>();
        }

        /// <summary>
        /// <c>Method</c> Navigates to the pedal view.
        /// </summary>
        private void NavigateToPedalView()
        {
            NavigationService.NavigatoTo<PedalViewModel>();
        }

        /// <summary>
        /// <c>Method</c> Navigates to the rack view.
        /// </summary>
        private void NavigateToRackView()
        {
            NavigationService.NavigatoTo<RackViewModel>();
        }
    }
}
