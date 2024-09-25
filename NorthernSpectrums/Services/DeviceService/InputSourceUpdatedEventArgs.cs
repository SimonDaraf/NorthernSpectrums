using NAudio.Wave;

namespace NorthernSpectrums.Services.DeviceService
{
    /// <summary>
    /// <c>EventArgs</c> Provides event arguments when a input source has been updated.
    /// </summary>
    public class InputSourceUpdatedEventArgs : EventArgs
    {
        private ISampleProvider inputSource;
        public ISampleProvider InputSource
        {
            get => inputSource;
        }

        /// <summary>
        /// <c>Constructor</c> Sets the input source.
        /// </summary>
        /// <param name="inputSource">The input source.</param>
        public InputSourceUpdatedEventArgs(ISampleProvider inputSource)
        {
            this.inputSource = inputSource;
        }
    }
}
