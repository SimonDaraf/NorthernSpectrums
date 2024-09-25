using NorthernSpectrums.MVVM.Model;
using NorthernSpectrums.Services.ThemeService;

namespace NorthernSpectrums.MVVM.ViewModel
{
    /// <summary>
    /// <c>Class</c> The General Settings View Model class.
    /// </summary>
    public class GeneralSettingsViewModel: Core.ViewModel, IPreservable
    {
        private IThemeService themeService;
        private Theme theme;
        private Theme[] themeSource = [];

        // Property bindings.
        public Theme Theme
        {
            get => theme;
            set
            {
                theme = value;
                ApplyTheme();
                OnPropertyChanged();
            }
        }
        public Theme[] ThemeSource
        {
            get => themeSource;
            set
            {
                themeSource = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the General Settings Viewmodel. Responsible for general application settings.
        /// </summary>
        public GeneralSettingsViewModel(IThemeService themeService)
        {
            this.themeService = themeService;
            ThemeSource = Enum.GetValues(typeof(Theme)).Cast<Theme>().ToArray();
            // TODO: Load from save.
        }

        /// <summary>
        /// <c>Method</c> Applies the selected theme.
        /// </summary>
        private void ApplyTheme()
        {
            themeService.ApplyTheme(theme);
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>
            {
                { "Theme", Theme }
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            try
            {
                foreach (KeyValuePair<string, object> entry in data) {
                    switch (entry.Key)
                    {
                        case "Theme":
                            _ = Enum.TryParse((string)entry.Value, out Theme parsedEnum) == true ?
                                Theme = parsedEnum : Theme = Theme.Dark; // If not a valid enum, set du default.
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                return; // In case something goes wrong, just return.
            }
        }
    }
}
