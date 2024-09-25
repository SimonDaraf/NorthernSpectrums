using System.Windows.Input;

namespace NorthernSpectrums.Core
{
    /// <summary>
    /// <c>Class</c> Used to relay commands via data context bindings.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool>? canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of a Relay Command.
        /// </summary>
        /// <param name="execute">Delegate to be executed.</param>
        /// <param name="canExecute">A delegate that verifies whether we can execute.</param>
        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// <c>Method</c> Determines whether passed delegate can be executed.
        /// </summary>
        /// <param name="parameter">Delegate parameter.</param>
        /// <returns>Whether we can execute.</returns>
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter!);
        }

        /// <summary>
        /// <c>Method</c> Executes passed delegate.
        /// </summary>
        /// <param name="parameter">Delegate parameter.</param>
        public void Execute(object? parameter)
        {
            execute(parameter!);
        }
    }
}
