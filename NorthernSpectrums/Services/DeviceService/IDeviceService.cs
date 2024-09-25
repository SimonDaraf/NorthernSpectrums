using NAudio.Wave;
using NorthernSpectrums.MVVM.Model;

namespace NorthernSpectrums.Services.DeviceService
{
    /// <summary>
    /// <c>Interface</c> Used to create a singletone device service.
    /// </summary>
    public interface IDeviceService
    {
        public Driver SelectedDriverMode { get;}
        public string SelectedDriver { get; }
        public int SelectedInput { get; }
        public int SelectedOutput { get; }
        public ISampleProvider? OutputSource { set; }

        /// <summary>
        /// <c>Method</c> Use this to update driver instance instead of creating a new one.
        /// This will ensure that the previous device is properly closed.
        /// </summary>
        /// <param name="driverMode">The new driver mode to be instantiated.</param>
        public void UpdateDriverMode(Driver driverMode);

        /// <summary>
        /// <c>Method</c> Returns an array of available drivers from the selected driver mode.
        /// </summary>
        /// <returns></returns>
        public string[] GetDrivers();

        /// <summary>
        /// <c>Method</c> Returns an array of available inputs.
        /// </summary>
        /// <returns></returns>
        public string[] GetInputs();

        /// <summary>
        /// <c>Method</c> Returns an array of available outputs.
        /// </summary>
        /// <returns></returns>
        public string[] GetOutputs();

        /// <summary>
        /// <c>Method</c> Validates the selected driver mode.
        /// This should validate through the current DeviceManager.
        /// </summary>
        /// <returns>A boolean indicating if the selected driver mode is valid.</returns>
        public bool ValidateDriverMode();

        /// <summary>
        /// <c>Method</c> Validates the selected driver.
        /// </summary>
        /// <param name="driverName">The selected driver name.</param>
        /// <returns>A boolean indicating if the selected driver is valid.</returns>
        public bool ValidateDriver(string driverName);

        /// <summary>
        /// <c>Method</c> Sets input to the selected channel index.
        /// </summary>
        /// <param name="input">Selected channel index.</param>
        public void SetInput(int input);

        /// <summary>
        /// <c>Method</c> Sets output to the selected channel index.
        /// </summary>
        /// <param name="input">Selected channel index.</param>
        public void SetOutput(int output);

        /// <summary>
        /// <c>Method</c> Opens the device current settings menu, if any.
        /// </summary>
        public void OpenDeviceSettingsMenu();
    }
}
