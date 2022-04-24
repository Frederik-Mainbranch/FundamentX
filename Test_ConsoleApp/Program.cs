using System;
using Helperklassen.Helper;
using Test_ConsoleApp.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace Test_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionstring = Sql_MsSql_helper.Create_Connectionstring("TestDB-04-24", "FREDERIK-PC", null, null);
            App_helper.Use_MsSql(connectionstring, true);

            using (UnitOfWork uow = new UnitOfWork())
            {
                var artikel = new XPCollection<ArtikelX>(uow);

                foreach (ArtikelX artikelX in artikel)
                {
                    artikelX.Delete();
                }

                uow.CommitChanges();
            }

            using (UnitOfWork uow = new UnitOfWork())
            {
                {
                    ArtikelX artikel = new ArtikelX(uow);
                    artikel.AnlegeDatum = DateTime.Now;
                    object maxArtikelnummer = uow.EvaluateInTransaction<ArtikelX>(CriteriaOperator.Parse("Max(Artikelnummer)"), null);
                    if(maxArtikelnummer != null)
                    {
                        artikel.Artikelnummer = (int)maxArtikelnummer + 1;
                    }
                    else
                    {
                        artikel.Artikelnummer = 1;
                    }
                    artikel.Beschreibung = "Fahrrad Herren";
                    artikel.Preis = 49.90M;
                }
             
                
                uow.CommitChanges();
            }
        }
    }
}
