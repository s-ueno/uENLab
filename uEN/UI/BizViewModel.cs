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
            string propertyName = null)
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

        //async public Task<T> AsyncGenericAction<T>() { return await Task.FromResult(default(T)); }

        #region StatusMessage

        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                SetProperty(ref statusMessage, value);
                SetRootProperty(x => x.StatusMessage = value);
            }
        }
        private string statusMessage;

        public string SubStatusMessage1
        {
            get { return subStatusMessage1; }
            set
            {
                SetProperty(ref subStatusMessage1, value);
                SetRootProperty(x => x.SubStatusMessage1 = value);
            }
        }
        private string subStatusMessage1;

        public string SubStatusMessage2
        {
            get { return subStatusMessage2; }
            set
            {
                SetProperty(ref subStatusMessage2, value);
                SetRootProperty(x => x.SubStatusMessage2 = value);
            }
        }
        private string subStatusMessage2;

        public string SubStatusMessage3
        {
            get { return subStatusMessage3; }
            set
            {
                SetProperty(ref subStatusMessage3, value);
                SetRootProperty(x => x.SubStatusMessage3 = value);
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

        //#region

        //public BizViewModel HeaderContent
        //{
        //    get
        //    {
        //        var win = Window.GetWindow(this.View);
        //        if (win != null)
        //        {
        //            var vm = win.Content as BizViewModel;
        //            if (vm != null)
        //            {
        //                var content = win.Template.FindName("PART_BlandContentPresenter", win) as System.Windows.Controls.ContentPresenter;
        //                if (content != null)
        //                {
        //                    var templateSelector = content.ContentTemplateSelector;
        //                    if (templateSelector == null)
        //                    {
        //                        content.ContentTemplateSelector = Repository.GetPriorityExport<ViewDataTemplateSelector>();
        //                    }
        //                    return content.Content as BizViewModel;
        //                }
        //            }
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        var win = Window.GetWindow(this.View);
        //        if (win != null)
        //        {
        //            var vm = win.Content as BizViewModel;
        //            if (vm != null)
        //            {
        //                var content = win.Template.FindName("PART_BlandContentPresenter", win) as System.Windows.Controls.ContentPresenter;
        //                if (content != null)
        //                {
        //                    var templateSelector = content.ContentTemplateSelector;
        //                    if (templateSelector == null)
        //                    {
        //                        content.ContentTemplateSelector = Repository.GetPriorityExport<ViewDataTemplateSelector>();
        //                    }
        //                    content.Content = value;
        //                }
        //            }
        //        }
        //    }
        //}

        //#endregion

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
