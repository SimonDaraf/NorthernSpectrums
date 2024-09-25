using NAudio.CoreAudioApi.Interfaces;

namespace NorthernSpectrums.MVVM.Model.Audio.AudioComponents
{
    /// <summary>
    /// <c>Class</c> Represents a first in first out float data storage.
    /// </summary>
    public class Fifo
    {
        private float[] storage;
        private int offset;
        private int pos;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the FIFO class.
        /// </summary>
        public Fifo()
        {
            storage = [];
            offset = 0;
            pos = 0;
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the FIFO class.
        /// </summary>
        /// <param name="size">The size of the data storage.</param>
        public Fifo(int size)
        {
            storage = new float[size];
        }

        /// <summary>
        /// <c>Method</c> Reconfigures the data storage with a new size.
        /// </summary>
        /// <param name="size">The new size.</param>
        /// <param name="offset">The offset to be used.</param>
        public void Reconfigure(int size, int offset)
        {
            storage = new float[size];
            this.offset = offset;
            pos = 0;
        }

        /// <summary>
        /// <c>Method</c> Pushes a new dataSample into the storage and increases the position to read from.
        /// </summary>
        /// <param name="sampleIn">The sample to push into the storage.</param>
        public void Push(float sampleIn)
        {
            storage[pos] = sampleIn;
            pos++; // Increment pos.

            if (pos >= storage.Length)
            {
                pos = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Pops the sample at offset pos.
        /// </summary>
        /// <returns>The offset sample.</returns>
        public float Pop()
        {
            float sampleOut;
            try
            {
                if (pos - offset < 0)
                {
                    // If offset pos is less than zero, offset from end of storage.
                    sampleOut = storage[storage.Length - (offset - pos)];
                }
                else
                {
                    sampleOut = storage[0];
                }

                return sampleOut;
            }
            catch (IndexOutOfRangeException)
            {
                return storage[0];
            }
        }
    }
}
