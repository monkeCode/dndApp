using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Helpers
{
    internal class Web
    {
       private static readonly Regex _apiParseRegex = new("\"original\": \"([^\'\" ])*\"", RegexOptions.Compiled);

        public static async Task<string> GetImageUri(string request)
        {
            string uriReq =
                $"https://serpapi.com/search.json?engine=yandex_images&text={request}&api_key=d7c2281e5d111c0beef6ba68be9fc13bc4062e4f450a450b9a6b5a4b18ace458";

            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("yandex_domain","yandex.ru");
            http.DefaultRequestHeaders.Add("site", "dnd.su");
            var responseMessage = await http.GetAsync(uriReq);
            var stream = await responseMessage.Content.ReadAsStreamAsync();

            byte[] dataArray = new byte[stream.Length];
            await stream.ReadAsync(dataArray, 0, (int)stream.Length);

            string dataStr = Encoding.UTF8.GetString(dataArray);
            var results = _apiParseRegex.Matches(dataStr).Select(it => string.Concat(it.Groups[1].Captures.Select(it => it.Value))).ToList();
            return results[0];
        }
    }
}
