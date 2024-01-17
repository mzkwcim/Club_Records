using System.Text.RegularExpressions;

namespace Club_Records
{
    internal class DataConversion
    {
        public static string TextSanitizer(string text) => Regex.Replace(text, @"[^0-9:,.]", "");
        public static double ConvertToDouble(string timeStringWithCollons)
        {
            string[] list = timeStringWithCollons.Split(':');
            double timeWithoutCollons = Math.Round(((list.Length > 1) ? (Convert.ToDouble(list[0]) * 60) + Convert.ToDouble(list[1]) : Convert.ToDouble(list[0])), 2);
            return timeWithoutCollons;
        }
        public static string DateTranslation(string englishDate)
        {
            string[] partsOfEnglishDate = englishDate.Split("&nbsp;");
            return (partsOfEnglishDate.Length == 4) ? $"{partsOfEnglishDate[1]} {GetMonthTranslation(partsOfEnglishDate[2])} {partsOfEnglishDate[3]}" : $"{partsOfEnglishDate[0]} {GetMonthTranslation(partsOfEnglishDate[1])} {partsOfEnglishDate[2]}";
        }
        private static string GetMonthTranslation(string partsOfEnglishDate)
        {
            switch (partsOfEnglishDate)
            {
                case "Jan": return "Stycznia";
                case "Feb": return "Lutego";
                case "Mar": return "Marca";
                case "Apr": return "Kwietnia";
                case "May": return "Maja";
                case "Jun": return "Czerwca";
                case "Jul": return "Lipca";
                case "Aug": return "Sierpnia";
                case "Sep": return "Września";
                case "Oct": return "Października";
                case "Nov": return "Listopada";
                case "Dec": return "Grudnia";
                default: return "";
            }
        }
        public static string StrokeTranslation(string englishNamedDistance)
        {
            try
            {
                string[] partsOfEnglishNamedDistance = englishNamedDistance.Split(' ');
                switch (partsOfEnglishNamedDistance[1])
                {
                    case "Freestyle": return partsOfEnglishNamedDistance[0] + " Dowolnym";
                    case "Backstroke": return partsOfEnglishNamedDistance[0] + " Grzbietowym";
                    case "Breaststroke": return partsOfEnglishNamedDistance[0] + " Klasycznym";
                    case "Butterfly": return partsOfEnglishNamedDistance[0] + " Motylkowym";
                    case "Medley": return partsOfEnglishNamedDistance[0] + " Zmiennym";
                    default: return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public static string ToTitleString(string fullname)
        {
            string[] words = fullname.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
            return string.Join(" ", words).Replace(",", "");
        }
    }
}
