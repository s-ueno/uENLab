using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.UI
{
    public class GroupRegionAttribute : Attribute
    {
        public GroupRegionAttribute(params string[] regionId)
        {
            RegionId = regionId;
        }
        public IEnumerable<string> RegionId { get; private set; }
    }
}
