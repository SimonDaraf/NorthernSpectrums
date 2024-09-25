namespace NorthernSpectrums.MVVM.Model.Audio.AudioModules
{
    /// <summary>
    /// <c>Class</c> Performs a phase invertion.
    /// </summary>
    public class PhaseInvertionModule : IAudioModule
    {
        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i] *= -1;
            }

            return count;
        }
    }
}
