using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helperklassen.Helper
{
    public static class App_helper
    {
        public static bool IsAktiv_MySql { get; set; }
        public static void Use_MsSql(string _conectionstring)
        {
            Use_MsSql(_conectionstring, false);
        }

        public static void Use_MsSql(string _conectionstring, bool use_xpo)
        {
            IsAktiv_MySql = true;
            Sql_MsSql_helper.Connectionstring = _conectionstring;
            if (use_xpo == false)
            {
                Sql_MsSql_helper.CheckDB_exist();
            }
            else
            {
                XPO_helper.Use_connectionstring(_conectionstring);
            }
        }
    }
}
