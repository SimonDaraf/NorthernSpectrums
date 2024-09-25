using MessagePack;
using NorthernSpectrums.Core;
using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.Model.Audio.AmpProviders;
using NorthernSpectrums.MVVM.Model.Audio.EffectsProviders;
using NorthernSpectrums.MVVM.Model.Audio.RackProviders;
using NorthernSpectrums.MVVM.ViewModel;

namespace NorthernSpectrums.Services.Presets
{
    /// <summary>
    /// <c>Class</c> Service responsible for saving and loading presets.
    /// </summary>
    public class PresetService
    { 
        private IPreservable pedalVm;
        private IPreservable ampVm;
        private IPreservable rackVm;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the preset service.
        /// Dependencies should be the same ones utilized during runtime.
        /// Dependencies needs to implement the IPreservable interface.
        /// </summary>
        /// <param name="pedalVm">The pedal view model</param>
        /// <param name="ampVm">The amp view model.</param>
        /// <param name="rackVm">The rack view model.</param>
        public PresetService(PedalViewModel pedalVm, AmpViewModel ampVm, RackViewModel rackVm)
        {
            this.pedalVm = pedalVm;
            this.ampVm = ampVm;
            this.rackVm = rackVm;
        }

        /// <summary>
        /// <c>Method</c> Attempts to save a preset.
        /// </summary>
        /// <param name="filePath">The file path to save to.</param>
        /// <returns>A boolean indicating if the operation was successful.</returns>
        public bool SavePreset(string filePath)
        {
            try
            {
                // This is pretty ugly but it does the job.
                // What we are doing is effectively going through each item in the dictionary and checking whether we can interpret it as the right structure.
                // If everything is as expected we proceed to use them in the preset structure.
                // If it would fail. We don't save the preset (Hopefully shouldn't happen...)
                //
                // Structure:
                //      Effect placement : Dictionary
                //          Package : Dictionary
                //              Effect identifier : Effect value
                //
                Dictionary<string, Dictionary<EffectsPackage, Dictionary<string, object>>> pedals = pedalVm.Save()
                    .Where(kvp => kvp.Value is Dictionary<EffectsPackage, Dictionary<string, object>>)
                    .ToDictionary(kvp => kvp.Key, kvp => (Dictionary<EffectsPackage, Dictionary<string, object>>)kvp.Value);

                Dictionary<string, Dictionary<AmpPackage, Dictionary<string, object>>> amps = ampVm.Save()
                    .Where(kvp => kvp.Value is Dictionary<AmpPackage, Dictionary<string, object>>)
                    .ToDictionary(kvp => kvp.Key, kvp => (Dictionary<AmpPackage, Dictionary<string, object>>)kvp.Value);

                Dictionary<string, Dictionary<RackPackage, Dictionary<string, object>>> racks = rackVm.Save()
                    .Where(kvp => kvp.Value is Dictionary<RackPackage, Dictionary<string, object>>)
                    .ToDictionary(kvp => kvp.Key, kvp => (Dictionary<RackPackage, Dictionary<string, object>>)kvp.Value);

                // Save to preset struct.
                Preset preset = new Preset(pedals, amps, racks);

                Serializer.ToBinary(preset, filePath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// <c>Method</c> Deconstructs and passes the preset struct components to each responsible view.
        /// </summary>
        /// <param name="filePath">The file path to deserialize.</param>
        /// <returns>A bool indicating if the process was successful.</returns>
        public bool LoadPreset(string filePath)
        {
            try
            {
                Preset preset = Serializer.Deserialize<Preset>(filePath);

                // We need to re-interpret before passing.
                pedalVm.Load(preset.Pedals.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value));
                ampVm.Load(preset.Amps.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value));
                rackVm.Load(preset.Racks.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// <c>Struct</c> Defines the structure of a preset.
    /// Is serializable.
    /// </summary>
    /// <param name="pedals">The inner pedalboard structure.</param>
    /// <param name="amps">The inner amp structure.</param>
    /// <param name="racks">The inner rack structure.</param>
    [MessagePackObject]
    public readonly struct Preset(Dictionary<string, Dictionary<EffectsPackage, Dictionary<string, object>>> pedals,
        Dictionary<string, Dictionary<AmpPackage, Dictionary<string, object>>> amps,
        Dictionary<string, Dictionary<RackPackage, Dictionary<string, object>>> racks)
    {
        /// <summary>
        /// <c>Property</c> Defines an inner pedalboard structure that can be read to recover saved data.
        /// </summary>
        [Key(0)]
        public Dictionary<string, Dictionary<EffectsPackage, Dictionary<string, object>>> Pedals { get; } = pedals;

        /// <summary>
        /// <c>Property</c> Defines an inner amp structure that can be read to recover saved data.
        /// </summary>
        [Key(1)]
        public Dictionary<string, Dictionary<AmpPackage, Dictionary<string, object>>> Amps { get; } = amps;

        /// <summary>
        /// <c>Property</c> Defines an inner rack structure that can be read to recover saved data.
        /// </summary>
        [Key(2)]
        public Dictionary<string, Dictionary<RackPackage, Dictionary<string, object>>> Racks { get; } = racks;
    }
}
