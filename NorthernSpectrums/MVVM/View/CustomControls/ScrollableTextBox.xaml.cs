using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NorthernSpectrums.MVVM.View.CustomControls
{
    /// <summary>
    /// Interaction logic for ScrollableTextBox.xaml
    /// </summary>
    public partial class ScrollableTextBox : UserControl
    {
        private readonly TextBox editableBox;
        private bool isInFocus = false;

        // Register a dependency property to allow data bindings.
        public static readonly DependencyProperty Value = 
            DependencyProperty.Register("SelectedValue", typeof(int), typeof(ScrollableTextBox), 
                new PropertyMetadata(120, new PropertyChangedCallback(ValueChanged)));

        [Bindable(true)]
        [Category("Appearance")]
        public int SelectedValue
        {
            get => (int)GetValue(Value);
            set
            {
                int validated = ValidateSelection(value);
                SetValue(Value, validated);
            }
        }

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the ScrollableTextBox controls.
        /// </summary>
        public ScrollableTextBox()
        {
            InitializeComponent();
            editableBox = EditableBox;

            editableBox.GotFocus += EditableBox_OnFocus;
            editableBox.LostFocus += EditableBox_LostFocus;
            editableBox.PreviewKeyDown += EditableBox_Enter;
            editableBox.PreviewMouseWheel += EditableBox_PreviewMouseWheel;
        }

        /// <summary>
        /// <c>Method</c> When the user scrolls.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void EditableBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            int changeSign = Math.Sign(e.Delta);

            SelectedValue += (10 * changeSign); // Increments of 10.
        }

        /// <summary>
        /// <c>Method</c> When the enter key is pressed.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void EditableBox_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && isInFocus)
            {
                try
                {
                    int value = int.Parse(editableBox.Text);
                    SelectedValue = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid value", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                    editableBox.Text = SelectedValue.ToString();
                }
            }
        }

        /// <summary>
        /// <c>Method</c> When the element losed focus.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void EditableBox_LostFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = false;
            try
            {
                int value = int.Parse(editableBox.Text);
                SelectedValue = value;
            }
            catch(Exception)
            {
                MessageBox.Show("Invalid value", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                editableBox.Text = SelectedValue.ToString();
            }
        }

        /// <summary>
        /// <c>Method</c> When the element is focused.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void EditableBox_OnFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
        }

        /// <summary>
        /// <c>Method</c> Validates that the given input is valid.
        /// </summary>
        /// <param name="toBeValidated">value to be validated.</param>
        private int ValidateSelection(int toBeValidated)
        {
            int selection = Math.Max(60, Math.Min(999, toBeValidated));
            editableBox.Text = selection.ToString();
            return selection;
        }

        /// <summary>
        /// <c>Callback</c> When selected value changes.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The event arguments.</param>
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is int newValue && d is ScrollableTextBox textBox && newValue != textBox.SelectedValue)
            {
                textBox.SelectedValue = newValue;
            }
        }
    }
}
