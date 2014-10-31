using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace uEN.UI
{
    public abstract class BizViewModel : INotifyPropertyChanged
    {
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

        public virtual void ApplyView()
        {

        }
        public virtual string Description { get { return string.Empty; } }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Description) ? base.ToString() : Description;
        }
    }
}
