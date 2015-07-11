using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace uEN.UI.DataBinding
{

    public class BindingBehaviorCollection : List<IBindingBehavior>, IDisposable
    {
        public IEnumerable<T> ListBehaviors<T>(string groupRegion) where T : IBindingBehavior
        {
            if (string.IsNullOrWhiteSpace(groupRegion))
            {
                return this.OfType<T>();
            }

            var list = new List<T>();
            foreach (var each in this.OfType<T>())
            {
                if (each.Attributes.OfType<GroupRegionAttribute>().Any(x => x.RegionId.Any(y => y == groupRegion)))
                {
                    list.Add(each);
                }
            }
            return list;
        }

        public IEnumerable<ValidationError> ListValidationErrors(IEnumerable<DependencyPropertyBehavior> behaviors)
        {
            foreach (var each in behaviors.OrderBy(x => KeyboardNavigation.GetTabIndex(x.Element)))
            {
                foreach (var child in each.ValidationErrors)
                {
                    yield return child;
                }   
            }
        }

        

        public void EnsureBinding()
        {
            foreach (var each in this)
            {
                each.Ensure();
            }
        }

        public void Dispose()
        {
            foreach (var each in this)
            {
                
            }
        }
    }
}
