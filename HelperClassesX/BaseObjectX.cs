using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Helperklassen
{
    public abstract class BaseObjectX 
    {
        public BaseObjectX()
        {
            Oid = Guid.NewGuid();
        }

        public Guid Oid { get; set; }
    }
}
