using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace NorthernSpectrums.Services.Presets
{
    /// <summary>
    /// <c>Class</c> Responsible for handling saving presets.
    /// </summary>
    public class PresetSaverService
    {
        private const string directory = "NorthernSpectrums";
        private readonly PresetService presetService;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the preset saver service.
        /// </summary>
        /// <param name="presetService">The preset service.</param>
        public PresetSaverService(PresetService presetService)
        {
            this.presetService = presetService;
            ValidatePresetDirectoryPath();
        }

        /// <summary>
        /// <c>Method</c> Tries to save a new preset.
        /// </summary>
        /// <returns>The full path to the newly created preset.</returns>
        public string SavePresetAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "NS Preset Files (*.nspreset)|*.nspreset";
            saveFileDialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), directory);

            bool? result = saveFileDialog.ShowDialog();

            if (result == true) // Is save was successful.
            {
                if (presetService.SavePreset(saveFileDialog.FileName))
                {
                    MessageBox.Show("Preset was successfully saved.", "Saved", MessageBoxButton.OK, MessageBoxImage.None);
                    return saveFileDialog.FileName;
                }

                // If save failed, display message box and return an empty string.
                MessageBox.Show("Failed to save preset.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }

            return ""; // If was cancelled, return empty string.
        }

        /// <summary>
        /// <c>Method</c> Saves current selected preset.
        /// </summary>
        /// <param name="presetPath">The full path to the current selected preset.</param>
        public string SavePreset(string presetPath)
        {
            // If no preset path present, try to save a new one.
            if (presetPath.Equals(""))
            {
                presetPath = SavePresetAs();
                return presetPath;
            }

            // if present.
            if (presetService.SavePreset(presetPath))
            {
                MessageBox.Show("Preset was successfully saved.", "Saved", MessageBoxButton.OK, MessageBoxImage.None);
                return presetPath;
            }

            MessageBox.Show("Failed to save preset.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return "";
        }

        /// <summary>
        /// <c>Method</c> Attempts to load the selected preset.
        /// </summary>
        /// <returns>The full path to the loaded preset.</returns>
        public string LoadPreset()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "NS Preset Files (*.nspreset)|*.nspreset";
            openFileDialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), directory);
            
            bool? result = openFileDialog.ShowDialog();

            if (result == true) // If load was successful.
            {
                if (presetService.LoadPreset(openFileDialog.FileName))
                {
                    MessageBox.Show("Preset was successfully loaded.", "Loaded", MessageBoxButton.OK, MessageBoxImage.None);
                    return openFileDialog.FileName;
                }

                // If load failed, display message box and return an empty string.
                MessageBox.Show("Failed to load preset.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }

            return ""; // If was cancelled, return empty string.
        }

        /// <summary>
        /// <c>Method</c> Cehcks whether the base directory exists. Otherwise create it.
        /// </summary>
        private static void ValidatePresetDirectoryPath()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), directory);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
