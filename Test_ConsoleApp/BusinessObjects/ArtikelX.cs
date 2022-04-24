using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using Helperklassen;

namespace Test_ConsoleApp.BusinessObjects
{
    public class ArtikelX : BaseObject
    {
        public ArtikelX(Session session) : base(session)
        {

        }

        public string Beschreibung { get; set; }
        public decimal Preis { get; set; }
        public DateTime AnlegeDatum { get; set; }
        public int Artikelnummer { get; set; }
    }
}
