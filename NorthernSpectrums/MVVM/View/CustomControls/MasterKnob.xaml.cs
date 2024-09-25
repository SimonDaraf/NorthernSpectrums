using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NorthernSpectrums.MVVM.View.CustomControls
{
    /// <summary>
    /// Interaction logic for MasterKnob.xaml
    /// </summary>
    public partial class MasterKnob : UserControl
    {
        private Action<double>? propertyAction;
        private Point pos = new Point(0, 0);

        // Dependency Property to allow data bindings.
        public static readonly DependencyProperty Value =
            DependencyProperty.Register("CurrentAngle", typeof(float), typeof(MasterKnob),
                new PropertyMetadata(0f, new PropertyChangedCallback(AngleChanged)));

        /// <summary>
        /// <c>Property</c> The current angle of the knob.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public float CurrentAngle
        {
            get => (float)GetValue(Value);
            set
            {
                float validated = ValidateSelection(value);
                SetValue(Value, validated);
            }
        }

        /// <summary>
        /// <c>Constructor</c> Creates an instance of the master knob and sets up event handlers
        /// </summary>
        public MasterKnob()
        {
            InitializeComponent();

            if (BorderContainer != null)
            {
                BorderContainer.PreviewMouseLeftButtonDown += GridContainer_OnMouseDown;
            }

            if (Application.Current?.MainWindow != null)
            {
                Application.Current.MainWindow.PreviewMouseLeftButtonUp += OnMouseUp;
                Application.Current.MainWindow.PreviewMouseMove += OnMouseMove;
            }
        }

        /// <summary>
        /// <c>Method</c> Called when the mouse button is released.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.ReleaseMouseCapture();
            propertyAction = null; // Make sure property action can't be invoked.
        }

        /// <summary>
        /// <c>Method</c> Called when the mouse moves.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            double diff = pos.Y - e.GetPosition(Application.Current.MainWindow).Y; // Calculate the position difference.

            double angle = CurrentAngle + diff;

            propertyAction?.Invoke(angle); // invoke the property action.

            pos = e.GetPosition(Application.Current.MainWindow); // Get the mouse position in the current main window.
        }

        /// <summary>
        /// <c>Method</c> Handles when mouse is pressed over grid container.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void GridContainer_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.CaptureMouse();

            // Handle Double click.
            if (e.ClickCount == 2)
            {
                CurrentAngle = 0; // Reset angle.
                return;
            }

            // Set the property action.
            propertyAction = new Action<double>(d =>
            {
                CurrentAngle = (float)d;
            });
        }

        /// <summary>
        /// <c>Method</c> Validates angle and sets element rotation;
        /// </summary>
        /// <param name="toBeValidated"></param>
        /// <returns>The validated angle.</returns>
        private float ValidateSelection(float toBeValidated)
        {
            float selection = MathF.Min(MathF.Max(toBeValidated, -140), 140);
            if (BorderContainer.RenderTransform is RotateTransform transform)
            {
                transform.Angle = selection;
            }
            else
            {
                RotateTransform newTransform = new RotateTransform { Angle = selection };
                BorderContainer.RenderTransform = newTransform;
            }
            return selection;
        }

        /// <summary>
        /// <c>Callback</c> When selected angle changed.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The event arguments.</param>
        private static void AngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is float newValue && d is MasterKnob masterKnob && newValue != masterKnob.CurrentAngle)
            {
                masterKnob.CurrentAngle = newValue;
            }
        }
    }
}
