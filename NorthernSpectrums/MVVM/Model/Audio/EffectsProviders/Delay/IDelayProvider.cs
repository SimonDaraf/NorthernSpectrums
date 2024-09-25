using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Delay
{
    public interface IDelayProvider
    {
        /// <summary>
        /// <c>Property</c> The bpm used when calculating total delay time.
        /// </summary>
        public int Bpm { get; set; }

        /// <summary>
        /// <c>Property</c> The note fraction used when calculating total delay time.
        /// </summary>
        public float NoteFraction { get; set; }

        /// <summary>
        /// <c>Property</c> The dry/wetness of the signal.
        /// </summary>
        public float Level { get; set; }

        /// <summary>
        /// <c>Property</c> The feedback of the delayed signal.
        /// </summary>
        public float Feedback { get; set; }
    }
}
