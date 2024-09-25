using NorthernSpectrums.MVVM.ViewModel.Racks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NorthernSpectrums.MVVM.View.Racks
{
    /// <summary>
    /// Interaction logic for EqRackView.xaml
    /// </summary>
    public partial class EqRackView : UserControl
    {
        /// <summary>
        /// <c>Constructor</c> Constructs an instance of the eq rack view.
        /// </summary>
        public EqRackView()
        {
            InitializeComponent();

            BandOne.PreviewMouseDoubleClick += BandOne_OnMouseDoubleClick;
            BandTwo.PreviewMouseDoubleClick += BandTwo_OnMouseDoubleClick;
            BandThree.PreviewMouseDoubleClick += BandThree_OnMouseDoubleClick;
            BandFour.PreviewMouseDoubleClick += BandFour_OnMouseDoubleClick;
            BandFive.PreviewMouseDoubleClick += BandFive_OnMouseDoubleClick;
            BandSix.PreviewMouseDoubleClick += BandSix_OnMouseDoubleClick;
            BandSeven.PreviewMouseDoubleClick += BandSeven_OnMouseDoubleClick;
            BandEight.PreviewMouseDoubleClick += BandEight_OnMouseDoubleClick;
        }

        /// <summary>
        /// <c>Method</c> Resets slider eight value.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BandEight_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EqRackViewModel viewModel)
            {
                viewModel.BandEightValue = 0.5f;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets slider seven value.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BandSeven_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EqRackViewModel viewModel)
            {
                viewModel.BandSevenValue = 0.5f;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets slider six value.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BandSix_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EqRackViewModel viewModel)
            {
                viewModel.BandSixValue = 0.5f;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets slider five value.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BandFive_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EqRackViewModel viewModel)
            {
                viewModel.BandFiveValue = 0.5f;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets slider four value.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BandFour_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EqRackViewModel viewModel)
            {
                viewModel.BandFourValue = 0.5f;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets slider three value.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BandThree_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EqRackViewModel viewModel)
            {
                viewModel.BandThreeValue = 0.5f;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets slider two value.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BandTwo_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EqRackViewModel viewModel)
            {
                viewModel.BandTwoValue = 0.5f;
            }
        }

        /// <summary>
        /// <c>Method</c> Resets slider one value.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BandOne_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EqRackViewModel viewModel)
            {
                viewModel.BandOneValue = 0.5f;
            }
        }
    }
}
