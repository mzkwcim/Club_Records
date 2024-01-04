using HtmlAgilityPack;
using Npgsql;
using System.Text.RegularExpressions;
using System;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

MainLoops.GenderLoop();
class DataConversion
{
    public static string TextSanitizer(string text)
    {
        string cleanedString = Regex.Replace(text, @"[^0-9:,.]", "");
        return cleanedString;
    }
    public static double ConvertToDouble(string doubles)
    {
        string[] list = doubles.Split(':');
        double newer = Math.Round(((list.Length > 1) ? (Convert.ToDouble(list[0]) * 60) + Convert.ToDouble(list[1]) : Convert.ToDouble(list[0])), 2);
        return newer;
    }
    public static string DateTranslation(string converter)
    {
        string date;
        string[] words = converter.Split("&nbsp;");
        if (words.Length == 3)
        {
            switch (words[1])
            {
                case "Jan":
                    date = words[0] + " Stycznia " + words[2];
                    return date;
                case "Feb":
                    date = words[0] + " Lutego " + words[2];
                    return date;
                case "Mar":
                    date = words[0] + " Marca " + words[2];
                    return date;
                case "Apr":
                    date = words[0] + " Kwietnia " + words[2];
                    return date;
                case "May":
                    date = words[0] + " Maja " + words[2];
                    return date;
                case "Jun":
                    date = words[0] + " Czerwca " + words[2];
                    return date;
                case "Jul":
                    date = words[0] + " Lipca " + words[2];
                    return date;
                case "Aug":
                    date = words[0] + " Sierpnia " + words[2];
                    return date;
                case "Sep":
                    date = words[0] + " Września " + words[2];
                    return date;
                case "Oct":
                    date = words[0] + " Października " + words[2];
                    return date;
                case "Nov":
                    date = words[0] + " Listopada " + words[2];
                    return date;
                case "Dec":
                    date = words[0] + " Grudnia " + words[2];
                    return date;
                default:
                    return "";
            }
        }
        else
        {
            switch (words[2])
            {
                case "Jan":
                    date = words[1] + " Stycznia " + words[3];
                    return date;
                case "Feb":
                    date = words[1] + " Lutego " + words[3];
                    return date;
                case "Mar":
                    date = words[1] + " Marca " + words[3];
                    return date;
                case "Apr":
                    date = words[1] + " Kwietnia " + words[3];
                    return date;
                case "May":
                    date = words[1] + " Maja " + words[3];
                    return date;
                case "Jun":
                    date = words[1] + " Czerwca " + words[3];
                    return date;
                case "Jul":
                    date = words[1] + " Lipca " + words[3];
                    return date;
                case "Aug":
                    date = words[1] + " Sierpnia " + words[3];
                    return date;
                case "Sep":
                    date = words[1] + " Września " + words[3];
                    return date;
                case "Oct":
                    date = words[1] + " Października " + words[3];
                    return date;
                case "Nov":
                    date = words[1] + " Listopada " + words[3];
                    return date;
                case "Dec":
                    date = words[1] + " Grudnia " + words[3];
                    return date;
                default:
                    return "";
            }
        }
    }
    public static string StrokeTranslation(string distance)
    {
        try
        {
            string stroke;
            string[] words = distance.Split(' ');
            switch (words[1])
            {
                case "Freestyle":
                    stroke = words[0] + " Dowolnym";
                    return stroke;
                case "Backstroke":
                    stroke = words[0] + " Grzbietowym";
                    return stroke;
                case "Breaststroke":
                    stroke = words[0] + " Klasycznym";
                    return stroke;
                case "Butterfly":
                    stroke = words[0] + " Motylkowym";
                    return stroke;
                case "Medley":
                    stroke = words[0] + " Zmiennym";
                    return stroke;
                default:
                    return "";
            }
        }
        catch
        {
            return "";
        }
        
    }
    public static string ToTitleString(string fullname)
    {
        string[] dividedFullName = fullname.Replace(",", "").Split(' ');
        string[] newFullName = new string[dividedFullName.Length];
        for (int i = 0; i < dividedFullName.Length; i++)
        {
            int adder = 0;
            foreach (char c in dividedFullName[i])
            {
                newFullName[i] += (adder == 0) ? c.ToString().ToUpper() : c.ToString().ToLower();
                adder++;
            }
        }
        string newFullName2 = string.Join(" ", newFullName);
        return newFullName2;
    }
}
class HtmlGetter
{
    public static HtmlAgilityPack.HtmlDocument Loader(string url)
    {
        var httpClient = new HttpClient();
        var html = httpClient.GetStringAsync(url).Result;
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        return htmlDocument;
    }
    public static string Records(string link)
    {
        string addValues = "";
        var htmlDocument = Loader(link);
        string distance = DataConversion.StrokeTranslation(DataChecker.LapChecker(htmlDocument.DocumentNode.SelectSingleNode("//td[@class='titleCenter']").InnerText));
        if (distance == "")
        {

        }
        else
        {
            string fullname = htmlDocument.DocumentNode.SelectSingleNode("//td[@class='fullname']").InnerText;
            string time = DataConversion.TextSanitizer(htmlDocument.DocumentNode.SelectSingleNode("//td[@class='time']").InnerText);
            string date = DataConversion.DateTranslation(htmlDocument.DocumentNode.SelectSingleNode("//td[@class='date']").InnerText);
            string city = htmlDocument.DocumentNode.SelectSingleNode("//td[@class='city']").InnerText;
            double convertedtime = DataConversion.ConvertToDouble(DataConversion.TextSanitizer(htmlDocument.DocumentNode.SelectSingleNode("//td[@class='time']").InnerText));
            if (!String.IsNullOrEmpty(distance))
            {
                return addValues += $" ( \'{distance}\', \'{fullname}\', \'{time}\', \'{date}\', \'{city}\', {convertedtime} ), ";
            }
        }
        return "";
    }
    public static string[] URL(string url)
    {
        var URLlink = Loader(url).DocumentNode.SelectNodes("//td[@class='swimstyle']//a[@href]");
        string[] linki = new string[URLlink.Count];
        for (int i = 0; i < URLlink.Count; i++)
        {
            linki[i] = "https://www.swimrankings.net/index.php" + URLlink[i].GetAttributeValue("href", "").Replace("amp;", "");
        }
        return linki;
    }
}
class DataChecker
{
    public static string LapChecker(string distance)
    {
        return distance.Contains("Lap") ? "" : distance;
    }
}
class SQLConnections
{
    public static string Inserter(string gender, int counter, string pool)
    {
        string inserter = $"INSERT INTO rekordy_{gender}_{counter}_letnich_{pool} (dystans, imie, czasczytelny, data, miasto, czas) " +
            $"SELECT r{counter-1}.dystans, r{counter - 1}.imie, r{counter - 1}.czasczytelny, r{counter - 1}.data, r{counter - 1}.miasto, r{counter - 1}.czas " +
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
        string comparator = $"UPDATE rekordy_{gender}_{counter}_letnich_{pool} AS r{counter} " +
            "SET " +
            $"imie = r{counter-1}.imie, " +
            $"czasCzytelny = r{counter-1}.czasCzytelny, " +
            $"data = r{counter-1}.data, " +
            $"miasto = r{counter-1}.miasto, " +
            $"czas = r{counter-1}.czas " +
            $"FROM rekordy_{gender}_{counter-1}_letnich_{pool} AS r{counter-1} " +
            "WHERE " +
            $"r{counter}.dystans = r{counter-1}.dystans " +
            $"AND r{counter}.czas > r{counter-1}.czas;";
        Console.WriteLine(comparator);
        return comparator;
    }
    public static void ComparatorConnection(string name, int counter, string pool, string gender)
    {
        string connectionString = "Host=localhost;Username=postgres;Password=Mzkwcim181099!;Database=postgres";
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                if (TableExists(connection, name))
                {
                    using (NpgsqlCommand command2 = new NpgsqlCommand(Comparator(counter, pool, gender), connection))
                    {
                        command2.ExecuteNonQuery();
                        Console.WriteLine("Powodzenie");
                    }
                    using (NpgsqlCommand command3 = new NpgsqlCommand(Inserter(gender, counter, pool), connection))
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
    public static string CreateTable(string name)
    {
        string createTableQueryTest = $"CREATE TABLE {name} ( " +
                          "ID SERIAL PRIMARY KEY, " +
                          "dystans VARCHAR(255) UNIQUE, " +
                          "imie VARCHAR(255), " +
                          "czasCzytelny VARCHAR(255), " +
                          "data VARCHAR(255), " +
                          "miasto VARCHAR(255), " +
                          "czas DOUBLE PRECISION  " +
                          ");";
        return createTableQueryTest;
    }
    static bool TableExists(NpgsqlConnection connection, string tableName)
    {
        using (NpgsqlCommand command = new NpgsqlCommand())
        {
            command.Connection = connection;
            command.CommandText = $"SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = @tableName)";
            command.Parameters.AddWithValue("@tableName", tableName.ToLower());
            object result = command.ExecuteScalar();
            return result != null && (bool)result;
        }
    }
    public static void Connection(string addValuesQuery, string name)
    {
        string connectionString = "Host=localhost;Username=postgres;Password=Mzkwcim181099!;Database=postgres";
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
                    string createTableQuery = CreateTable(name);
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
}
class MainLoops
{
    public static void GenderLoop()
    {
        int[] genderloop = [2, 1];
        for (int i = 0; i < genderloop.Length; i++)
        {
            string tablename = (genderloop[i] == 1) ? "rekordy_mężczyzn" : "rekordy_kobiet";
            PoolLengthLoop(tablename);
        }
    }
    static void PoolLengthLoop(string tablename)
    {
        string[] length = new string[] { "LCM", "SCM" };
        for (int i = 0; i < length.Length; i++)
        {
            string pool = (length[i] == "LCM") ? "LCM" : "SCM";
            AgeGroupLoop(pool, tablename);
        }
    }
    static void AgeGroupLoop(string pool, string tablename)
    {
        string[] ageGroup = new string[] { "11", "12012", "13013", "14014", "15015", "16016", "17017", "18018", "19000", "0" };
        int counter = 11;
        string gender = tablename.Replace("rekordy_", "");
        tablename += "_10_letnich_lcm";
        string url = "https://www.swimrankings.net/index.php?page=rankingDetail&clubId=89634&gender=1&season=-1&course=SCM&stroke=0&agegroup=14014";
        for (int i = 0; i < ageGroup.Length; i++)
        {
            string distanceURL = url.Replace("agegroup=14014", "agegroup=" + ageGroup[i]).Replace("course=SCM", "course=" + pool).Replace("gender=1", "gender=" + ((tablename.Contains("mężczyzn")) ? "1" : "2"));
            Console.WriteLine(distanceURL);
            DistanceLoop(distanceURL, tablename.Replace("_10_letnich_lcm", (counter <= 19) ? $"_{counter}_letnich_{pool}" : $"_20_letnich_{pool}"), counter, pool, gender);
            counter++;
        }
    }
    static void DistanceLoop(string distanceURL, string tablename, int counter, string pool, string gender)
    {
        try
        {
            string[] tratatata = HtmlGetter.URL(distanceURL);
            HashSet<string> set = new HashSet<string>(tratatata);
            string[] tabwithoutdups = set.ToArray();
            string addvalues = $"INSERT INTO {tablename} (dystans, imie, czasCzytelny, data, miasto, czas) VALUES ";
            for (int i = 0; i < tabwithoutdups.Length; i++)
            {
                addvalues += HtmlGetter.Records(tabwithoutdups[i]);
            }
            addvalues = addvalues.Substring(0, addvalues.Length - 2);
            addvalues += ";";
            Console.WriteLine(addvalues);
            SQLConnections.Connection(addvalues, tablename);
            if (counter > 11)
            {
                SQLConnections.ComparatorConnection(tablename, counter, pool, gender);
            }
        }
        catch
        {

        }
    }
}
