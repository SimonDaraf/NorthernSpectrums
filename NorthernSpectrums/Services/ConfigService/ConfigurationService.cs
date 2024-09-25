using NAudio.CoreAudioApi;
using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.MVVM.ViewModel;

namespace NorthernSpectrums.Services.ConfigService
{
    /// <summary>
    /// <c>Class</c> Handles saving and loading the application settings.
    /// </summary>
    public class ConfigurationService
    {
        private readonly IPreservable audioSettings;
        private readonly IPreservable generalSettings;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the configuration service.
        /// </summary>
        /// <param name="audioSettings">The audio settings view model.</param>
        /// <param name="generalSettings">The general settings view model.</param>
        public ConfigurationService(AudioSettingsViewModel audioSettings, GeneralSettingsViewModel generalSettings)
        {
            this.audioSettings = audioSettings;
            this.generalSettings = generalSettings;
        }

        /// <summary>
        /// <c>Method</c> Loads App.config data on startup to restore previous application settings.
        /// </summary>
        public void LoadOnStartup()
        {
            Dictionary<string, object> audioData = new Dictionary<string, object>
            {
                // Get settings from config.
                { "DriverMode", Properties.Settings.Default.DriverMode },
                { "Driver", Properties.Settings.Default.Driver },
                { "Input", Properties.Settings.Default.Input },
                { "Output", Properties.Settings.Default.Output }
            };

            Dictionary<string, object> generalData = new Dictionary<string, object>
            {
                // Get settings from config.
                { "Theme", Properties.Settings.Default.Theme }
            };

            // Load data.
            audioSettings.Load(audioData);
            generalSettings.Load(generalData);
        }

        /// <summary>
        /// <c>Method</c> Saves App.config when the application is closed.
        /// </summary>
        public void SaveOnExit()
        {
            try
            {
                Dictionary<string, object> audioData = audioSettings.Save();
                Dictionary<string, object> generalData = generalSettings.Save();

                foreach (KeyValuePair<string, object> entry in audioData)
                {
                    switch (entry.Key)
                    {
                        case "DriverMode":
                            Properties.Settings.Default.DriverMode = entry.Value.ToString();
                            break;
                        case "Driver":
                            Properties.Settings.Default.Driver = (string)entry.Value;
                            break;
                        case "Input":
                            Properties.Settings.Default.Input = (int)entry.Value;
                            break;
                        case "Output":
                            Properties.Settings.Default.Output = (int)entry.Value;
                            break;
                        default:
                            break;
                    }
                }

                foreach (KeyValuePair<string, object> entry in generalData)
                {
                    switch (entry.Key)
                    {
                        case "Theme":
                            Properties.Settings.Default.Theme = entry.Value.ToString();
                            break;
                        default:
                            break;
                    }
                }
                // Save the changes
                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
