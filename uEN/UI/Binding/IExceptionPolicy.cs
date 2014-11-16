using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.UI.Binding
{
    public interface IExceptionPolicy
    {
        void Do(Exception ex);
    }
}
