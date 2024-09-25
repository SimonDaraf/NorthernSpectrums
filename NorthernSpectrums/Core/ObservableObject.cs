using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NorthernSpectrums.Core
{
    /// <summary>
    /// <c>Class</c> Represents an observable object.
    /// Used to notify when a property has changed so correlating changes can be made in a view.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// <c>Method</c> Called on property changed.
        /// </summary>
        /// <param name="propertyName">The method/property name.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
