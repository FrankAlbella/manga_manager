using System;

namespace Doujin_Manager.Util
{
    public static class PathUtil
    {
        public static string appdataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\doujin_manager";
        public static string thumbnailDir = appdataDir + @"\thumbnails";
        public static string saveFilePath = appdataDir + @"\settings.xml";
        public static string cacheFilePath = appdataDir + @"\cache.json";
        public static string rootDoujinDirectory = SettingsUtil.DoujinDirectory;
    }
}
