using Microsoft.VisualBasic;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.Asio;
using System.Runtime.InteropServices;

namespace NorthernSpectrums.MVVM.Model.Audio.DeviceManager
{
    /// <summary>
    /// <c>Class</c> Manages a WASAPI device.
    /// </summary>
    public class WasapiManager : IDeviceManager
    {
        // Device enumerator used to access available input and output devices.
        private readonly MMDeviceEnumerator mDeviceEnumerator;
        private MMDevice[] availableInputDevices = [];
        private MMDevice[] availableOutputDevices = [];
        private MMDevice inputDevice;
        private MMDevice outputDevice;

        private int inputDeviceIndex;
        
        private AudioClientShareMode selectedAudioShareMode;
        private WasapiOut? wasapiDriver;
        private WasapiCapture? waveIn;

        private ISampleProvider? sourceProvider;

        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the WasapiManager class.
        /// </summary>
        public WasapiManager()
        {
            // Default setup.
            mDeviceEnumerator = new MMDeviceEnumerator();
            selectedAudioShareMode = AudioClientShareMode.Shared;

            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

            // In case of no valid devices.
            MMDevice[] inputDevices = [.. mDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)];
            inputDevice = inputDevices.Length > 0 ? inputDevices[0] : throw new UnsupportedDriverModeException();

            MMDevice[] outputDevices = [.. mDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)];
            outputDevice = outputDevices.Length > 0 ? outputDevices[0] : throw new UnsupportedDriverModeException();

            inputDeviceIndex = -1;
        }

        public void CloseDevice()
        {
            if (wasapiDriver != null)
            {
                wasapiDriver.Stop();
                wasapiDriver.Dispose();
                wasapiDriver = null;
            }

            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }
        }

        public string[] GetDevices()
        {
            // Return an array of share modes.
            return Enum.GetValues(typeof(AudioClientShareMode)).Cast<AudioClientShareMode>().Select(shareMode => shareMode.ToString()).ToArray();
        }

        public string[] GetInputs()
        {
            // Get all active capture devices.
            MMDeviceCollection inputDevices = mDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            availableInputDevices = [.. inputDevices];

            // Return only the device friendly names.
            return availableInputDevices.Select(device => device.DeviceFriendlyName).ToArray();
        }

        public string[] GetOutputs()
        {
            // Get all active output devices.
            MMDeviceCollection outputDevices = mDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            availableOutputDevices = [.. outputDevices];

            // Return only the device friendly names.
            return availableOutputDevices.Select(device => device.DeviceFriendlyName).ToArray();
        }

        public void OpenSettingsMenu()
        {
            throw new NotImplementedException();
        }

        public void SelectDriver(string driverName)
        {
            if (wasapiDriver != null)
            {
                CloseDevice();
            }

            // Instantiate a wasapi driver.
            selectedAudioShareMode = GetShareMode(driverName);
            wasapiDriver = new(outputDevice, selectedAudioShareMode, true, 20);
        }

        public void SelectInputChannel(int channel)
        {
            if (availableInputDevices.Length != 0)
            {
                inputDevice = availableInputDevices[channel];
            }
        }

        public void SelectOutputChannel(int channel)
        {
            if (availableOutputDevices.Length != 0)
            {
                outputDevice = availableOutputDevices[channel];
            }
        }

        /// <summary>
        /// <c>Method</c> Determines which share mode to use.
        /// </summary>
        /// <param name="driverName">Selected share mode.</param>
        /// <returns>The selected share mode enumerator.</returns>
        private static AudioClientShareMode GetShareMode(string driverName)
        {
            // Return switch statement.
            return driverName switch
            {
                "Shared" => AudioClientShareMode.Shared,
                "Exclusive" => AudioClientShareMode.Exclusive,
                _ => AudioClientShareMode.Shared,
            };
        }
        public void InitializeInput()
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        }

        public void InitializeOutput(ISampleProvider? provider)
        {
            sourceProvider = provider;

            // Initialize a source provider.
            waveIn = new WasapiCapture(inputDevice);
            waveIn.ShareMode = selectedAudioShareMode;
            waveIn.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(inputDeviceIndex).Channels);

            // Bridge wave in with a wave in provider.
            WaveInProvider waveProvider = new(waveIn);

            // Initialize wasapi driver.
            wasapiDriver!.Init(waveProvider);

            waveIn.DataAvailable += WasapiDriver_DataAvailable;

            waveIn.StartRecording();
            wasapiDriver!.Play();
        }

        private void WasapiDriver_DataAvailable(object? sender, WaveInEventArgs e)
        {
            // Increase buffer size.
            byte[] inputBuffer = new byte[e.BytesRecorded * 4];

            Array.Copy(e.Buffer, inputBuffer, e.BytesRecorded);

            float[] floatBuffer = new float[e.BytesRecorded];

            float maxVal = 1;
            float minVal = -1;

            for (int i = 0; i < e.BytesRecorded; i++)
            {
                int sampleValue = BitConverter.ToInt32(inputBuffer, i * 4); // 4-bytes per sample.
                float sample = (float)sampleValue / int.MaxValue; // Normalize to range -1, 1.
                floatBuffer[i] = sample;

                minVal = MathF.Min(minVal, sample);
                maxVal = MathF.Max(maxVal, sample);
            }

            // If any effect providers are present, we read from them.
            sourceProvider?.Read(floatBuffer, 0, e.BytesRecorded);

            // Convert back into byte array.
            byte[] processedBuffer = new byte[e.BytesRecorded * 4];
            for (int i = 0; i < e.BytesRecorded; i++)
            {
                int intValue = (int)(floatBuffer[i] * int.MaxValue);
                byte[] bytes = BitConverter.GetBytes(intValue);
                Buffer.BlockCopy(bytes, 0, processedBuffer, i * 4, 4);
            }
        }

        ~WasapiManager()
        {
            CloseDevice();
        }
    }
}
