namespace NorthernSpectrums.MVVM.Model.Audio.RackProviders.ReverbRack
{
    /// <summary>
    /// <c>Interface</c> Exposes reverb rack controls.
    /// </summary>
    public interface IReverbRackProvider
    {
        /// <summary>
        /// <c>Property</c> Controls the dry/wet on the reverb.
        /// </summary>
        public float Level { get; set; }

        /// <summary>
        /// <c>Property</c> Controls the decay of the reverb.
        /// </summary>
        public float Decay { get; set; }

        /// <summary>
        /// <c>Property</c> Controls the delay on the reverb in ms.
        /// </summary>
        public int DelayMs { get; set; }
    }
}
