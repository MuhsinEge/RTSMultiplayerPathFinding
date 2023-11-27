using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.RTS.Filters
{
    public unsafe struct GridFilter
    {
        public EntityRef entity;
        public GridDataLink* gridData;
    }
}
