namespace NorthernSpectrums.MVVM.Model.Audio.EffectsProviders.Distortion
{
    public interface IDistortionProvider
    {
        /// <summary>
        /// <c>Property</c> Controls the threshold where the signal should clip.
        /// </summary>
        public float Level { set; get; }

        /// <summary>
        /// <c>Property</c> Constrols the amplification of the signal.
        /// </summary>
        public float Gain { set; get; }
    }
}
