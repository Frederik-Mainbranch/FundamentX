using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helperklassen
{
    public static class App_helper
    {
        public static bool IsAktiv_MySql { get; set; }
        public static void Use_MsSql(string _conectionstring)
        {
            IsAktiv_MySql = true;
            Sql_MsSql_helper.Connectionstring = _conectionstring;
            Sql_MsSql_helper.CheckDB_exist();
        }
    }
}
