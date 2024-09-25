using NorthernSpectrums.MVVM.ViewModel.Pedals;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NorthernSpectrums.MVVM.View.Pedals
{
    /// <summary>
    /// Interaction logic for DistortionPedal.xaml
    /// </summary>
    public partial class DistortionPedal : UserControl
    {
        private Button levelKnob;
        private Button gainKnob;
        private Point lastPosition = new Point(0, 0);
        private bool isLevelInteracting = false;
        private bool isGainInteracting = false;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the distortion pedal view.
        /// </summary>
        public DistortionPedal()
        {
            InitializeComponent();

            levelKnob = LevelKnob;
            gainKnob = GainKnob;

            levelKnob.PreviewMouseLeftButtonDown += LevelKnob_OnMouseButtonDown;
            gainKnob.PreviewMouseLeftButtonDown += GainKnob_OnGainMouseButtonDown;
            levelKnob.PreviewMouseDoubleClick += LevelKnob_OnMouseDoubleClick;
            gainKnob.PreviewMouseDoubleClick += GainKnob_OnMouseDoubleClick;
            Application.Current.MainWindow.PreviewMouseLeftButtonUp += MainWindow_MouseButtonUp;
            Application.Current.MainWindow.PreviewMouseMove += MainWindow_OnMouseMove;
        }

        /// <summary>
        /// <c>Method</c> Resets rotation on double klick.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void GainKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is DistortionViewModel viewModel)
            {
                viewModel.GainKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets rotation on double klick.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void LevelKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is DistortionViewModel viewModel)
            {
                viewModel.LevelKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles mouse movement.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseEventArgs.</param>
        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (DataContext is DistortionViewModel viewModel)
            {
                if (isLevelInteracting)
                {
                    if (levelKnob.RenderTransform is RotateTransform rotation)
                    {
                        double angle = rotation.Angle; // Get current angle.

                        double change = lastPosition.Y - e.GetPosition(Application.Current.MainWindow).Y;

                        angle += change; // Apply change in rotation.

                        viewModel.LevelKnobRotation = MathF.Min(140, MathF.Max(-140, (float)angle)); // Clamp angle to [-140, 140].
                    }
                }

                if (isGainInteracting)
                {
                    if (gainKnob.RenderTransform is RotateTransform rotation)
                    {
                        double angle = rotation.Angle; // Get current angle.

                        double change = lastPosition.Y - e.GetPosition(Application.Current.MainWindow).Y;

                        angle += change; // Apply change in rotation.

                        viewModel.GainKnobRotation = MathF.Min(140, MathF.Max(-140, (float)angle)); // Clamp angle to [-140, 140].
                    }
                }
            }
            
            lastPosition = e.GetPosition(Application.Current.MainWindow);
        }

        /// <summary>
        /// <c>Method</c> Handles  when the mouse button is released.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs</param>
        private void MainWindow_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            isGainInteracting = false;
            isLevelInteracting = false;
        }

        /// <summary>
        /// <c>Method</c> Handles mouse down on gain knob.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void GainKnob_OnGainMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            isGainInteracting = true;
            isLevelInteracting = false;
        }

        /// <summary>
        /// <c>Method</c> Handles mouse down on level knob.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void LevelKnob_OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLevelInteracting = true;
            isGainInteracting = false;
        }
    }
}
