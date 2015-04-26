using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uEN.UI.AttachedProperties;

namespace uEN.UI.Controls
{
    /// <summary>
    /// DialogWidget.xaml の相互作用ロジック
    /// </summary>
    public partial class DialogWidget : UserControl
    {

        public DialogWidget()
        {
            InitializeComponent();
            IconButton.Click += OnIconButtonClick;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Container = this.Parent as Grid;
        }
        private Grid Container { get; set; }

        

        private void OnIconButtonClick(object sender, RoutedEventArgs e)
        {
            Pop();
        }

        private readonly Stack<BizViewModel> ViewModels = new Stack<BizViewModel>();
        public void Push(BizViewModel viewModel)
        {
            ViewModels.Push(viewModel);
            ViewModelPresenter.Content = viewModel;
            Caption.Text = viewModel.Description;

            if (Container.Visibility != System.Windows.Visibility.Visible)
            {
                Container.Visibility = System.Windows.Visibility.Visible;
                ViewTransition.Play(Container, TransitionStyle.Slide);
            }
            IconButton.Focus();
        }

        public void Pop()
        {
            var vm = ViewModels.Pop();
            if (ViewModels.Count != 0)
            {
                var current = ViewModels.Peek();
                ViewModelPresenter.Content = current;
                Caption.Text = current.Description;
            }
            else
            {
                ViewTransition.Play(Container, TransitionStyle.SlideOut,
                    new Action(() => Container.Visibility = System.Windows.Visibility.Hidden));
            }
            vm.Close();
        }


    }
}
