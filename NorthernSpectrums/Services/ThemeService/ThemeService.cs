using NorthernSpectrums.MVVM.Model;
using System.Windows;

namespace NorthernSpectrums.Services.ThemeService
{
    /// <summary>
    /// <c>Class</c> Class responsible for handling application themes during runtime.
    /// </summary>
    public class ThemeService : IThemeService
    {
        public void ApplyTheme(Theme selectedTheme)
        {
            // Get the first resource dictionary that contains the name ApplicationTheme.
            ResourceDictionary? themeResource = Application.Current.Resources.MergedDictionaries.FirstOrDefault(dict =>
            {
                // Iterate over all keys in resource dictionary.
                foreach (object key in dict.Keys)
                {
                    // We then check if that specific key is a string, and matches the specified name for the resource dictionary we are looking for.
                    if (key is string strKey && strKey.Equals("ApplicationTheme"))
                    {
                        return true;
                    }
                }
                return false;
            });

            if (themeResource != null)
            {
                themeResource.Source = new Uri($"MVVM/View/Themes/{selectedTheme}.xaml", UriKind.Relative);
            }
        }
    }
}
