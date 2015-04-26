using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using uEN.UI.DataBinding;

namespace uEN.UI.Validation
{
    public interface IValidationRuleProvider
    {
        ValidationRule Provide(IBindingBehavior bindingBehavior);
    }
}
