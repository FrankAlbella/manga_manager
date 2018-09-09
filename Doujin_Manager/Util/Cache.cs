using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Doujin_Manager.Util
{
    class Cache
    {
        private readonly string _jsonFileContents;
        public string JsonFileContents{ get { return _jsonFileContents; } }

        private readonly List<Doujin> _cachedDoujins; 
        public List<Doujin> CachedDoujins { get { return _cachedDoujins; } }

        public Cache()
        {
            _jsonFileContents = GetSerializedJsonFromFile();
            _cachedDoujins = GetDoujinsFromCache();
        }

        public void Save(List<Doujin> doujins)
        {
            File.WriteAllText(PathUtil.cacheFilePath, JsonConvert.SerializeObject(doujins, Formatting.Indented));
        }

        private List<Doujin> GetDoujinsFromCache()
        {
            return JsonConvert.DeserializeObject<List<Doujin>>(JsonFileContents);
        }

        public string GetSerializedJsonFromFile()
        {
            string fileContents;

            using (StreamReader s = new StreamReader(PathUtil.cacheFilePath))
            {
                fileContents = s.ReadToEnd();
            }

            return fileContents;
        }

    }
}
