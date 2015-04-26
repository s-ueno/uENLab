using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.UI.DataBinding
{
    public interface IExceptionPolicy
    {
        void Do(object sender, Exception ex);
    }
}
