using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Club_Records
{
    internal class Scraper
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
            if (distance != "")
            {
                string fullname = htmlDocument.DocumentNode.SelectSingleNode("//td[@class='fullname']").InnerText;
                string time = DataConversion.TextSanitizer(htmlDocument.DocumentNode.SelectSingleNode("//td[@class='time']").InnerText);
                string date = DataConversion.DateTranslation(htmlDocument.DocumentNode.SelectSingleNode("//td[@class='date']").InnerText);
                string city = htmlDocument.DocumentNode.SelectSingleNode("//td[@class='city']").InnerText.Replace("&nbsp;", " ");
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
}
