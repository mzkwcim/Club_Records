using HtmlAgilityPack;
using Npgsql;
using System;
using System.ComponentModel.Design;
using System.Xml.Linq;

MainLoops.GenderLoop();
class DataConversion
{
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
    public static List<string> URL(string url)
    {
        var URLlink = Loader(url).DocumentNode.SelectNodes("//td[@class='swimstyle']//a[@href]");
        List<string> linki = new List<string>();
        for (int i = 0; i < URLlink.Count; i++)
        {
            linki.Add("https://www.swimrankings.net/index.php" + URLlink[i].GetAttributeValue("href", "").Replace("amp;", ""));
        }
        return linki;
    }
    public static string Records(string dystans, int counter)
    {
        string addValues = "";
        var htmlDocument = Loader(dystans);
        List<string> distances = new List<string>();
        List<string> fullnames = new List<string>();
        List<string> times = new List<string>();
        List<string> dates = new List<string>();
        List<string> cities = new List<string>();
        var distance = htmlDocument.DocumentNode.SelectNodes("//td[@class='swimstyle']");
        var fullname = htmlDocument.DocumentNode.SelectNodes("//td[@class='fullname']");
        var time = htmlDocument.DocumentNode.SelectNodes("//td[@class='time']");
        var date = (htmlDocument.DocumentNode.SelectNodes("//td[@class='date']"));
        var city = (htmlDocument.DocumentNode.SelectNodes("//td[@class='city']"));
        for (int i = 0; i < distance.Count; i++)
        {
            distances.Add(distance[i].InnerText);
            fullnames.Add(fullname[i].InnerText);
            times.Add(time[i].InnerText);
            dates.Add(date[i].InnerText);
            cities.Add(city[i].InnerText);
        }
        if (counter > 11)
        {
            List<string> youngerdistances = new List<string>();
            List<string> youngerfullnames = new List<string>();
            List<string> youngertimes = new List<string>();
            List<string> youngerdates = new List<string>();
            List<string> youngercities = new List<string>();
            var youngerhtmlDocument = Loader(YoungerUrl(dystans));
            Console.WriteLine(YoungerUrl(dystans));
            var youngerDistance = youngerhtmlDocument.DocumentNode.SelectNodes("//td[@class='swimstyle']");
            Console.WriteLine(youngerDistance[0].InnerText);
            var youngerFullName = youngerhtmlDocument.DocumentNode.SelectNodes("//td[@class='fullname']");
            var youngerTime = youngerhtmlDocument.DocumentNode.SelectNodes("//td[@class='time']");
            var youngerDate = youngerhtmlDocument.DocumentNode.SelectNodes("//td[@class='date']");
            var youngerCity = youngerhtmlDocument.DocumentNode.SelectNodes("//td[@class='city']");
            for (int i = 0; i < youngerDistance.Count; i++)
            {
                youngerdistances.Add(youngerDistance[i].InnerText);
                youngerfullnames.Add(youngerFullName[i].InnerText);
                youngertimes.Add(youngerTime[i].InnerText);
                youngerdates.Add(youngerDate[i].InnerText);
                youngercities.Add(youngerCity[i].InnerText);
                Console.WriteLine(youngerdistances[i]);
            }
            List<int> indeksy = new List<int>();
            for (int i = 0; i < distances.Count; i++)
            {
                if (youngerdistances.Contains(distances[i]))
                {
                    indeksy.Add(i);
                }
            }
            int adder = 0;
            Console.WriteLine(distances.Count);
            Console.WriteLine(youngerdistances.Count);
            for (int i = 0; i < distance.Count; i++)
            {
                if (!String.IsNullOrEmpty(DataChecker.LapChecker(distances[i])))
                {
                    if (!String.IsNullOrEmpty(DataChecker.LapChecker(youngerdistances[adder])))
                    {
                        if (distances[i] == youngerdistances[adder])
                        {
                            if (DataConversion.ConvertToDouble(youngertimes[adder]) < DataConversion.ConvertToDouble(times[i]))
                            {
                                addValues += $" (\'{DataConversion.StrokeTranslation(youngerdistances[adder])}\', \'{DataConversion.ToTitleString(youngerfullnames[adder])}\', \'{youngertimes[adder]}\', \'{DataConversion.DateTranslation(youngerdates[adder])}\', \'{youngercities[adder]}\', \'{DataConversion.ConvertToDouble(youngertimes[adder])}\' ),";
                            }
                            adder++;
                        }
                    }
                    else
                    {
                        addValues += $" (\'{DataConversion.StrokeTranslation(distances[i])}\', \'{DataConversion.ToTitleString(fullnames[i])}\', \'{times[i]}\', \'{DataConversion.DateTranslation(dates[i])}\', \'{cities[i]}\', \'{DataConversion.ConvertToDouble(times[i])}\' ),";
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < distance.Count; i++)
            {
                if (!String.IsNullOrEmpty(DataChecker.LapChecker(distance[i].InnerText)))
                {
                    addValues += $" (\'{DataConversion.StrokeTranslation(distance[i].InnerText)}\', \'{DataConversion.ToTitleString(fullname[i].InnerText)}\', \'{time[i].InnerText}\', \'{DataConversion.DateTranslation(date[i].InnerText)}\', \'{city[i].InnerText}\', \'{DataConversion.ConvertToDouble(time[i].InnerText)}\' ),";
                }
            }     
        }
        
        return addValues;
    }
    static string YoungerUrl(string dystans)
    {
        string[] ageGroup = new string[] { "11", "12012", "13013", "14014", "15015", "16016", "17017", "18018", "19000", "0" };
        string dystansv2 = "";
        for (int i = 0; i < ageGroup.Length; i++)
        {
            if (dystans.Contains("agegroup=" + ageGroup[i]))
            {
                dystansv2 = dystans.Replace("agegroup=" + ageGroup[i], "agegroup=" + ageGroup[i - 1]);  
            }
        }
        return dystansv2;
    }
}
class DataChecker
{
    public static string LapChecker(string distance)
    {
        return distance.Contains("Lap") ? "" : distance;
    }
    public static bool UrlExists(string urlForEachDistance)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(urlForEachDistance).Result;

                if (response.IsSuccessStatusCode)
                {
                    string html = response.Content.ReadAsStringAsync().Result;
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(html);

                    // Tutaj możesz dalej przetwarzać dokument HTML
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }
    }
}
class SQLConnections
{
    public static string CreateTable(string name)
    {
        string createTableQueryTest = $"CREATE TABLE {name} (" +
                          "ID SERIAL PRIMARY KEY, " +
                          " dystans VARCHAR(255), imie VARCHAR(255), czasCzytelny VARCHAR(255), data VARCHAR(255), miasto VARCHAR(255), czas DOUBLE PRECISION  );";
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
        int[] genderloop = [1, 2];
        string tablename = "Rekordy_";
        for (int i = 0; i < genderloop.Length; i++)
        {
            tablename += (genderloop[i] == 1) ? "mężczyzn" : "kobiet";
            PoolLengthLoop(tablename);
        }
    }
    static void PoolLengthLoop(string tablename)
    {
        string[] length = new string[] { "LCM", "SCM" };
        for (int i = 0; i < length.Length;i++)
        {
            string pool = (length[i] == "LCM") ? "LCM" : "SCM";
            AgeGroupLoop(pool, tablename);
        }
    }
    static void AgeGroupLoop(string pool, string tablename)
    {
        string[] ageGroup = new string[] { "11", "12012", "13013", "14014", "15015", "16016", "17017", "18018", "19000", "0" };
        int counter = 11;
        tablename += "_10_letnich_lcm";
        List<string> urlForEachDistance = new List<string>();
        string url = "https://www.swimrankings.net/index.php?page=rankingDetail&clubId=89634&gender=1&season=-1&course=SCM&stroke=0&agegroup=14014";
        for (int i = 0; i < ageGroup.Length; i++)
        {
            string agegrouprecords = url.Replace("agegroup=14014", "agegroup=" + ageGroup[i]).Replace("course=SCM", "course=" + pool).Replace("gender=1", "gender=" + ((tablename.Contains("mężczyzn")) ? "1" : "2"));
            Console.WriteLine(agegrouprecords);
            DistanceLoop(agegrouprecords, tablename.Replace("_10_letnich_lcm", (counter <= 19) ? $"_{counter}_letnich_{pool}" : $"_20_{pool}"), counter);
            counter++;
        } 
    }
    static void DistanceLoop(string urlForEachDistance, string tablename, int counter)
    {
        string addValues = $"INSERT INTO {tablename.ToLower()} (dystans, imie, czasCzytelny, data, miasto, czas) VALUES";
        addValues += (DataChecker.UrlExists(urlForEachDistance)) ? HtmlGetter.Records(urlForEachDistance, counter) : "";
        addValues = addValues.Remove(addValues.Length - 1);
        addValues += ";";
        Console.WriteLine(addValues);
    }
}
