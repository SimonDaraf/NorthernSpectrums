using NorthernSpectrums.MVVM.Model.Audio.AudioComponents;

namespace NorthernSpectrums.MVVM.ViewModel
{
    public class EndControlViewModel : Core.ViewModel
    {
        private float volume;
        private float angle;
        private LevelReader levelReader;

        public float Volume
        {
            get => volume;
            set
            {
                volume = value;
                OnPropertyChanged();
            }
        }
        public float Angle
        {
            get => angle;
            set
            {
                angle = value;
                levelReader.Gain = CalculateGainFromAngle(value);
                OnPropertyChanged();
            }
        }

        public EndControlViewModel(LevelReader levelReader)
        {
            this.levelReader = levelReader;
            levelReader.DataAvailable += OnDataAvailable;
        }

        /// <summary>
        /// <c>Method</c> Calculates the gain from a given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The gain value.</returns>
        private float CalculateGainFromAngle(float angle)
        {
            return ((angle + 140f) / 280f) * 2;
        }

        /// <summary>
        /// <c>Callback</c> Called when level reader data is available.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDataAvailable(object? sender, LevelReaderDataAvailableEventArgs e)
        {
            float[] data = e.Data;
            float maxVal = 0;
            float minVal = 0;

            // Get the sum of all samples.
            for (int i = 0; i < data.Length; i++)
            {
                maxVal = Math.Max(maxVal, data[i]);
                minVal = Math.Min(minVal, data[i]);
            }

            Volume = maxVal - minVal;
        }
    }
}
