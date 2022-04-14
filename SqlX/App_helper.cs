using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helperklassen
{
    public static class App_helper
    {
        public static void Nutze_MsSql(string _conectionstring)
        {
            Sql_MS_helper.Connectionstring = _conectionstring;
        }
    }
}
