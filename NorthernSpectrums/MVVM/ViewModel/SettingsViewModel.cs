using NorthernSpectrums.Core;
using NorthernSpectrums.Services.NavigationService;

namespace NorthernSpectrums.MVVM.ViewModel
{
    /// <summary>
    /// <c>Class</c> The settings view model.
    /// </summary>
    public class SettingsViewModel : Core.ViewModel
    {
        // Private fields.
        private INavigationService navigationService;
        private bool audioViewActive;
        private bool generalViewActive;

        // Property bindings.
        public INavigationService NavigationService
        {
            get { return navigationService; }
            set
            {
                navigationService = value;
                OnPropertyChanged();
            }
        }
        public bool AudioViewActive
        {
            get => audioViewActive;
            set
            {
                audioViewActive = value;
                if (value == true)
                {
                    GeneralViewActive = false;
                }
                OnPropertyChanged();
            }
        }
        public bool GeneralViewActive
        {
            get => generalViewActive;
            set
            {
                generalViewActive = value;
                if (value == true)
                {
                    AudioViewActive = false;
                }
                OnPropertyChanged();
            }
        }
        public RelayCommand NavigateToAudio { get; set; }
        public RelayCommand NavigateToGeneral { get; set; }

        /// <summary>
        /// <c>Constructor</c> Constructs a new instance of the settings view model.
        /// </summary>
        /// <param name="navigationService"></param>
        public SettingsViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            // Setup relay command to navigate to views.
            NavigateToAudio = new RelayCommand(execute: o => { NavigationService.NavigatoTo<AudioSettingsViewModel>(); }, canExecute: o => true);
            NavigateToGeneral = new RelayCommand(execute: o => { NavigationService.NavigatoTo<GeneralSettingsViewModel>(); }, canExecute: o => true);

            // Set default active navigation button.
            AudioViewActive = true;

            NavigationService.NavigatoTo<AudioSettingsViewModel>(); // We set audio view to default.
        }
    }
}
