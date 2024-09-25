using NAudio.Wave;

namespace NorthernSpectrums.MVVM.Model.Audio.EffectsProviders
{
    /// <summary>
    /// <c>Interface</c> Extends the effetcs provider with a source property.
    /// </summary>
    public interface IEffectsProvider
    {
        /// <summary>
        /// The source to read from.
        /// The source can be changed depending on signal chain structure.
        /// </summary>
        ISampleProvider? SourceProvider { set; }

        /// <summary>
        /// The WaveFormat to be used, in this case this will be a 32 bit IEEE floating point wave format.
        /// </summary>
        WaveFormat WaveFormat { get; }

        /// <summary>
        /// <c>Method</c> Fill the specified buffer with 32 bit floating point samples. Based on NAudios ISampleProvider.
        /// </summary>
        /// <param name="buffer">The buffer to fill with samples.</param>
        /// <param name="offset">Offset into buffer.</param>
        /// <param name="count">The number of samples to read</param>
        /// <returns>the number of samples written to the buffer.</returns>
        public int Read(float[] buffer, int offset, int count);
    }
}
