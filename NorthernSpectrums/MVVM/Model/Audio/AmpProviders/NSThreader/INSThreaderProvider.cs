namespace NorthernSpectrums.MVVM.Model.Audio.AmpProviders.NSThreader
{
    /// <summary>
    /// <c>Interface</c> Provides setting controls for the NS Threader amplifier.
    /// </summary>
    public interface INSThreaderProvider
    {
        /// <summary>
        /// <c>Property</c> Controls the gain.
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        /// <c>Property</c> Controls the bass gain.
        /// </summary>
        public float BassGain { get; set; }

        /// <summary>
        /// <c>Property</c> Controls the middle gain.
        /// </summary>
        public float MiddleGain { get; set; }

        /// <summary>
        /// <c>Property</c> Controls the treble gain.
        /// </summary>
        public float TrebleGain { get; set; }

        /// <summary>
        /// <c>Property</c> Controls the master gain.
        /// </summary>
        public float MasterGain { get; set; }
    }
}
