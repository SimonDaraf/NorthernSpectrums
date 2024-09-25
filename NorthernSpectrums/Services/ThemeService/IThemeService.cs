using NorthernSpectrums.MVVM.Model;

namespace NorthernSpectrums.Services.ThemeService
{
    /// <summary>
    /// <c>Interface</c> Interface for interacting with the application theme service.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// <c>Method</c> Applies the selected theme.
        /// </summary>
        /// <param name="selectedTheme"></param>
        public void ApplyTheme(Theme selectedTheme);
    }
}
