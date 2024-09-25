namespace NorthernSpectrums.MVVM.Model.Audio.AudioModules
{
    /// <summary>
    /// <c>Interface</c> A simplified version of the ISampleProvider, but without the WaveFormat.
    /// </summary>
    public interface IAudioModule
    {
        /// <summary>
        /// <c>Method</c> Reads incomming buffer data.
        /// </summary>
        /// <param name="buffer">The buffer data.</param>
        /// <param name="offset"> The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>The amount of samples read.</returns>
        public int Read(float[] buffer, int offset, int count);
    }
}
