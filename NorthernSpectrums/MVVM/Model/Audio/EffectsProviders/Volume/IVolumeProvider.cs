namespace NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Volume
{
    /// <summary>
    /// <c>Interface</c> The IVolumeProvider interface provides interaction with the VolumeProvider properties. Effectively allowing bindings with the UI.
    /// </summary>
    public interface IVolumeProvider
    {
        /// <summary>
        /// <c>Property</c> The volume to be used.
        /// </summary>
        public float Volume { get; set; }
    }
}
