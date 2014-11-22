using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uEN.Utils;
namespace uEN.UI
{
    public abstract class BizViewModel : INotifyPropertyChanged
    {
        protected BizViewModel() { /*this.Compose();*/ }

        private VisualElementsAttribute visualElements;
        public VisualElementsAttribute VisualElements
        {
            get
            {
                if (visualElements == null)
                {
                    visualElements = this.GetType()
                                         .GetCustomAttributes(typeof(VisualElementsAttribute), false)
                                         .FirstOrDefault() as VisualElementsAttribute;
                }
                return visualElements;
            }
        }
        private BizView view;
        public BizView View
        {
            get { return view; }
            set { view = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T storage, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        public void UpdateSource(string groupRegion = null)
        {
            View.UpdateSource(groupRegion);
        }
        public void UpdateTarget(string groupRegion = null)
        {
            View.UpdateTarget(groupRegion);
        }
        public void ThrowValidationError()
        {
            View.ThrowValidationError();
        }

        public virtual void ApplyView() { }

        public virtual void LoadedView() { }

        async public Task<T> AsyncGenericAction<T>() { return await Task.FromResult(default(T)); }

        #region StatusMessage

        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                SetProperty(ref statusMessage, value);
                var win = Window.GetWindow(this.View);
                if (win != null)
                {
                    var vm = win.Content as BizViewModel;
                    if (vm != null && vm != this)
                    {
                        vm.StatusMessage = value;
                    }
                }
            }
        }
        private string statusMessage;

        public string SubStatusMessage1
        {
            get { return subStatusMessage1; }
            set
            {
                SetProperty(ref subStatusMessage1, value);
                var win = Window.GetWindow(this.View);
                if (win != null)
                {
                    var vm = win.Content as BizViewModel;
                    if (vm != null && vm != this)
                    {
                        vm.SubStatusMessage1 = value;
                    }
                }
            }
        }
        private string subStatusMessage1;

        public string SubStatusMessage2
        {
            get { return subStatusMessage2; }
            set
            {
                SetProperty(ref subStatusMessage2, value);
                var win = Window.GetWindow(this.View);
                if (win != null)
                {
                    var vm = win.Content as BizViewModel;
                    if (vm != null && vm != this)
                    {
                        vm.SubStatusMessage2 = value;
                    }
                }
            }
        }
        private string subStatusMessage2;

        public string SubStatusMessage3
        {
            get { return subStatusMessage3; }
            set
            {
                SetProperty(ref subStatusMessage3, value);
                var win = Window.GetWindow(this.View);
                if (win != null)
                {
                    var vm = win.Content as BizViewModel;
                    if (vm != null && vm != this)
                    {
                        vm.SubStatusMessage3 = value;
                    }
                }
            }
        }
        private string subStatusMessage3;

        public void ClearStatusMessage()
        {
            StatusMessage =
            SubStatusMessage1 =
            SubStatusMessage2 =
            SubStatusMessage3 = string.Empty;
        }

        #endregion

        #region Company

        public string CompanyName
        {
            get { return companyName; }
            set { SetProperty(ref companyName, value); }
        }
        private string companyName = BizUtils.AppSettings("CompanyName", "");

        public string CompanyDescription
        {
            get { return companyDescription; }
            set { SetProperty(ref companyDescription, value); }
        }
        private string companyDescription = BizUtils.AppSettings("CompanyDescription", "");

        #endregion

        #region User

        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }
        private string userName = Environment.UserName;

        public string Section
        {
            get { return section; }
            set { SetProperty(ref section, value); }
        }
        private string section = Environment.UserDomainName;

        #endregion

        public BreadcrumbNavigator Navigator
        {
            get
            {
                var win = Window.GetWindow(View);
                var navi = BreadcrumbNavigator.GetBreadcrumbNavigator(win);
                if (navi == null)
                {
                    navi = Repository.GetPriorityExport<BreadcrumbNavigator>();
                }
                win.SetValue(BreadcrumbNavigator.BreadcrumbNavigatorProperty, navi);
                return navi;
            }
        }

        public virtual string Description { get { return string.Empty; } }
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Description) ? base.ToString() : Description;
        }

    }
}
