using NorthernSpectrums.MVVM.ViewModel.Pedals;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NorthernSpectrums.MVVM.View.Pedals
{
    /// <summary>
    /// Interaction logic for DelayPedal.xaml
    /// </summary>
    public partial class DelayPedal : UserControl
    {
        private readonly Button levelKnob;
        private readonly Button feedbackKnob;
        private Point lastPosition = new Point(0, 0);
        private bool isLevelInteracting = false;
        private bool isFeedbackInteracting = false;

        /// <summary>
        /// <c>Constructor</c> Constructs an instance of a delay pedal.
        /// </summary>
        public DelayPedal()
        {
            InitializeComponent();

            levelKnob = LevelKnob;
            feedbackKnob = FeedbackKnob;

            levelKnob.PreviewMouseLeftButtonDown += LevelKnob_OnMouseButtonDown;
            feedbackKnob.PreviewMouseLeftButtonDown += FeedbackKnob_OnMouseButtonDown;
            levelKnob.PreviewMouseDoubleClick += LevelKnob_OnMouseDoubleClick;
            feedbackKnob.PreviewMouseDoubleClick += FeedbackKnob_OnMouseDoubleClick;
            Application.Current.MainWindow.PreviewMouseLeftButtonUp += MainWindow_MouseButtonUp;
            Application.Current.MainWindow.PreviewMouseMove += MainWindow_OnMouseMove;
        }

        /// <summary>
        /// <c>Method</c> Handles mouse movement.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseEventArgs.</param>
        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (DataContext is DelayViewModel viewModel)
            {
                if (isLevelInteracting)
                {
                    if (levelKnob.RenderTransform is RotateTransform rotation)
                    {
                        double angle = rotation.Angle; // Get current angle

                        double change = lastPosition.Y - e.GetPosition(Application.Current.MainWindow).Y;

                        angle += change; // Apply change in rotation.

                        viewModel.LevelKnobRotation = MathF.Min(140, MathF.Max(-140, (float)angle)); // Clamp angle to [-140, 140].
                    }
                }

                if (isFeedbackInteracting)
                {
                    if (feedbackKnob.RenderTransform is RotateTransform rotation)
                    {
                        double angle = rotation.Angle; // Get current angle

                        double change = lastPosition.Y - e.GetPosition(Application.Current.MainWindow).Y;

                        angle += change; // Apply change in rotation.

                        viewModel.FeedbackKnobRotation = MathF.Min(140, MathF.Max(-140, (float)angle)); // Clamp angle to [-140, 140].
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
            isFeedbackInteracting = false;
            isLevelInteracting = false;
        }

        /// <summary>
        /// <c>Method</c> Resets rotation on double klick.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void FeedbackKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is DelayViewModel viewModel)
            {
                viewModel.FeedbackKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets rotation on double klick.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void LevelKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is DelayViewModel viewModel)
            {
                viewModel.LevelKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles mouse down on feedback knob.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void FeedbackKnob_OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLevelInteracting = false;
            isFeedbackInteracting = true;
        }

        /// <summary>
        /// <c>Method</c> Handles mouse down on level knob.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void LevelKnob_OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLevelInteracting = true;
            isFeedbackInteracting = false;
        }
    }
}
