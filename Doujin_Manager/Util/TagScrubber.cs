using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Windows.Forms;

namespace Doujin_Manager.Util
{
    class TagScrubber
    {
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string Tags { get; set; } = "";
        public string ID { get; set; } = "000000";
        public bool HasValues { get; set; } = false;

        private bool shouldAbort = false;
        private readonly string nhentaiSearchUrl = @"https://nhentai.net/api/galleries/search?query=";
        private readonly string nhentaiGalleryUrl = @"https://nhentai.net/api/gallery/";

        public enum SearchMode
        {
            Title, 
            ID
        }

        /// <summary>
        /// Used to gather and set information related to the doujin online.
        /// Always use before calling Author, Tags, or ID.
        /// </summary>
        /// <param name="searchTerm">Term used to gather info. Can be doujin title or ID</param>
        /// <param name="mode">Specify whether to search from title or ID</param>
        public void GatherDoujinDetails(string searchTerm, SearchMode mode)
        {
            if (shouldAbort)
                return;

            string doujinTitle = WebUtility.UrlEncode(searchTerm);

            JObject jObject = GetJson(searchTerm, mode);

            if (jObject == null)
                return;

            if (!jObject.HasValues)
            {
                MessageBox.Show("Doujin does not exist",
                    "Invalid ID",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            switch (mode)
            {
                case SearchMode.Title:
                    GatherDoujinDetailsFromTitle(searchTerm, jObject);
                    break;
                case SearchMode.ID:
                    GatherDoujinDetailsFromID(searchTerm, jObject);
                    break;
                default:
                    break;
            }

            HasValues = true;
        }

        public void Clear()
        {
            Title = "";
            Author = "";
            Tags = "";
            ID = "000000";
            HasValues = false;
        }

        private void GatherDoujinDetailsFromTitle(string doujinTitle, JObject jObject)
        {
            if (Title == "")
                Title = doujinTitle;

            // Assign the value of the first search result to jObject if 
            // there is a search by Title
            if (jObject["result"] != null && !jObject["result"].HasValues)
                return;
            else if (jObject["result"] != null)
                jObject = (JObject)jObject["result"][0];

            Tags = GetTagsFromTitle(doujinTitle, jObject, "tag");
            Author = GetTagsFromTitle(doujinTitle, jObject, "artist");
            ID = GetIDFromTitle(doujinTitle, jObject);
        }

        private void GatherDoujinDetailsFromID(string doujinID, JObject jObject)
        {
            string doujinTitle = GetTitleFromID(doujinID, jObject);

            if (Title == "")
                Title = doujinTitle;

            GatherDoujinDetailsFromTitle(doujinTitle, jObject);
        }

        private JObject GetJson(string searchTerm, SearchMode mode)
        {
            if (shouldAbort)
                return null;

            string json = "";
            string jsonUrl = "";

            switch (mode)
            {
                case SearchMode.Title:
                    jsonUrl = nhentaiSearchUrl + searchTerm;
                    break;
                case SearchMode.ID:
                    jsonUrl = nhentaiGalleryUrl + searchTerm;
                    break;
                default:
                    break;
            }

            try
            {
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(jsonUrl);
                }
            }
            catch (WebException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString()); // probably 403 Forbidden
                shouldAbort = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            if (String.IsNullOrEmpty(json))
                return null;

            return JObject.Parse(json);
        }

        private string GetIDFromTitle(string doujinTitle, JObject jObject)
        {
            return jObject["id"].ToString();
        }

        private string GetTitleFromID(string doujinID, JObject jObject)
        {
            if (!jObject.HasValues)
                return "";

            return jObject["title"]["english"].ToString();
        }

        private string GetTagsFromTitle(string title, JObject jObject, string type)
        {
            string tags = "";

            foreach (var tag in jObject["tags"])
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
    }
}
