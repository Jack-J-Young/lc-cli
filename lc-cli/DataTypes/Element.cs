using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli.DataTypes
{
    public abstract class Element
    {
        public virtual Element Copy()
        {
            return this;
        }
    }
}
