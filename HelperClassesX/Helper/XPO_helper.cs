using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;

namespace Helperklassen.Helper
{
    public static class XPO_helper
    {
        //local storage connectionstring: "XpoProvider=InMemoryDataStore;Data Source=data.xml;Read Only=false"
        public static void Use_SqLite()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            string connectionString = SQLiteConnectionProvider.GetConnectionString(Path.Combine(appDataPath, Assembly.GetExecutingAssembly().GetName().Name + ".db"));
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
            XpoDefault.Session = null;
        }

        public static void Use_connectionstring(string connectionstring)
        {
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(connectionstring, AutoCreateOption.DatabaseAndSchema);
            XpoDefault.Session = null;
        }

        //public static void Use_SqLite(string filepath, string filename)
        //{

        //}
    }
}

namespace Helperklassen
{
    public abstract class BaseObject : XPBaseObject
    {
        public BaseObject(Session session) : base(session)
        {
            Oid = Guid.NewGuid();
        }

        [Key]
        public Guid Oid { get; set; }
    }
}
