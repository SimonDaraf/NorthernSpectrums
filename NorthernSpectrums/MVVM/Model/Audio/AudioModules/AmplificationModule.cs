using NAudio.Wave;

namespace NorthernSpectrums.MVVM.Model.Audio.AudioModules
{
    /// <summary>
    /// <c>Class</c> An audio module responsible for amplifying a signal.
    /// </summary>
    public class AmplificationModule : IAudioModule
    {
        /// <summary>
        /// <c>Property</c> Controls the signal gain.
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the amplification module.
        /// </summary>
        /// <param name="gain">The signal gain.</param>
        public AmplificationModule(float gain)
        {
            Gain = gain;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i] *= Gain;
            }

            return count;
        }
    }
}
