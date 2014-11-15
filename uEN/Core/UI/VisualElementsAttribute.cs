using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.UI
{

    public class VisualElementsAttribute : Attribute
    {
        public VisualElementsAttribute(Type visualType)
        {
            VisualType = visualType;
        }
        public Type VisualType { get; private set; }
    }
}
