using NorthernSpectrums.MVVM.ViewModel.Pedals;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NorthernSpectrums.MVVM.View.Pedals
{
    /// <summary>
    /// Interaction logic for VolumePedal.xaml
    /// </summary>
    public partial class VolumePedal : UserControl
    {
        private Button knob;
        private Point lastPosition = new Point(0, 0);
        private bool isInteracting = false;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the volume pedal view.
        /// </summary>
        public VolumePedal()
        {
            InitializeComponent();
            knob = PedalKnob;

            knob.PreviewMouseLeftButtonDown += OnMouseDown;
            knob.PreviewMouseDoubleClick += OnMouseDoubleClick;
            Application.Current.MainWindow.PreviewMouseLeftButtonUp += OnMouseUp;
            Application.Current.MainWindow.PreviewMouseMove += OnMouseMove;
        }

        /// <summary>
        /// <c>Method</c> Handles mouse double click.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is VolumeViewModel viewModel)
            {
                viewModel.VolumeKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles mouse down.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseEventArgs.</param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!isInteracting)
            {
                isInteracting = true;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles mouse move.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseEventArgs.</param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isInteracting && DataContext is VolumeViewModel viewModel)
            {
                if (knob.RenderTransform is RotateTransform rotation)
                {
                    double angle = rotation.Angle; // Get current angle.

                    double change = lastPosition.Y - e.GetPosition(Application.Current.MainWindow).Y;

                    angle += change; // Apply change in rotation.

                    viewModel.VolumeKnobRotation = MathF.Min(140, MathF.Max(-140, (float)angle)); // Clamp angle to [-140, 140].
                }
            }

            lastPosition = e.GetPosition(Application.Current.MainWindow);
        }

        /// <summary>
        /// <c>Method</c> Handles mouse up.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isInteracting = false;
        }
    }
}
