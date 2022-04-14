using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace Helperklassen
{
    public static class Sql_MS_helper
    {
        private static string _AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        private static string connectionstring;
        public static string Connectionstring
        {
          get { return connectionstring; }
          set { connectionstring = value; Connection = new SqlConnection(connectionstring); }
        }

        private static SqlConnection Connection { get; set; }

        public static object Execute_reader(string query, bool manageConnection = true)
        {
            List<object[]> query_result = new List<object[]>();
            try
            {
                if (manageConnection)
                {
                    Connection.Open();
                }

                SqlCommand sqlCommand = new SqlCommand(query, Connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                int anzahl_spalten = sqlDataReader.FieldCount;
                while (sqlDataReader.Read())
                {
                    object[] row = new object[anzahl_spalten];

                    for (int i = 0; i < anzahl_spalten; i++)
                    {
                        row[i] = sqlDataReader.GetValue(i);
                    }

                    query_result.Add(row);
                }

            }
            catch (Exception e)
            {
                Log_helper.Create_errorMessage(e.Message);
            }
            finally
            {
                if (manageConnection)
                {
                    Connection.Close();
                }
            }

            return query_result;
        }

        public static object Execute_scalar(string query, bool manageConnection = true)
        {
            //List<object[]> query_result = new List<object[]>();
            object query_result = new object();
            try
            {
                if (manageConnection)
                {
                    Connection.Open();
                }

                SqlCommand sqlCommand = new SqlCommand(query, Connection);
                query_result = sqlCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                Log_helper.Create_errorMessage(e.Message);
            }
            finally
            {
                if (manageConnection)
                {
                    Connection.Close();
                }
            }

            return query_result;
        }

        public static void Open_connection()
        {
            if(Connection.State == System.Data.ConnectionState.Closed)
                 Connection.Open();
        }

        public static void Close_connection()
        {
            if(Connection.State == System.Data.ConnectionState.Open)
                 Connection.Close();
        }

        public static bool CheckDB_exist()
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                using (var command = new SqlCommand($"SELECT db_id('{_AssemblyName}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        public static string Create_Connectionstring()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
        }
    }
}
