using NorthernSpectrums.Core;
using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.Services.DeviceService;
using System.Windows;

namespace NorthernSpectrums.MVVM.ViewModel
{
    /// <summary>
    /// <c>Class</c> The Audio settings viewmodel.
    /// </summary>
    public class AudioSettingsViewModel : Core.ViewModel, IPreservable
    {
        private readonly IDeviceService deviceService;

        // Private fields.
        private Driver selectedDriverMode;
        private string selectedDriver;
        private int selectedInput;
        private int selectedOutput;
        private Driver[] driverModeSource = [];
        private string[] driverSource = [];
        private string[] inputSource = [];
        private string[] outputSource = [];
        private Visibility driverSelectionVisibility;
        private Visibility asioSettingsVisibility;
        private string driverSelectionText;

        // Properties used for view binding.
        public RelayCommand AsioSettingsCommand { get; set; }
        public Visibility DriverSelectionVisibility
        {
            get => driverSelectionVisibility;
            set
            {
                driverSelectionVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility AsioSettingVisibility
        {
            get => asioSettingsVisibility;
            set
            {
                asioSettingsVisibility = value;
                OnPropertyChanged();
            }
        }
        public string DriverSelectionText
        {
            get => driverSelectionText;
            set
            {
                driverSelectionText = value;
                OnPropertyChanged();
            }
        }
        public Driver SelectedDriverMode
        {
            get => selectedDriverMode;
            set
            {
                selectedDriverMode = value;
                OnPropertyChanged();
                ValidateSelectedDriverMode();
            }
        }
        public string SelectedDriver
        {
            get => selectedDriver;
            set
            {
                selectedDriver = value;
                OnPropertyChanged();
                ValidateSelectedDriver();
            }
        }
        public int SelectedInput
        {
            get => selectedInput;
            set
            {
                selectedInput = value;
                OnPropertyChanged();
                OnSelectedInput();
            }
        }
        public int SelectedOutput
        {
            get => selectedOutput;
            set
            {
                selectedOutput = value;
                OnPropertyChanged();
                OnSelectedOutput();
            }
        }
        public Driver[] DriverModeSource
        {
            get => driverModeSource;
            private set
            {
                driverModeSource = value;
                OnPropertyChanged();
            }
        }
        public string[] DriverSource
        {
            get => driverSource;
            private set
            {
                driverSource = value;
                OnPropertyChanged();
            }
        }
        public string[] InputSource
        {
            get => inputSource;
            set
            {
                inputSource = value;
                OnPropertyChanged();
            }
        }
        public string[] OutputSource
        {
            get => outputSource;
            set
            {
                outputSource = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// <c>Constructor</c> constructs ans instance of the AudioSettingsViewmodel class.
        /// </summary>
        /// <param name="deviceService">The device service singleton instance to be utilized.</param>
        public AudioSettingsViewModel(IDeviceService deviceService)
        {
            this.deviceService = deviceService;

            DriverModeSource = Enum.GetValues(typeof(Driver)).Cast<Driver>().ToArray();

            // Get settings from preserved state.
            // Will need to be changed when saved settings are introduced.
            SelectedDriverMode = deviceService.SelectedDriverMode;
            selectedDriver = deviceService.SelectedDriver;
            selectedInput = deviceService.SelectedInput;
            selectedOutput = deviceService.SelectedOutput;
            driverSelectionText = "Driver:"; // Too avoid null warning.

            DriverSource = deviceService.GetDrivers();

            // Relay command for asio settings.
            AsioSettingsCommand = new RelayCommand(execute: o => OpenAsioSettings(), canExecute: o => true);
        }

        /// <summary>
        /// <c>Method</c> Validates driver mode source.
        /// </summary>
        private void ValidateSelectedDriverMode()
        {
            // If no changes has been made, return.
            if (selectedDriverMode == deviceService.SelectedDriverMode)
            {
                return;
            }

            DetermineVisibility();

            // Update the selected driver mode.
            deviceService.UpdateDriverMode(selectedDriverMode);

            // Empty all the sources to avoid showing incorrect options.
            DriverSource = [];
            InputSource = [];
            OutputSource = [];

            // If user currently can't use the selected driver mode.
            if (!deviceService.ValidateDriverMode())
            {
                MessageBox.Show("No drivers found. Please check if any devices are available.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // After selecting a driver mode, validate recieved drivers.
            ValidateDrivers();
        }

        /// <summary>
        /// <c>Method</c> After a driver mode has been selected this validates that the drivers returned are valid.
        /// An empty array is not valid.
        /// </summary>
        private void ValidateDrivers()
        {
            string[] drivers = deviceService.GetDrivers();

            if (drivers.Length == 0)
            {
                MessageBox.Show($"No {selectedDriverMode} drivers found. Please try another Driver Mode", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Try selecting the first available driver, should be standard.
            DriverSource = drivers;
            SelectedDriver = drivers[0];
        }

        /// <summary>
        /// <c>Method</c> Validates selected driver.
        /// </summary>
        private void ValidateSelectedDriver()
        {
            // If no changes has been made, return.
            // When the users switches driver mode, combobox sets selection to null, so we need to check it.
            if (selectedDriver == deviceService.SelectedDriver || selectedDriver == null)
            {
                return;
            }

            // If selected driver isn't valid, display to user.
            if(!deviceService.ValidateDriver(selectedDriver))
            {
                MessageBox.Show($"The selected driver {selectedDriver} could not be loaded.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                // Reset input and output to avoid user being able to select old inputs when they don't exist.
                InputSource = [];
                OutputSource = [];
                return;
            }

            GetInputsAndOutputs();
        }

        /// <summary>
        /// <c>Method</c> Gets inputs and outputs and stores them in the bound properties.
        /// </summary>
        private void GetInputsAndOutputs()
        {
            InputSource = deviceService.GetInputs();
            OutputSource = deviceService.GetOutputs();

            // We need to re-apply the values to show them in the view.
            // Values are set in the constructor from the device service.
            // As default they are set to 0.
            if (inputSource.Length > 0)
            {
                SelectedInput = selectedOutput != -1 ? selectedInput : 0;
            }
            if (outputSource.Length > 0)
            {
                SelectedOutput = selectedOutput != -1 ? selectedOutput : 0;
            }
        }

        /// <summary>
        /// <c>Method</c> When the selected input changes.
        /// </summary>
        private void OnSelectedInput()
        {
            if (selectedInput >= 0)
            {
                deviceService.SetInput(selectedInput);
            }
        }

        /// <summary>
        /// <c>Method</c> When the selected output changes.
        /// </summary>
        private void OnSelectedOutput()
        {
            if (selectedOutput >= 0)
            {
                deviceService.SetOutput(selectedOutput);
            }
        }

        /// <summary>
        /// <c>Method</c> Determines which selections should be visible depending on driver mode.
        /// </summary>
        private void DetermineVisibility()
        {
            // Determine whether driver selection should be visible.
            //DriverSelectionVisibility = selectedDriverMode == Drivers.Driver.ASIO || selectedDriverMode == Drivers.Driver.WASAPI? Visibility.Visible : Visibility.Collapsed;

            // Determine whether ASIO specific settings should be visible.
            AsioSettingVisibility = selectedDriverMode == Driver.ASIO? Visibility.Visible : Visibility.Collapsed;

            // Determine driver selection text,
            // will not be visible if selection is WaveOut,
            // but still set it to not bloat method with too many if else.
            DriverSelectionText = selectedDriverMode == Driver.ASIO ? "Driver:" : "Share Mode:";
        }

        /// <summary>
        /// <c>Method</c> Opens the asio settings menu from the Audio Settings View Model.
        /// </summary>
        private void OpenAsioSettings()
        {
            deviceService.OpenDeviceSettingsMenu();
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>
            {
                { "DriverMode", SelectedDriverMode },
                { "Driver", SelectedDriver },
                { "Input", SelectedInput },
                { "Output", SelectedOutput },
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            try
            {
                foreach (KeyValuePair<string, object> entry in data)
                {
                    switch (entry.Key)
                    {
                        case "DriverMode":
                            _ = Enum.TryParse((string)entry.Value, out Driver parsedEnum) == true ? 
                                SelectedDriverMode = parsedEnum : SelectedDriverMode = Driver.ASIO; // If not a valid enum, set du default.
                            break;
                        case "Driver":
                            string driver = (string)entry.Value;
                            SelectedDriver = driver;
                            break;
                        case "Input":
                            int input = (int)entry.Value;
                            SelectedInput = input;
                            break;
                        case "Output":
                            int output = (int)entry.Value;
                            SelectedOutput = output;
                            break;
                        default:
                            break; // Sometimes best to do nothing.
                    }
                }
            }
            catch (Exception)
            {
                // We only care about what that an exception was thrown, not what threw it.
                // It is better to just stop then and let the user reconfigure in that case.
                return;
            }
        }
    }
}
