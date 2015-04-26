using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN
{
    [Serializable]
    public class BizApplicationException : ApplicationException
    {
        public BizApplicationException(string message)
            : base(message)
        {

        }
    }
}
