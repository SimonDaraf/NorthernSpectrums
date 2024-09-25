using NAudio.Wave;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;

namespace NorthernSpectrums.Services.AmpHeadService
{
    public interface IAmpHeadService
    {
        /// <summary>
        /// <c>Property</c> The source effect the amp will read from. 
        /// </summary>
        ISampleProvider? EffectSource { get; set; }

        /// <summary>
        /// <c>Property</c> The amp head source.
        /// </summary>
        IEffectsProvider? AmpHeadSource { get; set; }
    }
}
