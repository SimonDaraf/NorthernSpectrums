namespace NorthernSpectrums.MVVM.Model.Matrices
{
    /// <summary>
    /// <c>Class</c> Provides a Hadamard Matrix helper class, based on implementation by Geraint Luff.
    /// <see cref="https://github.com/Signalsmith-Audio/reverb-example-code/blob/main/mix-matrix.h"/>
    /// </summary>
    public class HadamardMatrix
    {
        public static void RecursiveUnscaled(float[] data)
        {
            int size = data.Length;

            if (size <= 1)
            {
                return;
            }

            int hSize = size / 2;

            // Two (unscaled) Hadamards of half the size.
            // Split the float array into two segments.

            float[] s1 = new ArraySegment<float>(data, 0, hSize).ToArray();
            float[] s2 = new ArraySegment<float>(data, hSize, hSize).ToArray();

            RecursiveUnscaled(s1);
            RecursiveUnscaled(s2);

            // Combine the two halves using sum/difference
            for (int i = 0; i < hSize; i++)
            {
                float a = s1[i];
                float b = s2[i];
                data[i] = a + b;
                data[i + hSize] = a - b;
            }
        }

        public static void InPlace(float[] data)
        {
            RecursiveUnscaled(data);

            float scalingFactor = MathF.Sqrt(1.0f / data.Length);
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] *= scalingFactor;
            }
        }
    }
}
