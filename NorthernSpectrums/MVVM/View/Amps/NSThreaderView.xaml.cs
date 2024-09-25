using NorthernSpectrums.MVVM.ViewModel.Amps;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NorthernSpectrums.MVVM.View.Amps
{
    /// <summary>
    /// Interaction logic for NSThreaderView.xaml
    /// </summary>
    public partial class NSThreaderView : UserControl
    {
        // Delegate used to modify current property to be modified.
        private Action<double>? propertyAction;
        private Point lastPosition = new Point(0, 0);

        /// <summary>
        /// <c>Constructor</c> Sets upp all events related to the NS Threader amplifier.
        /// </summary>
        public NSThreaderView()
        {
            InitializeComponent();

            GainKnob.PreviewMouseLeftButtonDown += GainKnob_OnMouseDown;
            BassKnob.PreviewMouseLeftButtonDown += BassKnob_OnMouseDown;
            MiddleKnob.PreviewMouseLeftButtonDown += MiddleKnob_OnMouseDown;
            TrebleKnob.PreviewMouseLeftButtonDown += TrebleKnob_OnMouseDown;
            MasterKnob.PreviewMouseLeftButtonDown += MasterKnob_OnMouseDown;

            GainKnob.PreviewMouseDoubleClick += GainKnob_OnMouseDoubleClick;
            BassKnob.PreviewMouseDoubleClick += BassKnob_OnMouseDoubleClick;
            MiddleKnob.PreviewMouseDoubleClick += MiddleKnob_OnMouseDoubleClick;
            TrebleKnob.PreviewMouseDoubleClick += TrebleKnob_OnMouseDoubleClick;
            MasterKnob.PreviewMouseDoubleClick += MasterKnob_OnMouseDoubleClick;

            Application.Current.MainWindow.PreviewMouseMove += OnMouseMove;
            Application.Current.MainWindow.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        /// <summary>
        /// <c>Method</c> Handles double click on master knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void MasterKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NSThreaderViewModel viewModel)
            {
                viewModel.MasterKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles double click on treble knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void TrebleKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NSThreaderViewModel viewModel)
            {
                viewModel.TrebleKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles double click on middle knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void MiddleKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NSThreaderViewModel viewModel)
            {
                viewModel.MiddleKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles double click on bass knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void BassKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NSThreaderViewModel viewModel)
            {
                viewModel.BassKnobRotation = 0;
            }
        }

        /// <summary>
        /// <c>Method</c> Handles double click on gain knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void GainKnob_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NSThreaderViewModel viewModel)
            {
                viewModel.GainKnobRotation = 0;
            }
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
        /// <c>Method</c> Sets a delegate for the master knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void MasterKnob_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            propertyAction = d =>
            {
                if (DataContext is NSThreaderViewModel viewModel && MasterKnob.RenderTransform is RotateTransform rotation)
                {
                    viewModel.MasterKnobRotation = MathF.Min(140, MathF.Max(-140, (float)(rotation.Angle + (lastPosition.Y - d))));
                }
            };
        }

        /// <summary>
        /// <c>Method</c> Sets a delegate for the treble knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void TrebleKnob_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            propertyAction = d =>
            {
                if (DataContext is NSThreaderViewModel viewModel && TrebleKnob.RenderTransform is RotateTransform rotation)
                {
                    viewModel.TrebleKnobRotation = MathF.Min(140, MathF.Max(-140, (float)(rotation.Angle + (lastPosition.Y - d))));
                }
            };
        }

        /// <summary>
        /// <c>Method</c> Sets a delegate for the middle knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void MiddleKnob_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            propertyAction = d =>
            {
                if (DataContext is NSThreaderViewModel viewModel && MiddleKnob.RenderTransform is RotateTransform rotation)
                {
                    viewModel.MiddleKnobRotation = MathF.Min(140, MathF.Max(-140, (float)(rotation.Angle + (lastPosition.Y - d))));
                }
            };
        }

        /// <summary>
        /// <c>Method</c> Sets a delegate for the bass knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void BassKnob_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            propertyAction = d =>
            {
                propertyAction = d =>
                {
                    if (DataContext is NSThreaderViewModel viewModel && BassKnob.RenderTransform is RotateTransform rotation)
                    {
                        viewModel.BassKnobRotation = MathF.Min(140, MathF.Max(-140, (float)(rotation.Angle + (lastPosition.Y - d))));
                    }
                };
            };
        }

        /// <summary>
        /// <c>Method</c> Sets a delegate for the gain knob.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void GainKnob_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            propertyAction = d =>
            {
                if (DataContext is NSThreaderViewModel viewModel && GainKnob.RenderTransform is RotateTransform rotation)
                {
                    viewModel.GainKnobRotation = MathF.Min(140, MathF.Max(-140, (float)(rotation.Angle + (lastPosition.Y - d))));
                }
            };
        }
    }
}
