using System;
using Helperklassen;

namespace Test_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionstring = Sql_MsSql_helper.Create_Connectionstring("Blah", "Entwickler-PC15\\SQL2019", "sa", "mit");
            App_helper.Use_MsSql(connectionstring);
        }
    }
}
