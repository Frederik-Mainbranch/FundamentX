using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

using System.Data;
using System.Data.Sql;
using Microsoft.Win32;

namespace Helperklassen
{
    public static class Sql_MsSql_helper
    {
        private static string _AssemblyName { get {return Assembly.GetExecutingAssembly().GetName().Name; } }
        private static string connectionstring;
        public static string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; Connection = new SqlConnection(connectionstring); }
        }

        private static string initialCatalog;
        public static string InitialCatalog
        {
            get
            {
                if (string.IsNullOrEmpty(Connectionstring))
                {
                    return null;
                }
                else if (string.IsNullOrEmpty(initialCatalog) == false)
                {
                    return initialCatalog;
                }
                else
                {
                    string result = "";
                    string[] connectionstrings_lines = Connectionstring.Split(';');
                    foreach (string line in connectionstrings_lines)
                    {
                        if (line.Contains("Initial Catalog"))
                        {
                            result = line.Substring(line.IndexOf("=") + 1);
                        }
                    }

                    initialCatalog = result;
                    return result;
                }
            }
        }

        private static string Get_masterConnectionstring()
        {
            string[] connectionstrings_lines = Connectionstring.Split(';');
            string connectionstring = "";
            foreach (string line in connectionstrings_lines)
            {
                if (line.Contains("Initial Catalog"))
                {
                    connectionstring = connectionstring + ";" + "Initial Catalog=master";
                    continue;
                }
                connectionstring = connectionstring + ";" + line;
            }
            connectionstring = connectionstring.Substring(1, connectionstring.Length - 1);
            return connectionstring;
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

        public static DataTable Get_DataTable(string query)
        {
            return Get_DataTable(query, Connection, true);
        }

        public static DataTable Get_DataTable(string query, bool manageConnection)
        {
            return Get_DataTable(query, Connection, manageConnection);
        }

        public static DataTable Get_DataTable(string query, SqlConnection connection)
        {
            return Get_DataTable(query, connection, true);
        }

        public static DataTable Get_DataTable(string query, SqlConnection connection, bool manageConnection)
        {
            DataTable table = new DataTable();
            try
            {
                if (manageConnection)
                {
                    Connection.Open();
                }

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(table);
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
            return table;
        }

        //public static int Execute_nonQuery(string query)
        //{
        //    int count_rows_affected = 0;
        //    try
        //    {
        //        Connection.Open();
        //        SqlCommand sqlCommand = new SqlCommand(query, Connection);
        //        count_rows_affected = sqlCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        Log_helper.Create_errorMessage(e.Message);
        //    }
        //    finally
        //    {
        //        Connection.Close();
        //    }
        //    return count_rows_affected;
        //}

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


        public static void CheckDB_exist()
        {
            bool db_exists = false;
            string masterConnectionstring = Get_masterConnectionstring();
            using (var connection = new SqlConnection(masterConnectionstring))
            {
                using (SqlCommand command = new SqlCommand($"SELECT db_id('{InitialCatalog}')", connection))
                {
                    connection.Open();
                    db_exists = (command.ExecuteScalar() != DBNull.Value);
                }

                if (db_exists == false)
                {
                    using (SqlCommand command = new SqlCommand($"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{InitialCatalog}') BEGIN CREATE DATABASE[{InitialCatalog}] END", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }


        /// <summary>
        /// Creates the default Connectionstring for a local MS Sql database
        /// </summary>
        /// <returns></returns>
        public static string Create_Connectionstring()
        {
            //SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            //sqlConnectionStringBuilder.s
            return $"Integrated Security=True;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog={_AssemblyName}";
        }


        /// <summary>
        /// Creates the default Connectionstring for a local MS Sql database with a given username and password
        /// </summary>
        /// <returns></returns>
        public static string Create_Connectionstring(string userId = null, string password = null)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            //sqlConnectionStringBuilder.IntegratedSecurity = true;
            //sqlConnectionStringBuilder.Pooling = false;
            sqlConnectionStringBuilder.DataSource = "(localdb)\\mssqllocaldb";
            sqlConnectionStringBuilder.InitialCatalog = _AssemblyName;
            sqlConnectionStringBuilder.IntegratedSecurity = true;

            if (userId != null)
            {
                sqlConnectionStringBuilder.UserID = userId;
            }
            //else
            //{
            //    sqlConnectionStringBuilder.UserID = "";
            //}
            if (password != null)
            {
                sqlConnectionStringBuilder.Password = password;
            }
            //else
            //{
            //    sqlConnectionStringBuilder.Password = "";
            //}

            string connectionstring = sqlConnectionStringBuilder.ToString();
            return connectionstring;
        }


        /// <summary>
        /// Creates the Connectionstring to a specific datasource and initial catalog for a local MS Sql database with a given username and password
        /// </summary>
        /// <returns></returns>
        public static string Create_Connectionstring(string initialCatalog, string dataSource = null, string userId = null, string password = null)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            //sqlConnectionStringBuilder.IntegratedSecurity = true;
            //sqlConnectionStringBuilder.Pooling = false;
            sqlConnectionStringBuilder.InitialCatalog = initialCatalog;
            sqlConnectionStringBuilder.IntegratedSecurity = true;

            if (dataSource != null)
            {
                sqlConnectionStringBuilder.DataSource = dataSource;
            }
            else
            {
                sqlConnectionStringBuilder.DataSource = "(localdb)\\mssqllocaldb";
            }

            if (userId != null)
            {
                sqlConnectionStringBuilder.UserID = userId;
            }
            //else
            //{
            //    sqlConnectionStringBuilder.UserID = "";
            //}
            if (password != null)
            {
                sqlConnectionStringBuilder.Password = password;
            }
            //else
            //{
            //    sqlConnectionStringBuilder.Password = "";
            //}

            string connectionstring = sqlConnectionStringBuilder.ToString();
            return connectionstring;
        }
    }
}
