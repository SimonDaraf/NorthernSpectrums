namespace NorthernSpectrums.MVVM.ViewModel.Settings
{
    /// <summary>
    /// <c>Class</c> The Window Title ViewModel. Represents a datacontext for audio settings.
    /// </summary>
    /// <remarks>
    /// <c>Constructor</c> Constructs an instance of WindowTitleViewModel
    /// </remarks>
    /// <param name="windowTitle">The title of the window.</param>
    public class WindowTitleViewModel(string windowTitle)
    {
        private readonly string windowTitle = windowTitle;

        /// <summary>
        /// <c>Property</c> Accessor for the window title.
        /// </summary>
        public string WindowTitle
        {
            get { return windowTitle; }
        }
    }
}
