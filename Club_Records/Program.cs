using HtmlAgilityPack;
using Npgsql;
using Club_Records;

MainLoops.GenderLoop();
class DataChecker
{
    public static string LapChecker(string distance)
    {
        return distance.Contains("Lap") ? "" : distance;
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
            string[] tratatata = Scraper.URL(distanceURL);
            HashSet<string> set = new HashSet<string>(tratatata);
            string[] tabwithoutdups = set.ToArray();
            string addvalues = $"INSERT INTO {tablename} (dystans, imie, czasCzytelny, data, miasto, czas) VALUES ";
            for (int i = 0; i < tabwithoutdups.Length; i++)
            {
                addvalues += Scraper.Records(tabwithoutdups[i]);
            }
            addvalues = addvalues.Substring(0, addvalues.Length - 2);
            addvalues += ";";
            Console.WriteLine(addvalues);
            ConnectionManager.Connection(addvalues, tablename);
            if (counter > 11)
            {
                ConnectionManager.ComparatorConnection(tablename, counter, pool, gender);
            }
        }
        catch
        {

        }
    }
}
