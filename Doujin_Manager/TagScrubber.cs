using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Manager
{
    class TagScrubber
    {
        public string Author { get; set; }
        public string Tags { get; set; }

        private readonly string nhentaiUrl = @"https://nhentai.net";
        private readonly string nhentaiSearchUrl = @"https://nhentai.net/api/galleries/search?query=";
        private readonly string exhentaiSearchUrl = @"https://exhentai.org/?f_search=";

        public void GatherTagsAndAuthor(string doujinTitle)
        {
            doujinTitle = WebUtility.UrlEncode(doujinTitle);

            string json;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(nhentaiSearchUrl + doujinTitle);
                }
            }
            catch(WebException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString()); // probably 403 Forbidden

                Tags = "BANNED";
                Author = "BANNED";
                return;
            }

            JObject jObject = JObject.Parse(json);

            Tags = GetTagsFromTitle(doujinTitle, jObject);
            Author = GetTagsFromNhentai(doujinTitle, jObject, "artist");
        }

        private string GetTagsFromTitle(string doujinTitle, JObject jObject)
        {
            string tags = "";

            if ((tags = GetTagsFromExhentai(doujinTitle)) != "" ||
                (tags = GetTagsFromNhentai(doujinTitle, jObject, "tag")) != "")
            {
                return tags;
            }


            return "ERR";
        }

        private string GetTagsFromNhentai(string title, JObject jObject, string type)
        {
            string tags = "";

            if (((JArray)jObject["result"]).Count == 0)
                return tags;

            foreach (var tag in jObject["result"][0]["tags"])
            {
                if (tag["type"].ToString() == type)
                {
                    tags += tag["name"].ToString() + ", ";
                }
            }

            if (tags == "")
                return "UNKNOWN";

            // Remove the last set of ", " from the tags string; maybe temporary
            return tags.Remove(tags.Length-2);
        }

        private string GetTagsFromExhentai(string title)
        {
            return "";
        }

    }
}
