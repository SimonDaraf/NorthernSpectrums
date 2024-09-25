using NorthernSpectrums.Core;

namespace NorthernSpectrums.Services.NavigationService
{
    /// <summary>
    /// <c>Class</c> Responsible for handling navigation between different view models.
    /// </summary>
    /// <param name="viewModelFactory">Used to access a registered instance of a view model.</param>
    public class NavigationService(Func<Type, ViewModel> viewModelFactory) : ObservableObject, INavigationService
    {
        private ViewModel? currentView;
        private readonly Func<Type, ViewModel> viewModelFactory = viewModelFactory;

        /// <summary>
        /// <c>Property</c> Getter and setter for the current view.
        /// </summary>
        public ViewModel CurrentView
        {
            get => currentView!;
            private set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// <c>Method</c> Get registered instance of specified view model.
        /// </summary>
        /// <typeparam name="T">Specified view model.</typeparam>
        public void NavigatoTo<T>() where T : ViewModel
        {
            ViewModel viewModel = viewModelFactory.Invoke(typeof(T));
            CurrentView = viewModel;
        }
    }
}
