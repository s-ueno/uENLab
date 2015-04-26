using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uEN.UI.DataBinding
{
    /// <summary>
    /// バインディング処理の最後に処理を追加します。
    /// </summary>
    public interface IBindingAttribute
    {
        void Binding(IBindingBehavior behavior);
    }
}
