using NorthernSpectrums.Core;

namespace NorthernSpectrums.Services.NavigationService
{
    public interface INavigationService
    {
        /// <summary>
        /// <c>Property</c> The current view model active.
        /// </summary>
        ViewModel CurrentView { get; }

        /// <summary>
        /// <c>Method</c> Navigates to specific View.
        /// </summary>
        /// <typeparam name="T">The target ViewModel</typeparam>
        public void NavigatoTo<T>() where T : ViewModel;
    }
}
