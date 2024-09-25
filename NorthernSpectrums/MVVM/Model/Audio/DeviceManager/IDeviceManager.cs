using NAudio.Wave;

namespace NorthernSpectrums.MVVM.Model.Audio.DeviceManager
{
    /// <summary>
    /// <c>Interface</c> The IDeviceManager interface.
    /// </summary>
    public interface IDeviceManager
    {
        public WaveFormat WaveFormat { get; }

        /// <summary>
        /// <c>Method</c> Gets all available devices in the current context.
        /// </summary>
        /// <returns>An array of devices as strings.</returns>
        public string[] GetDevices();

        /// <summary>
        /// <c>Method</c> Gets all available inputs in the current context.
        /// </summary>
        /// <returns>An array of inputs as strings.</returns>
        public string[] GetInputs();

        /// <summary>
        /// <c>Method</c> Gets all available outputs in the current context.
        /// </summary>
        /// <returns>An array of outputs as strings.</returns>
        public string[] GetOutputs();

        /// <summary>
        /// <c>Method</c> Selects the input channel to be used.
        /// </summary>
        /// <param name="channel">The selected channel index.</param>
        public void SelectInputChannel(int channel);

        /// <summary>
        /// <c>Method</c> Selects the output channel to be used.
        /// </summary>
        /// <param name="channel">The selected channel index.</param>
        public void SelectOutputChannel(int channel);

        /// <summary>
        /// <c>Method</c> Selects a driver to be utilized in the current context.
        /// </summary>
        /// <param name="driverName">The driver name.</param>
        public void SelectDriver(string driverName);

        /// <summary>
        /// <c>Method</c> Initializes the device managers input.
        /// </summary>
        public void InitializeInput();

        /// <summary>
        /// <c>Method</c> Initializes the device managers output.
        /// <param name="waveProvider"/>The final waveProvider after signal chain has been set up.
        /// </summary>
        public void InitializeOutput(ISampleProvider? provider);

        /// <summary>
        /// <c>Method</c> Closes the selected device, used for cleanup.
        /// </summary>
        public void CloseDevice();

        /// <summary>
        /// <c>Method</c> Opens the relevant settings menu for the specific device (if any).
        /// </summary>
        public void OpenSettingsMenu();
    }
}
