namespace NorthernSpectrums.MVVM.Model.Matrices
{
    /// <summary>
    /// <c>Class</c> provides a Householder matrix helper class, based on implementation by Geraint Luff.
    /// <see cref="https://github.com/Signalsmith-Audio/reverb-example-code/blob/main/mix-matrix.h"/>
    /// </summary>
    public class HouseholderMatrix
    {
        public static void InPlace(float[] data)
        {
            float sum = 0;
            for (int i = 0; i < data.Length; ++i)
            {
                sum += data[i];
            }

            sum *= -2.0f / data.Length;

            for (int i = 0; i < data.Length; ++i)
            {
                data[i] += sum;
            }
        }
    }
}
