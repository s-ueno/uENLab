using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Settings.xaml の相互作用ロジック
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        private static readonly List<Type> settingTypes = new List<Type>();
        private static Type LoadType(string s)
        {
            Type type = null;
            try
            {
                type = Type.GetType(s);
            }
            catch
            {
            }
            return type;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var list = ConfigurationManager.GetSection("Settings.ViewModel") as NameValueCollection;
            foreach (var each in list.AllKeys)
            {
                var type = LoadType(each);
                if (type != null)
                {
                    settingTypes.Add(type);
                }
            }

            var viewModels = settingTypes.Select(x => CreateViewModel(x))
                             .Where(x => x != null)
                             .ToArray();
            IconButton.Click -= IconButton_Click;
            IconButton.Click += IconButton_Click;
            SettingViewModels.ItemsSource = viewModels;
            SettingViewModels.DisplayMemberPath = "Description";
            SettingViewModels.SelectionChanged -= SettingViewModels_SelectionChanged;
            SettingViewModels.SelectionChanged += SettingViewModels_SelectionChanged;
        }

        public void ShowContents(bool showMainMenu = true)
        {

            if (showMainMenu)
            {
                SettingViewModels.Visibility = Visibility.Visible;
                ViewModelPresenter.Visibility = Visibility.Collapsed;

                Caption.Text = "Settings";
                ViewModelPresenter.Visibility = Visibility.Collapsed;
                ViewTransition.Play(Caption, TransitionStyle.Slide);
                ViewTransition.Play(SettingViewModels, TransitionStyle.Slide);
            }
            else
            {
                SettingViewModels.Visibility = Visibility.Collapsed;
                ViewModelPresenter.Visibility = Visibility.Visible;

                var vm = SettingViewModels.SelectedItem as BizViewModel;
                if (vm == null)
                    return;

                ViewModelPresenter.Visibility = Visibility.Visible;
                ViewModelPresenter.Content = vm;
                ViewTransition.Play(ViewModelPresenter, TransitionStyle.Slide);

                Caption.Text = vm.Description;
                ViewTransition.Play(Caption, TransitionStyle.Slide,
                    () => SettingViewModels.SelectedIndex = -1);
            }
        }

        void IconButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingViewModels.Visibility == System.Windows.Visibility.Visible)
            {
                var grid = this.FindParentWithVisualTree<Grid>();
                ViewTransition.Play(grid, TransitionStyle.SlideOut, 
                    () => grid.Visibility = System.Windows.Visibility.Collapsed);
                return;
            }
            ShowContents(SettingViewModels.Visibility == Visibility.Collapsed);
        }

        void SettingViewModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowContents(false);
        }

        private static BizViewModel CreateViewModel(Type t)
        {
            BizViewModel vm = null;
            try
            {
                vm = Activator.CreateInstance(t) as BizViewModel;
            }
            catch (Exception ex)
            {
            }
            return vm;
        }
    }
}
