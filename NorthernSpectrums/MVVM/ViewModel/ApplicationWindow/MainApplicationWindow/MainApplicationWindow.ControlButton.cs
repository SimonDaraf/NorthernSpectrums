using System.Windows;

namespace NorthernSpectrums.MVVM.ViewModel.ApplicationWindow.MainApplicationWindow
{
    public partial class MainApplicationWindow : ApplicationWindow
    {
        /// <summary>
        /// Handles when the user clicks on the maximize button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Data related to the click event.</param>
        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }

        /// <summary>
        /// Handles when the user clicks on the restore button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Data related to the click event.</param>
        private void ButtonRestore_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }

        /// <summary>
        /// Handles when the user clicks on the minimize button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Data related to the click event.</param>
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
