using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Club_Records
{
    internal class ConnectionManager
    {
        static bool TableExists(NpgsqlConnection connection, string tableName)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $"SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = @tableName)";
                command.Parameters.AddWithValue("@tableName", tableName.ToLower());
                object ?result = command.ExecuteScalar();
                return result != null && (bool)result;
            }
        }
        public static void Connection(string addValuesQuery, string name)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=kk;Database=club_records";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (TableExists(connection, name))
                    {
                        using (NpgsqlCommand command2 = new NpgsqlCommand(addValuesQuery, connection))
                        {
                            command2.ExecuteNonQuery();
                            Console.WriteLine("Powodzenie");
                        }
                    }
                    else
                    {
                        string createTableQuery = PostgreSqlQueryBuilder.CreateTable(name);
                        using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                        {
                            command.ExecuteNonQuery();
                            Console.WriteLine("Powodzenie");
                        }
                        using (NpgsqlCommand command2 = new NpgsqlCommand(addValuesQuery, connection))
                        {
                            command2.ExecuteNonQuery();
                            Console.WriteLine("Powodzenie");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Błąd: {e.Message}");
                }
            }
        }
        public static void ComparatorConnection(string name, int counter, string pool, string gender)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=Mzkwcim181099!;Database=club_records";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (TableExists(connection, name))
                    {
                        using (NpgsqlCommand command2 = new NpgsqlCommand(PostgreSqlQueryBuilder.Comparator(counter, pool, gender), connection))
                        {
                            command2.ExecuteNonQuery();
                            Console.WriteLine("Powodzenie");
                        }
                        using (NpgsqlCommand command3 = new NpgsqlCommand(PostgreSqlQueryBuilder.Inserter(gender, counter, pool), connection))
                        {
                            command3.ExecuteNonQuery();
                            Console.WriteLine("Powodzenie");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Błąd: {e.Message}");
                }
            }
        }
    }
}
