using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using uEN.UI.DataBinding;
using uEN.UI.Controls;
using System.Windows.Threading;
namespace uEN.UI
{
    public abstract class BizViewModel : INotifyPropertyChanged, IDisposable
    {
        protected BizViewModel()
        {
            var vmr = Repository.GetPriorityExport<ViewModelWeakReference>();
            vmr.Push(this);
        }

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
            if (View != null)
                View.UpdateSource(groupRegion);
        }
        public void UpdateTarget(string groupRegion = null)
        {
            if (View != null)
                View.UpdateTarget(groupRegion);
        }
        public void ThrowValidationError()
        {
            if (View != null)
                View.ThrowValidationError();
        }

        public virtual void ApplyView() { }

        public virtual void LoadedView() { }

        public bool Initialized { get; internal set; }

        #region StatusMessage

        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                SetProperty(ref statusMessage, value, "StatusMessage");
                SetRootProperty(x => x.StatusMessage = value);
            }
        }
        private string statusMessage;

        public string SubStatusMessage
        {
            get { return subStatusMessage; }
            set
            {
                SetProperty(ref subStatusMessage, value, "SubStatusMessage");
                SetRootProperty(x => x.SubStatusMessage = value);
            }
        }
        private string subStatusMessage;

        public void ClearStatusMessage()
        {
            StatusMessage =
            SubStatusMessage = string.Empty;
        }

        #endregion

        #region Company

        public string CompanyName
        {
            get { return companyName; }
            set
            {
                SetProperty(ref companyName, value);
                SetRootProperty(x => x.CompanyName = value);
            }
        }
        private string companyName = BizUtils.AppSettings("CompanyName", "");

        public string CompanyDescription
        {
            get { return companyDescription; }
            set
            {
                SetProperty(ref companyDescription, value);
                SetRootProperty(x => x.CompanyDescription = value);
            }
        }
        private string companyDescription = BizUtils.AppSettings("CompanyDescription", "");

        #endregion

        #region User

        public string UserName
        {
            get { return userName; }
            set
            {
                SetProperty(ref userName, value, "UserName");
                SetRootProperty(x => x.UserName = value);
            }
        }
        private string userName = Environment.UserName;

        public string Section
        {
            get { return section; }
            set
            {
                SetProperty(ref section, value, "Section");
                SetRootProperty(x => x.Section = value);
            }
        }
        private string section = Environment.UserDomainName;

        #endregion


        internal void SetRootProperty(Action<BizViewModel> setter)
        {
            var win = Window.GetWindow(this.View);
            if (win != null)
            {
                var vm = win.Content as BizViewModel;
                if (vm != null && vm != this)
                {
                    setter(vm);
                }
            }
        }


        public Breadcrumb Navigator
        {
            get
            {
                var win = Window.GetWindow(View);
                return Breadcrumb.GetBreadcrumb(win);
            }
        }

        public DialogWidget DialogNavigator
        {
            get
            {
                var win = Window.GetWindow(View);
                var widget = win.FindVisualChildren<DialogWidget>().FirstOrDefault();
                return widget;
            }
        }

        public ExtendedContainer ExtendedNavigator
        {
            get
            {
                var win = Window.GetWindow(View);
                return new ExtendedContainer(win);
            }
        }

        public virtual string Description { get { return string.Empty; } }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Description) ? base.ToString() : Description;
        }

        public event EventHandler Collapsed;
        public void Collapse()
        {
            var view = this.View;
            if (view != null)
            {
                view.SetCurrentValue(BizView.VisibilityProperty, Visibility.Collapsed);
            }
            if (Collapsed != null)
                Collapsed(this, new EventArgs());
        }
        public void UnCollapse()
        {
            var view = this.View;
            if (view != null)
            {
                view.SetCurrentValue(BizView.VisibilityProperty, Visibility.Visible);
            }
        }

        public event CancelEventHandler Closing;
        public event EventHandler Closed;
        public void Close()
        {
            if (IsClosed)
                return;

            var e = new CancelEventArgs();
            OnClosing(this, e);

            if (e.Cancel)
                return;

            Collapse();
            this.View.BindingBehaviors.Clear();
            this.View = null;
            IsClosed = true;

            if (Closed != null)
                Closed(this, new EventArgs());
            return;
        }
        protected internal bool IsClosed = false;
        protected internal virtual void OnClosing(object sender, CancelEventArgs e)
        {
            if (Closing != null)
                Closing(sender, e);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Close();

                var vmr = Repository.GetPriorityExport<ViewModelWeakReference>();
                vmr.Pop(this);
            }
            disposed = true;
        }
        bool disposed = false;


        /// <summary>
        /// メッセージキューを処理します。
        /// </summary>
        /// <param name="priority"></param>
        public void DoEvents(DispatcherPriority priority = DispatcherPriority.Background)
        {
            UIElementExtensions.DoEvents(null, priority);
        }


        public event EventHandler<MessageNotificationEventArgs> MessageNotify;
        public object OnMessageNotify(string message, object userState = null)
        {
            var e = new MessageNotificationEventArgs(message, userState);
            if (this.MessageNotify != null)
                this.MessageNotify(this, e);

            return e.Result;
        }

        protected ISimpleGrid InitializeSimpleGrid<T>(int frozenColumnCount = 0, bool isSingleSelection = true)
        {
            return CreateSimpleGrid(new T[] { }, frozenColumnCount, isSingleSelection);
        }
        protected ISimpleGrid CreateSimpleGrid<T>(IEnumerable<T> source,
            int frozenColumnCount = 0, bool isSingleSelection = true)
        {
            var vm = new SimpleDataGridViewModel();
            vm.FrozenColumnCount = frozenColumnCount;
            vm.SelectionMode = isSingleSelection ? System.Windows.Controls.DataGridSelectionMode.Single : System.Windows.Controls.DataGridSelectionMode.Extended;
            vm.EnsureItemsSource(source);
            return vm;
        }
    }

    public class MessageNotificationEventArgs : EventArgs
    {
        public MessageNotificationEventArgs(string message)
            : this(message, null)
        {
        }
        public MessageNotificationEventArgs(string message, object userState)
        {
            Message = message;
            UserState = userState;
        }
        public string Message { get; private set; }
        public object UserState { get; private set; }
        public object Result { get; set; }
    }

}
