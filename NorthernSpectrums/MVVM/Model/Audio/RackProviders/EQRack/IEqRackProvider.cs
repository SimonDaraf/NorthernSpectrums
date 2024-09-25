namespace NorthernSpectrums.MVVM.Model.Audio.RackProviders.EQRack
{
    /// <summary>
    /// <c>Interface</c> Provides interactability with the 8-band eq provider.
    /// </summary>
    public interface IEqRackProvider
    {
        /// <summary>
        /// <c>Property</c> The band one db gain.
        /// </summary>
        public float BandOneDb { get; set; }

        /// <summary>
        /// <c>Property</c> The band two db gain.
        /// </summary>
        public float BandTwoDb { get; set; }

        /// <summary>
        /// <c>Property</c> The band three db gain.
        /// </summary>
        public float BandThreeDb { get; set; }

        /// <summary>
        /// <c>Property</c> The band four db gain.
        /// </summary>
        public float BandFourDb { get; set; }

        /// <summary>
        /// <c>Property</c> The band five db gain.
        /// </summary>
        public float BandFiveDb { get; set; }

        /// <summary>
        /// <c>Property</c> The band six db gain.
        /// </summary>
        public float BandSixDb { get; set; }

        /// <summary>
        /// <c>Property</c> The band seven db gain.
        /// </summary>
        public float BandSevenDb { get; set; }

        /// <summary>
        /// <c>Property</c> The band eight db gain.
        /// </summary>
        public float BandEightDb { get; set; }
    }
}
