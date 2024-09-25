using NorthernSpectrums.MVVM.ViewModel.Racks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NorthernSpectrums.MVVM.View.Racks
{
    /// <summary>
    /// Interaction logic for ReverbRackView.xaml
    /// </summary>
    public partial class ReverbRackView : UserControl
    {
        private Action<double>? propertyAction;
        private Point lastPosition = new Point(0, 0);

        /// <summary>
        /// <c>Constructor</c> Sets upp all events related to the Reverb Rack.
        /// </summary>
        public ReverbRackView()
        {
            InitializeComponent();

            LevelKnob.PreviewMouseLeftButtonDown += LevelKnob_OnMouseDown;
            DecayKnob.PreviewMouseLeftButtonDown += DecayKnob_OnMouseDown;
            TimeKnob.PreviewMouseLeftButtonDown += TimeKnob_OnMouseDown;

            LevelKnob.PreviewMouseDoubleClick += LevelKnob_OnDoubleClick;
            DecayKnob.PreviewMouseDoubleClick += DecayKnob_OnDoubleClick;
            TimeKnob.PreviewMouseDoubleClick += TimeKnob_OnDoubleClick;

            Application.Current.MainWindow.PreviewMouseMove += OnMouseMove;
            Application.Current.MainWindow.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        /// <summary>
        /// <c>Method</c> Handles when the left mouse button is released.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            propertyAction = null;
        }

        /// <summary>
        /// <c>Method</c> Invokes the property delegate and updates mouse position.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            propertyAction?.Invoke(e.GetPosition(Application.Current.MainWindow).Y);
            lastPosition = e.GetPosition(Application.Current.MainWindow);
        }

        /// <summary>
        /// <c>Method</c> Handles a double click on the time knob.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void TimeKnob_OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ReverbRackViewModel viewModel)
            {
                viewModel.TimeKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles a double click on the decay knob.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void DecayKnob_OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ReverbRackViewModel viewModel)
            {
                viewModel.DecayKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles a double click on the level knob.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void LevelKnob_OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ReverbRackViewModel viewModel)
            {
                viewModel.LevelKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Sets a delegate for the time knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void TimeKnob_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            propertyAction = d =>
            {
                if (DataContext is ReverbRackViewModel viewModel && TimeKnob.RenderTransform is RotateTransform rotation)
                {
                    viewModel.TimeKnobRotation = MathF.Min(140, MathF.Max(-140, (float)(rotation.Angle + (lastPosition.Y - d))));
                }
            };
        }

        /// <summary>
        /// <c>Method</c> Sets a delegate for the decay knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void DecayKnob_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            propertyAction = d =>
            {
                if (DataContext is ReverbRackViewModel viewModel && DecayKnob.RenderTransform is RotateTransform rotation)
                {
                    viewModel.DecayKnobRotation = MathF.Min(140, MathF.Max(-140, (float)(rotation.Angle + (lastPosition.Y - d))));
                }
            };
        }

        /// <summary>
        /// <c>Method</c> Sets a delegate for the level knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void LevelKnob_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            propertyAction = d =>
            {
                if (DataContext is ReverbRackViewModel viewModel && LevelKnob.RenderTransform is RotateTransform rotation)
                {
                    viewModel.LevelKnobRotation = MathF.Min(140, MathF.Max(-140, (float)(rotation.Angle + (lastPosition.Y - d))));
                }
            };
        }
    }
}
