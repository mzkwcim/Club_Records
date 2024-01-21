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
            string connectionString = "Host=localhost;Username=postgres;Password=Mzkwcim181099!;Database=WOZP";
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
                            Console.WriteLine("Tabela już istnieje, dane zostały poprawnie dodane do tabeli");
                        }
                    }
                    else
                    {
                        string createTableQuery = PostgreSqlQueryBuilder.CreateTable(name);
                        using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                        {
                            command.ExecuteNonQuery();
                            Console.WriteLine("Tabela została utworzona");
                        }
                        using (NpgsqlCommand command2 = new NpgsqlCommand(addValuesQuery, connection))
                        {
                            command2.ExecuteNonQuery();
                            Console.WriteLine("Dane zostały dodane do tabeli");
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
            string connectionString = "Host=localhost;Username=postgres;Password=Mzkwcim181099!;Database=WOZP";
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
                            Console.WriteLine("Porównanie wyników młodszych zawodników z wynikami starszych zawodników zostało zakończone sukcesem");
                        }
                        using (NpgsqlCommand command3 = new NpgsqlCommand(PostgreSqlQueryBuilder.Inserter(gender, counter, pool), connection))
                        {
                            command3.ExecuteNonQuery();
                            Console.WriteLine("Jeżeli w wynikach starszych grup wiekowych nie było danych co do dystansu to zostały przepisane z tabeli młodszych zaownidków");
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
