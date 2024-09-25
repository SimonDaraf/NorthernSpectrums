using NAudio.Wave;
using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.AudioComponents;
using NorthernSpectrums.MVVM.Model.Audio.DeviceManager;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.Services.DeviceService
{
    /// <summary>
    /// <c>Class</c> Used to handle a device manager.
    /// </summary>
    public class DeviceService : IDeviceService
    {
        // Create an empty asio manager to make sure it isn't null.
        private IDeviceManager deviceManager = new AsioManager();

        // Used to preserve state.
        private Driver selectedDriverMode;
        private string selectedDriver = "";
        private int selectedInput = 0;
        private int selectedOutput = 0;
        private IEffectsProvider levelReader;

        private ISampleProvider? outputSource;

        // Provider getters.
        public Driver SelectedDriverMode 
        {
            get => selectedDriverMode; 
            private set
            {
                selectedDriverMode = value;
            } 
        }
        public string SelectedDriver 
        {
            get => selectedDriver; 
            private set
            {
                selectedDriver = value;
            }
        }
        public int SelectedInput
        {
            get => selectedInput;
            private set
            {
                selectedInput = value;
            }
        }
        public int SelectedOutput
        {
            get => selectedOutput;
            private set
            {
                selectedOutput = value;
            }
        }
        public ISampleProvider? OutputSource
        {
            set
            {
                outputSource = value;
                levelReader.SourceProvider = value;
                deviceManager.InitializeOutput((ISampleProvider)levelReader);
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the Device Service class.
        /// </summary>
        public DeviceService(LevelReader levelReader)
        {
            this.levelReader = levelReader; // Used to read levels to display.
            UpdateDriverMode(selectedDriverMode);
        }

        public void UpdateDriverMode(Driver driverMode)
        {
            // If an instance of device manager is active, close it first.
            deviceManager?.CloseDevice();

            selectedDriverMode = driverMode;

            // This has no meaning as of now, I planned on supporting multiple drivers. But due to time restrictions and certain limitations this will be skipped.
            try
            {
                // Create a new device manager depending on selected driver mode.
                switch (driverMode)
                {
                    case Driver.ASIO:
                        deviceManager = new AsioManager();
                        break;
                    default:
                        // It should never reach this, but just to make sure.
                        deviceManager = new AsioManager();
                        break;
                }
            }
            catch (UnsupportedDriverModeException)
            {
                return;
            }
        }

        public bool ValidateDriverMode()
        {            
            // If an empty array is returned, the driver mode is not valid.
            string[] devices = deviceManager.GetDevices();
            if (devices.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateDriver(string driverName)
        {
            // If an invalid driver is selected an exception is thrown.
            // In that case we catch it and return false to indicate an invalid selection.
            try
            {
                deviceManager.SelectDriver(driverName);
                return true;
            } 
            catch (Exception)
            {
                return false;
            }
        }

        public string[] GetDrivers()
        {
            return deviceManager.GetDevices();
        }

        public string[] GetInputs()
        {
            return deviceManager.GetInputs();
        }

        public string[] GetOutputs()
        {
            return deviceManager.GetOutputs();
        }

        public void SetInput(int input)
        {
            deviceManager.SelectInputChannel(input);
            deviceManager.InitializeInput();
        }

        public void SetOutput(int output)
        {
            deviceManager.SelectOutputChannel(output);
            deviceManager.InitializeOutput((ISampleProvider)levelReader);
        }

        public void OpenDeviceSettingsMenu()
        {
            try
            {
                deviceManager.OpenSettingsMenu();
            }
            catch (NotImplementedException)
            {
                // If device doesn't contain a settings menu.
                return;
            }
        }
    }
}
