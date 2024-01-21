using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Club_Records
{
    internal class PostgreSqlQueryBuilder
    {
        public static string Inserter(string gender, int counter, string pool)
        {
            string inserter = $"INSERT INTO rekordy_{gender}_{counter}_letnich_{pool} (dystans, klub, imie, czasczytelny, data, miasto, czas) " +
                $"SELECT r{counter - 1}.dystans, r{counter - 1}.klub, r{counter - 1}.imie, r{counter - 1}.czasczytelny, r{counter - 1}.data, r{counter - 1}.miasto, r{counter - 1}.czas " +
                $"FROM rekordy_{gender}_{counter - 1}_letnich_{pool} r{counter - 1} " +
                "WHERE NOT EXISTS ( " +
                "SELECT 1 " +
                $"FROM rekordy_{gender}_{counter}_letnich_{pool} r{counter} " +
                $"WHERE r{counter}.dystans = r{counter - 1}.dystans " +
                ");";
            return inserter;
        }
        public static string Comparator(int counter, string pool, string gender)
        {
            string comparator = $"UPDATE rekordy_{gender.ToLower()}_{counter}_letnich_{pool.ToLower()} AS r{counter} " +
                "SET " +
                $"klub = r{counter - 1}.klub, " +
                $"imie = r{counter - 1}.imie, " +
                $"czasCzytelny = r{counter - 1}.czasCzytelny, " +
                $"data = r{counter - 1}.data, " +
                $"miasto = r{counter - 1}.miasto, " +
                $"czas = r{counter - 1}.czas " +
                $"FROM rekordy_{gender.ToLower()}_{counter - 1}_letnich_{pool.ToLower()} AS r{counter - 1} " +
                "WHERE " +
                $"r{counter}.dystans = r{counter - 1}.dystans " +
                $"AND r{counter}.czas > r{counter - 1}.czas;";
            return comparator;
        }
        public static string CreateTable(string tablename)
        {
            string createTableQueryTest = $"CREATE TABLE {tablename.ToLower()} ( " +
                              "ID SERIAL PRIMARY KEY, " +
                              "dystans VARCHAR(255) UNIQUE, " +
                              "klub VARCHAR(255), " + 
                              "imie VARCHAR(255), " +
                              "czasCzytelny VARCHAR(255), " +
                              "data VARCHAR(255), " +
                              "miasto VARCHAR(255), " +
                              "czas DOUBLE PRECISION  " +
                              ");";
            return createTableQueryTest;
        }
    }
}
