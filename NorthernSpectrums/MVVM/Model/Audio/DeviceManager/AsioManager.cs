using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NorthernSpectrums.MVVM.Model.Audio.DeviceManager
{
    /// <summary>
    /// <c>Class</c> The ASIO manager class. Used to manage and utilize ASIO devices.
    /// </summary>
    public class AsioManager : IDeviceManager
    {
        private AsioOut? asioDriver;

        private int inputChannel;
        private int outputChannel;
        private string selectedDriverName;
        private ISampleProvider? sampleProvider;
        private WaveFormat waveFormat;

        public WaveFormat WaveFormat
        {
            get => waveFormat;
            private set
            {
                waveFormat = value;
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs a new instance of a Device Manager.
        /// </summary>
        public AsioManager()
        {
            // Default for any device.
            inputChannel = 0;
            outputChannel = 0;
            selectedDriverName = "";
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        }

        public void CloseDevice()
        {
            // Can be called for cleanup by main application, therefore we do a null check even if it has already been done.
            if (asioDriver != null)
            {
                asioDriver!.AudioAvailable -= AsioDriver_AudioAvailable;
                asioDriver.Dispose();
                asioDriver = null;
            }
        }

        public string[] GetDevices()
        {
            return AsioOut.GetDriverNames();
        }

        public string[] GetInputs()
        {
            // If no asio driver has been instantiated, return an empty array.
            if (asioDriver == null)
            {
                return [];
            }

            int inputChannelCount = asioDriver.DriverInputChannelCount;
            string[] inputChannels = new string[inputChannelCount];

            for (int i = 0; i < inputChannelCount; i++)
            {
                // Get input channel name via channel index.
                inputChannels[i] = asioDriver.AsioInputChannelName(i);
            }

            return inputChannels;
        }

        public string[] GetOutputs()
        {
            // If no asio driver has been instantiated, return an empty array.
            if (asioDriver == null)
            {
                return [];
            }

            int outputChannelCount = asioDriver.DriverOutputChannelCount;
            string[] outputChannels = new string[outputChannelCount / 2];

            for (int i = 0; i < outputChannelCount / 2; i++)
            {
                // Get output channel name via channel index.
                // We want to couple ouput [1, 2], [3, 4] etc.
                outputChannels[i] = asioDriver.AsioOutputChannelName(i * 2) + " + " + asioDriver.AsioOutputChannelName((i * 2) + 1);
            }

            return outputChannels;
        }

        public void OpenSettingsMenu()
        {
            asioDriver?.ShowControlPanel();
        }

        public void SelectDriver(string driverName)
        {
            selectedDriverName = driverName;

            // Check if an instance is currently active.
            if (asioDriver != null)
            {
                CloseDevice();
            }

            // Create a new asioOut instance with the selected driver.
            // If ASIO out can't be instantiated, selected ASIO driver isn't available.
            // This problem occurs due to the fact that listed drivers are only installed drivers, and can not be confirmed as utilized.
            // This will need to be catched somewhere, preferably where the user can be notified and prompted to select a new driver.
            AsioOut asioOut = new(driverName);

            // Set current asio driver to the newly created one.
            asioDriver = asioOut;
        }

        public void SelectInputChannel(int channel)
        {
            if (channel < 0 || channel > asioDriver?.DriverInputChannelCount)
            {
                return;
            }
            inputChannel = channel;
        }

        public void SelectOutputChannel(int channel)
        {
            if (channel < 0 || channel > asioDriver?.DriverOutputChannelCount)
            {
                return;
            }
            // If index is for example 1 we want 2, 3 -> 6 etc.
            outputChannel = channel * 2;
        }

        public void InitializeInput()
        {
            if (asioDriver != null)
            {
                waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, asioDriver.DriverOutputChannelCount);
            }
        }

        public void InitializeOutput(ISampleProvider? provider)
        {
            try
            {
                if (asioDriver != null)
                {
                    SelectDriver(selectedDriverName);

                    // Subscribe to the audio available event.
                    asioDriver.AudioAvailable += AsioDriver_AudioAvailable;

                    // Sample provider is only necessary if effect providers are present.
                    // We do a null propagation later when reading the data.
                    sampleProvider = provider;

                    asioDriver.InitRecordAndPlayback(new BufferedWaveProvider(waveFormat), asioDriver.DriverInputChannelCount, 44100);
                    asioDriver.Play();
                }
            }
            catch (Exception)
            {
                // This will be prettier when I allow changes to sample count.
                CloseDevice();
                return;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles Asio data recorded, and increases buffersize to avoid audio quality problems.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Data related to the event.</param>
        private void AsioDriver_AudioAvailable(object? sender, AsioAudioAvailableEventArgs e)
        {
            // We set up input buffer and increase the buffer size, not sure why this is needed but otherwise the quality is... bad.
            byte[] inputBuffer = new byte[e.SamplesPerBuffer * 4];

            int samplesPerBuffer = e.SamplesPerBuffer;

            // Copy input data to output buffer.
            // Read from selected input channel;
            Marshal.Copy(e.InputBuffers[inputChannel], inputBuffer, 0, samplesPerBuffer * 4);

            float maxVal = 1;
            float minVal = -1;

            // Convert raw buffer data to 32-bit PCM.
            float[] floatBuffer = new float[samplesPerBuffer];
            for (int i = 0; i < samplesPerBuffer; i++)
            {
                int sampleValue = BitConverter.ToInt32(inputBuffer, i * 4); // 4-bytes per sample.
                float sample = (float)sampleValue / int.MaxValue; // Normalize to range -1, 1.
                floatBuffer[i] = sample;

                minVal = MathF.Min(minVal, sample);
                maxVal = MathF.Max(maxVal, sample);
            }

            // If any effect providers are present, we read from them.
            sampleProvider?.Read(floatBuffer, 0, samplesPerBuffer);

            // Convert back into byte array.
            byte[] processedBuffer = new byte[e.SamplesPerBuffer * 4];
            for (int i = 0; i < samplesPerBuffer; i++)
            {
                int intValue = (int)(floatBuffer[i] * int.MaxValue);
                byte[] bytes = BitConverter.GetBytes(intValue);
                Buffer.BlockCopy(bytes, 0, processedBuffer, i * 4, 4);
            }

            // Route processed buffer data to each output channel.
            for (int i = outputChannel; i <= outputChannel + 1; i++)
            {
                IntPtr outputPtr = e.OutputBuffers[i];
                Marshal.Copy(processedBuffer, 0, outputPtr, samplesPerBuffer * 4);
            }

            // Indicate that we have written to the output buffers, so that Asio doesn't read directly from the source. 
            e.WrittenToOutputBuffers = true;
        }

        ~AsioManager()
        {
            CloseDevice();
        }
    }
}