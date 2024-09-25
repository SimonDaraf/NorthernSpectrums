using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.MVVM.Model.Audio.AudioComponents
{
    /// <summary>
    /// <c>Class</c> Converts a signal to a readable level.
    /// </summary>
    public class LevelReader : IEffectsProvider, ISampleProvider
    {
        /// <summary>
        /// <c>Property</c> Controls the gain.
        /// </summary>
        public float Gain { get; set; }

        public WaveFormat WaveFormat { get; set; }
        public ISampleProvider? SourceProvider { get; set; }

        /// <summary>
        /// <c>Event</c> Raised when data is available.
        /// </summary>
        public event EventHandler<LevelReaderDataAvailableEventArgs>? DataAvailable;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the level reader class.
        /// </summary>
        /// <param name="waveFormat"></param>
        public LevelReader()
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            Gain = 1;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            SourceProvider?.Read(buffer, offset, count);

            float[] unsignedData = new float[count];

            for (int i = 0; i < count; i++)
            {
                buffer[i] *= Gain;

                unsignedData[i] = buffer[i] * MathF.Abs(buffer[i]); // Make sure the array is unsigned.
            }

            // Raise the event.
            DataAvailable?.Invoke(this, new LevelReaderDataAvailableEventArgs(unsignedData));

            return count;
        }
    }

    /// <summary>
    /// <c>Class</c> Holds Level Reader DataAvailable Arguments
    /// </summary>
    public class LevelReaderDataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// <c>Property</c> An array of unsigned signal data.
        /// </summary>
        public float[] Data { get; set; }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the level reader data available event arguments.
        /// </summary>
        /// <param name="data"></param>
        public LevelReaderDataAvailableEventArgs(float[] data)
        {
            Data = data;
        }
    }
}
